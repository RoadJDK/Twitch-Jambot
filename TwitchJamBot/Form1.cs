using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitchJamBot
{
    public partial class InstaLocker : System.Windows.Forms.Form
    {
        /// <summary>
        /// Imports
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        #region imports

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int MYACTION_HOTKEY_ID = 1;

        #endregion

        private Random random = new Random();
        private bool running = false;

        private List<string> Jams;
        private decimal Delay;

        /// <summary>
        /// Initialize Form
        /// </summary>
        public InstaLocker()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID, 0, (int)Keys.F11);//Press F11 to get current mouse coordinates
        }

        /// <summary>
        /// Run the program
        /// </summary>
        private void Run()
        {
            var selected1 = Jams[random.Next(1, Jams.Count)];
            var selected2 = Jams[random.Next(1, Jams.Count)];
            for (var i = 0; i < random.Next(1,15); i++)
            {
                CopyPaste(selected1);
                CopyPaste(selected2);
            }
            SendKeys.Send("{Enter}");
        }

        /// <summary>
        /// Copy and paste a text
        /// </summary>
        /// <param name="text"></param>
        private void CopyPaste(string text)
        {
            Clipboard.SetText(text + " ");
            SendKeys.Send("^{v}");
        }

        /// <summary>
        /// MouseEvent
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }
            base.WndProc(ref m);

            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                if (running == false)
                {
                    running = true;
                    OnOff.BackColor = Color.FromArgb(0, 192, 0);
                    Jams = JAM.Text.Split(',').ToList();
                    Delay = DelayCount.Value;
                    TIMER.Interval = Convert.ToInt32(Delay*1000);///s to ms
                    TIMER.Start();
                }
                else
                {
                    running = false;
                    OnOff.BackColor = Color.FromArgb(192, 0, 0);
                    TIMER.Stop();
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TIMER_Tick(object sender, EventArgs e)
        {
            Run();
        }

        /// <summary>
        /// close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
