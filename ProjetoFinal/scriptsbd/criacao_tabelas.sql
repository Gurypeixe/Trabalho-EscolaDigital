IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'guryflix')
BEGIN
    CREATE DATABASE guryflix;
END
GO

USE guryflix;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[contas]') AND type in (N'U'))
BEGIN
    CREATE TABLE contas (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nome_utilizador VARCHAR(150) UNIQUE NOT NULL,
        senha_hash VARCHAR(255) NOT NULL,
        admin INT DEFAULT 0 NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[perfis]') AND type in (N'U'))
BEGIN
    CREATE TABLE perfis (
        id INT IDENTITY(1,1) PRIMARY KEY,
        conta_id INT NOT NULL,
        nome_perfil VARCHAR(100) NOT NULL,
        senha_hash VARCHAR(255) NOT NULL,
        CONSTRAINT FK_Perfis_Contas FOREIGN KEY (conta_id) REFERENCES contas(id) ON DELETE CASCADE,
        CONSTRAINT UQ_Perfil UNIQUE (conta_id, nome_perfil)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[preferencias]') AND type in (N'U'))
BEGIN
    CREATE TABLE preferencias (
        id INT IDENTITY(1,1) PRIMARY KEY,
        perfil_id INT NOT NULL,
        genero VARCHAR(100) NOT NULL,
        CONSTRAINT FK_Preferencias_Perfis FOREIGN KEY (perfil_id) REFERENCES perfis(id) ON DELETE CASCADE,
        CONSTRAINT UQ_Preferencia UNIQUE (perfil_id, genero)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[historico]') AND type in (N'U'))
BEGIN
    CREATE TABLE historico (
        id INT IDENTITY(1,1) PRIMARY KEY,
        perfil_id INT NOT NULL,
        titulo_filme VARCHAR(150) NOT NULL,
        data_visualizacao DATETIME NOT NULL,
        CONSTRAINT FK_Historico_Perfis FOREIGN KEY (perfil_id) REFERENCES perfis(id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[videos_curtidos]') AND type in (N'U'))
BEGIN
    CREATE TABLE videos_curtidos (
        id INT IDENTITY(1,1) PRIMARY KEY,
        perfil_id INT NOT NULL,
        titulo_filme VARCHAR(150) NOT NULL,
        data_curtida DATETIME NOT NULL,
        CONSTRAINT FK_VideosCurtidos_Perfis FOREIGN KEY (perfil_id) REFERENCES perfis(id) ON DELETE CASCADE,
        CONSTRAINT UQ_VideoCurtido UNIQUE (perfil_id, titulo_filme)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[filmes]') AND type in (N'U'))
BEGIN
    CREATE TABLE filmes (
        id INT IDENTITY(1,1) PRIMARY KEY,
        titulo VARCHAR(150) UNIQUE NOT NULL,
        genero VARCHAR(100) NOT NULL,
        ano INT NOT NULL,
        afinidade VARCHAR(50) NOT NULL,
        sinopse VARCHAR(MAX) NOT NULL,
        url_video VARCHAR(500) DEFAULT NULL
    );
END
GO
