using System;
using tabuleiro;
using Xadrez;
using System.Collections.Generic;
using System.Text;
using Jogo_Xadrez.Util;

namespace Jogo_Xadrez
{
    class Tela
    {
        #region Variaveis"
        public const string SEM_PECA = "\u002D";
        public const string BOARD_HEADER = "  A  B  C  D  E  F  G  H";
        #endregion

        #region "Start Game"
        /// <summary>
        /// play game chess board
        /// </summary>
        /// <param name="ChessGame"></param>
        public static void PlayGame(PartidaDeXadrez ChessGame)
        {
            PrinterBoard(ChessGame.Board, null);

            Console.WriteLine();
            PrinterArrastedPieces(ChessGame);

            Console.WriteLine();
            Console.WriteLine(MessageGame.msg_Turno_X0.ToFormat(ChessGame.Turn));
          
            if (ChessGame.EndGame)
            {
                Console.WriteLine(MessageGame.msg_Xequemate);
                Console.WriteLine(MessageGame.msg_Vencedor_X0.ToFormat(ChessGame.CurrentPlayer));
                return;
            }
            
            Console.WriteLine(MessageGame.msg_Aguardando_Jogada_X0.ToFormat(ChessGame.CurrentPlayer));

            if (ChessGame.Xeque)
                Console.WriteLine(MessageGame.msg_Xeque);
            
        }

        #endregion

        #region "Imprimir tabuleiro"
        /// <summary>
        /// printer chess board with pieces
        /// </summary>
        /// <param name="board"></param>
        /// <param name="position">if setted get possible move of piece</param>
        public static void PrinterBoard(Tabuleiro board, Posicao position)
        {
            PrinterHeaderColumn();
                        
            bool[,] mPosssibleMove = null;
            if (position != null)
            {
                Peca piece = board.GetPiece(position);
                mPosssibleMove = piece.PossibleMove();
            }

            for (int line = 0; line < board.Line; line++)
            {
                PrinterHeaderLine(line, board);
                for (int column = 0; column < board.Column; column ++)
                {
                    ChangeColorConsole(background: position != null && 
                                                   mPosssibleMove[line, column]?
                                                   board.GetPiece(new Posicao(line, column)) != null ?
                                                   ConsoleColor.Red :
                                                   ConsoleColor.DarkGray:
                                                   ConsoleColor.Black,
                                       foreground: board.GetPiece(new Posicao(line, column)) != null && 
                                                   !board.GetPiece(new Posicao(line, column)).Color.Equals(Cor.Branca)?
                                                   ConsoleColor.DarkYellow:
                                                   ConsoleColor.Gray);

                    PrinterPiece(board.GetPiece(new Posicao(line, column)));
                }
                Console.WriteLine();
            }

            PrinterHeaderColumn();
        }

        #region "Imprimir cabeçalho coluna"
        /// <summary>
        /// printer number line chess board
        /// </summary>
        /// <param name="line"></param>
        /// <param name="board"></param>
        private static void PrinterHeaderColumn()
        {
            ChangeColorConsole(ConsoleColor.Magenta);
            Console.Write(BOARD_HEADER);
            ChangeColorConsole();
            Console.WriteLine();
        }
        #endregion

        #region "Imprimir Cabeçalho de linha"
        /// <summary>
        /// printer number line chess board
        /// </summary>
        /// <param name="line"></param>
        /// <param name="board"></param>
        private static void PrinterHeaderLine(int line, Tabuleiro board)
        {
            ChangeColorConsole(ConsoleColor.Magenta);
            Console.Write(board.Line - line + string.Empty.Space());
            ChangeColorConsole();
        }
        #endregion

        #endregion

        #region "Tela complementar"
        /// <summary>
        /// Printer arrasted pieces in the game
        /// </summary>
        /// <param name="chessGame"></param>
        public static void PrinterArrastedPieces(PartidaDeXadrez chessGame)
        {
            Console.WriteLine(MessageGame.msg_Pecas_Capturadas);
            
            Console.Write(MessageGame.msg_Branca);
            var arrastedWhite = GetArrasted(chessGame.GetArrastedPieces(Cor.Branca));
            Console.Write($"[{ arrastedWhite }]");
            Console.WriteLine();

            Console.Write(MessageGame.msg_Preta);
            ChangeColorConsole(foreground: ConsoleColor.Yellow);
            var arrastedBlack = GetArrasted(chessGame.GetArrastedPieces(Cor.Preta));
            Console.Write($"[{ arrastedBlack }]");
            Console.WriteLine();

            ChangeColorConsole();
        }

        /// <summary>
        /// get arrasted pieces
        /// </summary>
        /// <param name="pecasCapturadas"></param>
        /// <returns>string with pieces of color arrasted</returns>
        public static StringBuilder GetArrasted(HashSet<Peca> piecesArrasted)
        {
            StringBuilder capturadas = new StringBuilder();

            foreach (Peca piece in piecesArrasted)
                capturadas.Append(piece.ToString().Space());

            return capturadas;
        }

        /// <summary>
        /// Printer piece
        /// </summary>
        /// <param name="piece"></param>
        public static void PrinterPiece(Peca piece)
        {
            if (piece == null)
                Console.Write(SEM_PECA.Space());
            else
                Console.Write(piece);

            Console.Write(string.Empty.Space());
        }

  
        /// <summary>
        /// Troca a cor do Console
        /// </summary>
        /// <param name="fundo"></param>
        /// <param name="caracter"></param>
        private static void ChangeColorConsole(ConsoleColor background = ConsoleColor.Black, ConsoleColor foreground = ConsoleColor.Gray)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }
        
        #endregion
    }
}
