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

            string[] ligacoes = new string[]
            {
                @"Server=(localdb)\MSSQLLocalDB;Database=guryflix;Integrated Security=True;TrustServerCertificate=True;",
                @"Server=.\SQLEXPRESS;Database=guryflix;Integrated Security=True;TrustServerCertificate=True;"
            };

            foreach (var ligacao in ligacoes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ligacao))
                    {
                        conn.Open();
                        _activeConnectionString = ligacao;
                        return _activeConnectionString;
                    }
                }
                catch { }
            }

            _activeConnectionString = ligacoes[0];
            return _activeConnectionString;
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    
                    string criarTabelaContas = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[contas]') AND type in (N'U'))
                        CREATE TABLE contas (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            nome_utilizador VARCHAR(150) UNIQUE NOT NULL,
                            senha_hash VARCHAR(255) NOT NULL,
                            admin INT DEFAULT 0 NOT NULL
                        );";
                    using (SqlCommand cmd = new SqlCommand(criarTabelaContas, conn)) { cmd.ExecuteNonQuery(); }

                    string criarTabelaPerfis = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[perfis]') AND type in (N'U'))
                        CREATE TABLE perfis (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            conta_id INT NOT NULL FOREIGN KEY REFERENCES contas(id) ON DELETE CASCADE,
                            nome_perfil VARCHAR(100) NOT NULL,
                            senha_hash VARCHAR(255) NOT NULL,
                            CONSTRAINT UQ_Perfil UNIQUE (conta_id, nome_perfil)
                        );";
                    using (SqlCommand cmd = new SqlCommand(criarTabelaPerfis, conn)) { cmd.ExecuteNonQuery(); }

                    string criarTabelaPreferencias = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[preferencias]') AND type in (N'U'))
                        CREATE TABLE preferencias (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            genero VARCHAR(100) NOT NULL,
                            CONSTRAINT UQ_Preferencia UNIQUE (perfil_id, genero)
                        );";
                    using (SqlCommand cmd = new SqlCommand(criarTabelaPreferencias, conn)) { cmd.ExecuteNonQuery(); }

                    string criarTabelaHistorico = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[historico]') AND type in (N'U'))
                        CREATE TABLE historico (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            titulo_filme VARCHAR(150) NOT NULL,
                            data_visualizacao DATETIME NOT NULL
                        );";
                    using (SqlCommand cmd = new SqlCommand(criarTabelaHistorico, conn)) { cmd.ExecuteNonQuery(); }

                    string criarTabelaVideosCurtidos = @"
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[videos_curtidos]') AND type in (N'U'))
                        CREATE TABLE videos_curtidos (
                            id INT IDENTITY(1,1) PRIMARY KEY,
                            perfil_id INT NOT NULL FOREIGN KEY REFERENCES perfis(id) ON DELETE CASCADE,
                            titulo_filme VARCHAR(150) NOT NULL,
                            data_curtida DATETIME NOT NULL,
                            CONSTRAINT UQ_VideoCurtido UNIQUE (perfil_id, titulo_filme)
                        );";
                    using (SqlCommand cmd = new SqlCommand(criarTabelaVideosCurtidos, conn)) { cmd.ExecuteNonQuery(); }

                    string criarTabelaFilmes = @"
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
                    using (SqlCommand cmd = new SqlCommand(criarTabelaFilmes, conn)) { cmd.ExecuteNonQuery(); }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao inicializar a base de dados SQL Server. Certifique-se de que o SQL Server está ativo!\nDetalhes: " + ex.Message, "Erro SQL Server");
            }
        }

        public static bool AccountExists(string nomeUtilizador)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM contas WHERE nome_utilizador = @utilizador;", conn))
                {
                    cmd.Parameters.AddWithValue("@utilizador", nomeUtilizador);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public static bool VerifyAccountPassword(string nomeUtilizador, string palavraPasse)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT senha_hash FROM contas WHERE nome_utilizador = @utilizador;", conn))
                {
                    cmd.Parameters.AddWithValue("@utilizador", nomeUtilizador);
                    object res = cmd.ExecuteScalar();
                    if (res == null) return false;
                    string hash = res.ToString();
                    return BCrypt.Net.BCrypt.Verify(palavraPasse, hash);
                }
            }
        }

        public static bool CreateAccount(string nomeUtilizador, string senhaHash)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO contas (nome_utilizador, senha_hash) VALUES (@utilizador, @senhaHash);", conn))
                    {
                        cmd.Parameters.AddWithValue("@utilizador", nomeUtilizador);
                        cmd.Parameters.AddWithValue("@senhaHash", senhaHash);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public static string[] GetAllAccounts()
        {
            List<string> lista = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT nome_utilizador FROM contas;", conn))
                    {
                        using (SqlDataReader leitor = cmd.ExecuteReader())
                        {
                            while (leitor.Read())
                            {
                                lista.Add(leitor.GetString(0));
                            }
                        }
                    }
                }
            }
            catch { }
            return lista.ToArray();
        }

        public static string[] GetProfilesForAccount(string nomeUtilizador)
        {
            List<string> lista = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                string sql = @"
                    SELECT p.nome_perfil 
                    FROM perfis p
                    JOIN contas c ON p.conta_id = c.id
                    WHERE c.nome_utilizador = @utilizador;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@utilizador", nomeUtilizador);
                    using (SqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            lista.Add(leitor.GetString(0));
                        }
                    }
                }
            }
            return lista.ToArray();
        }

        public static bool VerifyProfilePassword(string nomeConta, string nomePerfil, string palavraPasse)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                string sql = @"
                    SELECT p.senha_hash 
                    FROM perfis p
                    JOIN contas c ON p.conta_id = c.id
                    WHERE c.nome_utilizador = @utilizador AND p.nome_perfil = @nomePerfil;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@utilizador", nomeConta);
                    cmd.Parameters.AddWithValue("@nomePerfil", nomePerfil);
                    object res = cmd.ExecuteScalar();
                    if (res == null) return false;
                    string hash = res.ToString();
                    return BCrypt.Net.BCrypt.Verify(palavraPasse, hash);
                }
            }
        }

        public static bool CreateProfile(string nomeConta, string nomePerfil, string senhaHash)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    
                    int contaId = 0;
                    using (SqlCommand cmd = new SqlCommand("SELECT id FROM contas WHERE nome_utilizador = @utilizador;", conn))
                    {
                        cmd.Parameters.AddWithValue("@utilizador", nomeConta);
                        object res = cmd.ExecuteScalar();
                        if (res == null) return false;
                        contaId = Convert.ToInt32(res);
                    }

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO perfis (conta_id, nome_perfil, senha_hash) VALUES (@contaId, @nomePerfil, @senhaHash);", conn))
                    {
                        cmd.Parameters.AddWithValue("@contaId", contaId);
                        cmd.Parameters.AddWithValue("@nomePerfil", nomePerfil);
                        cmd.Parameters.AddWithValue("@senhaHash", senhaHash);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        private static int GetProfileId(SqlConnection conn, string nomeConta, string nomePerfil)
        {
            string sql = @"
                SELECT p.id 
                FROM perfis p
                JOIN contas c ON p.conta_id = c.id
                WHERE c.nome_utilizador = @utilizador AND p.nome_perfil = @nomePerfil;";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@utilizador", nomeConta);
                cmd.Parameters.AddWithValue("@nomePerfil", nomePerfil);
                object res = cmd.ExecuteScalar();
                return res != null ? Convert.ToInt32(res) : 0;
            }
        }

        public static string[] GetProfilePreferences(string nomeConta, string nomePerfil)
        {
            List<string> lista = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int perfilId = GetProfileId(conn, nomeConta, nomePerfil);
                using (SqlCommand cmd = new SqlCommand("SELECT genero FROM preferencias WHERE perfil_id = @perfilId;", conn))
                {
                    cmd.Parameters.AddWithValue("@perfilId", perfilId);
                    using (SqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            lista.Add(leitor.GetString(0));
                        }
                    }
                }
            }
            return lista.ToArray();
        }

        public static void SaveProfilePreferences(string nomeConta, string nomePerfil, string[] generos)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int perfilId = GetProfileId(conn, nomeConta, nomePerfil);

                using (SqlCommand cmd = new SqlCommand("DELETE FROM preferencias WHERE perfil_id = @perfilId;", conn))
                {
                    cmd.Parameters.AddWithValue("@perfilId", perfilId);
                    cmd.ExecuteNonQuery();
                }

                foreach (string genero in generos)
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO preferencias (perfil_id, genero) VALUES (@perfilId, @genero);", conn))
                    {
                        cmd.Parameters.AddWithValue("@perfilId", perfilId);
                        cmd.Parameters.AddWithValue("@genero", genero);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static string[] GetProfileHistory(string nomeConta, string nomePerfil)
        {
            List<string> lista = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int perfilId = GetProfileId(conn, nomeConta, nomePerfil);
                using (SqlCommand cmd = new SqlCommand("SELECT titulo_filme FROM historico WHERE perfil_id = @perfilId ORDER BY id DESC;", conn))
                {
                    cmd.Parameters.AddWithValue("@perfilId", perfilId);
                    using (SqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            lista.Add(leitor.GetString(0));
                        }
                    }
                }
            }
            return lista.ToArray();
        }

        public static void AddMovieToHistory(string nomeConta, string nomePerfil, string tituloFilme)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    int perfilId = GetProfileId(conn, nomeConta, nomePerfil);
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO historico (perfil_id, titulo_filme, data_visualizacao) VALUES (@perfilId, @titulo, @data);", conn))
                    {
                        cmd.Parameters.AddWithValue("@perfilId", perfilId);
                        cmd.Parameters.AddWithValue("@titulo", tituloFilme);
                        cmd.Parameters.AddWithValue("@data", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static string[] GetProfileLikedVideos(string nomeConta, string nomePerfil)
        {
            List<string> lista = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int perfilId = GetProfileId(conn, nomeConta, nomePerfil);
                using (SqlCommand cmd = new SqlCommand("SELECT titulo_filme FROM videos_curtidos WHERE perfil_id = @perfilId ORDER BY id DESC;", conn))
                {
                    cmd.Parameters.AddWithValue("@perfilId", perfilId);
                    using (SqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            lista.Add(leitor.GetString(0));
                        }
                    }
                }
            }
            return lista.ToArray();
        }

        public static bool IsVideoLiked(string nomeConta, string nomePerfil, string tituloFilme)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                int perfilId = GetProfileId(conn, nomeConta, nomePerfil);
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM videos_curtidos WHERE perfil_id = @perfilId AND titulo_filme = @titulo;", conn))
                {
                    cmd.Parameters.AddWithValue("@perfilId", perfilId);
                    cmd.Parameters.AddWithValue("@titulo", tituloFilme);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public static void AddVideoToLiked(string nomeConta, string nomePerfil, string tituloFilme)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    int perfilId = GetProfileId(conn, nomeConta, nomePerfil);
                    string sql = @"
                        IF NOT EXISTS (SELECT 1 FROM videos_curtidos WHERE perfil_id = @perfilId AND titulo_filme = @titulo)
                        INSERT INTO videos_curtidos (perfil_id, titulo_filme, data_curtida) VALUES (@perfilId, @titulo, @data);";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@perfilId", perfilId);
                        cmd.Parameters.AddWithValue("@titulo", tituloFilme);
                        cmd.Parameters.AddWithValue("@data", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static void RemoveVideoFromLiked(string nomeConta, string nomePerfil, string tituloFilme)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    int perfilId = GetProfileId(conn, nomeConta, nomePerfil);
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM videos_curtidos WHERE perfil_id = @perfilId AND titulo_filme = @titulo;", conn))
                    {
                        cmd.Parameters.AddWithValue("@perfilId", perfilId);
                        cmd.Parameters.AddWithValue("@titulo", tituloFilme);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        public static string[] GetMoviesByGenres(string[] generos)
        {
            List<string> lista = new List<string>();
            if (generos == null || generos.Length == 0) return lista.ToArray();

            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                List<string> nomesParametros = new List<string>();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                for (int i = 0; i < generos.Length; i++)
                {
                    string nomeParametro = "@g" + i;
                    nomesParametros.Add(nomeParametro);
                    cmd.Parameters.AddWithValue(nomeParametro, generos[i].Trim());
                }

                cmd.CommandText = "SELECT titulo FROM filmes WHERE genero IN (" + string.Join(",", nomesParametros) + ") ORDER BY NEWID();";

                using (SqlDataReader leitor = cmd.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        lista.Add(leitor.GetString(0));
                    }
                }
            }
            return lista.ToArray();
        }

        public static string[] GetAllMovies()
        {
            List<string> lista = new List<string>();
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT titulo FROM filmes;", conn))
                {
                    using (SqlDataReader leitor = cmd.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            lista.Add(leitor.GetString(0));
                        }
                    }
                }
            }
            return lista.ToArray();
        }

        public static DadosFilme GetMovieDetails(string tituloFilme)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT titulo, genero, ano, afinidade, sinopse FROM filmes WHERE titulo = @titulo;", conn))
                {
                    cmd.Parameters.AddWithValue("@titulo", tituloFilme);
                    using (SqlDataReader leitor = cmd.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            return new DadosFilme
                            {
                                Titulo = leitor.GetString(0),
                                Genero = leitor.GetString(1),
                                Ano = leitor.GetInt32(2),
                                Afinidade = leitor.GetString(3),
                                Sinopse = leitor.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static string GetMovieGenre(string tituloFilme)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT genero FROM filmes WHERE titulo = @titulo;", conn))
                {
                    cmd.Parameters.AddWithValue("@titulo", tituloFilme);
                    object res = cmd.ExecuteScalar();
                    return res != null ? res.ToString() : "";
                }
            }
        }

        public static string GetMovieVideoUrl(string tituloFilme)
        {
            using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT url_video FROM filmes WHERE titulo = @titulo;", conn))
                {
                    cmd.Parameters.AddWithValue("@titulo", tituloFilme);
                    object res = cmd.ExecuteScalar();
                    return res != null ? res.ToString() : "";
                }
            }
        }

        public static bool IsAccountAdmin(string nomeUtilizador)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetActiveConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT admin FROM contas WHERE nome_utilizador = @utilizador;", conn))
                    {
                        cmd.Parameters.AddWithValue("@utilizador", nomeUtilizador);
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
