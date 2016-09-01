using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MCDaemon
{
    public partial class AboutBox : Form
    {
        private static AboutBox instance;
        public AboutBox()
        {
            InitializeComponent();
            textBox1.Text =
                AssemblyTitle + " is created by Elias Ringhauge ( www.ringhauge.dk) \u00a9 2016"+ Environment.NewLine + Environment.NewLine +
                "Icons made by Roundicons ( www.flaticon.com/authors/roundicons ) from www.flaticon.com" + Environment.NewLine + Environment.NewLine +
                "This software is licensed under the MIT License ( www.opensource.org/licenses/MIT ) " + Environment.NewLine + Environment.NewLine +
                "The use of the icons are limited by the Flaticon Basic License. Please consult the Flaticon_License.pdf for more info." + Environment.NewLine +
                "Redistributed and modified work must include the Flaticon_License.pdf unless all icons are replaced" + Environment.NewLine + Environment.NewLine +
                "Should you find the MIT License to be insufficient, you may contact me at eliasringhauge@hotmail.com to request a license change";
            
        }

        public static AboutBox Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AboutBox();
                }
                return instance;
            }
        }

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
    }
}
