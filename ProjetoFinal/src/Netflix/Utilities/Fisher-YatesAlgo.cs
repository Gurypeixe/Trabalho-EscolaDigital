/**
    ** Um algoritmo especial usado para o sistema de recomendações da Guryflix que mimetiza o comportamento do Youtube
    ** com base em preferências
 */
using System;

using Guryflix.Structures;

namespace Guryflix.Utilities
{
    class Fisher_YatesAlgo
    {
        public string[] arr;
        public Fisher_YatesAlgo(string[] arr2)
        {
            arr = new string[arr2.Length];
            try
            {
                for (int i = 0; i < arr2.Length; i++)
                {
                    arr[i] = arr2[i];
                    Console.Write(arr[i] + " ");
                }
            }
            catch
            {
                Console.WriteLine("Tamanho da matriz: " + this.arr.Length);
            }
            randomize(this.arr, arr2.Length);
        }
        static void randomize(string[] arr, int n)
        {
            Random r = new Random();
            for (int i = n - 1; i > 0; i--)
            {
                int j = r.Next(0, i + 1);
                string temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
    }
}
