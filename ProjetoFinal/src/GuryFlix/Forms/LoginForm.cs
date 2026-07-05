using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Guryflix.Estruturas;
using Guryflix.Utilitarios;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class LoginForm : Form
    {
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

        ListaLigada palavrasPasse, contas;
        UtilitariosFicheiros utilitarios = new UtilitariosFicheiros();

        public LoginForm()
        {
            InitializeComponent();
            importarDadosLogin();
        }

        private void logInBtn_Click(object sender, EventArgs e)
        {
            iniciarSessao();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            limparCampos();
        }

        private void iniciarSessao()
        {
            string utilizadorID = userIDBox.Text;
            string palavraPasse = passwordBox.Text;

            if (verificarCredenciais(utilizadorID, palavraPasse))
            {
                this.Hide();
                ProfilesHandling f = new ProfilesHandling(utilizadorID);
                f.Show();
            }
        }

        private void limparCampos()
        {
            userIDBox.Text = "";
            passwordBox.Text = "";
        }

        private void setStatus(int statusNumber, bool setStatus, string message)
        {
            string imageLocation = Environment.CurrentDirectory + @"\Interface\IconesUI\";
            switch (statusNumber)
            {
                case 0:
                    if (setStatus)
                    {
                        statusID.Text = "";
                        statusSymbolID.ImageLocation = imageLocation + "Right.png";
                    }
                    else
                    {
                        statusID.ForeColor = Color.Red;
                        statusSymbolID.ImageLocation = imageLocation + "Wrong.png";
                        statusID.Text = message;
                    }
                    break;
                case 1:
                    if (setStatus)
                    {
                        statusPassword.Text = "";
                        statusSymbolPassword.ImageLocation = imageLocation + "Right.png";
                    }
                    else
                    {
                        statusPassword.ForeColor = Color.Red;
                        statusSymbolPassword.ImageLocation = imageLocation + "Wrong.png";
                        statusPassword.Text = message;
                    }
                    break;
            }
        }

        private void importarDadosLogin()
        {
            contas = new ListaLigada();
            palavrasPasse = new ListaLigada();
            
            try
            {
                string[] utilizadores = Guryflix.Data.DatabaseContext.GetAllAccounts();
                foreach (string user in utilizadores)
                {
                    contas.InserirInicio(user);
                }
            }
            catch { }
        }

        public bool verificarUtilizador(string utilizadorID)
        {
            if (contas.Procurar(utilizadorID))
            {
                setStatus(0, true, "");
                return true;
            }
            setStatus(0, false, "Utilizador não encontrado");
            return false;
        }

        private bool verificarPalavraPasse(string palavraPasse)
        {
            if (palavrasPasse.Procurar(palavraPasse))
            {
                setStatus(1, true, "");
                return true;
            }
            setStatus(1, false, "Palavra-passe incorreta");
            return false;
        }

        private bool verificarCredenciais(string utilizadorID, string palavraPasse)
        {
            if (!verificarUtilizador(utilizadorID))
                return false;

            if (Guryflix.Data.DatabaseContext.VerifyAccountPassword(utilizadorID, palavraPasse))
            {
                palavrasPasse.InserirInicio(palavraPasse);
                return verificarPalavraPasse(palavraPasse);
            }

            setStatus(1, false, "Palavra-passe incorreta");
            return false;
        }

        private void passwordBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                iniciarSessao();
            if (e.KeyChar == (char)Keys.Escape)
                limparCampos();
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
