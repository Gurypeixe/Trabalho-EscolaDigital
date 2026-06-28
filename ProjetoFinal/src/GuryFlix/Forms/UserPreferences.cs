using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

using Guryflix.Structures;
using Guryflix.Utilities;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class UserPreferences : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        string currentProfile, currentAccount;
        static int count = 0;
        const int totalOptions = 7, preferenceLimit = 4;
        int profileIndex = 0;
        string[] selectedLabels;
        public UserPreferences(string user, string account, int index)
        {
            InitializeComponent();
            this.currentProfile = user;
            this.currentAccount = account;
            this.profileIndex = index;
            selectedLabels = new string[totalOptions];
            for (int i = 0; i < totalOptions; i++)
                selectedLabels[i] = "";
        }
        public void checkIfPreferencesPresent()
        {
            string[] prefs = Guryflix.Data.DatabaseContext.GetProfilePreferences(currentAccount, currentProfile);
            if (prefs.Length > 0)
            {
                this.Hide();
                MainPage f = new MainPage(currentProfile, currentAccount, profileIndex);
                f.Show();
            }
            else this.Show();
        }
        private void makeLog(string labelName, int i)
        {
            if (count == preferenceLimit)
            {
                if (isLabelStored(labelName, i))
                    return;
                MessageBox.Show("Ups! Atingiu o limite de seleção!");
                return;
            }
            if (isLabelStored(labelName, i))
                return;
            if (selectedLabels[i] == "")
            {
                setIDImage(i, true);
                selectedLabels[i] = labelName;
                count++;
            }
        }
        
        private void setIDImage(int index, bool type)
        {
            string imageLocation = Environment.CurrentDirectory + @"\Data\Movie Titles\Genre Icons\";
            switch (index)
            {
                case 0:
                    if (type == false)
                        ID0.ImageLocation = (imageLocation + "Ação.png");
                    else
                        ID0.ImageLocation = (imageLocation + "Selected_Ação.png");
                    ID0.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                case 1:
                    if (type == false)
                        ID1.ImageLocation = (imageLocation + "Infantil.png");
                    else
                        ID1.ImageLocation = (imageLocation + "Selected_Infantil.png");
                    ID1.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                case 2:
                    if (type == false)
                        ID2.ImageLocation = (imageLocation + "Mistério.png");
                    else
                        ID2.ImageLocation = (imageLocation + "Selected_Mistério.png");
                    ID2.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                case 3:
                    if (type == false)
                        ID3.ImageLocation = (imageLocation + "Drama.png");
                    else
                        ID3.ImageLocation = (imageLocation + "Selected_Drama.png");
                    ID3.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                case 4:
                    if (type == false)
                        ID4.ImageLocation = (imageLocation + "Comédia.png");
                    else
                        ID4.ImageLocation = (imageLocation + "Selected_Comédia.png");
                    ID4.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                case 5:
                    if (type == false)
                        ID5.ImageLocation = (imageLocation + "Terror.png");
                    else
                        ID5.ImageLocation = (imageLocation + "Selected_Terror.png");
                    ID5.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
                case 6:
                    if (type == false)
                        ID6.ImageLocation = (imageLocation + "Romance.png");
                    else
                        ID6.ImageLocation = (imageLocation + "Selected_Romance.png");
                    ID6.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
            }
        }
        private bool isLabelStored(string labelName, int i)
        {
            if (labelName == selectedLabels[i])
            {
                setIDImage(i, false);
                selectedLabels[i] = "";
                count--;
                return true;
            }
            return false;
        }
        private void storeLog()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < totalOptions; i++)
                if (selectedLabels[i] != "")
                    list.Add(selectedLabels[i]);
            Guryflix.Data.DatabaseContext.SaveProfilePreferences(currentAccount, currentProfile, list.ToArray());
        }
        private void nextBtn_Click(object sender, EventArgs e)
        {
            if(count <= 2)
            {
                MessageBox.Show("Por favor, selecione pelo menos 3 preferências!");
                return;
            }
            storeLog();
            this.Hide();
            MainPage f = new MainPage(currentProfile, currentAccount, profileIndex);
            f.Show();
        }
        private void ID0_Click(object sender, EventArgs e)
        {
            makeLog(label0.Text, 0);
        }

        private void ID1_Click(object sender, EventArgs e)
        {
            makeLog(label1.Text, 1);
        }

        private void ID2_Click(object sender, EventArgs e)
        {
            makeLog(label2.Text, 2);
        }

        private void ID3_Click(object sender, EventArgs e)
        {
            makeLog(label3.Text, 3);
        }

        private void ID4_Click(object sender, EventArgs e)
        {
            makeLog(label4.Text, 4);
        }
        private void ID5_Click(object sender, EventArgs e)
        {
            makeLog(label5.Text, 5);
        }

        private void ID6_Click(object sender, EventArgs e)
        {
            makeLog(label6.Text, 6);
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProfilesHandling f = new ProfilesHandling(currentAccount);
            f.Show();
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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

    }
}
