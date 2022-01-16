using System;
using tabuleiro;
using Xadrez;
using System.Collections.Generic;
using System.Text;

namespace Jogo_Xadrez
{
    class Tela
    {
        public const string SEM_PECA = "\u002D";
        public const string CABECALHO_COLUNAS_TABULEIRO = "  A  B  C  D  E  F  G  H";

        /// <summary>
        /// Inicia a partida de Xadrez
        /// </summary>
        /// <param name="partida"></param>
        public static void IniciarJogo(PartidaDeXadrez partida)
        {
            ImprimirTabuleiro(partida.Tabuleiro);

            Console.WriteLine();
            ImprimirPecasCapturadas(partida);

            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.Turno);
            if (!partida.Terminada)
            {
                Console.WriteLine("Agurdando Jogada: " + partida.JogadorAtual);

                if (partida.Xeque)
                {
                    Console.WriteLine("Xeque!");
                }
            }
            else
            {
                Console.WriteLine("XEQUEMATE!");
                Console.WriteLine("Vencedor: " + partida.JogadorAtual);
            }
        }

        /// <summary>
        /// Imprime tabuleiro e peças
        /// </summary>
        /// <param name="tabuleiro"></param>
        public static void ImprimirTabuleiro(Tabuleiro tabuleiro)
        {
            for (int linha = 0; linha < tabuleiro.Linhas; linha++)
            {
                ImprimeCabecalhoLinha(linha, tabuleiro);
                for (int coluna = 0; coluna < tabuleiro.Colunas; coluna++)
                {
                    TrocarCorConsole(background: ConsoleColor.Black,
                        foreground: tabuleiro.Peca(linha, coluna) == null ? ConsoleColor.Gray : tabuleiro.Peca(linha, coluna).Cor.Equals(Cor.Branca) ? ConsoleColor.Gray : ConsoleColor.DarkYellow);
                    ImprimirPeca(tabuleiro.Peca(linha, coluna));
                }
                Console.WriteLine();
            }

            TrocarCorConsole(ConsoleColor.DarkGray);
            Console.Write(CABECALHO_COLUNAS_TABULEIRO);
            TrocarCorConsole();
        }

        private static void ImprimeCabecalhoLinha(int linha, Tabuleiro tabuleiro)
        {
            TrocarCorConsole(ConsoleColor.DarkGray);
            Console.Write(tabuleiro.Linhas - linha + string.Empty.Space());
            TrocarCorConsole();
        }

        public static void ImprimirMovimentosPossiveis(Tabuleiro tabuleiro, Posicao posicao)
        {
            Peca peca = tabuleiro.Peca(posicao);
            bool[,] mPosicoesPossiveis = peca.MovimentosPossiveis();

            for (int linha = 0; linha < tabuleiro.Linhas; linha++)
            {
                ImprimeCabecalhoLinha(linha, tabuleiro);
                for (int coluna = 0; coluna < tabuleiro.Colunas; coluna++)
                {
                    TrocarCorConsole(background: mPosicoesPossiveis[linha, coluna] ? ConsoleColor.DarkGray : ConsoleColor.Black, 
                        foreground: tabuleiro.Peca(linha, coluna) == null ? ConsoleColor.Gray : tabuleiro.Peca(linha, coluna).Cor.Equals(Cor.Branca) ? ConsoleColor.Gray : ConsoleColor.DarkYellow);
                    ImprimirPeca(tabuleiro.Peca(linha, coluna));
                }

                Console.WriteLine();
            }
            Console.Write(CABECALHO_COLUNAS_TABULEIRO);
            TrocarCorConsole();
        }

        /// <summary>
        /// input posição de origem e destino
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TabuleiroException"></exception>


       

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            Console.WriteLine("Peças capturadas:");
            Console.Write("Brancas: ");
            imprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.WriteLine();
            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            imprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void imprimirConjunto(HashSet<Peca> pecasCapturadas)
        {
            StringBuilder capturadas = new StringBuilder();

            foreach (Peca x in pecasCapturadas)
                capturadas.Append(x.ToString().Space());
            
            Console.Write($"[{ capturadas }]");
        }

        public static void ImprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                ImprimirPosicaoVazia();
                return;
            }
            Console.Write(peca);
            Console.Write(string.Empty.Space());
        }

        private static void ImprimirPosicaoVazia()
        {
            Console.Write(SEM_PECA.Space());
            Console.Write(string.Empty.Space());
        }

        /// <summary>
        /// Troca a cor do Console
        /// </summary>
        /// <param name="fundo"></param>
        /// <param name="caracter"></param>
        private static void TrocarCorConsole(ConsoleColor background = ConsoleColor.Black, ConsoleColor foreground = ConsoleColor.Gray)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }
    }
}
