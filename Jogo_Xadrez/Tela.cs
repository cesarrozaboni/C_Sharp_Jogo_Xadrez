using System;
using tabuleiro;

namespace Jogo_Xadrez
{
    class Tela
    {
        public void imprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                for (int j = 0; j < tab.Colunas; j++)
                {
                    Console.Write(tab.peca(i, j) == null ? "-" : tab.peca(i, j) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
