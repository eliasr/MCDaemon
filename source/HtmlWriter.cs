using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace MCDaemon
{
    /// <summary>
    /// Is responsible for writing a new html file based on a template and filling data into fields.
    /// </summary>
    class HtmlWriter
    {
        enum Cmds
        {
            start,
            stop
        }
        private static Mutex mutex = new Mutex();
        private HtmlWriter()
        {
            WriteHTML();
        }
        //private Server server;
        private static HtmlWriter instance;
        public static HtmlWriter Instance()
        {
            if (instance == null)
            {
                instance = new HtmlWriter();
            }
            return instance;
        }
        private bool isbusy;
        public void WriteHTML(bool busy = false)
        {
            isbusy = busy;
            mutex.WaitOne();   // Wait until it is safe to enter.
            var watch = System.Diagnostics.Stopwatch.StartNew();
            String result = MakeAll();
            ResourceManager.Instance().WriteWeb(result);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("---->Webpage generated in " + elapsedMs.ToString() + " ms");
            mutex.ReleaseMutex();    // Release the Mutex.
        }

        String MakeAll()
        {
            StringBuilder sb = new StringBuilder();
            String tmp = "";
            sb.Append(
@"<!DOCTYPE html>
<html>
<head>
<title>MCDeamon WebClient</title>
<meta name = ""viewport"" content = ""width=device-width, initial-scale=1"">
<meta http-equiv=""refresh"" content=""10"">
<meta http-equiv=""Content-Type"" content=""text/html;charset=UTF-8"">
<link rel = ""stylesheet"" href = ""http://www.w3schools.com/lib/w3.css"">
<link rel = ""stylesheet"" href = ""http://www.w3schools.com/lib/w3-theme-black.css"">
<link rel = ""stylesheet"" href = ""https://fonts.googleapis.com/css?family=Roboto"">
<link rel = ""stylesheet"" href = ""http://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css"">
<style>
html,body,h1,h2,h3,h4,h5,h6 { font - family: ""Roboto"", sans - serif}
.w3 - sidenav a,.w3 - sidenav h4{ padding: 12px; }
.w3 - navbar a{ padding - top:12px !important; padding - bottom:12px !important; }
</style>
</head>
");

            sb.Append(MakeBody());
            //sb.Append(MakeFooter());
            sb.Append(MakeScript());
            sb.Append(@"</body>
</html>");
            tmp = sb.ToString();
            return tmp;
            
        }

        String MakeNav()
        {
            StringBuilder sb = new StringBuilder();
            String tmp = "";

            sb.Append(@"<!--Navbar-->"
);

            sb.Append(@"<ul class=""w3-navbar w3-theme w3-top w3-left-align w3-large"" style=""z-index:4;"">
");

            sb.Append(@"  <li><a href = ""#"" class=""w3-theme-l1"">MCWebClient</a></li>
");
            sb.Append(@"  <li><a href=""#"" class=""w3-theme-l1"">");
            //number of clients
            sb.Append(Server.Instance.ActiveClientsMsg());
            sb.Append(@"</a></li>
");
            sb.Append(@"</ul>


");

            tmp = sb.ToString();
        return tmp;
        }

        String MakeBody()
        {
            StringBuilder sb = new StringBuilder();
            String tmp = "";
            sb.Append(@"<body>

");
            sb.Append(MakeNav());

            sb.Append(@"<!--Main content: shift it to the right by 250 pixels when the sidenav is visible -->
<div class=""w3-main"" style=""margin-left:250px"">

<div class=""w3-row w3-padding-64"" >
<div class=""w3-twothird w3-container"">
<h1 class=""w3-text-teal"">Welcome</h1>" +
/*
<p>Til højre ser du alle tilgændelige serverer, deres navne, statuser og mulige handlinger.
</p>
<p>
Bemærk at der kan kun kører EN server ad gangen! Starter du en inaktiv server, slukkes den aktive server automatisk.Du skal forvente at en server start tager ca. 5 minutter.
</p>
<p>
Siden her opdatere sig selv hver 10. sekundt, det kan du selv gøre ved at trykke F5.
Vær tålmodig og giv serveren en chance for at skrive den nye side inden du spammer handlingsknapperne.
</p>
*/
@"
<p>
On the right side you can see all the available services, their names, status and possible options.
</p>
<p>
Notice that only ONE service is active at any time! If you start an inactive service, the active service will automaticly be closed. Depending on the service you might experience wait time.
</p>
<p>
The page will automaticly update every 10 second, you can manually update the page by pressing F5.
Remain patient if nothing seems to happen, bashing F5 will not increase the speed.
</p>
</div>
");

            sb.Append(MakeServers());
            sb.Append(@"</div>
");
            sb.Append(MakeOutput());


            sb.Append(@"<!-- END MAIN -->
</div>
");
            
            sb.Append(MakeFooter());

            tmp = sb.ToString();
            return tmp;
        }

        String MakeServers()
        {
            StringBuilder sb = new StringBuilder();
            String tmp = "";
            sb.Append(@"<div class=""w3-third w3-container"">
");         
            if(Server.Instance.mcservers.Count > 0)
            { 
            //Hver server ramme
            for (int i = 0; i < Server.Instance.mcservers.Count; i++)
            {

                //Set form tag
                sb.Append(@"<form action = """);
                sb.Append(@""" method=""post"">
");

                sb.Append(@"<p class=""w3-border w3-padding-large w3-padding-32 w3-center"">
");
                //add content
                sb.Append(Server.Instance.mcservers[i].ServerName);
                sb.Append(@"
<br>
");
                if (Server.Instance.mcservers[i].isOnline)
                {
                    sb.Append(@"<span style=""background-color:green; color:white; font-weight:bold"">Online</span>");
                }
                else
                {
                    sb.Append(@"<span style=""background-color:red; color:white; font-weight:bold"">Offline</span>");
                }
                sb.Append(@"
<br>
");
                //input to server
                if (Server.Instance.mcservers[i].isOnline)
                {
                    sb.Append(MakeCmd(i, Cmds.stop));
                }
                else
                {
                    sb.Append(MakeCmd(i, Cmds.start));
                }

                //control buttons
                if (Server.Instance.mcservers[i].isOnline)
                {
                    sb.Append(MakeButton("Stop server")); //read text from config
                }
                else
                {
                    sb.Append(MakeButton("Start server")); //read text from config
                }
                sb.Append(@"</p>
");
                //Luk form
                sb.Append(@"</form>
");
            };
            }
            else
	        {
                sb.Append(@"No service has been added, yet.");
            }
            //Luk div
            sb.Append(@"
</div>
");

            tmp = sb.ToString();
            return tmp;

        }

        String MakeOutput()
        {
            StringBuilder sb = new StringBuilder();
            String tmp = "";
            sb.Append(@"
  <div class=""w3-row w3-padding-64"">
    <div class=""w3-twothird w3-container"">
<iframe id=""outbox"" src = ""http://" + Server.Instance.Host + ":" + Server.Instance.Port + @"/output.html"" width=""150%"" height=""150%"">
   <p>Your browser does not support iframes.</p>
</iframe>");

if (Server.Instance.mcservers.Count > Server.Instance.GetActiveServer() 
                && Server.Instance.mcservers[Server.Instance.GetActiveServer()].AllowInput 
                && Server.Instance.mcservers[Server.Instance.GetActiveServer()].isOnline)
        {
                sb.Append(@"Input:<br>" + MakeInputField());
        }
sb.Append(@"
    </div>
  </div>
");
            tmp = sb.ToString();
            return tmp;
        }

        String MakeFooter()
        {
            StringBuilder sb = new StringBuilder();
            String tmp = "";
            sb.Append(@"<footer id = ""myFooter"">
 <div class=""w3-container w3-theme-l2"">
  <p><a href=""https://github.com/eliasr/MCDaemon"" target=""_blank"">MCDaemon</a> by <a href=""http://www.ringhauge.dk"" target=""_blank"">Elias Ringhauge</a> &copy; 2016 under the <a href=""http://choosealicense.com/licenses/mit/"" target=""_blank"">MIT License</a>.  </p>
 </div>
");
            sb.Append(@"<div class=""w3-container w3-theme-l1"">
  <p>Powered by <a href= ""http://www.w3schools.com/w3css/default.asp"" target=""_blank"">w3.css</a></p>
 </div>
</footer>
");


            tmp = sb.ToString();
            return tmp;

        }
        //This method makes the input tag for the form
        String MakeButton ( string buttonName)
        {
            StringBuilder sb = new StringBuilder();
            String tmp="";

            if (isbusy)
            {
                buttonName = "Busy";
            }

            sb.Append(@"<input type=""submit"" value=""");
            sb.Append(buttonName);
            sb.Append(@"""");
            if (isbusy)
            {
                sb.Append(@" disabled=""disabled""");
            }
            sb.Append(@">
");
            tmp = sb.ToString();
            return tmp;
        }

        String MakeCmd(int id, Cmds cmd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<input type=""hidden"" name=""id"" value=""" + id.ToString() + @""">
");
            sb.Append(@"<input type=""hidden"" name=""cmd"" value="""+ ((int)cmd).ToString() +  @""" >
");
            sb.Append(@"<input type=""hidden"" name=""key"" value=""" + Server.Instance.GetKey() + @""">
");
            String tmp = sb.ToString();// " /?id=" + id.ToString() + "&" + "cmd=" + cmd.ToString();
            return tmp;
        }

        String MakeInputCmd()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"<input type=""text"" name=""str"" maxlength=""200"" >
");
            sb.Append(@"<input type=""hidden"" name=""key"" value=""" + Server.Instance.GetKey() + @""">
");
            String tmp = sb.ToString();// " /?id=" + id.ToString() + "&" + "cmd=" + cmd.ToString();
            return tmp;
        }

        String MakeInputField()
        {
            StringBuilder sb = new StringBuilder();
            //Set form tag
            sb.Append(@"<form action = """);
            sb.Append(@""" method=""post"">
");
            //input to server
            sb.Append(MakeInputCmd());

            //control buttons
            sb.Append(MakeButton("Send command"));

            //            sb.Append(@"</p>
            //");
            //Luk form
            sb.Append(@"</form>
");
            String tmp = sb.ToString();
            return tmp;
        }

        String MakeScript()
        {
            StringBuilder sb = new StringBuilder();
            String tmp = "";
            sb.Append(@"<script>
// Get the Sidenav
var mySidenav = document.getElementById(""mySidenav"");

// Get the DIV with overlay effect
var overlayBg = document.getElementById(""myOverlay"");

// Toggle between showing and hiding the sidenav, and add overlay effect
                        function w3_open() {
                            if (mySidenav.style.display === 'block')
                            {
                                mySidenav.style.display = 'none';
                                overlayBg.style.display = ""none"";
                            }
                            else {
                                mySidenav.style.display = 'block';
                                overlayBg.style.display = ""block"";
                            }
                        }

                        // Close the sidenav with the close button
                        function w3_close() {
                            mySidenav.style.display = ""none"";
                            overlayBg.style.display = ""none"";
                        }
</script>
");
            tmp = sb.ToString();
            tmp += @"<script>
var myIframe = document.getElementById('outbox');
myIframe.onload = function(){
    var y = myIframe.contentDocument.body.scrollHeight;
    myIframe.contentWindow.scrollTo(0,y);
};
</script>";
            tmp += @"<script>
window.onunload = function(){
};
</script>";
            return tmp;
        }


    }
}
