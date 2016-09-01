using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MCDaemon
{


    /// <summary>
    /// The network class handles communications with clients and listens for new ones.
    /// The list of clients will be here
    /// </summary>
    class Network
    {
        //beskeder
private string strBadKey = @"<html>
<head><meta http-equiv=""Content-Type"" content=""text/html;charset=UTF-8""></head>
<body>Your sessions key has expired (someone sent a request before you).
<br>The page will be reloaded in 5 seconds</body></html>";

private string strError = @"<html>
<head><meta http-equiv=""Content-Type"" content=""text/html;charset=UTF-8""></head>
<body>Ups.<br>There was an error while processing your request.
<br>The page will be reloaded in 5 seconds</body></html>";

        class TcpUser
        {
            public TcpClient _tcpclient;
            public String Host;
            public TcpUser(TcpClient tcp)
            {
                _tcpclient = tcp;
                Host = Server.Instance.Host; //safty
            }
            public void SetUrl(String[] Lines)
            {
                String url;
                if (Lines[1].Contains("Host"))
                {
                    string[] host = Lines[1].Replace(" ", "").Split(':');
                    url = "http://" + host[1];
                    if (host.Length >= 3)
                    {
                        url += ":" + host[2];
                    }
                }
                else
                {
                    url = "http://ringhauge.com:81";  //should read from config file
                }
                Host = url;
            }
        }

        private List<TcpUser> connections;
        private static Network instance;
        public static bool isRunning = false;
        public static Network Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Network();
                }
                return instance;
            }
        }

        public Network()
        {
            connections = new List<TcpUser>();
            //server = Server.Instance; // lazy implementation that expect the server to be started already 
        }

        public int GetNumConnections()
        {
            return connections.Count;
        }

        public void StartTCPListener()
        {
            if (isRunning)
            {
                return;
            }
            ushort port = Server.Instance.Port;
            IPAddress localAddr = IPAddress.Parse("0.0.0.0");
            TcpListener tcpListener = null;
            try
            {
                tcpListener = new TcpListener(localAddr, port);
                tcpListener.Start();
                Server.Instance.Port = (ushort)((IPEndPoint)tcpListener.LocalEndpoint).Port; //get the port we are using.
                isRunning = true;
                ProcessIcon.Instance.UpdateIcon();
                while (true)
                {

                    try
                    {
                        // Use the Pending method to poll the underlying socket instance for client connection requests.

                        if (tcpListener.Pending())
                        {
                            //Accept the pending client connection and return a TcpClient object initialized for communication.
                            TcpClient tcpClient = tcpListener.AcceptTcpClient();
                            // Using the RemoteEndPoint property.
                            //Console.WriteLine("I am listening for connections on " +
                            //                            IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString()) +
                            //                               "on port number " + ((IPEndPoint)tcpListener.LocalEndpoint).Port.ToString());
                            TcpUser user = new TcpUser(tcpClient);
                            connections.Add(user);
                            Task.Factory.StartNew(() =>
                            {
                                ClientThread(user);
                            });
                            Server.Instance.UpdateHTML();
                        }

                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("Socket Exception: {0}", e);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Critical Error: Socket not started");
                Console.WriteLine("Socket Exception: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                tcpListener.Stop();
                isRunning = false;
                ProcessIcon.Instance.UpdateIcon();
            }
        }

        void ClientThread(TcpUser user)
        {
            TcpClient client = user._tcpclient; //less renaming
            bool goturl = false;
            // Buffer for reading data
            Byte[] bytes = new Byte[1024];
            String data = null;
            NetworkStream stream = client.GetStream();

            try
            {

                    client.ReceiveTimeout = 20000;
                    data = null;

                    // Get a stream object for reading and writing


                    int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    //Console.WriteLine("Received: {0}", data);

                    //check header for request type
                    String HttpRequestType = ""; ;
                    string[] Lines = data.Replace("\r\n", "\n").Split('\n');
                    int  Ret;
                    //Extract requested URL
                    if (Lines.Length > 0)
                    {
                        //Parse the Http Request Type
                        Ret = Lines[0].IndexOf(' ');
                        if (Ret > 0)
                        {
                            HttpRequestType = Lines[0].Substring(0, Ret);
                            Lines[0] = Lines[0].Substring(Ret).Trim();
                            if (!goturl)
                            {
                                user.SetUrl(Lines);
                            }
                        }

                        if (HttpRequestType.ToUpper().Equals("GET"))
                        {
                            int id = 0;
                            if (Lines[0].ToUpper().Contains("OUTPUT.HTML"))
                            {
                                id = 1;
                            }
                            SendWebpage(stream, id );
                        }
                        else if (HttpRequestType.ToUpper().Equals("POST"))
                        {

                                int index = data.IndexOf("\r\n\r\n");
                                String m_HttpPost = data.Substring(index + 4);
                                int status = Server.Instance.ParseCommand(m_HttpPost);

                                switch (status)
                                {
                                    case 0:
                                        SendRedirect(stream, user.Host);
                                        break;
                                    case -1:
                                        SendBad(stream, user.Host, strError);
                                        break;
                                    case -2:
                                        SendBad(stream, user.Host, strBadKey);
                                        break;
                                    default:
                                        break;
                                }
                                
                            
                        }
                    }
                }

                    // Shutdown and end connection

                
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine("IOException: {0}", e);
            }
            finally
            {
                bytes = null;
                data = null;
                stream.Flush();
                stream.Dispose();
                // Stop listening for new clients.
                connections.Remove(user);
                client.Close();
                //Console.WriteLine("#CLOSED CONNECTION#");
                Server.Instance.UpdateHTML();
                
            }
        }

        private void SendWebpage(NetworkStream stream, int id)
        {
            String page;
            byte[] msg;
            if (id == 1)
            {
                page = ResourceManager.Instance().ReadOutput();
               
            }
            else
            {
                page = ResourceManager.Instance().ReadWeb();
            }

            msg = System.Text.Encoding.UTF8.GetBytes(addHTTPHeader(page));
            stream.Write(msg, 0, msg.Length);
        }

        private string addHTTPHeader(string buffer)
        {
            string str = "";
            int contentLength = System.Text.Encoding.UTF8.GetByteCount(buffer);
            str += "HTTP/1.1\r\n";
            str += "Server: eftpos-application\r\n";
            str += "Content-Type: text/html; charset=utf-8\r\n";
            str += "Accept-Ranges: bytes\r\n";
            str += "Content-Length: " + contentLength.ToString() + "\r\n";
            str += "Connection: Keep-Alive\r\n";
            str += "Keep-Alive: timeout = 100, max = 10\r\n";
            str += "\r\n";
            //Console.WriteLine("---->Sent header: {0}\n", str);
            return str + buffer;
        }

        private void SendRedirect(NetworkStream stream , string url)
        {
            string str = "";
            str += "HTTP/1.1 200 ok\r\n";
            str += "Refresh: 0; url="+url+"\r\n";
            str += "Content-Type: text/html\r\n";
            str += "Content-Length: " + "0" + "\r\n";
            str += "Connection: Keep-Alive\r\n";
            str += "Keep-Alive: timeout = 100, max = 10\r\n";
            str += "\r\n";
            //Console.WriteLine("---->Sent refresh: {0}\n", str);

            byte[] msg;
            msg = System.Text.Encoding.UTF8.GetBytes(str);
            stream.Write(msg, 0, msg.Length);

        }

        private void SendBad(NetworkStream stream, string url, string text)
        {
            int contentLength = System.Text.Encoding.UTF8.GetByteCount(text);
            string str = "";
            str += "HTTP/1.1 200 ok\r\n";
            str += "Refresh: 5; url=" + url + "\r\n";
            str += "Content-Type: text/html; charset=utf-8\r\n";
            str += "Content-Length: " + contentLength.ToString() + "\r\n";
            str += "Connection: Keep-Alive\r\n";
            str += "Keep-Alive: timeout = 100, max = 10\r\n";
            str += "\r\n";
            str += text;
            //Console.WriteLine("---->Sent refresh: {0}\n", str);

            byte[] msg;
            msg = System.Text.Encoding.UTF8.GetBytes(str);
            stream.Write(msg, 0, msg.Length);

        }





    }
}
