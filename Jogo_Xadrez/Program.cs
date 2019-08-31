using System;
using tabuleiro;

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
        }
    }
}
