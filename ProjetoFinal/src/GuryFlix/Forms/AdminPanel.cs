using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Guryflix.Dados;

namespace Guryflix.Forms
{
    public partial class AdminPanel : Form
    {
        private string accountUsername;
        private string profileName;
        private int profileIndex;

        public AdminPanel(string accountUsername, string profileName, int profileIndex)
        {
            this.accountUsername = accountUsername;
            this.profileName = profileName;
            this.profileIndex = profileIndex;
            InitializeComponent();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            
            StyleDataGridView(dgvMovies);
            StyleDataGridView(dgvUsers);
            StyleDataGridView(dgvHistory);
            StyleDataGridView(dgvLikes);

            LoadMovies();
            LoadUsers();
            LoadHistory();
            LoadLikes();
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            
            
            dgv.BackgroundColor = System.Drawing.Color.FromArgb(32, 32, 32);
            dgv.GridColor = System.Drawing.Color.FromArgb(64, 64, 64);
            
            
            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Chocolate;
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(38, 38, 38);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Chocolate;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            
            
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Chocolate;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Chocolate;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            
            
            dgv.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            dgv.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Chocolate;
            dgv.RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            
            
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 30;
            dgv.RowTemplate.Height = 28;
            
            
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            AccountInfo f = new AccountInfo(profileName, accountUsername, profileIndex);
            f.Show();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPageMovies) LoadMovies();
            else if (tabControl1.SelectedTab == tabPageUsers) LoadUsers();
            else if (tabControl1.SelectedTab == tabPageHistory) LoadHistory();
            else if (tabControl1.SelectedTab == tabPageLikes) LoadLikes();
        }

        private void LoadMovies()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT id AS [ID], titulo AS [Título], genero AS [Género], ano AS [Ano], afinidade AS [Afinidade], sinopse AS [Sinopse], url_video AS [URL do Vídeo] FROM filmes ORDER BY id DESC;";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvMovies.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar filmes: " + ex.Message, "Erro");
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        SELECT c.id AS [ID Conta], c.nome_utilizador AS [Utilizador], 
                               p.id AS [ID Perfil], p.nome_perfil AS [Nome do Perfil]
                        FROM contas c
                        LEFT JOIN perfis p ON c.id = p.conta_id
                        ORDER BY c.id, p.id;";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvUsers.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar utilizadores: " + ex.Message, "Erro");
            }
        }

        private void LoadHistory()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        SELECT h.id AS [ID], c.nome_utilizador AS [Conta], p.nome_perfil AS [Perfil], 
                               h.titulo_filme AS [Filme], h.data_visualizacao AS [Data]
                        FROM historico h
                        JOIN perfis p ON h.perfil_id = p.id
                        JOIN contas c ON p.conta_id = c.id
                        ORDER BY h.id DESC;";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvHistory.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar histórico: " + ex.Message, "Erro");
            }
        }

        private void LoadLikes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        SELECT l.id AS [ID], c.nome_utilizador AS [Conta], p.nome_perfil AS [Perfil], 
                               l.titulo_filme AS [Filme Curtido], l.data_curtida AS [Data]
                        FROM videos_curtidos l
                        JOIN perfis p ON l.perfil_id = p.id
                        JOIN contas c ON p.conta_id = c.id
                        ORDER BY l.id DESC;";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvLikes.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar curtidas: " + ex.Message, "Erro");
            }
        }

        private void dgvMovies_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMovies.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvMovies.SelectedRows[0];
                txtTitle.Text = row.Cells["Título"].Value.ToString();
                txtGenre.Text = row.Cells["Género"].Value.ToString();
                txtYear.Text = row.Cells["Ano"].Value.ToString();
                txtAffinity.Text = row.Cells["Afinidade"].Value.ToString();
                txtSynopsis.Text = row.Cells["Sinopse"].Value.ToString();
                txtVideoUrl.Text = row.Cells["URL do Vídeo"].Value.ToString();
            }
        }

        private void btnClearMovieFields_Click(object sender, EventArgs e)
        {
            txtTitle.Text = "";
            txtGenre.Text = "";
            txtYear.Text = "";
            txtAffinity.Text = "";
            txtSynopsis.Text = "";
            txtVideoUrl.Text = "";
            dgvMovies.ClearSelection();
        }

        private void btnAddMovie_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtGenre.Text) || string.IsNullOrEmpty(txtYear.Text))
            {
                MessageBox.Show("Preencha pelo menos o Título, Género e Ano!", "Aviso");
                return;
            }

            int year = 0;
            if (!int.TryParse(txtYear.Text, out year))
            {
                MessageBox.Show("O Ano deve ser um número válido!", "Aviso");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        IF NOT EXISTS (SELECT 1 FROM filmes WHERE titulo = @title)
                        BEGIN
                            INSERT INTO filmes (titulo, genero, ano, afinidade, sinopse, url_video) 
                            VALUES (@title, @genre, @year, @affinity, @synopsis, @video_url);
                        END
                        ELSE
                        BEGIN
                            THROW 50000, 'Já existe um filme com este título!', 1;
                        END";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@genre", txtGenre.Text.Trim());
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@affinity", string.IsNullOrEmpty(txtAffinity.Text) ? "98% Afinidade" : txtAffinity.Text.Trim());
                        cmd.Parameters.AddWithValue("@synopsis", string.IsNullOrEmpty(txtSynopsis.Text) ? "Sem sinopse." : txtSynopsis.Text.Trim());
                        cmd.Parameters.AddWithValue("@video_url", string.IsNullOrEmpty(txtVideoUrl.Text) ? (object)DBNull.Value : txtVideoUrl.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Filme adicionado com sucesso!", "Sucesso");
                LoadMovies();
                btnClearMovieFields_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar filme: " + ex.Message, "Erro");
            }
        }

        private void btnUpdateMovie_Click(object sender, EventArgs e)
        {
            if (dgvMovies.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um filme na lista para atualizar!", "Aviso");
                return;
            }

            int movieId = Convert.ToInt32(dgvMovies.SelectedRows[0].Cells["ID"].Value);
            int year = 0;
            if (!int.TryParse(txtYear.Text, out year))
            {
                MessageBox.Show("O Ano deve ser um número válido!", "Aviso");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                {
                    conn.Open();
                    string query = @"
                        UPDATE filmes 
                        SET titulo = @title, genero = @genre, ano = @year, 
                            afinidade = @affinity, sinopse = @synopsis, url_video = @video_url
                        WHERE id = @id;";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", movieId);
                        cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@genre", txtGenre.Text.Trim());
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@affinity", txtAffinity.Text.Trim());
                        cmd.Parameters.AddWithValue("@synopsis", txtSynopsis.Text.Trim());
                        cmd.Parameters.AddWithValue("@video_url", string.IsNullOrEmpty(txtVideoUrl.Text) ? (object)DBNull.Value : txtVideoUrl.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Filme atualizado com sucesso!", "Sucesso");
                LoadMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar filme: " + ex.Message, "Erro");
            }
        }

        private void btnDeleteMovie_Click(object sender, EventArgs e)
        {
            if (dgvMovies.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um filme na lista para eliminar!", "Aviso");
                return;
            }

            int movieId = Convert.ToInt32(dgvMovies.SelectedRows[0].Cells["ID"].Value);
            string movieTitle = dgvMovies.SelectedRows[0].Cells["Título"].Value.ToString();

            var res = MessageBox.Show($"Tem a certeza que deseja eliminar o filme '{movieTitle}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                    {
                        conn.Open();
                        string query = "DELETE FROM filmes WHERE id = @id;";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", movieId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Filme eliminado com sucesso!", "Sucesso");
                    LoadMovies();
                    btnClearMovieFields_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao eliminar filme: " + ex.Message, "Erro");
                }
            }
        }
        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um registo de utilizador na lista!", "Aviso");
                return;
            }

            int accountId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["ID Conta"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["Utilizador"].Value.ToString();

            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Não é possível eliminar a conta do administrador principal!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var res = MessageBox.Show($"Tem a certeza que deseja eliminar permanentemente a conta '{username}' e todos os seus perfis/dados associados?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                    {
                        conn.Open();
                        string query = "DELETE FROM contas WHERE id = @id;";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", accountId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Utilizador eliminado com sucesso!", "Sucesso");
                    LoadUsers();
                    LoadHistory();
                    LoadLikes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao eliminar utilizador: " + ex.Message, "Erro");
                }
            }
        }

        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um registo de histórico na lista!", "Aviso");
                return;
            }

            int historyId = Convert.ToInt32(dgvHistory.SelectedRows[0].Cells["ID"].Value);

            var res = MessageBox.Show("Deseja apagar este registo de histórico?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                    {
                        conn.Open();
                        string query = "DELETE FROM historico WHERE id = @id;";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", historyId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Registo removido!", "Sucesso");
                    LoadHistory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao limpar histórico: " + ex.Message, "Erro");
                }
            }
        }

        private void btnRemoveLike_Click(object sender, EventArgs e)
        {
            if (dgvLikes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um registo de curtida na lista!", "Aviso");
                return;
            }

            int likeId = Convert.ToInt32(dgvLikes.SelectedRows[0].Cells["ID"].Value);

            var res = MessageBox.Show("Deseja remover esta curtida?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(DatabaseContext.GetActiveConnectionString()))
                    {
                        conn.Open();
                        string query = "DELETE FROM videos_curtidos WHERE id = @id;";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", likeId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Curtida removida!", "Sucesso");
                    LoadLikes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao remover curtida: " + ex.Message, "Erro");
                }
            }
        }
    }
}
