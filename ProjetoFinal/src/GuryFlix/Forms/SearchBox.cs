using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using Guryflix.Estruturas;
using Guryflix.Utilitarios;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class SearchBox : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        Panel label2, label3, label4, label5, label6, label7;
        bool estaRecolhido = false, estaMaximizado = false;
        PesquisaHashing pesquisa;
        string[] resultados;
        string novoNomeImagem = "", nomeUtilizador, nomeConta;
        int contador = 0, indicePerfil;
        public SearchBox(string nomeUtilizador, string nomeConta, int index)
        {
            InitializeComponent();
            initializeLabels();
            string[] movieNames = Guryflix.Data.DatabaseContext.GetAllMovies();
            pesquisa = new PesquisaHashing(movieNames);
            resultados = new string[pesquisa.tamanho];
            this.nomeUtilizador = nomeUtilizador; this.nomeConta = nomeConta;
            this.indicePerfil = index;
        }


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            string selected = listView1.SelectedItems[0].Text;
            if (!VideoPlayer.IsMovieAvailable(selected))
                return;
            Guryflix.Data.DatabaseContext.AddMovieToHistory(nomeConta, nomeUtilizador, selected);
            this.Hide();
            VideoPlayer j = new VideoPlayer(nomeUtilizador, nomeConta, selected, indicePerfil);
            j.Show();
        }

        private void preencherLista()
        {
            int i = 0;
            string imageLocation = Environment.CurrentDirectory + @"\Dados\Filmes\Icones\";
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(150, 100);
            listView1.SmallImageList = imgs; // Setting Size Of Images
            string[] paths = { };
            paths = Directory.GetFiles(imageLocation);
            try
            {
                foreach (String path in paths)
                {
                    for (int l = 0; l < pesquisa.tamanho; l++)
                    {
                        if (path == imageLocation + pesquisa.stringFinal[l] + ".png")
                        {
                            imgs.Images.Add(Image.FromFile(path));
                            resultados[i++] = pesquisa.stringFinal[l];
                            pesquisa.stringFinal[l] = "NULL";
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Imagem não encontrada!");
            }

            for (int j = 0; j < imgs.Images.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = j;
                item.SubItems.Add(resultados[j]);
                item.Text = resultados[j];
                listView1.Items.Add(item);

            }
        }

        private void fill(string termoPesquisa)
        {
            listView1.Items.Clear();
            pesquisa.Pesquisar(termoPesquisa);
            preencherLista();
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;
            novoNomeImagem = Convert.ToString(e.Label);
            ListViewItem item1 = listView1.SelectedItems[0];
            FileInfo fileInfo = new FileInfo(item1.Tag.ToString());
            fileInfo.MoveTo(fileInfo.Directory.FullName + "\\" + novoNomeImagem + fileInfo.Extension);
            listView1.Items[contador].Text = novoNomeImagem;
        }

        private void SearchBox_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Add("Miniaturas", 150);
            listView1.Columns.Add("Títulos", 300);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                fill(textBox1.Text);
                resetTransition();
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                listView1.Items.Clear();
                textBox1.Text = "";
            }
        }
        void initializeLabels()
        {
            label3 = new Panel();
            label3.Location = new Point(searchBtn.Location.X, searchBtn.Location.Y + 30);
            this.Controls.Add(label3);
            label3.BringToFront();
            label3.BackColor = Color.Chocolate;
            label3.Width = 0;
            label3.Height = 5;
            while (label3.Width != searchBtn.Width)
                label3.Width += 1;
            
            label2 = new Panel();
            label2.Width = 0;
            label2.Height = 5;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(homeBtn.Location.X, homeBtn.Location.Y + 35);
            this.Controls.Add(label2);
            label2.BringToFront();

            label4 = new Panel();
            label4.Width = 0;
            label4.Height = 5;
            label4.BackColor = Color.Transparent;
            label4.Location = new Point(historyBtn.Location.X, historyBtn.Location.Y + 30);
            this.Controls.Add(label4);
            label4.BringToFront();

            label5 = new Panel();
            label5.Width = 0;
            label5.Height = 5;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(profileBtn.Location.X, profileBtn.Location.Y + 30);
            this.Controls.Add(label5);
            label5.BringToFront();

            label6 = new Panel();
            label6.Width = 0;
            label6.Height = 5;
            label6.BackColor = Color.Transparent;
            label6.Location = new Point(settingsBtn.Location.X, settingsBtn.Location.Y + 35);
            this.Controls.Add(label6);
            label6.BringToFront();

            label7 = new Panel();
            label7.Width = 0;
            label7.Height = 5;
            label7.BackColor = Color.Transparent;
            label7.Location = new Point(likedVideosBtn.Location.X, likedVideosBtn.Location.Y + 35);
            this.Controls.Add(label7);
            label7.BringToFront();
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            IntroPage f = new IntroPage();
            f.Show();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            SearchBox f = new SearchBox(nomeUtilizador, nomeConta, indicePerfil);
            f.Show();
        }


        private void settingsBtn_Click(object sender, EventArgs e)
        {
            if (!estaRecolhido)
            {
                menuItem1.Width = settingsBtn.Width;
                menuItem1.Height = settingsBtn.Height;
                menuItem1.Text = "Sair";
                menuItem1.BackColor = ColorTranslator.FromHtml("#202020");
                label6.Width = settingsBtn.Width;
                label6.Height = 5;
                label6.BackColor = ColorTranslator.FromHtml("#0066B4");
                estaRecolhido = true;
            }
            else
            {
                menuItem1.Width = 0;
                menuItem1.Height = 0;
                menuItem1.Text = "";
                menuItem1.BackColor = Color.Transparent;
                label6.Width = 0;
                label6.BackColor = Color.Transparent;
                estaRecolhido = false;
            }
        }

        private void settingsBtn_MouseLeave(object sender, EventArgs e)
        {
            if (!estaRecolhido)
            {
                label6.Width = 0;
                label6.Height = 5;
                label6.BackColor = Color.Transparent;
            }
        }

        private void likedVideosBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            LikedVideos f = new LikedVideos(nomeUtilizador, nomeConta, indicePerfil);
            f.Show();
        }

        private void likedVideosBtn_MouseHover(object sender, EventArgs e)
        {
            label7.BackColor = Color.Chocolate;
            label7.Height = 5;
            label7.Width = likedVideosBtn.Width;
        }

        private void likedVideosBtn_MouseLeave(object sender, EventArgs e)
        {
            label7.Width = 0;
            label7.Height = 5;
            label7.BackColor = Color.Transparent;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm f = new LoginForm();
            f.Show();
        }

        private void settingsBtn_MouseHover(object sender, EventArgs e)
        {
            if (!estaRecolhido)
            {
                label6.BackColor = ColorTranslator.FromHtml("#0066B4");
                label6.Height = 5;
                label6.Width = settingsBtn.Width;
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            resetTransition();
            label1.Hide();
            textBox1.BorderStyle = BorderStyle.Fixed3D;
            textBox1.Text = "";
        }
        void resetTransition()
        {
            label1.Show();
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Text = " Pesquisar";
        }

        private void homeBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainPage f = new MainPage(nomeUtilizador, nomeConta, indicePerfil);
            f.Show();
        }

        private void homeBtn_MouseHover(object sender, EventArgs e)
        {
            label2.BackColor = Color.Chocolate;
            label2.Height = 5;
            label2.Width = homeBtn.Width;
        }

        private void homeBtn_MouseLeave(object sender, EventArgs e)
        {
            label2.Width = 0;
            label2.Height = 5;
            label2.BackColor = Color.Transparent;
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void maximizeBtn_Click(object sender, EventArgs e)
        {
            if (!estaMaximizado)
            {
                this.WindowState = FormWindowState.Maximized;
                estaMaximizado = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                estaMaximizado = false;
            }
        }

        private void minimizebtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Close_MouseHover(object sender, EventArgs e)
        {
            Close.BackColor = Color.Chocolate;
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            Close.BackColor = Color.Transparent;
        }

        private void Maximize_MouseHover(object sender, EventArgs e)
        {
            Maximize.BackColor = Color.DodgerBlue;
        }

        private void Maximize_MouseLeave(object sender, EventArgs e)
        {
            Maximize.BackColor = Color.Transparent;
        }

        private void Minimize_MouseHover(object sender, EventArgs e)
        {
            Minimize.BackColor = Color.DodgerBlue;
        }

        private void Minimize_MouseLeave(object sender, EventArgs e)
        {
            Minimize.BackColor = Color.Transparent;
        }


        private void historyBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            History f = new History(nomeUtilizador, nomeConta, indicePerfil);
            f.Show();
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
        private void profileBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            AccountInfo f = new AccountInfo(nomeUtilizador, nomeConta, indicePerfil);
            f.Show();
        }

        private void profileBtn_MouseHover(object sender, EventArgs e)
        {
            label5.BackColor = Color.Chocolate;
            label5.Height = 5;
            label5.Width = profileBtn.Width;
        }

        private void profileBtn_MouseLeave(object sender, EventArgs e)
        {
            label5.Width = 0;
            label5.Height = 5;
            label5.BackColor = Color.Transparent;
        }
    }
}
