namespace Guryflix.Estruturas
{
    class ListaDuplamenteLigada
    {
        public string[] nomesFilmes;

        public class No
        {
            public string nome;
            public No seguinte;
            public No anterior;
        }

        No inicio;

        public void InserirFim(string nomeFilme)
        {
            No novoNo = new No { nome = nomeFilme };

            if (inicio == null)
            {
                novoNo.seguinte = novoNo.anterior = novoNo;
                inicio = novoNo;
                return;
            }

            No ultimo = inicio.anterior;
            novoNo.seguinte = inicio;
            inicio.anterior = novoNo;
            novoNo.anterior = ultimo;
            ultimo.seguinte = novoNo;
        }

        public void GuardarDados(int quantidade)
        {
            nomesFilmes = new string[quantidade];
            No temp = inicio;
            for (int i = 0; i < quantidade; i++)
            {
                nomesFilmes[i] = temp.nome;
                temp = temp.seguinte;
            }
        }
    }
}
