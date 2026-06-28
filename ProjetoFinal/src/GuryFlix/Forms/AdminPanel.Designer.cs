namespace Guryflix.Forms
{
    partial class AdminPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMovies = new System.Windows.Forms.TabPage();
            this.btnClearMovieFields = new System.Windows.Forms.Button();
            this.btnDeleteMovie = new System.Windows.Forms.Button();
            this.btnUpdateMovie = new System.Windows.Forms.Button();
            this.btnAddMovie = new System.Windows.Forms.Button();
            this.txtVideoUrl = new System.Windows.Forms.TextBox();
            this.lblVideoUrl = new System.Windows.Forms.Label();
            this.txtSynopsis = new System.Windows.Forms.TextBox();
            this.lblSynopsis = new System.Windows.Forms.Label();
            this.txtAffinity = new System.Windows.Forms.TextBox();
            this.lblAffinity = new System.Windows.Forms.Label();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.txtGenre = new System.Windows.Forms.TextBox();
            this.lblGenre = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvMovies = new System.Windows.Forms.DataGridView();
            this.tabPageUsers = new System.Windows.Forms.TabPage();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.tabPageHistory = new System.Windows.Forms.TabPage();
            this.btnClearHistory = new System.Windows.Forms.Button();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.tabPageLikes = new System.Windows.Forms.TabPage();
            this.btnRemoveLike = new System.Windows.Forms.Button();
            this.dgvLikes = new System.Windows.Forms.DataGridView();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageMovies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovies)).BeginInit();
            this.tabPageUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.tabPageHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.tabPageLikes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLikes)).BeginInit();
            this.SuspendLayout();
            
            
            
            this.tabControl1.Controls.Add(this.tabPageMovies);
            this.tabControl1.Controls.Add(this.tabPageUsers);
            this.tabControl1.Controls.Add(this.tabPageHistory);
            this.tabControl1.Controls.Add(this.tabPageLikes);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 54);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(910, 480);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            
            
            
            this.tabPageMovies.BackColor = System.Drawing.Color.FromArgb(((byte)(28)), ((byte)(28)), ((byte)(28)));
            this.tabPageMovies.Controls.Add(this.btnClearMovieFields);
            this.tabPageMovies.Controls.Add(this.btnDeleteMovie);
            this.tabPageMovies.Controls.Add(this.btnUpdateMovie);
            this.tabPageMovies.Controls.Add(this.btnAddMovie);
            this.tabPageMovies.Controls.Add(this.txtVideoUrl);
            this.tabPageMovies.Controls.Add(this.lblVideoUrl);
            this.tabPageMovies.Controls.Add(this.txtSynopsis);
            this.tabPageMovies.Controls.Add(this.lblSynopsis);
            this.tabPageMovies.Controls.Add(this.txtAffinity);
            this.tabPageMovies.Controls.Add(this.lblAffinity);
            this.tabPageMovies.Controls.Add(this.txtYear);
            this.tabPageMovies.Controls.Add(this.lblYear);
            this.tabPageMovies.Controls.Add(this.txtGenre);
            this.tabPageMovies.Controls.Add(this.lblGenre);
            this.tabPageMovies.Controls.Add(this.txtTitle);
            this.tabPageMovies.Controls.Add(this.lblTitle);
            this.tabPageMovies.Controls.Add(this.dgvMovies);
            this.tabPageMovies.ForeColor = System.Drawing.Color.White;
            this.tabPageMovies.Location = new System.Drawing.Point(4, 26);
            this.tabPageMovies.Name = "tabPageMovies";
            this.tabPageMovies.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMovies.Size = new System.Drawing.Size(902, 450);
            this.tabPageMovies.TabIndex = 0;
            this.tabPageMovies.Text = "Filmes";
            
            
            
            this.btnClearMovieFields.BackColor = System.Drawing.Color.FromArgb(((byte)(60)), ((byte)(60)), ((byte)(60)));
            this.btnClearMovieFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearMovieFields.ForeColor = System.Drawing.Color.White;
            this.btnClearMovieFields.Location = new System.Drawing.Point(780, 400);
            this.btnClearMovieFields.Name = "btnClearMovieFields";
            this.btnClearMovieFields.Size = new System.Drawing.Size(100, 32);
            this.btnClearMovieFields.TabIndex = 16;
            this.btnClearMovieFields.Text = "Limpar";
            this.btnClearMovieFields.UseVisualStyleBackColor = false;
            this.btnClearMovieFields.Click += new System.EventHandler(this.btnClearMovieFields_Click);
            
            
            
            this.btnDeleteMovie.BackColor = System.Drawing.Color.FromArgb(((byte)(229)), ((byte)(9)), ((byte)(20)));
            this.btnDeleteMovie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteMovie.ForeColor = System.Drawing.Color.White;
            this.btnDeleteMovie.Location = new System.Drawing.Point(674, 400);
            this.btnDeleteMovie.Name = "btnDeleteMovie";
            this.btnDeleteMovie.Size = new System.Drawing.Size(100, 32);
            this.btnDeleteMovie.TabIndex = 15;
            this.btnDeleteMovie.Text = "Eliminar";
            this.btnDeleteMovie.UseVisualStyleBackColor = false;
            this.btnDeleteMovie.Click += new System.EventHandler(this.btnDeleteMovie_Click);
            
            
            
            this.btnUpdateMovie.BackColor = System.Drawing.Color.Chocolate;
            this.btnUpdateMovie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateMovie.ForeColor = System.Drawing.Color.White;
            this.btnUpdateMovie.Location = new System.Drawing.Point(568, 400);
            this.btnUpdateMovie.Name = "btnUpdateMovie";
            this.btnUpdateMovie.Size = new System.Drawing.Size(100, 32);
            this.btnUpdateMovie.TabIndex = 14;
            this.btnUpdateMovie.Text = "Atualizar";
            this.btnUpdateMovie.UseVisualStyleBackColor = false;
            this.btnUpdateMovie.Click += new System.EventHandler(this.btnUpdateMovie_Click);
            
            
            
            this.btnAddMovie.BackColor = System.Drawing.Color.FromArgb(((byte)(0)), ((byte)(120)), ((byte)(215)));
            this.btnAddMovie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMovie.ForeColor = System.Drawing.Color.White;
            this.btnAddMovie.Location = new System.Drawing.Point(462, 400);
            this.btnAddMovie.Name = "btnAddMovie";
            this.btnAddMovie.Size = new System.Drawing.Size(100, 32);
            this.btnAddMovie.TabIndex = 13;
            this.btnAddMovie.Text = "Adicionar";
            this.btnAddMovie.UseVisualStyleBackColor = false;
            this.btnAddMovie.Click += new System.EventHandler(this.btnAddMovie_Click);
            
            
            
            this.txtVideoUrl.Location = new System.Drawing.Point(462, 355);
            this.txtVideoUrl.Name = "txtVideoUrl";
            this.txtVideoUrl.Size = new System.Drawing.Size(418, 25);
            this.txtVideoUrl.TabIndex = 12;
            
            
            
            this.lblVideoUrl.AutoSize = true;
            this.lblVideoUrl.Location = new System.Drawing.Point(459, 335);
            this.lblVideoUrl.Name = "lblVideoUrl";
            this.lblVideoUrl.Size = new System.Drawing.Size(126, 17);
            this.lblVideoUrl.TabIndex = 11;
            this.lblVideoUrl.Text = "URL do Video (YT):";
            
            
            
            this.txtSynopsis.Location = new System.Drawing.Point(462, 230);
            this.txtSynopsis.Multiline = true;
            this.txtSynopsis.Name = "txtSynopsis";
            this.txtSynopsis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSynopsis.Size = new System.Drawing.Size(418, 90);
            this.txtSynopsis.TabIndex = 10;
            
            
            
            this.lblSynopsis.AutoSize = true;
            this.lblSynopsis.Location = new System.Drawing.Point(459, 210);
            this.lblSynopsis.Name = "lblSynopsis";
            this.lblSynopsis.Size = new System.Drawing.Size(61, 17);
            this.lblSynopsis.TabIndex = 9;
            this.lblSynopsis.Text = "Sinopse:";
            
            
            
            this.txtAffinity.Location = new System.Drawing.Point(462, 170);
            this.txtAffinity.Name = "txtAffinity";
            this.txtAffinity.Size = new System.Drawing.Size(418, 25);
            this.txtAffinity.TabIndex = 8;
            
            
            
            this.lblAffinity.AutoSize = true;
            this.lblAffinity.Location = new System.Drawing.Point(459, 150);
            this.lblAffinity.Name = "lblAffinity";
            this.lblAffinity.Size = new System.Drawing.Size(70, 17);
            this.lblAffinity.TabIndex = 7;
            this.lblAffinity.Text = "Afinidade:";
            
            
            
            this.txtYear.Location = new System.Drawing.Point(462, 115);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(418, 25);
            this.txtYear.TabIndex = 6;
            
            
            
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(459, 95);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(37, 17);
            this.lblYear.TabIndex = 5;
            this.lblYear.Text = "Ano:";
            
            
            
            this.txtGenre.Location = new System.Drawing.Point(462, 65);
            this.txtGenre.Name = "txtGenre";
            this.txtGenre.Size = new System.Drawing.Size(418, 25);
            this.txtGenre.TabIndex = 4;
            
            
            
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(459, 45);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(56, 17);
            this.lblGenre.TabIndex = 3;
            this.lblGenre.Text = "Género:";
            
            
            
            this.txtTitle.Location = new System.Drawing.Point(462, 17);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(418, 25);
            this.txtTitle.TabIndex = 2;
            
            
            
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(459, -3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(48, 17);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Título:";
            
            
            
            this.dgvMovies.AllowUserToAddRows = false;
            this.dgvMovies.AllowUserToDeleteRows = false;
            this.dgvMovies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovies.Location = new System.Drawing.Point(6, 6);
            this.dgvMovies.MultiSelect = false;
            this.dgvMovies.Name = "dgvMovies";
            this.dgvMovies.ReadOnly = true;
            this.dgvMovies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMovies.Size = new System.Drawing.Size(437, 438);
            this.dgvMovies.TabIndex = 0;
            this.dgvMovies.SelectionChanged += new System.EventHandler(this.dgvMovies_SelectionChanged);
            
            
            
            this.tabPageUsers.BackColor = System.Drawing.Color.FromArgb(((byte)(28)), ((byte)(28)), ((byte)(28)));
            this.tabPageUsers.Controls.Add(this.btnDeleteUser);
            this.tabPageUsers.Controls.Add(this.dgvUsers);
            this.tabPageUsers.ForeColor = System.Drawing.Color.White;
            this.tabPageUsers.Location = new System.Drawing.Point(4, 26);
            this.tabPageUsers.Name = "tabPageUsers";
            this.tabPageUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUsers.Size = new System.Drawing.Size(902, 450);
            this.tabPageUsers.TabIndex = 1;
            this.tabPageUsers.Text = "Utilizadores";
            
            
            
            this.btnDeleteUser.BackColor = System.Drawing.Color.FromArgb(((byte)(229)), ((byte)(9)), ((byte)(20)));
            this.btnDeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteUser.Location = new System.Drawing.Point(746, 17);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(139, 37);
            this.btnDeleteUser.TabIndex = 1;
            this.btnDeleteUser.Text = "Eliminar Conta";
            this.btnDeleteUser.UseVisualStyleBackColor = false;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            
            
            
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(6, 6);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(720, 438);
            this.dgvUsers.TabIndex = 0;
            
            
            
            this.tabPageHistory.BackColor = System.Drawing.Color.FromArgb(((byte)(28)), ((byte)(28)), ((byte)(28)));
            this.tabPageHistory.Controls.Add(this.btnClearHistory);
            this.tabPageHistory.Controls.Add(this.dgvHistory);
            this.tabPageHistory.ForeColor = System.Drawing.Color.White;
            this.tabPageHistory.Location = new System.Drawing.Point(4, 26);
            this.tabPageHistory.Name = "tabPageHistory";
            this.tabPageHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHistory.Size = new System.Drawing.Size(902, 450);
            this.tabPageHistory.TabIndex = 2;
            this.tabPageHistory.Text = "Histórico";
            
            
            
            this.btnClearHistory.BackColor = System.Drawing.Color.FromArgb(((byte)(229)), ((byte)(9)), ((byte)(20)));
            this.btnClearHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearHistory.Location = new System.Drawing.Point(746, 17);
            this.btnClearHistory.Name = "btnClearHistory";
            this.btnClearHistory.Size = new System.Drawing.Size(139, 37);
            this.btnClearHistory.TabIndex = 2;
            this.btnClearHistory.Text = "Limpar Registo";
            this.btnClearHistory.UseVisualStyleBackColor = false;
            this.btnClearHistory.Click += new System.EventHandler(this.btnClearHistory_Click);
            
            
            
            this.dgvHistory.AllowUserToAddRows = false;
            this.dgvHistory.AllowUserToDeleteRows = false;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(6, 6);
            this.dgvHistory.MultiSelect = false;
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistory.Size = new System.Drawing.Size(720, 438);
            this.dgvHistory.TabIndex = 1;
            
            
            
            this.tabPageLikes.BackColor = System.Drawing.Color.FromArgb(((byte)(28)), ((byte)(28)), ((byte)(28)));
            this.tabPageLikes.Controls.Add(this.btnRemoveLike);
            this.tabPageLikes.Controls.Add(this.dgvLikes);
            this.tabPageLikes.ForeColor = System.Drawing.Color.White;
            this.tabPageLikes.Location = new System.Drawing.Point(4, 26);
            this.tabPageLikes.Name = "tabPageLikes";
            this.tabPageLikes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLikes.Size = new System.Drawing.Size(902, 450);
            this.tabPageLikes.TabIndex = 3;
            this.tabPageLikes.Text = "Curtidos";
            
            
            
            this.btnRemoveLike.BackColor = System.Drawing.Color.FromArgb(((byte)(229)), ((byte)(9)), ((byte)(20)));
            this.btnRemoveLike.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveLike.Location = new System.Drawing.Point(746, 17);
            this.btnRemoveLike.Name = "btnRemoveLike";
            this.btnRemoveLike.Size = new System.Drawing.Size(139, 37);
            this.btnRemoveLike.TabIndex = 2;
            this.btnRemoveLike.Text = "Remover Curtida";
            this.btnRemoveLike.UseVisualStyleBackColor = false;
            this.btnRemoveLike.Click += new System.EventHandler(this.btnRemoveLike_Click);
            
            
            
            this.dgvLikes.AllowUserToAddRows = false;
            this.dgvLikes.AllowUserToDeleteRows = false;
            this.dgvLikes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLikes.Location = new System.Drawing.Point(6, 6);
            this.dgvLikes.MultiSelect = false;
            this.dgvLikes.Name = "dgvLikes";
            this.dgvLikes.ReadOnly = true;
            this.dgvLikes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLikes.Size = new System.Drawing.Size(720, 438);
            this.dgvLikes.TabIndex = 1;
            
            
            
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((byte)(40)), ((byte)(40)), ((byte)(40)));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(822, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 36);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Voltar";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            
            
            
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((byte)(229)), ((byte)(9)), ((byte)(20)));
            this.lblHeader.Location = new System.Drawing.Point(12, 12);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(287, 32);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Text = "Painel de Administração";
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((byte)(17)), ((byte)(17)), ((byte)(17)));
            this.ClientSize = new System.Drawing.Size(934, 546);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AdminPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Guryflix - Administração";
            this.Load += new System.EventHandler(this.AdminPanel_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMovies.ResumeLayout(false);
            this.tabPageMovies.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovies)).EndInit();
            this.tabPageUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.tabPageHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.tabPageLikes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLikes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMovies;
        private System.Windows.Forms.TabPage tabPageUsers;
        private System.Windows.Forms.TabPage tabPageHistory;
        private System.Windows.Forms.TabPage tabPageLikes;
        private System.Windows.Forms.DataGridView dgvMovies;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtGenre;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.TextBox txtAffinity;
        private System.Windows.Forms.Label lblAffinity;
        private System.Windows.Forms.TextBox txtSynopsis;
        private System.Windows.Forms.Label lblSynopsis;
        private System.Windows.Forms.TextBox txtVideoUrl;
        private System.Windows.Forms.Label lblVideoUrl;
        private System.Windows.Forms.Button btnAddMovie;
        private System.Windows.Forms.Button btnUpdateMovie;
        private System.Windows.Forms.Button btnDeleteMovie;
        private System.Windows.Forms.Button btnClearMovieFields;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Button btnClearHistory;
        private System.Windows.Forms.DataGridView dgvLikes;
        private System.Windows.Forms.Button btnRemoveLike;
    }
}
