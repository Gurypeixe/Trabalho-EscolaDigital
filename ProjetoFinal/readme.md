# Guryflix — Plataforma de Streaming de Filmes

Este é o projeto **Guryflix**, uma plataforma desktop de catálogo e reprodução de trailers de filmes desenvolvida em C# Windows Forms com base de dados SQL Server.

## Estrutura do Repositório

- `docs/` — Pasta contendo a documentação obrigatória do projeto (Proposta, Relatório Técnico, Manual do Utilizador e Apresentação).
- `scriptsbd/` — Scripts SQL para base de dados:
  - `criacao_tabelas.sql` — Criação das tabelas no SQL Server.
  - `carga_dados.sql` — Inserção de dados de teste (Seeding).
- `src/` — Código fonte da Solução do Visual Studio (`Guryflix.sln`).
- `dist/` — Versão compilada e pronta a executar (contém os binários finais e pastas de dados).

## Requisitos de Execução

1. **Linguagem**: C# (.NET Framework 4.8)
2. **Interface**: Windows Forms
3. **Base de Dados**: SQL Server

No arranque da aplicação, a base de dados `guryflix` será inicializada e carregada com dados de teste de forma automática.
