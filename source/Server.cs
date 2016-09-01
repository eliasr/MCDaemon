using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDaemon
{
    /// <summary>
    /// The server handles requests, perform system changes and updates its clients with the results. Includind updating the HTML
    /// </summary>
    class Server
    {
        enum Cmds
        {
            start,
            stop
        }
        private int activeServer;
        public List<MCServer> mcservers;
        private static Server instance;
        //private Network network;
        private HtmlWriter HW;
        private String myKey;
        public string Host;
        public ushort Port;
        public static Server Instance
        {
            get{
                if (instance == null)
                {
                    instance = new Server();
                }
                return instance;
            }
        }

        public Server ()
        {
            instance = this;
            mcservers = new List<MCServer>();
            MakeServerList(); //read config and populate the MCServer list so these are available to be called.
            GenerateKey();
            InitializeOthers();

        }
        private void InitializeOthers()
        {
            ResourceManager.Instance().ClearOutput();
            //network = Network.Instance();
            HW = HtmlWriter.Instance();
            //open up for users
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                Network.Instance.StartTCPListener();
            });
        }

        private void MakeServerList()
        {

            XMLConfig con = XmlHandler.Deserialize();
            for (int i = 0; i < con.ServerList.Count; i++)
            {
                mcservers.Add(new MCServer(con.ServerList[i].Name, con.ServerList[i].FilePath,con.ServerList[i].AllowInput));
            }
            Host = con.Host;
            Port = con.Port;

        }

        //When there is a significant change, so we need to update our clients
        public void UpdateClients()
        {
            GenerateKey();
            UpdateHTML();
            //network.RefreshClients();
            // no C# clients yet.
        }
        public void UpdateHTML(bool busy = false)
        {
            HW.WriteHTML(busy);
        }


        public String ActiveClientsMsg()
        {
            
            String msg = "Active users: " + Network.Instance.GetNumConnections().ToString(); //Note this is not entirely true
            return msg;
        }

        private static System.Threading.Mutex mutex = new System.Threading.Mutex();
        public int ParseCommand(String msg)
        {
            mutex.WaitOne();   // Wait until it is safe to enter.
            UpdateHTML(true);
            int status = -1;

            //format: id=<int>&cmd=<int>&key=<key>
            //format: str=<text>&key=<key>
            status = ParseType(msg);
            if (status == 0)
            {
                status = ParseIdCmd(msg);
            }
            else if(status == 1)
            {
                status = ParseStrCmd(msg);
            }

            UpdateClients();
            mutex.ReleaseMutex();    // Release the Mutex.
            return status;
        }

        public int ParseType(String msg)
        {
            String format = msg.Substring(0, 3);

            if (format == "id=")
            {
                return 0;
            }
            else if (format == "str")
            {
                return 1;
            }

            return -1;
        }

        private int ParseIdCmd(String msg)
        {
            int status = -1;
            int id = -1;
            int cmd = -1;
            string[] Lines = msg.Split('&');
            if (Lines.Length == 3)
            {
                string[] sid = Lines[0].Split('=');
                string[] scmd = Lines[1].Split('=');
                string[] skey = Lines[2].Split('=');
                if (skey.Length == 2)
                {
                    if (skey[1] == myKey)
                    {
                        if (sid.Length == 2)
                        {
                            if (int.TryParse(sid[1], out id))
                            {
                                if (int.TryParse(scmd[1], out cmd))
                                {
                                    status = 0;
                                    DoIdCommand(id, cmd);
                                }
                            }
                        }
                    }
                    else
                    {
                        status = -2; //skey did not match;
                    }

                }

            }
            return status;
        }

        private int ParseStrCmd(String msg)
        {
            int status = -1;
            string[] Lines = msg.Split('&');
            if (Lines.Length == 2)
            {
                string[] scmd = Lines[0].Split('=');
                string[] skey = Lines[1].Split('=');
                if (skey.Length == 2)
                {
                    if (skey[1] == myKey)
                    {
                        if (scmd.Length == 2)
                        {
                            if (!String.IsNullOrEmpty(scmd[1]))
                            {
                                status = 0;
                                DoStrCommand(scmd[1]);
                            }
                        }
                    }
                    else
                    {
                        status = -2; //skey did not match;
                    }

                }

            }

            return status;
        }

        private void DoStrCommand(String cmd)
        {

            if ( mcservers.Count > activeServer && mcservers[activeServer].isOnline)
            {
                if (mcservers[activeServer].AllowInput)
                {
                    cmd = cmd.Replace("+", " ");
                    cmd = System.Uri.UnescapeDataString(cmd);
                    mcservers[activeServer].GiveInput(cmd);
                }
            }
            return;
        }

        private void DoIdCommand(int id, int cmd)
        {
            if (id >= mcservers.Count || id < 0)
            {
                return;
            }
            if (cmd > 1 || cmd <0)
            {
                return;
            }
            //======= real deal here
            //GenerateKey();

            if (cmd == (int)Cmds.start)
            {
                StartMCServer(id);
            }
            else
            {
                StopMCServer(id);
            }

            return;
        }
        private void GenerateKey()
        {
            Random r = new Random();
            byte[] key = new byte[100];
            for (int i = 0; i < 100; i++)
            {
                key[i] = (byte)r.Next(48, 57);
            }

            myKey = System.Text.Encoding.ASCII.GetString(key);
        }

        public string GetKey()
        {
            return myKey;
        }

        private void StartMCServer(int id)
        {
            if (mcservers[id].isOnline)
            {
                return;
            }

            for (int i = 0; i < mcservers.Count; i++)
            {
                StopMCServer(i);
            }


            //code here to start the server process
            ResourceManager.Instance().ClearOutput();
            ResourceManager.Instance().WriteOutput("Starting server: " + mcservers[id].ServerName + " ....");
            mcservers[id].StartProcess();
            mcservers[id].isOnline = true;
            activeServer = id;
            ProcessIcon.Instance.UpdateIcon();
            return;
        }

        private void StopMCServer(int id)
        {
            if (!mcservers[id].isOnline)
            {
                return;
            }
            //code here to stop the server process
            mcservers[id].StopProcess();
            mcservers[id].isOnline = false;
            ResourceManager.Instance().ClearOutput();
            ProcessIcon.Instance.UpdateIcon();
            return;
        }

        public int GetActiveServer()
        {
            return activeServer;
        }

        //private void SendMCCommand(String cmd)
        //{

        //}
    }
}
