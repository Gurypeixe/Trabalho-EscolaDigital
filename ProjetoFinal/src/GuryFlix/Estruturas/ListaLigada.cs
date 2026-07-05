namespace Guryflix.Estruturas
{
    class ListaLigada
    {
        public class No
        {
            public string dado;
            public No seguinte;
            public No(string d)
            {
                dado = d;
                seguinte = null;
            }
        }

        public No cabeca;

        public void InserirInicio(string novoDado)
        {
            No novoNo = new No(novoDado) { seguinte = cabeca };
            cabeca = novoNo;
        }

        public bool Procurar(string procurado)
        {
            No temp = cabeca;
            while (temp != null)
            {
                if (temp.dado == procurado)
                {
                    return true;
                }
                temp = temp.seguinte;
            }
            return false;
        }
    }
}
