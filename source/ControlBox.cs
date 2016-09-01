using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Drawing;

namespace MCDaemon
{
	partial class ControlBox : Form
	{
        string formTitle = "";
        public static void Swap(IList<GBServer> list, int indexA, int indexB)
        {
            GBServer tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
        /// <summary>
        /// A groupbox with multiple controls inside. The group represent a server entry.
        /// </summary>
        internal class GBServer 
        {
            public static int innerHeight = 20;
            public static int outerHeight = 20 + (GBServer.innerHeight * 5) + 40;
            int serverNumber;
            public GroupBox gbServer = new GroupBox();
            Label lblName = new Label();
            Label lblFilepath = new Label();
            TextBox tbName = new TextBox();
            TextBox tbFilepath = new TextBox();
            PictureBox pbMove = new PictureBox();
            PictureBox pbDelete = new PictureBox();
            CheckBox cbInput = new CheckBox();

            Point mouseDownLocation;

            public GBServer(int serverNum, string serverName, string serverFilePath , bool allowInput = false)
            {
                cbInput.Text = "Allow text input from user?";
                cbInput.AutoSize = true;
                lblName.AutoSize = true;
                lblFilepath.AutoSize = true;
                tbFilepath.Width = 210;
                tbName.Width = 210;
                pbDelete.Size = new Size(16, 16);
                pbDelete.Image = MCDaemon.Properties.Resources.remove;
                pbDelete.SizeMode = PictureBoxSizeMode.StretchImage;
                pbMove.Size = new Size(32, 32);
                pbMove.Image = MCDaemon.Properties.Resources.move;
                pbMove.SizeMode = PictureBoxSizeMode.StretchImage;
                MCDaemon.ControlBox.Instance.toolTip1.SetToolTip(pbDelete, "Click to remove server");
                MCDaemon.ControlBox.Instance.toolTip1.SetToolTip(pbMove, "Drag to rearrange server order");
                
                

                //modification and positions
                int gy = 20;
                int gx = 5;

                SetNumber(serverNum);
                pbMove.Location = new Point(gx, gbServer.Height / 2);
                gx += 32 + 5;
                gbServer.Controls.Add(pbMove);
                lblName.Text = "Name";
                lblName.Location = new Point(gx,gy);
                gbServer.Controls.Add(lblName);
                gy += innerHeight;
                tbName.Location = new Point(gx, gy);
                tbName.Text = serverName;
                gbServer.Controls.Add(tbName);


                gy += innerHeight;
                gy += 5;
                

                lblFilepath.Text = "FilePath";
                lblFilepath.Location = new Point(gx, gy);
                gbServer.Controls.Add(lblFilepath);
                gy += innerHeight;
                tbFilepath.Location = new Point(gx, gy);
                tbFilepath.Text = serverFilePath;
                gbServer.Controls.Add(tbFilepath);

                gy += innerHeight + 5;
                cbInput.Location = new Point(gx, gy);
                cbInput.Checked = allowInput;
                gbServer.Controls.Add(cbInput);
                gbServer.AutoSize = true;

                pbDelete.Location = new Point(gx+tbName.Width-pbDelete.Width, lblName.Location.Y);
                gbServer.Controls.Add(pbDelete);


                //set event handlers
                pbMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gbServer_MouseDown);
                pbMove.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gbServer_MouseMove);
                pbMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gbServer_MouseUp);
                gbServer.MouseDown  += new System.Windows.Forms.MouseEventHandler(this.gbServer_MouseDown);
                gbServer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gbServer_MouseMove);
                gbServer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gbServer_MouseUp);
                tbName.TextChanged += Config_Changed;
                tbFilepath.TextChanged += Config_Changed;
                pbDelete.Click += PbDelete_Click;
                cbInput.CheckedChanged += Config_Changed;

                // Add to server list

                // Extend ServerBox so there is space for it
                //SetLocationNum
            }

            public void SetNumber(int serverNum)
            {
                serverNumber = serverNum;
                gbServer.Text = "#" + (serverNum + 1).ToString();
            }
            //combines SetNumber and Location but defines the location by the number.
            public void SetNumberLocation(int serverNum)
            {
                SetNumber(serverNum);
                SetLocation(0, (serverNum * outerHeight) + MCDaemon.ControlBox.Instance.ServerBox.AutoScrollPosition.Y);
            }

            public void SetLocation(Point p)
            {
                gbServer.Location = p;
            }
            public void SetLocation(int x, int y)
            {
                SetLocation(new Point(x, y));
            }
            public int GetPosition()
            {
                return  Math.Min(Math.Max(0, (int)Math.Floor((double)(gbServer.Top + (gbServer.Height / 2) - MCDaemon.ControlBox.Instance.ServerBox.AutoScrollPosition.Y) / outerHeight)), MCDaemon.ControlBox.Instance.ListServerBox.Count - 1);
            }
            public xmlServer ToXmlServer()
            {
                return new xmlServer(this.tbName.Text, this.tbFilepath.Text,this.cbInput.Checked);
            }

            //events

            private void gbServer_MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    mouseDownLocation = e.Location;
                }
            }

            private void gbServer_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    //gbServer.Left = e.X + gbServer.Left - mouseDownLocation.X;
                    gbServer.Top = e.Y + gbServer.Top - mouseDownLocation.Y;
                    //pos is gbServer.Top /outer height, where Min and Max is to prevent out of bound error
                    int pos = GetPosition();
                    if (pos != serverNumber)
                    {

                        GBServer gs = MCDaemon.ControlBox.Instance.ListServerBox[pos];
                        Swap(MCDaemon.ControlBox.Instance.ListServerBox, pos, serverNumber);
                        gs.SetNumberLocation(serverNumber);
                        this.SetNumber(pos);
                        gbServer_change();
                        MCDaemon.ControlBox.Instance.Refresh();
                    }
                    
                }
            }

            private void gbServer_MouseUp(object sender, MouseEventArgs e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    int pos = GetPosition();
                    this.SetNumberLocation(pos);
                    MCDaemon.ControlBox.Instance.Refresh();
                }
            }

            public static void Config_Changed(object sender, EventArgs e)
            {
                gbServer_change();
            }

            private void PbDelete_Click(object sender, EventArgs e)
            {
                gbserver_remove();
                gbServer_change();
            }

            public static void gbServer_change()
            {
                //MCDaemon.ControlBox.instance.changed = true;
                MCDaemon.ControlBox.Instance.buttonApply.Enabled = true;
            }

            private void gbserver_remove()
            {
                if (MessageBox.Show("Remove server?", MCDaemon.ControlBox.Instance.formTitle,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int arrNum = this.serverNumber;
                    MCDaemon.ControlBox.Instance.ServerBox.Controls.Remove(this.gbServer);
                    MCDaemon.ControlBox.Instance.ListServerBox.RemoveAt(arrNum);
                    for (int i = arrNum; i < MCDaemon.ControlBox.Instance.ListServerBox.Count; i++)
                    {
                        MCDaemon.ControlBox.Instance.ListServerBox[i].SetNumberLocation(i);
                    }
                    MCDaemon.ControlBox.Instance.UpdateScrollHeight();
                    MCDaemon.ControlBox.Instance.Refresh();
                }
            }


        }
        //ControlBox

        private List<GBServer> ListServerBox = new List<GBServer>();
        private static ControlBox instance;
        private XMLConfig config;
        private bool isAboutBoxLoaded;

        //string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //bool changed = false;
        public void UpdateTxt()
        {
            //is this method relevant any longer?
            //Application.DoEvents();
            this.Refresh();
        }
		public ControlBox()
		{
            instance = this;
			InitializeComponent();
            formTitle = AssemblyTitle + " Configuration";
            //Add XML servers
            
            config = XmlHandler.Deserialize(); //not interfering with anything the server might use.

            for (int i = 0; i < config.ServerList.Count; i++)
            {
                // Read from config and fill in the provided info
                int outerHeight = GBServer.outerHeight;//20 + (GBServer.innerHeight * 4) + 40;
               
                int y = i * outerHeight; //outer y offset

                GBServer server = new GBServer(i, config.ServerList[i].Name, config.ServerList[i].FilePath, config.ServerList[i].AllowInput);
                server.SetLocation(0, y);
                ListServerBox.Add(server);
                ServerBox.Controls.Add(server.gbServer);
               
            }
            UpdateScrollHeight();

            this.Text = MCDaemon.ControlBox.Instance.formTitle;


            toolTip1.SetToolTip(this.buttonAdd, "Add server");
            toolTip1.SetToolTip(this.buttonApply, "Save config");

            TbHost.Text = config.Host;
            NumPort.Value = config.Port;

            TbHost.TextChanged += GBServer.Config_Changed;
            NumPort.ValueChanged += GBServer.Config_Changed;

        }

        public void UpdateScrollHeight()
        {
            ServerBox.AutoScrollMinSize = new Size(0, ListServerBox.Count * GBServer.outerHeight);
        }

        public static ControlBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ControlBox();
                }
                return instance;
            }
        }




        #region Assembly Attribute Accessors

        public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				//
                return "";
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}




        #endregion

        private void buttonApply_Click(object sender, EventArgs e)
        {
            buttonApply.Enabled = false;
            SaveXml();
        }

        private void SaveXml()
        {
            List<xmlServer> xserver = new List<xmlServer>();
            for (int i = 0; i < ListServerBox.Count; i++)
            {
                xserver.Add(ListServerBox[i].ToXmlServer());
            }

            XMLConfig tmp = new XMLConfig(xserver,TbHost.Text,(ushort)NumPort.Value);
            
            XmlHandler.Serialize(tmp);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            
            if (buttonApply.Enabled)
            {

                DialogResult result = MessageBox.Show("Do you want to save changes to your config?", formTitle,
                    MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Cancel:
                        this.DialogResult = DialogResult.None;
                        break;
                    case DialogResult.Yes:
                        SaveXml();
                        break;
                    default:
                        break;
                }

            }
        }

        private void ControlBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null; //Force reloading form and config from file
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            GBServer server = new GBServer(ListServerBox.Count, "","");
            server.SetNumberLocation(ListServerBox.Count);
            ListServerBox.Add(server);
            ServerBox.Controls.Add(server.gbServer);
            UpdateScrollHeight();
            GBServer.gbServer_change();
        }

        private void ControlBox_Load(object sender, EventArgs e)
        {
            if (Network.isRunning)
            {
                LabelCurPort.Text = "Currently running on port " + Server.Instance.Port.ToString();
            }
            else
            {
                LabelCurPort.Text = "Offline - is port " + Server.Instance.Port.ToString() + " in use?";
            }
        }

        private void button_about_Click(object sender, EventArgs e)
        {
            if (!isAboutBoxLoaded)
            {
                isAboutBoxLoaded = true;
                AboutBox.Instance.ShowDialog();
                isAboutBoxLoaded = false;
            }
        }
    }
}