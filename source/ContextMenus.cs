using System;
using System.Diagnostics;
using System.Windows.Forms;
using MCDaemon.Properties;
using System.Drawing;

namespace MCDaemon
{
	/// <summary>
	/// 
	/// </summary>
	class ContextMenus
	{
		/// <summary>
		/// Is the About box displayed?
		/// </summary>
		static bool isControlBoxLoaded = false;

		/// <summary>
		/// Creates this instance.
		/// </summary>
		/// <returns>ContextMenuStrip</returns>
		public ContextMenuStrip Create()
		{
			// Add the default menu options.
			ContextMenuStrip menu = new ContextMenuStrip();
			ToolStripMenuItem item;
			ToolStripSeparator sep;

            //// Windows Explorer.
            //item = new ToolStripMenuItem();
            //item.Text = "Explorer";
            //item.Click += new EventHandler(Explorer_Click);
            //item.Image = Resources.Explorer;
            //menu.Items.Add(item);

			// About.
			item = new ToolStripMenuItem();
			item.Text = "Config";
			item.Click += new EventHandler(Config_Click);
			item.Image = Resources.settings;
			menu.Items.Add(item);

			// Separator.
			sep = new ToolStripSeparator();
			menu.Items.Add(sep);

			// Exit.
			item = new ToolStripMenuItem();
			item.Text = "Exit";
			item.Click += new System.EventHandler(Exit_Click);
			item.Image = System.Drawing.SystemIcons.Error.ToBitmap();
			menu.Items.Add(item);

			return menu;
		}

        ///// <summary>
        ///// Handles the Click event of the Explorer control.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        //void Explorer_Click(object sender, EventArgs e)
        //{
        //    Process.Start("explorer", null);
        //}

		/// <summary>
		/// Handles the Click event of the Config control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Config_Click(object sender, EventArgs e)
		{
			if (!isControlBoxLoaded)
			{
				isControlBoxLoaded = true;
				ControlBox.Instance.ShowDialog();
				isControlBoxLoaded = false;
			}
		}

		/// <summary>
		/// Processes a menu item.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void Exit_Click(object sender, EventArgs e)
		{
			// Quit without further ado.
			Application.Exit();
		}
	}
}