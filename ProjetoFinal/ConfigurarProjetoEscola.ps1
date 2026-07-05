# Script de configuracao automatica para o PC da Escola (GuryFlix)
# Este script resolve o bloqueio de segurança dos ficheiros (Mark of the Web), restaura as dependencias NuGet e compila o executavel.

Write-Host "==========================================================================" -ForegroundColor Cyan
Write-Host "         CONFIGURADOR DE PROJETO GURYFLIX - PC DA ESCOLA                  " -ForegroundColor Cyan
Write-Host "==========================================================================" -ForegroundColor Cyan
Write-Host ""

$projectRoot = $PSScriptRoot
$slnPath = Join-Path $projectRoot "Guryflix.sln"

# 1. Resolver o bloqueio de segurança do Windows (Mark of the Web) nos ficheiros .resx
Write-Host "[1/3] A remover bloqueios de seguranca dos ficheiros (.resx da Web)..." -ForegroundColor Yellow
Get-ChildItem -Path $projectRoot -Recurse | Unblock-File
Write-Host "-> Bloqueios de seguranca removidos com sucesso!" -ForegroundColor Green
Write-Host ""

# 2. Restaurar dependencias NuGet (WebView2, BCrypt, etc.)
Write-Host "[2/3] A restaurar dependencias e pacotes NuGet da solucao..." -ForegroundColor Yellow
$msbuildPaths = @(
    "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)

if (Test-Path -LiteralPath $slnPath) {
    # Tentar restaurar com dotnet restore
    $restoreSuccess = $false
    try {
        & dotnet restore $slnPath
        Write-Host "-> Pacotes NuGet restaurados com sucesso via dotnet restore!" -ForegroundColor Green
        $restoreSuccess = $true
    } catch {
        Write-Host "-> dotnet restore falhou. A tentar restaurar via MSBuild..." -ForegroundColor Yellow
        foreach ($path in $msbuildPaths) {
            if (Test-Path -LiteralPath $path) {
                & $path $slnPath /t:Restore /p:Configuration=Debug /nologo
                Write-Host "-> Pacotes NuGet restaurados com sucesso via MSBuild!" -ForegroundColor Green
                $restoreSuccess = $true
                break
            }
        }
    }
    if (!$restoreSuccess) {
        Write-Host "AVISO: Nao foi possivel restaurar os pacotes de forma automatica pelo script." -ForegroundColor Red
    }
    Write-Host ""

    # 3. Compilar o projeto automaticamente para gerar o Guryflix.exe e as pastas Dados/Interface no output
    Write-Host "[3/3] A compilar o projeto para gerar o executavel (.exe) e copiar recursos..." -ForegroundColor Yellow
    $buildSuccess = $false
    try {
        & dotnet build $slnPath -c Debug -nologo
        Write-Host "-> Projeto compilado com sucesso via dotnet build!" -ForegroundColor Green
        $buildSuccess = $true
    } catch {
        Write-Host "-> dotnet build falhou. A tentar compilar via MSBuild..." -ForegroundColor Yellow
        foreach ($path in $msbuildPaths) {
            if (Test-Path -LiteralPath $path) {
                & $path $slnPath /t:Build /p:Configuration=Debug /nologo
                Write-Host "-> Projeto compilado com sucesso via MSBuild!" -ForegroundColor Green
                $buildSuccess = $true
                break
            }
        }
    }
    if (!$buildSuccess) {
        Write-Host "AVISO: Nao foi possivel compilar automaticamente pelo script. Abre a solucao no Visual Studio e clica no menu 'Compilar' -> 'Compilar Solucao'." -ForegroundColor Yellow
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
