

using System.Windows.Forms;
using System.Drawing;
using System;

using Guryflix.Structures;
using Guryflix.Utilities;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class AccountInfo : Form
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

        
        Panel label2, label3, label4, label5, label6, label8;
        string userName, accountName;
        int profileIndex = 1, numberOfVideos;
        bool isCollapsed = false;
        public AccountInfo(string userName, string accountName, int index)
        {
            this.userName = userName;
            this.accountName = accountName;
            this.profileIndex = index;
            InitializeComponent();
            initializeLabels();
            importInformation();
        }
        public AccountInfo(string userName, string accountName, int index, int numberOfVideos)
        {
            this.userName = userName;
            this.accountName = accountName;
            this.profileIndex = index;
            this.numberOfVideos = numberOfVideos;
            InitializeComponent();
            initializeLabels();
            importInformation();
        }
        void initializeLabels()
        {
            label2 = new Panel();
            label2.Width = 0;
            label2.Height = 5;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(homeBtn.Location.X, homeBtn.Location.Y + 35);
            this.Controls.Add(label2);
            label2.BringToFront();

            label3 = new Panel();
            label3.Width = 0;
            label3.Height = 5;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(searchBtn.Location.X, searchBtn.Location.Y + 30);
            this.Controls.Add(label3);
            label3.BringToFront();

            label4 = new Panel();
            label4.Width = 0;
            label4.Height = 5;
            label4.BackColor = Color.Transparent;
            label4.Location = new Point(historyBtn.Location.X, historyBtn.Location.Y + 30);
            this.Controls.Add(label4);
            label4.BringToFront();

            label5 = new Panel();
            label5.Location = new Point(profileBtn.Location.X, profileBtn.Location.Y + 30);
            label5.BackColor = Color.Chocolate;
            label5.Width = searchBtn.Width;
            label5.Height = 5;
            this.Controls.Add(label5);
            label5.BringToFront();

            label6 = new Panel();
            label6.Width = 0;
            label6.Height = 5;
            label6.BackColor = Color.Transparent;
            label6.Location = new Point(settingsBtn.Location.X, settingsBtn.Location.Y + 35);
            this.Controls.Add(label6);
            label6.BringToFront();

            label8 = new Panel();
            label8.Width = 0;
            label8.Height = 5;
            label8.BackColor = Color.Transparent;
            label8.Location = new Point(likedVideosBtn.Location.X, likedVideosBtn.Location.Y + 35);
            this.Controls.Add(label8);
            label8.BringToFront();
        }

        void importInformation()
        {
            string imageLocation = Environment.CurrentDirectory + @"\Data\Profiles\Profiles Icons\" + profileIndex + ".png";
            pictureBox1.ImageLocation = imageLocation;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            Console.WriteLine(imageLocation);
            nameLabel.Text = userName;
            accountLabel.Text = accountName;

            // Criar botão Painel de Administração dinamicamente apenas se o utilizador for Admin
            if (Guryflix.Data.DatabaseContext.IsAccountAdmin(accountName))
            {
                Button adminBtn = new Button();
                adminBtn.Text = "Painel de Administração";
                adminBtn.Location = new Point(424, 270);
                adminBtn.Size = new Size(220, 38);
                adminBtn.BackColor = Color.Chocolate;
                adminBtn.ForeColor = Color.White;
                adminBtn.FlatStyle = FlatStyle.Flat;
                adminBtn.FlatAppearance.BorderSize = 0;
                adminBtn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                adminBtn.Cursor = Cursors.Hand;
                adminBtn.Click += (s, ev) =>
                {
                    this.Hide();
                    AdminPanel f = new AdminPanel(accountName, userName, profileIndex);
                    f.Show();
                };
                this.Controls.Add(adminBtn);
            }
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Chocolate;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Transparent;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void homeBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainPage f = new MainPage(userName, accountName, profileIndex);
            f.Show();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            SearchBox f = new SearchBox(userName, accountName, profileIndex);
            f.Show();
        }

        private void historyBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            History f = new History(userName, accountName, profileIndex);
            f.Show();
        }

        private void profileBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            AccountInfo f = new AccountInfo(userName, accountName, profileIndex);
            f.Show();
        }

        private void homeBtn_MouseHover(object sender, EventArgs e)
        {
            label2.BackColor = Color.Chocolate;
            label2.Height = 5;
            label2.Width = homeBtn.Width;
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            if (!isCollapsed)
            {
                menuItem1.Width = settingsBtn.Width;
                menuItem1.Height = settingsBtn.Height;
                menuItem1.Text = "Sair";
                menuItem1.BackColor = ColorTranslator.FromHtml("#202020");
                label6.Width = settingsBtn.Width;
                label6.BackColor = ColorTranslator.FromHtml("#0066B4");
                isCollapsed = true;
            }
            else
            {
                menuItem1.Width = 0;
                menuItem1.Height = 0;
                menuItem1.Text = "";
                menuItem1.BackColor = Color.Transparent;
                label6.Width = 0;
                label6.BackColor = Color.Transparent;
                isCollapsed = false;
            }
        }

        private void settingsBtn_MouseLeave(object sender, EventArgs e)
        {
            if (!isCollapsed)
            {
                label6.Width = 0;
                label6.Height = 5;
                label6.BackColor = Color.Transparent;
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm f = new LoginForm();
            f.Show();
        }

        private void likedVideosBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            LikedVideos f = new LikedVideos(userName, accountName, profileIndex);
            f.Show();
        }

        private void likedVideosBtn_MouseHover(object sender, EventArgs e)
        {
            label8.BackColor = Color.Chocolate;
            label8.Height = 5;
            label8.Width = likedVideosBtn.Width;
        }

        private void likedVideosBtn_MouseLeave(object sender, EventArgs e)
        {
            label8.Width = 0;
            label8.Height = 5;
            label8.BackColor = Color.Transparent;
        }

        private void settingsBtn_MouseHover(object sender, EventArgs e)
        {
            if (!isCollapsed)
            {
                label6.BackColor = ColorTranslator.FromHtml("#0066B4");
                label6.Height = 5;
                label6.Width = settingsBtn.Width;
            }
        }


        private void homeBtn_MouseLeave(object sender, EventArgs e)
        {
            label2.Width = 0;
            label2.Height = 5;
            label2.BackColor = Color.Transparent;
        }


        private void searchBtn_MouseHover(object sender, EventArgs e)
        {
            label3.BackColor = Color.Chocolate;
            label3.Height = 5;
            label3.Width = searchBtn.Width;
        }

        private void searchBtn_MouseLeave(object sender, EventArgs e)
        {
            label3.Width = 0;
            label3.Height = 5;
            label3.BackColor = Color.Transparent;
        }

        private void historyBtn_MouseHover(object sender, EventArgs e)
        {
            label4.BackColor = Color.Chocolate;
            label4.Height = 5;
            label4.Width = historyBtn.Width;
        }

        private void historyBtn_MouseLeave(object sender, EventArgs e)
        {
            label4.Width = 0;
            label4.Height = 5;
            label4.BackColor = Color.Transparent;
        }

    }
}
