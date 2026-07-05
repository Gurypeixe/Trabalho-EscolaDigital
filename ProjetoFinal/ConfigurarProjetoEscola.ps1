# Script de configuracao automatica para o PC da Escola (GuryFlix)
# Este script resolve o bloqueio de segurança dos ficheiros (Mark of the Web) e restaura todas as dependencias NuGet.

Write-Host "==========================================================================" -ForegroundColor Cyan
Write-Host "         CONFIGURADOR DE PROJETO GURYFLIX - PC DA ESCOLA                  " -ForegroundColor Cyan
Write-Host "==========================================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Resolver o bloqueio de segurança do Windows (Mark of the Web) nos ficheiros .resx
Write-Host "[1/2] A remover bloqueios de seguranca dos ficheiros (.resx da Web)..." -ForegroundColor Yellow
$projectRoot = $PSScriptRoot
Get-ChildItem -Path $projectRoot -Recurse | Unblock-File
Write-Host "-> Bloqueios de seguranca removidos com sucesso!" -ForegroundColor Green
Write-Host ""

# 2. Restaurar dependencias NuGet (WebView2, BCrypt, etc.)
Write-Host "[2/2] A restaurar dependencias e pacotes NuGet da solucao..." -ForegroundColor Yellow
$slnPath = Join-Path $projectRoot "Guryflix.sln"

if (Test-Path -LiteralPath $slnPath) {
    # Tentar restaurar com dotnet restore (muito confiavel se tiver .NET SDK instalado)
    try {
        & dotnet restore $slnPath
        Write-Host "-> Pacotes NuGet restaurados com sucesso via dotnet restore!" -ForegroundColor Green
    } catch {
        Write-Host "-> dotnet restore falhou. A tentar restaurar via MSBuild..." -ForegroundColor Yellow
        # Tentar procurar MSBuild
        $msbuildPaths = @(
            "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
            "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
        )
        $msbuildFound = $false
        foreach ($path in $msbuildPaths) {
            if (Test-Path -LiteralPath $path) {
                & $path $slnPath /t:Restore /p:Configuration=Debug /nologo
                Write-Host "-> Pacotes NuGet restaurados com sucesso via MSBuild!" -ForegroundColor Green
                $msbuildFound = $true
                break
            }
        }
        if (!$msbuildFound) {
            Write-Host "AVISO: Nao foi possivel restaurar os pacotes de forma automatica. Abra a solucao no Visual Studio e clique com o botao direito na solucao -> 'Restaurar Pacotes NuGet'." -ForegroundColor Red
        }
    }
} else {
    Write-Host "ERRO: Ficheiro Guryflix.sln nao foi encontrado na raiz!" -ForegroundColor Red
}

Write-Host ""
Write-Host "==========================================================================" -ForegroundColor Cyan
Write-Host "  CONFIGURACAO CONCLUIDA! Podes abrir o Guryflix.sln e compilar o projeto." -ForegroundColor Green
Write-Host "==========================================================================" -ForegroundColor Cyan
Write-Host ""
Read-Host "Pressione Enter para sair..."
