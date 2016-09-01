using System;
using System.Windows.Forms;

namespace MCDaemon
{
	/// <summary>
	/// 
	/// </summary>
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Show the system tray icon.					
			using (ProcessIcon pi = ProcessIcon.Instance)
			{
				pi.Display();
                Server server = Server.Instance;      
                // Make sure the application runs!
                Application.Run();
			}
		}
	}
}