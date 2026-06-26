using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Guryflix.Structures;
using Guryflix.Utilities;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class LoginForm : Form
    {
        // ? Handles External User Interactions
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void LoginForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        LinkedList passwords, accounts;

        FileHandlingUtilites f = new FileHandlingUtilites();
        public LoginForm()
        {
            InitializeComponent();
            importPasswordsAndUsers();
        }

        private void logInBtn_Click(object sender, EventArgs e)
        {
            login();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            resetEntries();
        }

        // ? A Function That Acts As A Gate Keeper To Proceed Towards The Next Function
        private void login()
        {
            string ID = userIDBox.Text;
            string pass = passwordBox.Text;
            if (checkIDAndPassword(ID, pass))
            {
                this.Hide();
                ProfilesHandling f = new ProfilesHandling(ID);
                f.Show();
            }
        }
        private void resetEntries()
        {
            userIDBox.Text = "";
            passwordBox.Text = "";
        }

        // ? A Utility Funtion That Display The Input Is Right/Wrong
        private void setStatus(int statusNumber, bool setStatus, string message)
        {
            string imageLocation = Environment.CurrentDirectory + @"\Custom UI\UI Icons\";
            switch (statusNumber)
            {
                case 0:
                    if (setStatus)
                    {
                        statusID.Text = "";
                        statusSymbolID.ImageLocation = imageLocation + "Right" + ".png";
                    }
                    else
                    {
                        statusID.ForeColor = Color.Red;
                        statusSymbolID.ImageLocation = imageLocation + "Wrong" + ".png";
                        statusID.Text = message;
                    }
                    break;
                case 1:
                    if (setStatus)
                    {
                        statusPassword.Text = "";
                        statusSymbolPassword.ImageLocation = imageLocation + "Right" + ".png";
                    }
                    else
                    {
                        statusPassword.ForeColor = Color.Red;
                        statusSymbolPassword.ImageLocation = imageLocation + "Wrong" + ".png";
                        statusPassword.Text = message;
                    }
                    break;
            }
        }

        private void importPasswordsAndUsers()
        {
            accounts = new LinkedList();
            passwords = new LinkedList();
            
            try
            {
                string[] users = Guryflix.Data.DatabaseContext.GetAllAccounts();
                foreach (string user in users)
                {
                    accounts.Push(user);
                }
            }
            catch { }
        }

        public bool checkID(string ID)
        {
            if (accounts.Search(ID))
            {
                setStatus(0, true, "");
                return true;
            }
            setStatus(0, false, "Utilizador não encontrado");
            return false;
        }

        private bool checkPassword(string pass)
        {
            if (passwords.Search(pass))
            {
                setStatus(1, true, "");
                return true;
            }
            setStatus(1, false, "Palavra-passe incorreta");
            return false;
        }

        private bool checkIDAndPassword(string ID, string pass)
        {
            if (!checkID(ID))
                return false;

            if (Guryflix.Data.DatabaseContext.VerifyAccountPassword(ID, pass))
            {
                passwords.Push(pass);
                return checkPassword(pass);
            }

            setStatus(1, false, "Palavra-passe incorreta");
            return false;
        }

        // ? Consists Of ShortCut Keys In Password Input Box
        private void passwordBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                login();
            if (e.KeyChar == (char)Keys.Escape)
                resetEntries();
        }

        private void resetBtn_MouseHover(object sender, EventArgs e)
        {
            resetBtn.BackColor = Color.FromArgb(230, 125, 45);
        }

        private void resetBtn_MouseLeave(object sender, EventArgs e)
        {
            resetBtn.BackColor = Color.Chocolate;
        }

        private void logInBtn_MouseHover(object sender, EventArgs e)
        {
            logInBtn.BackColor = Color.FromArgb(230, 125, 45);
        }

        private void logInBtn_MouseLeave(object sender, EventArgs e)
        {
            logInBtn.BackColor = Color.Chocolate;
        }
        private void userIDBox_MouseClick(object sender, MouseEventArgs e)
        {
            resetTransition();
            label1.Hide();
            userIDBox.BorderStyle = BorderStyle.Fixed3D;
        }
        private void passwordBox_MouseClick(object sender, MouseEventArgs e)
        {
            resetTransition();
            label2.Hide();
            passwordBox.BorderStyle = BorderStyle.Fixed3D;
        }
        void resetTransition()
        {
            label1.Show();
            label2.Show();
            userIDBox.BorderStyle = BorderStyle.None;
            passwordBox.BorderStyle = BorderStyle.None;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StartPage f = new StartPage();
            f.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            closeBtn.BackColor = Color.Chocolate;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            closeBtn.BackColor = Color.Transparent;
        }

    }
}
