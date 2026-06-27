using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using BCrypt.Net;

namespace Guryflix.Data
{
    public class MovieData
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string Affinity { get; set; }
        public string Synopsis { get; set; }
    }

    public static class DatabaseContext
    {
        private static string _activeConnectionString = null;

        public static string GetActiveConnectionString()
        {
            if (_activeConnectionString != null)
                return _activeConnectionString;

            string[] masterConnStrings = new string[]
            {
                @"Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;TrustServerCertificate=True;Connection Timeout=3;",
                @"Server=.\SQLEXPRESS;Database=master;Integrated Security=True;TrustServerCertificate=True;Connection Timeout=3;",
                @"Server=localhost;Database=master;Integrated Security=True;TrustServerCertificate=True;Connection Timeout=3;"
            };

            foreach (var connStr in masterConnStrings)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        // Constrói a string de ligação final substituindo master por guryflix
                        string baseConn = connStr.Replace("Database=master;", "Database=guryflix;");
                        _activeConnectionString = baseConn;
                        return _activeConnectionString;
                    }
                }
                catch
                {
                    // Tenta o próximo servidor SQL
                }
            }

            // Fallback padrão se nada for detetado
            _activeConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=guryflix;Integrated Security=True;TrustServerCertificate=True;";
            return _activeConnectionString;
        }

        public static void InitializeDatabase()
        {
            try
            {
                string activeMasterConnStr = GetActiveConnectionString().Replace("Database=guryflix;", "Database=master;");
                
                // 1. Criar a base de dados guryflix se não existir
                using (SqlConnection conn = new SqlConnection(activeMasterConnStr))
                {
                    conn.Open();
                    string checkDbSql = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'guryflix') CREATE DATABASE guryflix;";
                    using (SqlCommand cmd = new SqlCommand(checkDbSql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Criar as tabelas na base de dados guryflix se não existirem
                string guryflixConnStr = GetActiveConnectionString();
                using (SqlConnection conn = new SqlConnection(guryflixConnStr))
                {
                    conn.Open();

                    // Tabela contas
                    string createContasTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[contas]') AND type in (N'U'))
                        CREATE TABLE contas (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            nome_utilizador VARCHAR(150) UNIQUE NOT NULL,
                            senha_hash VARCHAR(255) NOT NULL
                        );";
                    using (SqlCommand cmd = new SqlCommand(createContasTable, conn)) { cmd.ExecuteNonQuery(); }

                    // Tabela perfis
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

                    // Tabela preferencias
                    string createPreferenciasTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[preferencias]') AND type in (N'U'))
                        CREATE TABLE preferencias (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            genero VARCHAR(100) NOT NULL,
                            CONSTRAINT UQ_Preferencia UNIQUE (perfil_id, genero)
                        );";
                    using (SqlCommand cmd = new SqlCommand(createPreferenciasTable, conn)) { cmd.ExecuteNonQuery(); }

                    // Tabela historico
                    string createHistoricoTable = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[historico]') AND type in (N'U'))
                        CREATE TABLE historico (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            titulo_filme VARCHAR(150) NOT NULL,
                            data_visualizacao DATETIME NOT NULL
                        );";
                    using (SqlCommand cmd = new SqlCommand(createHistoricoTable, conn)) { cmd.ExecuteNonQuery(); }

                    // Tabela videos_curtidos
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

                    // Tabela filmes
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

                // 3. Executar o Seeding
                SeedDatabase();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao inicializar a base de dados SQL Server. Certifique-se de que o SQL Server está ativo!\nDetalhes: " + ex.Message, "Erro SQL Server");
            }
        }

        private static void SeedDatabase()
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();

                // Seed de Filmes
                int movieCount = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM filmes;", conn))
                {
                    movieCount = (int)cmd.ExecuteScalar();
                }

                if (movieCount == 0)
                {
                    string movieTitlesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Movie Titles");
                    if (Directory.Exists(movieTitlesDir))
                    {
                        string[] genreFiles = Directory.GetFiles(movieTitlesDir, "*.txt");
                        foreach (string genreFile in genreFiles)
                        {
                            string genreName = Path.GetFileNameWithoutExtension(genreFile);
                            if (genreName.Equals("Movie Posters", StringComparison.OrdinalIgnoreCase)) continue;

                            string[] movieNames = File.ReadAllLines(genreFile);
                            foreach (string rawMovieName in movieNames)
                            {
                                string movieName = rawMovieName.Trim();
                                if (string.IsNullOrEmpty(movieName)) continue;

                                string detailsFile = Path.Combine(movieTitlesDir, "Movie Posters", movieName + ".txt");
                                int year = DateTime.Now.Year;
                                string affinity = "98% Afinidade";
                                string synopsis = "Sinopse não disponível.";

                                if (File.Exists(detailsFile))
                                {
                                    try
                                    {
                                        string[] details = File.ReadAllLines(detailsFile);
                                        if (details.Length > 0)
                                        {
                                            if (details.Length > 1)
                                            {
                                                string[] meta = details[1].Split(' ');
                                                if (meta.Length >= 3)
                                                {
                                                     string pct = meta[0] + " Afinidade";
                                                     affinity = pct;
                                                     int.TryParse(meta[2], out year);
                                                }
                                                else if (meta.Length >= 2)
                                                {
                                                     int.TryParse(meta[1], out year);
                                                }
                                            }
                                            if (details.Length > 2)
                                            {
                                                synopsis = string.Join("\n", details, 2, details.Length - 2).Trim();
                                            }
                                        }
                                    }
                                    catch { }
                                }

                                string insertMovie = @"
                                    IF NOT EXISTS (SELECT 1 FROM filmes WHERE titulo = @title)
                                    INSERT INTO filmes (titulo, genero, ano, afinidade, sinopse) VALUES (@title, @genre, @year, @affinity, @synopsis);";
                                using (SqlCommand cmd = new SqlCommand(insertMovie, conn))
                                {
                                    cmd.Parameters.AddWithValue("@title", movieName);
                                    cmd.Parameters.AddWithValue("@genre", genreName);
                                    cmd.Parameters.AddWithValue("@year", year);
                                    cmd.Parameters.AddWithValue("@affinity", affinity);
                                    cmd.Parameters.AddWithValue("@synopsis", synopsis);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                // Seed de Contas e Perfis Existentes nos ficheiros .txt
                int accountsCount = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM contas;", conn))
                {
                    accountsCount = (int)cmd.ExecuteScalar();
                }

                if (accountsCount == 0)
                {
                    string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                    string accountsFile = Path.Combine(dataDir, "accounts.txt");
                    string passwordsFile = Path.Combine(dataDir, "passwords.txt");

                    if (File.Exists(accountsFile) && File.Exists(passwordsFile))
                    {
                        string[] usernames = File.ReadAllLines(accountsFile);
                        string[] passwords = File.ReadAllLines(passwordsFile);

                        int count = Math.Min(usernames.Length, passwords.Length);
                        for (int i = 0; i < count; i++)
                        {
                            string username = usernames[i].Trim();
                            string passHash = passwords[i].Trim();
                            if (string.IsNullOrEmpty(username)) continue;

                            int contaId = 0;
                            string insertConta = @"
                                IF NOT EXISTS (SELECT 1 FROM contas WHERE nome_utilizador = @user)
                                BEGIN
                                    INSERT INTO contas (nome_utilizador, senha_hash) VALUES (@user, @hash);
                                    SELECT SCOPE_IDENTITY();
                                END
                                ELSE
                                BEGIN
                                    SELECT id FROM contas WHERE nome_utilizador = @user;
                                END";
                            using (SqlCommand cmd = new SqlCommand(insertConta, conn))
                            {
                                cmd.Parameters.AddWithValue("@user", username);
                                cmd.Parameters.AddWithValue("@hash", passHash);
                                object res = cmd.ExecuteScalar();
                                if (res != null) int.TryParse(res.ToString(), out contaId);
                            }

                            string profilesFile = Path.Combine(dataDir, "Profiles", username + "Profiles.txt");
                            string profilePasswordsFile = Path.Combine(dataDir, "Profiles", username + "Passwords.txt");

                            if (File.Exists(profilesFile) && File.Exists(profilePasswordsFile))
                            {
                                string[] profNames = File.ReadAllLines(profilesFile);
                                string[] profPasses = File.ReadAllLines(profilePasswordsFile);
                                int profCount = Math.Min(profNames.Length, profPasses.Length);

                                for (int p = 0; p < profCount; p++)
                                {
                                    string profName = profNames[p].Trim();
                                    string profPass = profPasses[p].Trim();
                                    if (string.IsNullOrEmpty(profName)) continue;

                                    int perfilId = 0;
                                    string insertPerfil = @"
                                        IF NOT EXISTS (SELECT 1 FROM perfis WHERE conta_id = @cid AND nome_perfil = @name)
                                        BEGIN
                                            INSERT INTO perfis (conta_id, nome_perfil, senha_hash) VALUES (@cid, @name, @hash);
                                            SELECT SCOPE_IDENTITY();
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT id FROM perfis WHERE conta_id = @cid AND nome_perfil = @name;
                                        END";
                                    using (SqlCommand cmd = new SqlCommand(insertPerfil, conn))
                                    {
                                        cmd.Parameters.AddWithValue("@cid", contaId);
                                        cmd.Parameters.AddWithValue("@name", profName);
                                        cmd.Parameters.AddWithValue("@hash", profPass);
                                        object res = cmd.ExecuteScalar();
                                        if (res != null) int.TryParse(res.ToString(), out perfilId);
                                    }

                                    string userProfileDir = Path.Combine(dataDir, "Profiles", username, profName);
                                    if (Directory.Exists(userProfileDir))
                                    {
                                        string prefFile = Path.Combine(userProfileDir, "preferences.txt");
                                        if (File.Exists(prefFile))
                                        {
                                            string[] prefs = File.ReadAllLines(prefFile);
                                            foreach (string rawPref in prefs)
                                            {
                                                string pref = rawPref.Trim();
                                                if (string.IsNullOrEmpty(pref)) continue;

                                                string insertPref = @"
                                                    IF NOT EXISTS (SELECT 1 FROM preferencias WHERE perfil_id = @pid AND genero = @genre)
                                                    INSERT INTO preferencias (perfil_id, genero) VALUES (@pid, @genre);";
                                                using (SqlCommand cmd = new SqlCommand(insertPref, conn))
                                                {
                                                    cmd.Parameters.AddWithValue("@pid", perfilId);
                                                    cmd.Parameters.AddWithValue("@genre", pref);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }

                                        string logFile = Path.Combine(userProfileDir, "Log.txt");
                                        if (File.Exists(logFile))
                                        {
                                            string[] logs = File.ReadAllLines(logFile);
                                            foreach (string rawLog in logs)
                                            {
                                                string log = rawLog.Trim();
                                                if (string.IsNullOrEmpty(log)) continue;

                                                string insertHist = "INSERT INTO historico (perfil_id, titulo_filme, data_visualizacao) VALUES (@pid, @title, @date);";
                                                using (SqlCommand cmd = new SqlCommand(insertHist, conn))
                                                {
                                                    cmd.Parameters.AddWithValue("@pid", perfilId);
                                                    cmd.Parameters.AddWithValue("@title", log);
                                                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }

                                        string likedFile = Path.Combine(userProfileDir, "likedVideos.txt");
                                        if (File.Exists(likedFile))
                                        {
                                            string[] likeds = File.ReadAllLines(likedFile);
                                            foreach (string rawLiked in likeds)
                                            {
                                                string liked = rawLiked.Trim();
                                                if (string.IsNullOrEmpty(liked)) continue;

                                                string insertLiked = @"
                                                    IF NOT EXISTS (SELECT 1 FROM videos_curtidos WHERE perfil_id = @pid AND titulo_filme = @title)
                                                    INSERT INTO videos_curtidos (perfil_id, titulo_filme, data_curtida) VALUES (@pid, @title, @date);";
                                                using (SqlCommand cmd = new SqlCommand(insertLiked, conn))
                                                {
                                                    cmd.Parameters.AddWithValue("@pid", perfilId);
                                                    cmd.Parameters.AddWithValue("@title", liked);
                                                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                                    cmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // ==========================================
        // MÉTODOS DE SUPORTE ÀS OPERAÇÕES DA APP
        // ==========================================

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
                    // Obter ID da conta
                    int contaId = 0;
                    using (SqlCommand cmd = new SqlCommand("SELECT id FROM contas WHERE nome_utilizador = @user;", conn))
                    {
                        cmd.Parameters.AddWithValue("@user", accountUsername);
                        object res = cmd.ExecuteScalar();
                        if (res == null) return false;
                        contaId = int.Parse(res.ToString());
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
                return res != null ? int.Parse(res.ToString()) : 0;
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

                // Limpar antigas
                using (SqlCommand cmd = new SqlCommand("DELETE FROM preferencias WHERE perfil_id = @pid;", conn))
                {
                    cmd.Parameters.AddWithValue("@pid", pid);
                    cmd.ExecuteNonQuery();
                }

                // Inserir novas
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

        public static MovieData GetMovieDetails(string movieTitle)
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
                            return new MovieData
                            {
                                Title = reader.GetString(0),
                                Genre = reader.GetString(1),
                                Year = reader.GetInt32(2),
                                Affinity = reader.GetString(3),
                                Synopsis = reader.GetString(4)
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
    }
}
