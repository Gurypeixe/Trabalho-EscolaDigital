using System;

namespace Guryflix.Utilitarios
{
    public class PesquisaHashing
    {
        string[] filmes;
        public string[] stringFinal;
        public int tamanho;

        public PesquisaHashing(string[] nomesFilmes)
        {
            filmes = nomesFilmes;
            tamanho = nomesFilmes.Length;
            stringFinal = new string[tamanho];
        }

        private int CalcularHash(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return 0;
            return char.ToLower(texto[0]) % 10;
        }

        public void Pesquisar(string termoPesquisa)
        {
            for (int i = 0; i < tamanho; i++)
            {
                stringFinal[i] = null;
            }
            if (string.IsNullOrEmpty(termoPesquisa)) return;

            int codigoHash = CalcularHash(termoPesquisa);
            int indice = 0;

            for (int i = 0; i < tamanho; i++)
            {
                if (filmes[i].ToLower().Contains(termoPesquisa.ToLower()))
                {
                    stringFinal[indice++] = filmes[i];
                }
            }
        }
    }
}
