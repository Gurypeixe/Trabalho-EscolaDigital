using System;

namespace Guryflix.Structures
{
    class Stack
    {
        
        int MAX = 1000000;
        public int top;
        string[] stack;
        public bool IsEmpty()
        {
            return (top < 0);
        }
        public Stack()
        {
            top = -1;
            stack = new string[MAX];
        }
        internal bool Push(string data)
        {
            if (top >= MAX)
            {
                Console.WriteLine("Estouro da pilha (Stack Overflow)");
                return false;
            }
            else
            {
                stack[++top] = data;
                Console.WriteLine("{0} foi adicionado à pilha (Pushed)!", stack[top]);
                return true;
            }
        }

        internal void Pop()
        {
            if (top < 0)
                Console.WriteLine("Esvaziamento da pilha (Stack Underflow)");
            else
            
            {
                Console.WriteLine("{0} foi removido da pilha (Popped)!", stack[top--]);
            }
        }

        internal string Peek()
        {
            if (top < 0)
                Console.WriteLine("Esvaziamento da pilha (Stack Underflow)");
            else
            {
                
                return stack[top];
            }
            return "";
        }

        internal void PrintStack()
        {
            if (top < 0)
                Console.WriteLine("Esvaziamento da pilha (Stack Underflow)");
            else
            {
                Console.WriteLine("Os itens na pilha são:");
                for (int i = top; i >= 0; i--)
                    Console.WriteLine(stack[i]);
            }
            return;
        }
    }
}
