using Jogo_Xadrez.Util;
using System;
using tabuleiro;
using Xadrez;

namespace Jogo_Xadrez
{
    static class Program
    {
        #region "Main"
        static void Main(string[] args)
        {
            PartidaDeXadrez chessGame = new PartidaDeXadrez();
            while (!chessGame.EndGame)
            {
                PlayGame(chessGame);

                if (GetOrigin(chessGame, out Posicao positionOrigin))
                    continue;
                
                PrinterPossibleMove(chessGame, positionOrigin);

                if (GetDestiny(positionOrigin, chessGame, out Posicao positionDestiny))
                     continue;

                try
                {
                    chessGame.PlayRound(positionOrigin, positionDestiny);
                }
                catch (TabuleiroException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
            }

            PlayGame(chessGame);
        }
        #endregion

        #region "Play Game"
        private static void PlayGame(PartidaDeXadrez PartidaDeXadrez)
        {
            ClearScreen();
            Console.Title = "JOGO DE XADREZ";
            Tela.PlayGame(PartidaDeXadrez);
        }
        #endregion

        #region "Possible Move"
        private static void PrinterPossibleMove(PartidaDeXadrez partida, Posicao posicao)
        {
            ClearScreen();
            Tela.PrinterBoard(partida.Board, posicao);
            Console.WriteLine();
        }
        #endregion

        #region "Get Origin"
        private static bool GetOrigin(PartidaDeXadrez PartidaDeXadrez, out Posicao posicao)
        {
            Console.Write(MessageGame.msg_Origem);
            
            string inputOrigem = Console.ReadLine().ToUpper();
            var result = PartidaDeXadrez.PositionOriginIsValid(inputOrigem);
            
            posicao = result.Item ?? result.Item;
            return GameUtil.ValidaRetorno(result);
        }
        #endregion

        #region "Get Destiny"
        private static bool GetDestiny(Posicao positionOrigin, PartidaDeXadrez chessGame, out Posicao outPosition)
        {
            Console.Write(MessageGame.msg_Destino);
            string inputDestino = Console.ReadLine().ToUpper();

            var result = chessGame.PositionDestinyIsValid(positionOrigin, inputDestino);
            outPosition = result.Item ?? result.Item;
            return GameUtil.ValidaRetorno(result);
        }
        #endregion

        #region "Clear Screen"
        private static void ClearScreen()
        {
            Console.Clear();
        }
        #endregion
    }
}
