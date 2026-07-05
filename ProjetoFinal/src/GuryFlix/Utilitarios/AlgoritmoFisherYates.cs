using System;

namespace Guryflix.Utilitarios
{
    class AlgoritmoFisherYates
    {
        public string[] arr;

        public AlgoritmoFisherYates(string[] arrOrigem)
        {
            arr = (string[])arrOrigem.Clone();
            Random r = new Random();
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int j = r.Next(0, i + 1);
                string temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
    }
}
