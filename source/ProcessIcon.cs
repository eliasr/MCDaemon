using System;
using System.Diagnostics;
using System.Windows.Forms;
using MCDaemon.Properties;
using Microsoft.Win32;

namespace MCDaemon
{
	/// <summary>
	/// 
	/// </summary>
	class ProcessIcon : IDisposable
	{
        private static ProcessIcon instance;   
        
		/// <summary>
		/// The NotifyIcon object.
		/// </summary>
		NotifyIcon ni;
        

		/// <summary>
		/// Initializes a new instance of the <see cref="ProcessIcon"/> class.
		/// </summary>
		private ProcessIcon()
		{
			// Instantiate the NotifyIcon object.
			ni = new NotifyIcon();
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;
            
		}

        public static ProcessIcon Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProcessIcon();
                }
                return instance;
            }
        }
		/// <summary>
		/// Displays the icon in the system tray.
		/// </summary>
		public void Display()
		{
			// Put the icon in the system tray and allow it react to mouse clicks.			
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			ni.Icon = Resources.modem_red;
			ni.Text = "MCDaemon";
			ni.Visible = true;

			// Attach a context menu.
			ni.ContextMenuStrip = new ContextMenus().Create();        
                        
		}


        public void UpdateIcon()
        {
            if (Network.isRunning)
            {
                
                //LightBlue: Deamon is available, but no server is in use
                //update notification text (optional)
                ni.Icon = Resources.modem_blue;
                ni.Text = "MCDaemon: Ready";
                if (Server.Instance.mcservers.Count > Server.Instance.GetActiveServer())
                {
                    if (Server.Instance.mcservers[Server.Instance.GetActiveServer()].isOnline)
                    {
                        //Green: A server is in use
                        //update notification text (optional)
                        ni.Icon = Resources.modem_green;
                        ni.Text = "MCDaemon: Active";
                    }
                }
                


            }
            else
            {
                //Red: Deamon is not running - maybe the TCP port was occupied
                //update notification text (optional)
                ni.Icon = Resources.modem_red;
                ni.Text = "MCDaemon: Error, check config";
            }

        }

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			// When the application closes, this will remove the icon from the system tray immediately.
			ni.Dispose();
		}

		/// <summary>
		/// Handles the MouseClick event of the ni control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		void ni_MouseClick(object sender, MouseEventArgs e)
		{
            //// Handle mouse button clicks.
            //if (e.Button == MouseButtons.Left)
            //{
            //    // Start Windows Explorer.
            //    Process.Start("explorer", null);
            //}
		}

        
	}


}