
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

using Guryflix.Estruturas;
using Guryflix.Utilitarios;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class VideoPlayer : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        bool isCollapsed = false, isMaximized = false;
        Panel label5, label6, label7;

        UtilitariosFicheiros fileHandling = new UtilitariosFicheiros();
        ListaDuplamenteLigada circularLinkedList;
        string[] arr, likedVideos;
        string ImageNewName = "", userName, accountName;
        int count = 0, tagIndex = 0, profileIndex;
        string currentMovie;
        bool playBackStatus, currentLikeStatus = false;
        string imageLocation = Environment.CurrentDirectory + @"\Interface\IconesReprodutor\";

        public static bool IsMovieAvailable(string movieName)
        {
            string videoUrl = Guryflix.Data.DatabaseContext.GetMovieVideoUrl(movieName);
            string localPath = Environment.CurrentDirectory + @"\Dados\Filmes\Trailers\" + movieName + ".mp4";
            string devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Dados\Filmes\Trailers\" + movieName + ".mp4");
            
            bool localExists = System.IO.File.Exists(localPath) || System.IO.File.Exists(devPath);
            
            if (string.IsNullOrEmpty(videoUrl) && !localExists)
            {
                MessageBox.Show("Filme não disponível de momento!", "Indisponível", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public VideoPlayer(string userName, string accountName, string movieName, int index)
        {
            importAccountInformation(userName, accountName, index);
            initalizeAppComponents();
            initalizeMoviePlayer(movieName);
            Method1();
        }

        public void importAccountInformation(string userName, string accountName, int profileIndex)
        {
            this.userName = userName; this.accountName = accountName;
            this.profileIndex = profileIndex;
        }

        public void initalizeAppComponents()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += VideoPlayer_KeyDown;
            axWindowsMediaPlayer1.uiMode = "none";
            initializeLabels();
            circularLinkedList = new ListaDuplamenteLigada();
            importLikedVideos();
        }

        public void initalizeMoviePlayer(string movieName)
        {
            initializeAXMoviePlayerSettings();
            currentMovie = movieName;
            playBackStatus = true;
            initalizeControlBar();
            startMovie();
        }

        void initializeLabels()
        {
            label5 = new Panel();
            label5.Width = 0;
            label5.Height = 5;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(profileBtn.Location.X, profileBtn.Location.Y + 35);
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

        void initalizeControlBar()
        {
            likeBtn.ImageLocation = imageLocation + "heart_unselected.png";
            likeBtn.SizeMode = PictureBoxSizeMode.Zoom;
            playPauseBtn.ImageLocation = imageLocation + "Pause.png";
            playPauseBtn.SizeMode = PictureBoxSizeMode.Zoom;
            fastForwardBtn.ImageLocation = imageLocation + "Forward.png";
            fastForwardBtn.SizeMode = PictureBoxSizeMode.Zoom;
            reverseBtn.ImageLocation = imageLocation + "Reverse.png";
            reverseBtn.SizeMode = PictureBoxSizeMode.Zoom;
            reverseBtn.ImageLocation = imageLocation + "Rewind.png";
            reverseBtn.SizeMode = PictureBoxSizeMode.Zoom;
            previousBtn.ImageLocation = imageLocation + "Start.png";
            previousBtn.SizeMode = PictureBoxSizeMode.Zoom;
            nextBtn.ImageLocation = imageLocation + "End.png";
            nextBtn.SizeMode = PictureBoxSizeMode.Zoom;
            fullScreenBtn.ImageLocation = imageLocation + "Full Screen.png";
            fullScreenBtn.SizeMode = PictureBoxSizeMode.StretchImage;
            fullScreenBtn.BackColor = Color.Transparent;
        }

        void initializeAXMoviePlayerSettings()
        {
            volumeControl.Value = 10000;
            axWindowsMediaPlayer1.settings.volume = 100;
            axWindowsMediaPlayer1.settings.autoStart = true;
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.PlayStateChange += axWindowsMediaPlayer1_PlayStateChange;
            axWindowsMediaPlayer1.KeyDownEvent += axWindowsMediaPlayer1_KeyDownEvent;
            axWindowsMediaPlayer1.DoubleClickEvent += axWindowsMediaPlayer1_DoubleClickEvent;
        }

        private void VideoPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (isFullScreen)
                {
                    fullScreenBtn_Click(sender, EventArgs.Empty);
                }
            }
            else if (e.KeyCode == Keys.Space)
            {
                playPauseBtn_Click(sender, EventArgs.Empty);
            }
        }

        private void axWindowsMediaPlayer1_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            if (e.nKeyCode == 27) // Escape
            {
                if (isFullScreen)
                {
                    fullScreenBtn_Click(sender, EventArgs.Empty);
                }
            }
            else if (e.nKeyCode == 32) // Space
            {
                playPauseBtn_Click(sender, EventArgs.Empty);
            }
        }

        private void axWindowsMediaPlayer1_DoubleClickEvent(object sender, AxWMPLib._WMPOCXEvents_DoubleClickEvent e)
        {
            fullScreenBtn_Click(sender, EventArgs.Empty);
        }

        void importLikedVideos()
        {
            try
            {
                likedVideos = Guryflix.Data.DatabaseContext.GetProfileLikedVideos(accountName, userName);
            }
            catch { likedVideos = new string[0]; }
        }





        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            // Estado 3 = A Reproduzir (Playing)
            if (e.newState == 3)
            {
                try
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                int dur = (int)(axWindowsMediaPlayer1.currentMedia?.duration ?? 0);
                                progressBar1.Maximum = dur > 0 ? dur : 100;
                                TimeSpan time = TimeSpan.FromSeconds(progressBar1.Maximum);
                                totalDuration.Text = time.ToString();
                            }
                            catch { }
                        }));
                    }
                }
                catch { }
            }
        }
        // Guarda a localização e tamanho originais do player
        System.Drawing.Point playerOriginalLocation = new System.Drawing.Point(111, 104);
        System.Drawing.Size playerOriginalSize = new System.Drawing.Size(677, 270);
        bool isYouTube = false;

        /// <summary>Converte URL YouTube (watch?v=ID) para URL embed.</summary>
        private string GetYouTubeEmbedUrl(string url)
        {
            try
            {
                // Extrair o ID do v&iacute;deo sem System.Web
                // Formato: youtube.com/watch?v=ID ou youtu.be/ID
                string videoId = null;
                if (url.Contains("watch?v="))
                {
                    int idx = url.IndexOf("watch?v=") + 8;
                    int end = url.IndexOf('&', idx);
                    videoId = end >= 0 ? url.Substring(idx, end - idx) : url.Substring(idx);
                }
                else if (url.Contains("youtu.be/"))
                {
                    int idx = url.IndexOf("youtu.be/") + 9;
                    int end = url.IndexOf('?', idx);
                    videoId = end >= 0 ? url.Substring(idx, end - idx) : url.Substring(idx);
                }
                if (!string.IsNullOrEmpty(videoId))
                    return "https://www.youtube.com/embed/" + videoId;
            }
            catch { }
            return url;
        }

        private bool IsYouTubeUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            return url.Contains("youtube.com") || url.Contains("youtu.be");
        }

        
        Panel youTubePanel;

        void CreateYouTubePanel()
        {
            if (youTubePanel != null) return;
            youTubePanel = new Panel();
            youTubePanel.BackColor = Color.FromArgb(15, 15, 15);
            youTubePanel.Location = playerOriginalLocation;
            youTubePanel.Size = playerOriginalSize;
            youTubePanel.Visible = false;

            
            Label ytIcon = new Label();
            ytIcon.Text = "▶";
            ytIcon.Font = new Font("Arial", 40, FontStyle.Bold);
            ytIcon.ForeColor = Color.FromArgb(255, 0, 0);
            ytIcon.AutoSize = true;
            ytIcon.Location = new Point(youTubePanel.Width / 2 - 30, youTubePanel.Height / 2 - 60);
            youTubePanel.Controls.Add(ytIcon);

            Label msg = new Label();
            msg.Text = "Clique para ver o trailer no YouTube";
            msg.Font = new Font("Arial", 12);
            msg.ForeColor = Color.White;
            msg.AutoSize = true;
            msg.Location = new Point(youTubePanel.Width / 2 - 130, youTubePanel.Height / 2);
            youTubePanel.Controls.Add(msg);

            Button openBtn = new Button();
            openBtn.Text = "▶  Abrir Trailer";
            openBtn.Font = new Font("Arial", 12, FontStyle.Bold);
            openBtn.BackColor = Color.FromArgb(200, 0, 0);
            openBtn.ForeColor = Color.White;
            openBtn.FlatStyle = FlatStyle.Flat;
            openBtn.FlatAppearance.BorderSize = 0;
            openBtn.Size = new Size(200, 45);
            openBtn.Location = new Point(youTubePanel.Width / 2 - 100, youTubePanel.Height / 2 + 40);
            openBtn.Cursor = Cursors.Hand;
            openBtn.Click += (s, e) => {
                if (!string.IsNullOrEmpty(currentYouTubeUrl))
                    System.Diagnostics.Process.Start(currentYouTubeUrl);
            };
            youTubePanel.Controls.Add(openBtn);

            this.Controls.Add(youTubePanel);
            youTubePanel.BringToFront();
        }

        string currentYouTubeUrl = null;
        void startMovie()
        {
            if (isVideoLiked(currentMovie))
                likeON();
            else
                likeOFF();
            progressBar1.Value = 0;
            progressBar1.Maximum = 100;
            totalDuration.Text = "0:00:00";
            timer1.Stop();
            titleLabel.Text = currentMovie;
            label1.Width = titleLabel.Width + 70;

            string videoUrl = Guryflix.Data.DatabaseContext.GetMovieVideoUrl(currentMovie);
            string localPath = Environment.CurrentDirectory + @"\Dados\Filmes\Trailers\" + currentMovie + ".mp4";
            string devPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Dados\Filmes\Trailers\" + currentMovie + ".mp4");
            string actualPath = System.IO.File.Exists(localPath) ? localPath : (System.IO.File.Exists(devPath) ? devPath : localPath);

            if (System.IO.File.Exists(actualPath))
            {
                isYouTube = false;
                if (youTubePanel != null) youTubePanel.Visible = false;
                youTubePlayer.Visible = false;
                axWindowsMediaPlayer1.Visible = true;
                axWindowsMediaPlayer1.BringToFront();
                timer1.Start();
                axWindowsMediaPlayer1.URL = actualPath;
            }
            else if (IsYouTubeUrl(videoUrl))
            {
                isYouTube = true;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.Visible = false;
                youTubePlayer.Visible = false;
                currentYouTubeUrl = videoUrl;
                CreateYouTubePanel();
                youTubePanel.Visible = true;
                youTubePanel.BringToFront();
                currentDuration.Text = "--:--:--";
                totalDuration.Text = "--:--:--";
            }
            else if (!string.IsNullOrEmpty(videoUrl))
            {
                isYouTube = false;
                if (youTubePanel != null) youTubePanel.Visible = false;
                youTubePlayer.Visible = false;
                axWindowsMediaPlayer1.Visible = true;
                axWindowsMediaPlayer1.BringToFront();
                timer1.Start();
                axWindowsMediaPlayer1.URL = videoUrl;
            }
            else
            {
                isYouTube = false;
                if (youTubePanel != null) youTubePanel.Visible = false;
                youTubePlayer.Visible = false;
                axWindowsMediaPlayer1.Visible = true;
                axWindowsMediaPlayer1.BringToFront();
                timer1.Start();
                axWindowsMediaPlayer1.URL = actualPath;
            }
        }

        public void Method1()
        {
            try
            {
                string[] genres = Guryflix.Data.DatabaseContext.GetProfilePreferences(accountName, userName);
                arr = Guryflix.Data.DatabaseContext.GetMoviesByGenres(genres);
                AlgoritmoFisherYates randomize = new AlgoritmoFisherYates(arr);
                arr = randomize.arr;
                addToLinkedList();
            }
            catch { }
        }

        private void addToLinkedList()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != " " || arr[i] != "\n")
                    circularLinkedList.InserirFim(arr[i]);
            }
            circularLinkedList.GuardarDados(arr.Length);
        }

        private void VideoPlayBack_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '+')
            {
                int vol = axWindowsMediaPlayer1.settings.volume;
                if (vol < 100)
                    axWindowsMediaPlayer1.settings.volume = Math.Min(100, vol + 10);
            }
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            tagIndex++;
            if (tagIndex < 0 || tagIndex == circularLinkedList.nomesFilmes.Length - 1)
                tagIndex = 0;
            try
            {
                currentMovie = listView1.Items[tagIndex].Text;
            }
            catch
            {
                tagIndex = 1;
                currentMovie = listView1.Items[tagIndex].Text;
            }
            while (currentMovie == "" || currentMovie == null)
                currentMovie = listView1.Items[++tagIndex].Text;
            string selected = circularLinkedList.nomesFilmes[tagIndex];
            Guryflix.Data.DatabaseContext.AddMovieToHistory(accountName, userName, selected);
            startMovie();
        }
        private void previousBtn_Click(object sender, EventArgs e)
        {
            tagIndex--;
            if (tagIndex < 0 || tagIndex == circularLinkedList.nomesFilmes.Length)
                tagIndex = circularLinkedList.nomesFilmes.Length - 1;
            try
            {
                currentMovie = listView1.Items[tagIndex].Text;
            }
            catch
            {
                tagIndex = circularLinkedList.nomesFilmes.Length - 5;
                currentMovie = listView1.Items[tagIndex].Text;
            }
            while (currentMovie == "" || currentMovie == null)
                currentMovie = listView1.Items[--tagIndex].Text;
            string selected = circularLinkedList.nomesFilmes[tagIndex];
            Guryflix.Data.DatabaseContext.AddMovieToHistory(accountName, userName, selected);
            startMovie();
        }

        private void playPauseBtn_Click(object sender, EventArgs e)
        {
            playOrPause();
        }
        private void playOrPause()
        {
            if (playBackStatus == true)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                timer1.Start();
                playPauseBtn.ImageLocation = imageLocation + "Pause.png";
                playPauseBtn.SizeMode = PictureBoxSizeMode.Zoom;
                playBackStatus = false;
            }
            else if (playBackStatus == false)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                timer1.Stop();
                playPauseBtn.ImageLocation = imageLocation + "Play.png";
                playPauseBtn.SizeMode = PictureBoxSizeMode.Zoom;
                playBackStatus = true;
            }
        }

        private void reverseBtn_Click(object sender, EventArgs e)
        {
            double pos = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = Math.Max(0, pos - 10);
        }
        private void fastForwardBtn_Click(object sender, EventArgs e)
        {
            double pos = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = pos + 10;
        }

        private void volume_Scroll(object sender, EventArgs e)
        {
            // volumeControl.Maximum = 10000, mapear para 0-100
            int vol = (int)((volumeControl.Value / (double)volumeControl.Maximum) * 100);
            axWindowsMediaPlayer1.settings.volume = vol;
            volumePercentage.Text = vol.ToString() + "%";
        }

        bool isFullScreen = false;
        private void fullScreenBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Determinar qual player está ativo
                Control activePlayer = isYouTube ? (Control)youTubePlayer : (Control)axWindowsMediaPlayer1;

                if (!isFullScreen)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    activePlayer.Location = new System.Drawing.Point(0, 0);
                    activePlayer.Size = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height);
                    activePlayer.BringToFront();
                    isFullScreen = true;
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Normal;
                    activePlayer.Location = playerOriginalLocation;
                    activePlayer.Size = playerOriginalSize;
                    // Trazer os controlos para a frente novamente
                    panel1.BringToFront();
                    panel3.BringToFront();
                    fullScreenBtn.BringToFront();
                    progressBar1.BringToFront();
                    listView1.BringToFront();
                    isFullScreen = false;
                }
            }
            catch { }
        }



        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedIndices.Count <= 0)
            {
                Console.WriteLine(listView1.SelectedItems.Count);
                return;
            }
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                currentMovie = listView1.SelectedItems[0].Text;
                Guryflix.Data.DatabaseContext.AddMovieToHistory(accountName, userName, currentMovie);
                startMovie();
            }
        }

        private void populate()
        {
            string imageLocation = "";
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(110, 75);
            string[] paths = { };
            for (int i = 0; i < circularLinkedList.nomesFilmes.Length; i++)
            {
                try
                {
                    if (circularLinkedList.nomesFilmes[i] == " " || circularLinkedList.nomesFilmes[i] == "\n")
                        continue;
                    imageLocation = Environment.CurrentDirectory + @"\Dados\Filmes\Icones\" + circularLinkedList.nomesFilmes[i] + ".png";
                    imgs.Images.Add(Image.FromFile(imageLocation));
                    listView1.Items.Add(new ListViewItem
                    {
                        ImageIndex = count,
                        Text = circularLinkedList.nomesFilmes[i],
                        Tag = circularLinkedList.nomesFilmes[i],
                    }); ; ; ;
                    count++;
                }
                catch
                {
                    Console.WriteLine(circularLinkedList.nomesFilmes[i] + " não foi encontrado");
                }
            }
            listView1.LargeImageList = imgs; // Setting Size Of Images
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;
            ImageNewName = Convert.ToString(e.Label);
            ListViewItem item1 = listView1.SelectedItems[0];
            FileInfo fileInfo = new FileInfo(item1.Tag.ToString());
            fileInfo.MoveTo(fileInfo.Directory.FullName + "\\" + ImageNewName + fileInfo.Extension);
            listView1.Items[count].Text = ImageNewName;
        }

        private void VideoPlayBack_Load(object sender, EventArgs e)
        {
            listView1.Alignment = ListViewAlignment.Left;
            populate();
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
                label6.Height = 5;
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

        private void settingsBtn_MouseHover(object sender, EventArgs e)
        {
            if (!isCollapsed)
            {
                label6.BackColor = ColorTranslator.FromHtml("#0066B4");
                label6.Height = 5;
                label6.Width = settingsBtn.Width;
            }
        }


        private void menuItem1_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Hide();
            LoginForm f = new LoginForm();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int dur = (int)(axWindowsMediaPlayer1.currentMedia?.duration ?? 0);
                if (dur > 0 && progressBar1.Value >= dur)
                {
                    object s = null;
                    EventArgs ef = null;
                    nextBtn_Click(s, ef);
                }
                TimeSpan time = TimeSpan.FromSeconds(progressBar1.Value);
                currentDuration.Text = time.ToString();
                if (progressBar1.Value < progressBar1.Maximum)
                    progressBar1.Increment(1);
            }
            catch { }
        }


        private void likedVideosBtn_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Hide();
            LikedVideos f = new LikedVideos(userName, accountName, profileIndex);
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


        private void homeBtn_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Hide();
            MainPage f = new MainPage(userName, accountName, profileIndex);
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

        private void searchBtn_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Hide();
            SearchBox f = new SearchBox(userName, accountName, profileIndex);
            f.Show();
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
        private void historyBtn_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Hide();
            History f = new History(userName, accountName, profileIndex);
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
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Hide();
            SearchBox f = new SearchBox(userName, accountName, profileIndex);
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

        private void likeBtn_Click(object sender, EventArgs e)
        {
            if (currentLikeStatus == false)
            {
                likeON();
                Guryflix.Data.DatabaseContext.AddVideoToLiked(accountName, userName, currentMovie);
                importLikedVideos();
            }
            else
            {
                likeOFF();
                Guryflix.Data.DatabaseContext.RemoveVideoFromLiked(accountName, userName, currentMovie);
                importLikedVideos();
            }
        }
        bool isVideoLiked(string currentVideo)
        {
            foreach (string video in likedVideos)
            {
                if (video == currentVideo)
                    return true;
            }
            return false;
        }

        private void likeON()
        {
            currentLikeStatus = true;
            likeBtn.ImageLocation = imageLocation + "heart_selected.png";
            likeBtn.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void likeOFF()
        {
            currentLikeStatus = false;
            likeBtn.ImageLocation = imageLocation + "heart_unselected.png";
            likeBtn.SizeMode = PictureBoxSizeMode.Zoom;
        }


        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void maximizeBtn_Click(object sender, EventArgs e)
        {
            if (!isMaximized)
            {
                this.WindowState = FormWindowState.Maximized;
                Console.WriteLine(this.Width);
                isMaximized = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                isMaximized = false;
            }
            axWindowsMediaPlayer1.Refresh();
            axWindowsMediaPlayer1.Width = (this.Width - 150);
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Hide();
            MainPage f = new MainPage(userName, accountName, profileIndex);
            f.Show();
        }
    }
}
