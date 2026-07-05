using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BCrypt.Net;

namespace Guryflix.Dados
{
    public class DadosFilme
    {
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public int Ano { get; set; }
        public string Afinidade { get; set; }
        public string Sinopse { get; set; }
    }

    public static class DatabaseContext
    {
        private static string _activeConnectionString = null;

        public static string GetActiveConnectionString()
        {
            if (_activeConnectionString != null)
                return _activeConnectionString;

            string[] connectionStrings = new string[]
            {
                @"Server=(localdb)\MSSQLLocalDB;Database=guryflix;Integrated Security=True;TrustServerCertificate=True;",
                @"Server=.\SQLEXPRESS;Database=guryflix;Integrated Security=True;TrustServerCertificate=True;"
            };

            foreach (var connStr in connectionStrings)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        _activeConnectionString = connStr;
                        return _activeConnectionString;
                    }
                }
                catch { }
            }

            _activeConnectionString = connectionStrings[0];
            return _activeConnectionString;
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    // Garante apenas a criação das tabelas essenciais se não existirem
                    string createContasTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[contas]') AND type in (N'U'))
                        CREATE TABLE contas (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            nome_utilizador VARCHAR(150) UNIQUE NOT NULL,
                            senha_hash VARCHAR(255) NOT NULL,
                            admin INT DEFAULT 0 NOT NULL
                        );";
                    using (SqlCommand cmd = new SqlCommand(createContasTable, conn)) { cmd.ExecuteNonQuery(); }

                    string createPerfisTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[perfis]') AND type in (N'U'))
                        CREATE TABLE perfis (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            conta_id INT NOT NULL FOREIGN KEY REFERENCES contas(id) ON DELETE CASCADE,
                            nome_perfil VARCHAR(100) NOT NULL,
                            senha_hash VARCHAR(255) NOT NULL,
                            CONSTRAINT UQ_Perfil UNIQUE (conta_id, nome_perfil)
                        );";
                    using (SqlCommand cmd = new SqlCommand(createPerfisTable, conn)) { cmd.ExecuteNonQuery(); }

                    string createPreferenciasTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[preferencias]') AND type in (N'U'))
                        CREATE TABLE preferencias (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            genero VARCHAR(100) NOT NULL,
                            CONSTRAINT UQ_Preferencia UNIQUE (perfil_id, genero)
                        );";
                    using (SqlCommand cmd = new SqlCommand(createPreferenciasTable, conn)) { cmd.ExecuteNonQuery(); }

                    string createHistoricoTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[historico]') AND type in (N'U'))
                        CREATE TABLE historico (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            titulo_filme VARCHAR(150) NOT NULL,
                            data_visualizacao DATETIME NOT NULL
                        );";
                    using (SqlCommand cmd = new SqlCommand(createHistoricoTable, conn)) { cmd.ExecuteNonQuery(); }

                    string createVideosCurtidosTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[videos_curtidos]') AND type in (N'U'))
                        CREATE TABLE videos_curtidos (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            titulo_filme VARCHAR(150) NOT NULL,
                            data_curtida DATETIME NOT NULL,
                            CONSTRAINT UQ_VideoCurtido UNIQUE (perfil_id, titulo_filme)
                        );";
                    using (SqlCommand cmd = new SqlCommand(createVideosCurtidosTable, conn)) { cmd.ExecuteNonQuery(); }

                    string createFilmesTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[filmes]') AND type in (N'U'))
                        CREATE TABLE filmes (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            titulo VARCHAR(150) UNIQUE NOT NULL,
                            genero VARCHAR(100) NOT NULL,
                            ano INT NOT NULL,
                            afinidade VARCHAR(50) NOT NULL,
                            sinopse VARCHAR(MAX) NOT NULL,
                            url_video VARCHAR(500) DEFAULT NULL
                        );";
                    using (SqlCommand cmd = new SqlCommand(createFilmesTable, conn)) { cmd.ExecuteNonQuery(); }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao inicializar a base de dados SQL Server. Certifique-se de que o SQL Server está ativo!\nDetalhes: " + ex.Message, "Erro SQL Server");
            }
        }

        public static bool AccountExists(string username)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM contas WHERE nome_utilizador = @user;", conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public static bool VerifyAccountPassword(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT senha_hash FROM contas WHERE nome_utilizador = @user;", conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    object res = cmd.ExecuteScalar();
                    if (res == null) return false;
                    string hash = res.ToString();
                    return BCrypt.Net.BCrypt.Verify(password, hash);
                }
            }
        }

        public static bool CreateAccount(string username, string hashedPassword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO contas (nome_utilizador, senha_hash) VALUES (@user, @hash);", conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@hash", hashedPassword);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public static string[] GetAllAccounts()
        {
            List<string> list = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT nome_utilizador FROM contas;", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch { }
            return list.ToArray();
        }

        public static string[] GetProfilesForAccount(string username)
        {
            List<string> list = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                string sql = @"
                    SELECT p.nome_perfil 
                    FROM perfis p
                    JOIN contas c ON p.conta_id = c.id
                    WHERE c.nome_utilizador = @user;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", username);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static bool VerifyProfilePassword(string accountUsername, string profileName, string password)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                string sql = @"
                    SELECT p.senha_hash 
                    FROM perfis p
                    JOIN contas c ON p.conta_id = c.id
                    WHERE c.nome_utilizador = @user AND p.nome_perfil = @pname;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", accountUsername);
                    cmd.Parameters.AddWithValue("@pname", profileName);
                    object res = cmd.ExecuteScalar();
                    if (res == null) return false;
                    string hash = res.ToString();
                    return BCrypt.Net.BCrypt.Verify(password, hash);
                }
            }
        }

        public static bool CreateProfile(string accountUsername, string profileName, string hashedPassword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    
                    int contaId = 0;
                    using (SqlCommand cmd = new SqlCommand("SELECT id FROM contas WHERE nome_utilizador = @user;", conn))
                    {
                        cmd.Parameters.AddWithValue("@user", accountUsername);
                        object res = cmd.ExecuteScalar();
                        if (res == null) return false;
                        contaId = Convert.ToInt32(res);
                    }

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO perfis (conta_id, nome_perfil, senha_hash) VALUES (@cid, @name, @hash);", conn))
                    {
                        cmd.Parameters.AddWithValue("@cid", contaId);
                        cmd.Parameters.AddWithValue("@name", profileName);
                        cmd.Parameters.AddWithValue("@hash", hashedPassword);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        private static int GetProfileId(SqlConnection conn, string accountUsername, string profileName)
        {
            string sql = @"
                SELECT p.id 
                FROM perfis p
                JOIN contas c ON p.conta_id = c.id
                WHERE c.nome_utilizador = @user AND p.nome_perfil = @pname;";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@user", accountUsername);
                cmd.Parameters.AddWithValue("@pname", profileName);
                object res = cmd.ExecuteScalar();
                return res != null ? Convert.ToInt32(res) : 0;
            }
        }

        public static string[] GetProfilePreferences(string accountUsername, string profileName)
        {
            List<string> list = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int pid = GetProfileId(conn, accountUsername, profileName);
                using (SqlCommand cmd = new SqlCommand("SELECT genero FROM preferencias WHERE perfil_id = @pid;", conn))
                {
                    cmd.Parameters.AddWithValue("@pid", pid);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static void SaveProfilePreferences(string accountUsername, string profileName, string[] genres)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int pid = GetProfileId(conn, accountUsername, profileName);

                using (SqlCommand cmd = new SqlCommand("DELETE FROM preferencias WHERE perfil_id = @pid;", conn))
                {
                    cmd.Parameters.AddWithValue("@pid", pid);
                    cmd.ExecuteNonQuery();
                }

                foreach (string genre in genres)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO preferencias (perfil_id, genero) VALUES (@pid, @genre);", conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", pid);
                        cmd.Parameters.AddWithValue("@genre", genre);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static string[] GetProfileHistory(string accountUsername, string profileName)
        {
            List<string> list = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int pid = GetProfileId(conn, accountUsername, profileName);
                using (SqlCommand cmd = new SqlCommand("SELECT titulo_filme FROM historico WHERE perfil_id = @pid ORDER BY id DESC;", conn))
                {
                    cmd.Parameters.AddWithValue("@pid", pid);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static void AddMovieToHistory(string accountUsername, string profileName, string movieTitle)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    int pid = GetProfileId(conn, accountUsername, profileName);
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO historico (perfil_id, titulo_filme, data_visualizacao) VALUES (@pid, @title, @date);", conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", pid);
                        cmd.Parameters.AddWithValue("@title", movieTitle);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static string[] GetProfileLikedVideos(string accountUsername, string profileName)
        {
            List<string> list = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int pid = GetProfileId(conn, accountUsername, profileName);
                using (SqlCommand cmd = new SqlCommand("SELECT titulo_filme FROM videos_curtidos WHERE perfil_id = @pid ORDER BY id DESC;", conn))
                {
                    cmd.Parameters.AddWithValue("@pid", pid);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static bool IsVideoLiked(string accountUsername, string profileName, string movieTitle)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int pid = GetProfileId(conn, accountUsername, profileName);
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM videos_curtidos WHERE perfil_id = @pid AND titulo_filme = @title;", conn))
                {
                    cmd.Parameters.AddWithValue("@pid", pid);
                    cmd.Parameters.AddWithValue("@title", movieTitle);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public static void AddVideoToLiked(string accountUsername, string profileName, string movieTitle)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    int pid = GetProfileId(conn, accountUsername, profileName);
                    string sql = @"
                        IF NOT EXISTS (SELECT 1 FROM videos_curtidos WHERE perfil_id = @pid AND titulo_filme = @title)
                        INSERT INTO videos_curtidos (perfil_id, titulo_filme, data_curtida) VALUES (@pid, @title, @date);";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", pid);
                        cmd.Parameters.AddWithValue("@title", movieTitle);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static void RemoveVideoFromLiked(string accountUsername, string profileName, string movieTitle)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    int pid = GetProfileId(conn, accountUsername, profileName);
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM videos_curtidos WHERE perfil_id = @pid AND titulo_filme = @title;", conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", pid);
                        cmd.Parameters.AddWithValue("@title", movieTitle);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static string[] GetMoviesByGenres(string[] genres)
        {
            List<string> list = new List<string>();
            if (genres == null || genres.Length == 0) return list.ToArray();

            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                List<string> paramNames = new List<string>();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                for (int i = 0; i < genres.Length; i++)
                {
                    string paramName = "@g" + i;
                    paramNames.Add(paramName);
                    cmd.Parameters.AddWithValue(paramName, genres[i].Trim());
                }

                cmd.CommandText = "SELECT titulo FROM filmes WHERE genero IN (" + string.Join(",", paramNames) + ") ORDER BY NEWID();";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }
            return list.ToArray();
        }

        public static string[] GetAllMovies()
        {
            List<string> list = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT titulo FROM filmes;", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static DadosFilme GetMovieDetails(string movieTitle)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT titulo, genero, ano, afinidade, sinopse FROM filmes WHERE titulo = @title;", conn))
                {
                    cmd.Parameters.AddWithValue("@title", movieTitle);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new DadosFilme
                            {
                                Titulo = reader.GetString(0),
                                Genero = reader.GetString(1),
                                Ano = reader.GetInt32(2),
                                Afinidade = reader.GetString(3),
                                Sinopse = reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static string GetMovieGenre(string movieTitle)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT genero FROM filmes WHERE titulo = @title;", conn))
                {
                    cmd.Parameters.AddWithValue("@title", movieTitle);
                    object res = cmd.ExecuteScalar();
                    return res != null ? res.ToString() : "";
                }
            }
        }

        public static string GetMovieVideoUrl(string movieTitle)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT url_video FROM filmes WHERE titulo = @title;", conn))
                {
                    cmd.Parameters.AddWithValue("@title", movieTitle);
                    object res = cmd.ExecuteScalar();
                    return res != null ? res.ToString() : "";
                }
            }
        }

        public static bool IsAccountAdmin(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT admin FROM contas WHERE nome_utilizador = @user;", conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        object res = cmd.ExecuteScalar();
                        if (res != null)
                        {
                            int isAdmin = Convert.ToInt32(res);
                            return isAdmin == 1;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
