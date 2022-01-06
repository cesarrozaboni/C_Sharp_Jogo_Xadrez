using Jogo_Xadrez.Util;
using System;
using tabuleiro;
using Xadrez;

namespace Jogo_Xadrez
{
    static class Program
    {
        static void Main(string[] args)
        {
            PartidaDeXadrez PartidaDeXadrez = new PartidaDeXadrez();
            while (!PartidaDeXadrez.Terminada)
            {
                IniciarPartida(PartidaDeXadrez);

                if (ValidarOrigem(PartidaDeXadrez, out Posicao posicaoOrigem))
                    continue;
                
                ImprimirMovimentosPossiveis(PartidaDeXadrez, posicaoOrigem);

                Console.Write(MessageGame.msg_Destino);
                string inputDestino = Console.ReadLine().ToUpper();

                var destino = PartidaDeXadrez.validarPosicaoDeDestino(posicaoOrigem, inputDestino);
                if (GameUtil.ValidaRetorno(destino))
                    continue;
                try
                {
                    PartidaDeXadrez.realizaJogada(posicaoOrigem, destino.Item);
                }
                catch (TabuleiroException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
            }

            IniciarPartida(PartidaDeXadrez);
        }

        private static void IniciarPartida(PartidaDeXadrez PartidaDeXadrez)
        {
            LimparTela();
            Tela.IniciarJogo(PartidaDeXadrez);
        }

        private static void ImprimirMovimentosPossiveis(PartidaDeXadrez partida, Posicao posicao)
        {
            LimparTela();
            Tela.ImprimirMovimentosPossiveis(partida.Tabuleiro, posicao);
            Console.WriteLine();
        }

        private static bool ValidarOrigem(PartidaDeXadrez PartidaDeXadrez, out Posicao posicao)
        {
            Console.Write(MessageGame.msg_Origem);
            string inputOrigem = Console.ReadLine().ToUpper();
            var result = PartidaDeXadrez.ValidarPosicaoDeOrigem(inputOrigem);
            
            posicao = result.Item ?? result.Item;
            return GameUtil.ValidaRetorno(result);
        }

        private static void LimparTela()
        {
            Console.Clear();
        }
    }
}
