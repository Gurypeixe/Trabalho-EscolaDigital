using System;
using System.Windows.Forms;
using System.Drawing;

using Guryflix.Estruturas;
using Guryflix.Utilitarios;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class SignUP : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        ListaLigada listaContas = new ListaLigada();

        public SignUP()
        {
            InitializeComponent();
            importarContas();
        }

        void importarContas()
        {
            listaContas = new ListaLigada();
            try
            {
                string[] utilizadores = Guryflix.Dados.DatabaseContext.GetAllAccounts();
                foreach (string acc in utilizadores)
                {
                    listaContas.InserirInicio(acc);
                }
            }
            catch { }
        }

        private void limparCampos()
        {
            userIDBox.Text = "";
            passwordBox.Text = "";
            confirmPasswordBox.Text = "";
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            limparCampos();
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
                case 2:
                    if (setStatus)
                    {
                        statusConfirmPassword.Text = "";
                        statusSymbolConfirmPassword.ImageLocation = imageLocation + "Right.png";
                    }
                    else
                    {
                        statusConfirmPassword.ForeColor = Color.Red;
                        statusSymbolConfirmPassword.ImageLocation = imageLocation + "Wrong.png";
                        statusConfirmPassword.Text = message;
                    }
                    break;
            }
        }

        private bool verificarUtilizador(string utilizadorID)
        {
            int contagemLetras = 0, contagemNumeros = 0;
            if (listaContas.Procurar(utilizadorID))
            {
                setStatus(0, false, "Esta conta já existe!");
                return false;
            }
            for (int i = 0; i < utilizadorID.Length; i++)
            {
                if (utilizadorID[i] == ' ')
                {
                    setStatus(0, false, "Espaço detetado!");
                    return false;
                }
                if ((utilizadorID[i] >= 65 && utilizadorID[i] <= 90) || (utilizadorID[i] >= 97 && utilizadorID[i] <= 122))
                    contagemLetras++;
                if ((utilizadorID[i] >= 48 && utilizadorID[i] <= 57))
                    contagemNumeros++;
            }
            if (utilizadorID.Length <= 6)
            {
                setStatus(0, false, "Muito curto (mín. 7 carac.)!");
                return false;
            }
            if (contagemLetras <= 3)
            {
                setStatus(0, false, "Falta letras!");
                return false;
            }
            if (contagemNumeros <= 2)
            {
                setStatus(0, false, "Falta números!");
                return false;
            }
            return true;
        }

        private bool verificarPalavraPasse(string palavraPasse)
        {
            bool temMaiuscula = false, temMinuscula = false, temEspecial = false, temNumero = false;
            for (int i = 0; i < palavraPasse.Length; i++)
            {
                if (palavraPasse[i] == ' ')
                {
                    setStatus(1, false, "Espaço detetado!");
                    return false;
                }
                if (palavraPasse[i] >= 65 && palavraPasse[i] <= 90)
                    temMaiuscula = true;
                if (palavraPasse[i] >= 97 && palavraPasse[i] <= 122)
                    temMinuscula = true;
                if ((palavraPasse[i] >= 33 && palavraPasse[i] <= 47) || (palavraPasse[i] >= 58 && palavraPasse[i] <= 64))
                    temEspecial = true;
                if ((palavraPasse[i] >= 48 && palavraPasse[i] <= 57))
                    temNumero = true;
            }
            if (palavraPasse.Length <= 6)
            {
                setStatus(1, false, "Muito curto (mín. 7 carac.)!");
                return false;
            }
            if (!temMaiuscula)
            {
                setStatus(1, false, "Falta letra maiúscula!");
                return false;
            }
            if (!temMinuscula)
            {
                setStatus(1, false, "Falta letra minúscula!");
                return false;
            }
            if (!temEspecial)
            {
                setStatus(1, false, "Falta caractere especial!");
                return false;
            }
            if (!temNumero)
            {
                setStatus(1, false, "Falta números!");
                return false;
            }
            return true;
        }

        private void registarUtilizador()
        {
            string utilizadorID = userIDBox.Text;
            string palavraPasse = passwordBox.Text;
            string confirmarPalavraPasse = confirmPasswordBox.Text;

            if (verificarUtilizador(utilizadorID))
                setStatus(0, true, "");
            else
                return;

            if (verificarPalavraPasse(palavraPasse))
                setStatus(1, true, "");
            else
                return;

            if (palavraPasse == confirmarPalavraPasse)
            {
                setStatus(2, true, "");
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(palavraPasse, 12);
                if (Guryflix.Dados.DatabaseContext.CreateAccount(utilizadorID, hashedPassword))
                {
                    this.Hide();
                    LoginForm f = new LoginForm();
                    f.Show();
                }
                else
                {
                    MessageBox.Show("Erro ao criar a conta no banco de dados.");
                }
            }
            else
                setStatus(2, false, "As palavras-passe não coincidem!");
        }

        private void signUpBtn_Click(object sender, EventArgs e)
        {
            registarUtilizador();
        }

        private void resetBtn_MouseHover(object sender, EventArgs e)
        {
            resetBtn.BackColor = Color.FromArgb(230, 125, 45);
        }

        private void resetBtn_MouseLeave(object sender, EventArgs e)
        {
            resetBtn.BackColor = Color.Chocolate;
        }

        private void signUpBtn_MouseHover(object sender, EventArgs e)
        {
            signUpBtn.BackColor = Color.FromArgb(230, 125, 45);
        }

        private void signUpBtn_MouseLeave(object sender, EventArgs e)
        {
            signUpBtn.BackColor = Color.Chocolate;
        }

        private void confirmPasswordBox_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                registarUtilizador();
            if (e.KeyChar == (char)Keys.Escape)
                limparCampos();
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

        private void confirmPasswordBox_MouseClick(object sender, MouseEventArgs e)
        {
            resetTransition();
            label3.Hide();
            confirmPasswordBox.BorderStyle = BorderStyle.Fixed3D;
        }

        void resetTransition()
        {
            label1.Show();
            label2.Show();
            label3.Show();
            userIDBox.BorderStyle = BorderStyle.None;
            passwordBox.BorderStyle = BorderStyle.None;
            confirmPasswordBox.BorderStyle = BorderStyle.None;
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Close_MouseHover(object sender, EventArgs e)
        {
            Close.BackColor = Color.Chocolate;
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            Close.BackColor = Color.Transparent;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StartPage f = new StartPage();
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
    }
}
