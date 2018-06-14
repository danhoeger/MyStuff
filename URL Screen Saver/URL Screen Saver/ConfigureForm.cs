using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace URL_Screen_Saver
{
    public partial class ConfigureForm : Form
    {
        #region Preview API's
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        #endregion

        public ConfigureForm()
        {
            InitializeComponent();
            textBox1.Enabled = !checkBox1.Checked;
        }

        public ConfigureForm(IntPtr ParentHandle)
        {
            InitializeComponent();
            textBox1.Enabled = !checkBox1.Checked;

            //set the preview window as the parent of this window
            SetParent(this.Handle, ParentHandle);

            //make this a child window, so when the select screensaver 
            //dialog closes, this will also close
            SetWindowLong(this.Handle, -16,
                  new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !checkBox1.Checked;
        }
    }
}
