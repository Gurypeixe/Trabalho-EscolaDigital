using System;
using System.Windows.Forms;

using Guryflix.Structures;
using Guryflix.Utilities;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class IntroPage : Form
    {
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        int time = 0;
        public IntroPage()
        {
            InitializeComponent();
            startIntro();
        }
        
        void startIntro()
        {
            string vidLocation = Environment.CurrentDirectory + @"\Custom UI\GuryFlix Starting Animation.mp4";
            axWindowsMediaPlayer1.URL = vidLocation;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            timer1.Interval = 1000;
            timer1.Start();
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            double duration = 4;
            try
            {
                if (axWindowsMediaPlayer1.currentMedia != null && axWindowsMediaPlayer1.currentMedia.duration > 0)
                {
                    duration = axWindowsMediaPlayer1.currentMedia.duration;
                }
            }
            catch { }

            if (time >= (int)duration)
            {
                timer1.Stop();
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                this.Hide();
                StartPage f = new StartPage();
                f.Show();
            }
            time++;
        }

    }
}
