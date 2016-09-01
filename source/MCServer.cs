using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace MCDaemon
{
    /// <summary>
    /// This class represents a server. It handles startup, communication (I/O) and ending of the process.
    /// </summary>
    class MCServer
    {
        class Program
        {
            private Process process;
            //private StreamReader sr;
            private StreamWriter sw;
            public System.Threading.Tasks.Task Worker;
            volatile bool stop = false;
            //volatile bool dispose = false;
            bool gotInput = false;
            String cmd; //input string
            //StringBuilder outputText = new StringBuilder();
            public Stopwatch killTimer;


            public Program( ProcessStartInfo psi)
            {
                killTimer = new Stopwatch();
                process = Process.Start(psi);
                sw = process.StandardInput;
                Worker = System.Threading.Tasks.Task.Factory.StartNew(() => {
                    RunService();
                },System.Threading.Tasks.TaskCreationOptions.LongRunning);

                process.OutputDataReceived += (sendingProcess, outLine) =>
                {
                    if (stop)
                    {
                        killTimer.Restart();
                    }
                    ResourceManager.Instance().WriteOutput(outLine.Data); // capture the output
                    Console.Out.WriteLine(outLine.Data); // echo the output
                };
                //TODO: Add error Redirect
                process.BeginOutputReadLine();
                
            }


            private void RunService()
            {
                while (!stop)
                {
                    

                    if (gotInput)
                    {
                        Console.WriteLine("----> Got Input <-----\n");
                        //send the input to the program
                        sw.WriteLine(cmd);
                        ResourceManager.Instance().WriteOutput(cmd); // capture the output
                        cmd = "";
                        gotInput = false;
                    }
                    

                    //Thread.Sleep(1000); //check again in a second
                }
                Console.WriteLine("----> Got STOP - Shutting down <-----\n");
                //send stop command to program
                //sw.WriteLine("stop");
                //dispose = true;
            }

            public void SetInput(String scmd)
            {
                if (!stop)
                {
                    cmd = scmd;
                    gotInput = true;
                }

            }



            public void Stop(){stop = true; sw.WriteLine("stop"); }
            //public bool Disposable(){return dispose;} //obsolete, the use of this is instead handled by waiting for Worker;
            public void DisposeMe()
            {
                process.CancelOutputRead();
                sw.Flush();
                sw.Close();
                sw = null;
                try
                {
                    process.Kill();
                    process.Dispose();
                }
                catch (Exception)
                {

                   
                }
                process = null;
            }

        }

        public readonly String ServerName;
        public readonly String FilePath;
        public readonly bool AllowInput;
        public bool isOnline;
        public ProcessStartInfo PSI;
        private Program prog;

        public MCServer()
        {
            ServerName = "FakeServer";
            isOnline = false;
            FilePath = "";
        }

        public MCServer(string servername, string filepath, bool input = false)
        {
            ServerName = servername;
            FilePath = filepath;
            AllowInput = input;
            isOnline = false;
        }

        public void StartProcess()
        {
            if (prog != null)
            {
                StopProcess();
            }

            PSI = new ProcessStartInfo();

            PSI.FileName = "cmd.exe";
            String argument = @" /k " + @"call """ + FilePath + @""" & exit"; // FilePath;


            PSI.WorkingDirectory = System.IO.Path.GetDirectoryName(FilePath);
            PSI.Arguments = argument;
            PSI.UseShellExecute = false;
            PSI.RedirectStandardOutput = true;
            PSI.RedirectStandardInput = true;

            prog = new Program(PSI);


        }

        public void StopProcess()
        {
            if (prog != null)
            {
            prog.Stop();
            prog.Worker.Wait(5000);
            prog.killTimer.Restart();
            int forceCount = 0;
            while (prog.killTimer.ElapsedMilliseconds < 3500 && forceCount < 20) //making sure the output has fully written
            {
                Thread.Sleep(500);
                forceCount++;

            }
            //kill it whether it is ready or not
            prog.DisposeMe();
            prog = null;
            Console.WriteLine("------>Process killed<---------");
            PSI = null;
            }
        }

        public void GiveInput(String cmd)
        {
            if (prog != null)
            {
                prog.SetInput(cmd);
            }
        }

    }
}
