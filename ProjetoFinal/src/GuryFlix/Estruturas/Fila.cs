using System;

namespace Guryflix.Estruturas
{
    class Fila
    {
        public string[] elementos = new string[1000];
        public int frente = 0;
        public int fim = -1;

        public void Enfileirar(string nomeFicheiro)
        {
            fim++;
            elementos[fim] = nomeFicheiro;
        }

        public string Desenfileirar()
        {
            if (frente > fim)
            {
                return "";
            }
            string val = elementos[frente];
            frente++;
            return val;
        }
    }
}
