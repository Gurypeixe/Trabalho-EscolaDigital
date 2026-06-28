using System;

namespace Guryflix.Structures
{
    class Queue
    {
        public string[] fileNames;
        public int front;
        public int rear;
        private int max = 100000;

        public Queue()
        {
            fileNames = new string[max];
            front = 0;
            rear = -1;
        }
        public void enqueue(string fileName)
        {
            if (rear == max - 1)
            {
                Console.WriteLine("Estouro da fila (Queue Overflow)");
                return;
            }
            else
            {
                rear++;
                fileNames[rear] = fileName;
                Console.WriteLine("Nomes de ficheiros {0} adicionados à fila!", rear);
            }
        }

        public string dequeue()
        {
            if (front == rear + 1)
            {
                Console.WriteLine("A fila está vazia");
                return "";
            }
            string fileName = fileNames[front];
            Console.WriteLine("Nomes de ficheiros {0} removidos da fila!", front);
            front++;
            return fileName;
        }

        
        public void printQueue()
        {
            if (front == rear + 1)
            {
                Console.WriteLine("A fila está vazia");
                return;
            }
            else
            {
                for (int i = front; i <= rear; i++)
                {
                    Console.WriteLine("Nome do ficheiro {0} adicionado à fila!", i);
                }
            }
        }
    }
}
