using System;
using tabuleiro;
using Xadrez;

namespace Jogo_Xadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Posicao (3, 4);
            Console.WriteLine(p);
            Console.ReadKey();

            Tabuleiro tab = new Tabuleiro(8, 8);

            tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
            tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 3));
            tab.colocarPeca(new Rei(tab, Cor.Preta), new Posicao(2, 4));

            var tela = new Tela();
            tela.imprimirTabuleiro(tab);
            Console.ReadKey();
        }
    }
}
