

using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

using Guryflix.Structures;
using Guryflix.Utilities;
using Guryflix.Components;

namespace Guryflix.Forms
{
    public partial class MainPage : Form
    {
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        
        Panel label2, label3, label4, label5, label6, label7;
        FileHandlingUtilites fileHandling = new FileHandlingUtilites();
        Stack stack;
        string ImageNewName = "";
        int count = 0, profileIndex = 0, posterIndex = 0;
        string[] arr, posterArr;
        private string userName = "", accountName = "";
        bool isMaximized = false;
        public MainPage(string userName, string accountName, int index)
        {
            InitializeComponent();
            timer1.Enabled = true;
            timer1.Start();
            initializeLabels();
            this.userName = userName;
            this.accountName = accountName;
            this.profileIndex = index;
            importTypeOfPosters();
            initializePoster();
            for (int i = 0; i < posterArr.Length; i++)
                Console.WriteLine(posterArr[i]);
        }

        private void importTypeOfPosters()
        {
            posterArr = Guryflix.Data.DatabaseContext.GetProfilePreferences(accountName, userName);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            initializePoster();
        }

        void initializePoster()
        {
            changePoster();
            importPosterDetails();
            posterIndex++;
        }
        void importPosterDetails()
        {
            string detailsLocation = Environment.CurrentDirectory + @"\Data\Movie Titles\Movie Posters\" + posterArr[posterIndex] + ".txt";
            int lineCount = 0;
            richTextBox1.Text = "";
            FileStream fs = new FileStream(detailsLocation, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string str = sr.ReadLine();
            while (str != null)
            {
                if (str != null)
                {
                    if (str != "" || str != "\n")
                    {
                        if (lineCount == 0)
                            movieLabel.Text = str;
                        else if (lineCount == 1)
                        {
                            string[] splitString = str.Split(' ');
                            string matchWord = splitString.Length > 1 ? splitString[1] : "";
                            if (matchWord.Equals("Match", StringComparison.OrdinalIgnoreCase))
                                matchWord = "Afinidade";
                            AudiencWatchLabel.Text = (splitString[0] + " " + matchWord);
                            yearLabel.Text = splitString[2];
                        }
                        else
                        {
                            richTextBox1.Text += str;
                        }
                        lineCount++;
                    }
                }
                str = sr.ReadLine();
            }
            sr.Close();
            fs.Close();
        }

        void changePoster()
        {
            if (posterIndex == posterArr.Length)
                posterIndex = 0;
            string imageLocation = Environment.CurrentDirectory + @"\Data\Movie Titles\Movie Posters\" + posterArr[posterIndex] + ".png";
            pictureBox1.ImageLocation = imageLocation;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        /// <summary>
        /// *  Method 1 Contains the following steps
        /// 1. Import Directories Of All Selected Preferences
        /// 2. Copy Content of all Selected directories in an array
        /// 3. Use the Fisher-Yates Algoithm to efficiently sparse/randomize the content
        /// 4. Store the content in stack
        /// 5. Load Images From Files
        /// </summary>
        public void Method1()
        {
            arr = Guryflix.Data.DatabaseContext.GetMoviesByGenres(posterArr);
            Fisher_YatesAlgo randomize = new Fisher_YatesAlgo(arr);
            arr = randomize.arr;
            addToStack();
        }

        private void addToStack()
        {
            stack = new Stack();
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] != " " || arr[i] != "\n")
                    stack.Push(arr[i]);
        }

        // ? During The Loading Of Main Page, Start Storing The Data Sets According TO Prferences
        // ? In the array and display them
        private void MainPage_Load(object sender, EventArgs e)
        {
            Method1();
            initializePoster();
            ImageList imageList1 = new ImageList();
            imageList1.ImageSize = new Size(110, 75);
            string imageLocation = "";
            while (!stack.IsEmpty())
            {
                try
                {
                    if (stack.Peek() == " " || stack.Peek() == "\n")
                        continue;
                    imageLocation = Environment.CurrentDirectory + @"\Data\Movie Titles\Movie Icons\" + stack.Peek() + ".png";
                    imageList1.Images.Add(Image.FromFile(imageLocation));
                    listView1.Items.Add(new ListViewItem
                    {
                        ImageIndex = count,
                        Text = stack.Peek(),
                        Tag = stack.Peek()
                    }); ; ;
                    count++;
                    stack.Pop();
                }
                catch
                {
                    Console.WriteLine(stack.Peek() + " não foi encontrado");
                    stack.Pop();
                }
            }
            listView1.Alignment = ListViewAlignment.Left;
            listView1.LargeImageList = imageList1;
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
                string selected = listView1.SelectedItems[0].Text;
                if (!VideoPlayer.IsMovieAvailable(selected))
                    return;
                this.Hide();
                Guryflix.Data.DatabaseContext.AddMovieToHistory(accountName, userName, selected);
                VideoPlayer j = new VideoPlayer(userName, accountName, selected, profileIndex);
                j.Show();
            }
        }

        void initializeLabels()
        {
            label2 = new Panel();
            label2.Location = new Point(homeBtn.Location.X, homeBtn.Location.Y + 35);
            this.Controls.Add(label2);
            label2.BringToFront();
            label2.BackColor = Color.Chocolate;
            label2.Height = 5;
            label2.Width = homeBtn.Width;
            
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
        private void closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void maximizeBtn_Click(object sender, EventArgs e)
        {
            if (!isMaximized)
            {
                this.WindowState = FormWindowState.Maximized;
                isMaximized = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                isMaximized = false;
            }
        }

        private void minimizebtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Close_MouseHover(object sender, EventArgs e)
        {
            Close.BackColor = Color.Red;
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            Close.BackColor = Color.Black;
        }

        private void Maximize_MouseHover(object sender, EventArgs e)
        {
            Maximize.BackColor = Color.DodgerBlue;
        }

        private void Maximize_MouseLeave(object sender, EventArgs e)
        {
            Maximize.BackColor = Color.Black;
        }

        private void Minimize_MouseHover(object sender, EventArgs e)
        {
            Minimize.BackColor = Color.DodgerBlue;
        }

        private void Minimize_MouseLeave(object sender, EventArgs e)
        {
            Minimize.BackColor = Color.Black;
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
        private void likedVideosBtn_Click(object sender, EventArgs e)
        {
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


        bool isCollapsed = false;


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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Interval >= 1000)
            {
                initializePoster();
                timer1.Interval = 100;
            }
            timer1.Interval += 100;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm f = new LoginForm();
            f.Show();
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
