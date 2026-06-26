using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;
using BCrypt.Net;

using Guryflix.Structures;
using Guryflix.Utilities;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class ProfilesHandling : Form
    {
        // ? Handles External User Interactions
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        static private int userCount = 0;
        private string currentAccount = "";
        private string[] profiles;
        private string[] passwords;
        const int size = 5;

        // ? Handles External User Interactions
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        public ProfilesHandling(string accountName)
        {
            InitializeComponent();
            currentAccount = accountName;
            profiles = new string[size];
            passwords = new string[size];
            storeProfiles();
        }
        private void storeProfiles()
        {
            userCount = 0;
            // Limpar vetores
            Array.Clear(profiles, 0, profiles.Length);
            Array.Clear(passwords, 0, passwords.Length);

            // Resetar labels e PictureBoxes
            ID1.ImageLocation = null; label1.Text = "";
            ID2.ImageLocation = null; label2.Text = "";
            ID3.ImageLocation = null; label3.Text = "";
            ID4.ImageLocation = null; label4.Text = "";
            ID5.ImageLocation = null; label5.Text = "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;port=3306;uid=root;pwd=;database=guryflix;"))
                {
                    conn.Open();
                    string sql = @"
                        SELECT p.nome_perfil, p.senha_hash 
                        FROM perfis p
                        JOIN contas c ON p.conta_id = c.id
                        WHERE c.nome_utilizador = @user LIMIT 5;";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", currentAccount);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = reader.GetString(0);
                                string passHash = reader.GetString(1);

                                string imageLocation = Environment.CurrentDirectory + @"\Data\Profiles\Profiles Icons\" + (userCount).ToString() + ".png";
                                profiles[userCount] = name;
                                passwords[userCount] = passHash;

                                switch (userCount)
                                {
                                    case 0:
                                        ID1.ImageLocation = imageLocation;
                                        ID1.SizeMode = PictureBoxSizeMode.Zoom;
                                        label1.Text = name;
                                        break;
                                    case 1:
                                        ID2.ImageLocation = imageLocation;
                                        ID2.SizeMode = PictureBoxSizeMode.Zoom;
                                        label2.Text = name;
                                        break;
                                    case 2:
                                        ID3.ImageLocation = imageLocation;
                                        ID3.SizeMode = PictureBoxSizeMode.Zoom;
                                        label3.Text = name;
                                        break;
                                    case 3:
                                        ID4.ImageLocation = imageLocation;
                                        ID4.SizeMode = PictureBoxSizeMode.Zoom;
                                        label4.Text = name;
                                        break;
                                    case 4:
                                        ID5.ImageLocation = imageLocation;
                                        ID5.SizeMode = PictureBoxSizeMode.Zoom;
                                        label5.Text = name;
                                        break;
                                }
                                userCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar perfis: " + ex.Message);
            }
        }
        private void addUserAndPassword()
        {
            string profileName = inputBox(false);
            if (string.IsNullOrEmpty(profileName)) return;
            string profilePassword = inputBox(true);
            if (string.IsNullOrEmpty(profilePassword)) return;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(profilePassword, 12);
            if (Guryflix.Data.DatabaseContext.CreateProfile(currentAccount, profileName, hashedPassword))
            {
                storeProfiles();
            }
            else
            {
                MessageBox.Show("Erro ao criar o perfil no banco de dados MySQL.");
            }
        }
        private string inputBox(bool choice) // ? Choice to select password or user name input
        {
            string input = "";
            ShowInputDialog(ref input, choice);
            if (choice)
                passwords[userCount] = input;
            else
                profiles[userCount] = input;
            return input;
        }
        private static DialogResult ShowInputDialog(ref string input, bool choice)
        {
            System.Drawing.Size size = new System.Drawing.Size(250, 100);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.MaximizeBox = false;
            inputBox.MinimizeBox = false;
            if (choice)
                inputBox.Text = "Palavra-passe";
            else
                inputBox.Text = "Nome";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 20, 23);
            textBox.Location = new System.Drawing.Point(10, 15);
            textBox.Text = input;
            textBox.BackColor = System.Drawing.Color.FromArgb(40, 45, 55);
            textBox.ForeColor = System.Drawing.Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            if (choice)
                textBox.UseSystemPasswordChar = true;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 28);
            okButton.Text = "OK";
            okButton.Location = new System.Drawing.Point(size.Width - 170, 50);
            okButton.BackColor = System.Drawing.Color.Chocolate;
            okButton.ForeColor = System.Drawing.Color.White;
            okButton.FlatStyle = FlatStyle.Flat;
            okButton.FlatAppearance.BorderSize = 0;
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 28);
            cancelButton.Text = "Cancelar";
            cancelButton.Location = new System.Drawing.Point(size.Width - 85, 50);
            cancelButton.BackColor = System.Drawing.Color.FromArgb(50, 55, 65);
            cancelButton.ForeColor = System.Drawing.Color.White;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.FlatAppearance.BorderSize = 0;
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (userCount == size)
                MessageBox.Show("Limite de perfis atingido!");
            else
                addUserAndPassword();
        }


        private void ID1_Click(object sender, EventArgs e)
        {
            checkForNextPage(0);
        }

        private void ID2_Click(object sender, EventArgs e)
        {
            checkForNextPage(1);
        }

        private void ID3_Click(object sender, EventArgs e)
        {
            checkForNextPage(2);
        }

        private void ID4_Click(object sender, EventArgs e)
        {
            checkForNextPage(3);
        }

        private void ID5_Click(object sender, EventArgs e)
        {
            checkForNextPage(4);
        }

        private void checkForNextPage(int index)
        {
            string pass = "";
            DialogResult result = ShowInputDialog(ref pass, true);
            if (result == DialogResult.Cancel)
                return;

            if (pass == null) pass = "";

            string storedHash = passwords[index];
            bool isCorrect = false;

            if (string.IsNullOrEmpty(storedHash))
            {
                isCorrect = string.IsNullOrEmpty(pass);
            }
            else
            {
                try
                {
                    isCorrect = BCrypt.Net.BCrypt.Verify(pass, storedHash);
                }
                catch
                {
                    isCorrect = (pass == storedHash);
                }
            }

            if (isCorrect)
            {
                this.Hide();
                UserPreferences f = new UserPreferences(profiles[index], currentAccount, index);
                f.checkIfPreferencesPresent();
            }
            else
                MessageBox.Show("Palavra-passe incorreta!");
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm f = new LoginForm();
            f.Show();
        }

    }
}
