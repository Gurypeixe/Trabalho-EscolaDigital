using System;

namespace Guryflix.Estruturas
{
    class Pilha
    {
        public int topo = -1;
        public string[] elementos = new string[1000];

        public bool EstaVazia()
        {
            return topo < 0;
        }

        public void Empilhar(string dado)
        {
            elementos[++topo] = dado;
        }

        public void Desempilhar()
        {
            if (topo >= 0)
            {
                topo--;
            }
        }

        public string Espreitar()
        {
            return topo >= 0 ? elementos[topo] : "";
        }
    }
}
