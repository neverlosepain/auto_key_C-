using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Key_Presser
{
    public partial class MainFrm : Form
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private KeyHandler ghk;

        private int Interval = 10000;

        public MainFrm()
        {
            InitializeComponent();
            ghk = new KeyHandler(Constants.CTRL, Keys.O, this);
            ghk.Register();
            KeyTxtBox.Enabled = false;
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(Interval);

            timer1.Start();
            button1.Enabled = false;
            button2.Enabled = true;

            Process[] notepads = Process.GetProcessesByName("notepad");

            if (notepads.Length > 0)
            {
                // Notepad is already running, bring it to the foreground
                IntPtr hWnd = notepads[0].MainWindowHandle;
                SetForegroundWindow(hWnd);
            }
            else
            {
                // Notepad is not running, start a new instance
                Process.Start("notepad.exe");
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] notepads = Process.GetProcessesByName("notepad");

            if (notepads.Length > 0)
            {
                // Notepad is already running, bring it to the foreground
                IntPtr hWnd = notepads[0].MainWindowHandle;
                SetForegroundWindow(hWnd);
            }
            SendKeys.Send(KeyTxtBox.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void HandleHotkey()
        {
            if (button1.Enabled)
            {
                button1_Click(this, null);
            }
            else
            {
                button2_Click(this, null);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            KeyTxtBox.Enabled = checkBox1.Checked;  
        }

        private void KeyTxtBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
