USE guryflix;
GO

IF NOT EXISTS (SELECT 1 FROM contas WHERE nome_utilizador = 'admin')
BEGIN
    INSERT INTO contas (nome_utilizador, senha_hash, admin) VALUES ('admin', '$2a$11$f5.yH7K4T.G3b16JjG0W1eH6o7KxV0jH6o7KxV0jH6o7KxV0jH6o7', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM contas WHERE nome_utilizador = 'user')
BEGIN
    INSERT INTO contas (nome_utilizador, senha_hash, admin) VALUES ('user', '$2a$11$f5.yH7K4T.G3b16JjG0W1eH6o7KxV0jH6o7KxV0jH6o7KxV0jH6o7', 0);
END
GO

DECLARE @adminId INT;
SELECT @adminId = id FROM contas WHERE nome_utilizador = 'admin';

IF @adminId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM perfis WHERE conta_id = @adminId AND nome_perfil = 'Admin')
BEGIN
    INSERT INTO perfis (conta_id, nome_perfil, senha_hash) VALUES (@adminId, 'Admin', '$2a$11$f5.yH7K4T.G3b16JjG0W1eH6o7KxV0jH6o7KxV0jH6o7KxV0jH6o7');
END
GO

DECLARE @userId INT;
SELECT @userId = id FROM contas WHERE nome_utilizador = 'user';

IF @userId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM perfis WHERE conta_id = @userId AND nome_perfil = 'Espectador')
BEGIN
    INSERT INTO perfis (conta_id, nome_perfil, senha_hash) VALUES (@userId, 'Espectador', '');
END
GO

IF NOT EXISTS (SELECT 1 FROM filmes WHERE titulo = 'The Matrix')
BEGIN
    INSERT INTO filmes (titulo, genero, ano, afinidade, sinopse, url_video) 
    VALUES ('The Matrix', 'Ação', 1999, '99% Afinidade', 'Um programador de computador descobre que a realidade é na verdade uma simulação criada por máquinas e junta-se a uma rebelião.', 'https://www.youtube.com/watch?v=vKQi3bBA1y8');
END
GO

IF NOT EXISTS (SELECT 1 FROM filmes WHERE titulo = 'Inception')
BEGIN
    INSERT INTO filmes (titulo, genero, ano, afinidade, sinopse, url_video) 
    VALUES ('Inception', 'Ação', 2010, '97% Afinidade', 'Um ladrão que invade os sonhos das pessoas para roubar segredos corporativos tem a tarefa de plantar uma ideia na mente de um CEO.', 'https://www.youtube.com/watch?v=YoHD9XEInc0');
END
GO

IF NOT EXISTS (SELECT 1 FROM filmes WHERE titulo = 'Interstellar')
BEGIN
    INSERT INTO filmes (titulo, genero, ano, afinidade, sinopse, url_video) 
    VALUES ('Interstellar', 'Drama', 2014, '98% Afinidade', 'Uma equipa de exploradores viaja através de um buraco de minhoca no espaço numa tentativa de garantir a sobrevivência da humanidade.', 'https://www.youtube.com/watch?v=zSWdZAZE3gU');
END
GO

IF NOT EXISTS (SELECT 1 FROM filmes WHERE titulo = 'Shrek')
BEGIN
    INSERT INTO filmes (titulo, genero, ano, afinidade, sinopse, url_video) 
    VALUES ('Shrek', 'Infantil', 2001, '95% Afinidade', 'Um ogre verde e solitário vê o seu pântano invadido por criaturas de contos de fadas banidas pelo Lorde Farquaad.', 'https://www.youtube.com/watch?v=CwXOrWvSBuM');
END
GO

IF NOT EXISTS (SELECT 1 FROM filmes WHERE titulo = 'The Conjuring')
BEGIN
    INSERT INTO filmes (titulo, genero, ano, afinidade, sinopse, url_video) 
    VALUES ('The Conjuring', 'Terror', 2013, '92% Afinidade', 'Investigadores paranormais trabalham para ajudar uma família aterrorizada por uma presença escura na sua quinta.', 'https://www.youtube.com/watch?v=k10ETZ41q5o');
END
GO
