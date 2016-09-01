using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MCDaemon
{

    class ResourceManager
    {
        private static Mutex WebMutex = new Mutex(); //the mutex for the index file
        private static Mutex OutputMutex = new Mutex(); //the mutex for the txt output file
        //public String WebPath;
        //public String OutputPath;
        private string WebCache;
        private string OutputCache;
        private static ResourceManager instance;
        public static ResourceManager Instance()
        {
            if (instance == null)
            {
                instance = new ResourceManager();
            }
            return instance;
        }
        private ResourceManager()
        {
            System.IO.Directory.CreateDirectory(@"C:\MCDaemon\");
            //WebPath = @"C:\MCDaemon\index.html";
            //OutputPath = @"C:\MCDaemon\output.html";
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                if (!file.Exists)
                {
                    file.Create();
                }
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);

            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        //private bool CanOpen(String fp)
        //{
        //    bool available = false; // We will skip this if we can't access the file
        //    FileInfo fi = new FileInfo(fp);
        //    int tries = 0;
        //    while (tries < 3 && !available) //try max 3 times 
        //    {
        //        if (IsFileLocked(fi))
        //        {
        //            tries++;
        //            Thread.Sleep(1000);
        //        }
        //        else
        //        {
        //            available = true;
        //        }
        //    }
        //    return available;
        //}

        public String ReadWeb()
        {
            return WebCache;
            //String Fail = @"<html><head><META HTTP-EQUIV=""refresh"" CONTENT=""5""></head></html>";
            //if (WebMutex.WaitOne(1000))
            //{
            //    if (WebFlag)
            //    {
            //        if (CanOpen(WebPath))
            //        {
            //            WebCache = File.ReadAllText(WebPath);
            //            WebFlag = false;
            //        }
            //        else
            //        {
            //            WebCache = Fail;
            //        }
            //    }
            //    // Release the Mutex.
            //    WebMutex.ReleaseMutex();
            //    return WebCache;

            //}
            //else
            //{
                
            //    return Fail;
            //}
            
        }

        public void WriteWeb(String data)
        {
            //Realized that simply storing the string is enough
            if (WebMutex.WaitOne(1000))
            {
                WebCache = data;
                // Release the Mutex.
                WebMutex.ReleaseMutex();
                return;
                ////old below
                //if (CanOpen(WebPath))
                //{
                //    File.WriteAllText(WebPath, data, Encoding.UTF8);
                //    WebFlag = true;
                //}

                //// Release the Mutex.
                //WebMutex.ReleaseMutex();

            }
            else
            {
                return; // simply skip writing
            }
        }

        public String ReadOutput()
        {
            return OutputCache;
            //String Fail = @"<html><head><META HTTP-EQUIV=""refresh"" CONTENT=""5""></head></html>";
            //if (OutputMutex.WaitOne(1000))
            //{
            //    String result;
            //    if (CanOpen(OutputPath))
            //    {
            //        result = File.ReadAllText(OutputPath);
            //    }
            //    else
            //    {
            //        result = Fail;
            //    }

            //    // Release the Mutex.
            //    OutputMutex.ReleaseMutex();
            //    return result;

            //}
            //else
            //{

            //    return Fail;
            //}

        }

        public void WriteOutput(String data)
        {
            if (OutputMutex.WaitOne(1000))
            {
                OutputCache += "<br>" + data;
                // Release the Mutex.
                OutputMutex.ReleaseMutex();
                return;
                ////old below
                //if (CanOpen(OutputPath))
                //{
                //    File.AppendAllText(OutputPath, "<br>" + data, Encoding.UTF8);
                //}

                //// Release the Mutex.
                //OutputMutex.ReleaseMutex();

            }
            else
            {
                return; // simply skip writing
            }
        }

        public void ClearOutput()
        {
            String data = @" <!DOCTYPE html>
<html>
<head>
<title>output</title>
<meta http-equiv=""Content-Type"" content=""text/html;charset=UTF-8"">
</head>
</html>
";
            if (OutputMutex.WaitOne(10000))
            {
                OutputCache = data;
                // Release the Mutex.
                OutputMutex.ReleaseMutex();
                return;
                //if (CanOpen(OutputPath))
                //{
                //    File.WriteAllText(OutputPath, data, Encoding.UTF8);
                //}



            }
            else
            {
                OutputCache = data;//fuck everything :D
                return; // simply skip writing
            }
        }

    }
}
