namespace MCDaemon
{
	partial class ControlBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlBox));
            this.okButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ServerBoxGroup = new System.Windows.Forms.GroupBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.ServerBox = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelHost = new System.Windows.Forms.Label();
            this.TbHost = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LabelCurPort = new System.Windows.Forms.Label();
            this.NumPort = new System.Windows.Forms.NumericUpDown();
            this.labelPort = new System.Windows.Forms.Label();
            this.PanelButtons = new System.Windows.Forms.Panel();
            this.buttonApply = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_about = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.ServerBoxGroup.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPort)).BeginInit();
            this.PanelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(265, 15);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 99;
            this.okButton.TabStop = false;
            this.okButton.Text = "Close";
            this.okButton.UseMnemonic = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.81511F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.18489F));
            this.tableLayoutPanel.Controls.Add(this.ServerBoxGroup, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.PanelButtons, 1, 3);
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.641975F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.395061F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.83951F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(503, 447);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // ServerBoxGroup
            // 
            this.ServerBoxGroup.Controls.Add(this.buttonAdd);
            this.ServerBoxGroup.Controls.Add(this.ServerBox);
            this.ServerBoxGroup.Location = new System.Drawing.Point(157, 78);
            this.ServerBoxGroup.Name = "ServerBoxGroup";
            this.ServerBoxGroup.Size = new System.Drawing.Size(343, 319);
            this.ServerBoxGroup.TabIndex = 25;
            this.ServerBoxGroup.TabStop = false;
            this.ServerBoxGroup.Text = "Server List";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonAdd.Image = global::MCDaemon.Properties.Resources.add;
            this.buttonAdd.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonAdd.Location = new System.Drawing.Point(117, 295);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(108, 24);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.TabStop = false;
            this.buttonAdd.Text = "Add server";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // ServerBox
            // 
            this.ServerBox.AutoScroll = true;
            this.ServerBox.AutoScrollMinSize = new System.Drawing.Size(0, 300);
            this.ServerBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.ServerBox.Location = new System.Drawing.Point(3, 22);
            this.ServerBox.Name = "ServerBox";
            this.ServerBox.Size = new System.Drawing.Size(337, 268);
            this.ServerBox.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelHost);
            this.panel1.Controls.Add(this.TbHost);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(157, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 32);
            this.panel1.TabIndex = 26;
            // 
            // labelHost
            // 
            this.labelHost.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelHost.AutoSize = true;
            this.labelHost.Location = new System.Drawing.Point(6, 7);
            this.labelHost.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelHost.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelHost.Name = "labelHost";
            this.labelHost.Size = new System.Drawing.Size(41, 17);
            this.labelHost.TabIndex = 19;
            this.labelHost.Text = "Host:";
            this.labelHost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TbHost
            // 
            this.TbHost.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TbHost.Location = new System.Drawing.Point(60, 5);
            this.TbHost.Name = "TbHost";
            this.TbHost.Size = new System.Drawing.Size(210, 26);
            this.TbHost.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LabelCurPort);
            this.panel2.Controls.Add(this.NumPort);
            this.panel2.Controls.Add(this.labelPort);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(157, 41);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(343, 31);
            this.panel2.TabIndex = 27;
            // 
            // LabelCurPort
            // 
            this.LabelCurPort.AutoSize = true;
            this.LabelCurPort.Location = new System.Drawing.Point(137, 5);
            this.LabelCurPort.Name = "LabelCurPort";
            this.LabelCurPort.Size = new System.Drawing.Size(88, 19);
            this.LabelCurPort.TabIndex = 4;
            this.LabelCurPort.Text = "LabelCurPort";
            // 
            // NumPort
            // 
            this.NumPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NumPort.Location = new System.Drawing.Point(60, 2);
            this.NumPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumPort.Name = "NumPort";
            this.NumPort.Size = new System.Drawing.Size(71, 26);
            this.NumPort.TabIndex = 3;
            // 
            // labelPort
            // 
            this.labelPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(6, 5);
            this.labelPort.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelPort.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(37, 17);
            this.labelPort.TabIndex = 0;
            this.labelPort.Text = "Port:";
            this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelButtons
            // 
            this.PanelButtons.Controls.Add(this.buttonApply);
            this.PanelButtons.Controls.Add(this.okButton);
            this.PanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelButtons.Location = new System.Drawing.Point(157, 403);
            this.PanelButtons.Name = "PanelButtons";
            this.PanelButtons.Size = new System.Drawing.Size(343, 41);
            this.PanelButtons.TabIndex = 28;
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonApply.Enabled = false;
            this.buttonApply.Location = new System.Drawing.Point(0, 15);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(131, 23);
            this.buttonApply.TabIndex = 11;
            this.buttonApply.TabStop = false;
            this.buttonApply.Text = "Apply changes";
            this.buttonApply.UseMnemonic = false;
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(0, 1);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(150, 203);
            this.logoPictureBox.TabIndex = 12;
            this.logoPictureBox.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_about);
            this.panel3.Controls.Add(this.logoPictureBox);
            this.panel3.Location = new System.Drawing.Point(9, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(151, 447);
            this.panel3.TabIndex = 1;
            // 
            // button_about
            // 
            this.button_about.Location = new System.Drawing.Point(38, 419);
            this.button_about.Name = "button_about";
            this.button_about.Size = new System.Drawing.Size(75, 23);
            this.button_about.TabIndex = 13;
            this.button_about.Text = "About";
            this.button_about.UseVisualStyleBackColor = true;
            this.button_about.Click += new System.EventHandler(this.button_about_Click);
            // 
            // ControlBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 468);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControlBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ControlBox_FormClosing);
            this.Load += new System.EventHandler(this.ControlBox_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ServerBoxGroup.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPort)).EndInit();
            this.PanelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel PanelButtons;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox ServerBoxGroup;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Panel ServerBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelHost;
        private System.Windows.Forms.TextBox TbHost;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label LabelCurPort;
        private System.Windows.Forms.NumericUpDown NumPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_about;
    }
}
