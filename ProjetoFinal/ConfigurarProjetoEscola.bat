@echo off
title Configurar Projeto GuryFlix - Escola
cd /d "%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0ConfigurarProjetoEscola.ps1"
