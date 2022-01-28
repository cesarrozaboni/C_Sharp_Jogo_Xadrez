using Jogo_Xadrez.Util;
using System;
using System.Collections.Generic;
using tabuleiro;
using System.Linq;
using Jogo_Xadrez;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        #region "Variaveis"
        public Tabuleiro Board      { get; private set; }
        public int Turn             { get; private set; }
        public Cor CurrentPlayer    { get; private set; }
        public bool EndGame         { get; private set; }
        public bool Xeque           { get; private set; }
        public Peca PieceEnPassant  { get; private set; }
        private int AmountMoves     { get; set; }
        private const int LIMITE_MOVE = 25;

        private readonly HashSet<Peca> HashPiecesInGame;
        private readonly HashSet<Peca> HashPiecesArrasted;
        #endregion

        #region "Constructor"
        /// <summary>
        /// Initialize a new board to game
        /// </summary>
        public PartidaDeXadrez()
        {
            Board              = new Tabuleiro(8, 8);
            Turn               = 1;
            CurrentPlayer      = Cor.Branca;
            EndGame            = false;
            Xeque              = false;
            PieceEnPassant     = null;
            HashPiecesInGame   = new HashSet<Peca>();
            HashPiecesArrasted = new HashSet<Peca>();
            AmountMoves        = 0;
            AddPieceInGame();
        }
        #endregion

        #region "Executa Movimento"
        /// <summary>
        /// Play current turn of game
        /// </summary>
        /// <param name="positionOrigin">position origin of piece</param>
        /// <param name="positionDestiny">position destiny of piece</param>
        /// <exception cref="TabuleiroException"></exception>
        public void PlayRound(Posicao positionOrigin, Posicao positionDestiny)
        {
            Peca pecaCapturada = ExecuteMove(positionOrigin, positionDestiny);

            if (PlayerIsXeque(CurrentPlayer))
            {
                UndoMove(positionOrigin, positionDestiny, pecaCapturada);
                throw new TabuleiroException(MessageGame.msg_Voce_Nao_Pode_Se_Por_Em_Xeque);
            }

            Peca piece = Board.GetPiece(positionDestiny);

            if (pecaCapturada == null)
                AmountMoves++;
            else
                AmountMoves = 0;

            if (PromotionPlay(piece, positionDestiny))
                ExecutePlayPromotion(positionDestiny);

            Xeque = PlayerIsXeque(EnemyColor(CurrentPlayer));

            if (Xeque && PlayIsXequeMate(EnemyColor(CurrentPlayer)))
            {
                Console.WriteLine("FIM DE JOGO, VENCEDOR: " + CurrentPlayer + "!");
                EndGame = true;
                return;
            }

            if(AmountMoves == LIMITE_MOVE)
            {
                Console.WriteLine("EMPATE!");
                EndGame = true;
                return;
            }

            Turn++;
            ChangePlayer();
            PieceEnPassant = IsVulnarableEnPassant(piece, positionOrigin, positionDestiny);
        }

        /// <summary>
        /// execute a moviment of game
        /// </summary>
        /// <param name="posicaoOrigem">input origin of user</param>
        /// <param name="posicaoDestino">input destination of user</param>
        /// <returns>piece captured if have</returns>
        public Peca ExecuteMove(Posicao positionOrigin, Posicao positionDestiny)
        {
            Peca pieceOrigin = Board.RemovePiece(positionOrigin);
            pieceOrigin.IncrementAmountMoves();

            Peca pieceArrasted = Board.RemovePiece(positionDestiny);
            
            if (pieceArrasted != null)
                HashPiecesArrasted.Add(pieceArrasted);
                        
            Board.AddNewPiece(pieceOrigin, positionDestiny);

            if (SmallRoqueIsValid(pieceOrigin, positionDestiny, positionOrigin))
                ExecuteSmallRoque(positionOrigin);
            
            if (BigRoqueIsValid(pieceOrigin, positionDestiny, positionOrigin))
                ExecuteBigRoque(positionOrigin);

            if (EnPassantIsValid(pieceOrigin, positionOrigin, positionDestiny, pieceArrasted))
                ExecuteEnPassant(pieceOrigin, positionDestiny);
           
            return pieceArrasted;
        }

        /// <summary>
        /// Undo move when has one or more error 
        /// </summary>
        /// <param name="positionOrigin"></param>
        /// <param name="positionDestiny"></param>
        /// <param name="pieceArrasted"></param>
        public void UndoMove(Posicao positionOrigin, Posicao positionDestiny, Peca pieceArrasted)
        {
            Peca piece = Board.RemovePiece(positionDestiny);
            piece.DecreaseAmountMoves();

            if (pieceArrasted != null)
            {
                Board.AddNewPiece(pieceArrasted, positionDestiny);
                HashPiecesArrasted.Remove(pieceArrasted);
            }

            Board.AddNewPiece(piece, positionOrigin);

            if (SmallRoqueIsValid(piece, positionDestiny, positionOrigin))
                UndoSmallRoque(positionOrigin);

            if (BigRoqueIsValid(piece, positionDestiny, positionOrigin))
                UndoBigRoque(positionOrigin);

            if(PlayedEnPassant(piece, positionOrigin, positionDestiny, pieceArrasted))
                UndoEnPassant(piece, positionDestiny);
            
        }

        #endregion

        #region "Jogada Especial"

        #region "Small roque"
        /// <summary>
        /// Check if play Small Roque is valid
        /// </summary>
        /// <param name="pieceOrigin">piece of origin</param>
        /// <param name="positionDestiny">position origin</param>
        /// <param name="positionOrigin">position destini</param>
        /// <returns>true if play is valid</returns>
        private bool SmallRoqueIsValid(Peca pieceOrigin, Posicao positionDestiny, Posicao positionOrigin)
        {
            return pieceOrigin is Rei && positionDestiny.Column == positionOrigin.Column + 2;
        }
        
        /// <summary>
        /// Excute play small roque
        /// </summary>
        /// <param name="positionOrigin">position oirigin of piece</param>
        private void ExecuteSmallRoque(Posicao positionOrigin)
        {
            Posicao origin = new Posicao(positionOrigin.Line, positionOrigin.Column + 3);
            Posicao destiny = new Posicao(positionOrigin.Line, positionOrigin.Column + 1);
            Peca piece = Board.RemovePiece(origin);
            piece.IncrementAmountMoves();
            Board.AddNewPiece(piece, destiny);
        }

        /// <summary>
        /// Undo play small roque
        /// </summary>
        /// <param name="positionOrigin">position origin of piece</param>
        private void UndoSmallRoque(Posicao positionOrigin)
        {
            Posicao origin = new Posicao(positionOrigin.Line, positionOrigin.Column + 3);
            Posicao destiny = new Posicao(positionOrigin.Line, positionOrigin.Column + 1);
            Peca piece = Board.RemovePiece(destiny);
            piece.DecreaseAmountMoves();
            Board.AddNewPiece(piece, origin);
        }
        #endregion

        #region "big roque"
        /// <summary>
        /// check if play big roque is valid
        /// </summary>
        /// <param name="pieceOrigin">piece origin</param>
        /// <param name="positionDestiny">position destiny of piece</param>
        /// <param name="positionOrigin">position origin of piece</param>
        /// <returns>true if play big roque is valid</returns>
        private bool BigRoqueIsValid(Peca pieceOrigin, Posicao positionDestiny, Posicao positionOrigin)
        {
            return pieceOrigin is Rei && positionDestiny.Column == positionOrigin.Column - 2;
        }

        /// <summary>
        /// Excute play big roque
        /// </summary>
        /// <param name="positionOrigin">position oirigin of piece</param>
        private void ExecuteBigRoque(Posicao positionOrigin)
        {
            Posicao origin = new Posicao(positionOrigin.Line, positionOrigin.Column - 4);
            Posicao destiny = new Posicao(positionOrigin.Line, positionOrigin.Column - 1);
            Peca piece = Board.RemovePiece(origin);
            piece.IncrementAmountMoves();
            Board.AddNewPiece(piece, destiny);
        }

        /// <summary>
        /// Undo play big roque
        /// </summary>
        /// <param name="positionOrigin">position origin of piece</param>
        private void UndoBigRoque(Posicao positionOrigin)
        {
            Posicao origin = new Posicao(positionOrigin.Line, positionOrigin.Column - 4);
            Posicao destiny = new Posicao(positionOrigin.Line, positionOrigin.Column - 1);
            Peca piece = Board.RemovePiece(destiny);
            piece.DecreaseAmountMoves();
            Board.AddNewPiece(piece, origin);
        }
        #endregion

        #region "EnPassant"
        /// <summary>
        /// Check if play EnPassant is valid
        /// </summary>
        /// <param name="pieceOrigin"></param>
        /// <param name="positionOrigin"></param>
        /// <param name="positionDestiny"></param>
        /// <param name="pieceArrasted"></param>
        /// <returns></returns>
        private bool EnPassantIsValid(Peca pieceOrigin, Posicao positionOrigin, Posicao positionDestiny, Peca pieceArrasted)
        {
            return pieceOrigin is Peao && positionOrigin.Column != positionDestiny.Column && pieceArrasted == null;
        }

        /// <summary>
        /// Execute move EnPassant
        /// </summary>
        /// <param name="pieceOrigin"></param>
        /// <param name="positionDestiny"></param>
        private void ExecuteEnPassant(Peca pieceOrigin, Posicao positionDestiny)
        {
            Posicao position = pieceOrigin.Color.Equals(Cor.Branca) ?
                               new Posicao(positionDestiny.Line + 1, positionDestiny.Column):
                               new Posicao(positionDestiny.Line - 1, positionDestiny.Column);
            
            Peca pecaCapturada = Board.RemovePiece(position);
            HashPiecesArrasted.Add(pecaCapturada);
        }

        /// <summary>
        /// check if current play is EnPassant
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="positionOrigin"></param>
        /// <param name="positionDestiny"></param>
        /// <param name="pieceArrasted"></param>
        private bool PlayedEnPassant(Peca piece, Posicao positionOrigin, Posicao positionDestiny, Peca pieceArrasted)
        {
            return piece is Peao && positionOrigin.Column!= positionDestiny.Column && pieceArrasted == PieceEnPassant;
        }

        /// <summary>
        /// undo the move EnPassant
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="positionDestiny"></param>
        private void UndoEnPassant(Peca piece, Posicao positionDestiny)
        {
            Peca peao = Board.RemovePiece(positionDestiny);
           
            Posicao position = piece.Color == Cor.Branca ?
                               new Posicao(3, positionDestiny.Column):
                               new Posicao(4, positionDestiny.Column);

            Board.AddNewPiece(peao, position);
        }

        /// <summary>
        /// check if piece is vulnerable EnPassant
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="positionOrigin"></param>
        /// <param name="positionDestiny"></param>
        /// <returns></returns>
        private Peca IsVulnarableEnPassant(Peca piece, Posicao positionOrigin, Posicao positionDestiny) 
        {
            return piece is Peao 
                   && (
                         positionDestiny.Line.Equals(positionOrigin.Line - 2) || 
                         positionDestiny.Line.Equals(positionOrigin.Line + 2)
                      ) ? piece : null;

        }
        #endregion

        #region "Promotion Play"
        /// <summary>
        /// check if play is promotion
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="destiny"></param>
        /// <returns>true if is promotion</returns>
        private bool PromotionPlay(Peca piece, Posicao destiny)
        {
            return piece.Color == Cor.Branca && destiny.Line == 0 ||
                   piece.Color == Cor.Preta && destiny.Line == 7;
        }

        /// <summary>
        /// execute promotion play
        /// </summary>
        /// <param name="destiny"></param>
        private void ExecutePlayPromotion(Posicao destiny)
        {
            Peca piece = Board.RemovePiece(destiny);
            HashPiecesInGame.Remove(piece);
            
            Peca dama = new Dama(Board, piece.Color);
            Board.AddNewPiece(dama, destiny);
            
            HashPiecesInGame.Add(dama);
        }
        #endregion

        #endregion

        #region "Valida Check Mate"
        /// <summary>
        /// check if player is check mate
        /// </summary>
        /// <param name="cor"></param>
        /// <returns>true if check mate</returns>
        /// <exception cref="TabuleiroException">not have rei in game</exception>
        public bool PlayerIsXeque(Cor cor)
        {
            Peca rei = GetRei(cor);

            if (rei == null)
                throw new TabuleiroException(MessageGame.msg_Nao_Possui_Rei_Da_Cor_X0_Em_Jogo.ToFormat(cor));

            foreach (Peca piece in GetPiecesInGame(EnemyColor(cor)))
            {
                bool[,] mPossibleMoves = piece.PossibleMove();
                if (mPossibleMoves[rei.Position.Line, rei.Position.Column])
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Play is checkMate
        /// </summary>
        /// <param name="cor"></param>
        /// <returns>true if Xeque Mate</returns>
        public bool PlayIsXequeMate(Cor color)
        {
            foreach (Peca piece in GetPiecesInGame(color))
            {
                bool[,] mPossibleMove = piece.PossibleMove();
                for (int line = 0; line < Board.Line; line ++)
                {
                    for (int column = 0; column < Board.Column; column ++)
                    {
                        if (mPossibleMove[line, column])
                        {
                            Posicao positionOrigin = piece.Position;
                            Posicao positionDestiny = new Posicao(line, column);
                            Peca pieceArrasted = ExecuteMove(positionOrigin, positionDestiny);
                            
                            bool xeque = PlayerIsXeque(color);
                            UndoMove(positionOrigin, positionDestiny, pieceArrasted);
                            
                            if (!xeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region "Validar Posicao"

        #region "Validar Posicao de origem"
        /// <summary>
        /// validate position origin
        /// </summary>
        /// <param name="inputPosicao">player position origin setted</param>
        /// <returns></returns>
        public ResultInfo<Posicao> PositionOriginIsValid(string inputPosicao)
        {
            ResultInfo<Posicao> result = new ResultInfo<Posicao>();

            if (!PositionIsValid(inputPosicao))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Posicao_Invalida);
                return result;
            }
                        
            var positionChess = new PosicaoXadrez(inputPosicao[0], int.Parse(inputPosicao[1] + ""));
            var position = positionChess.GetPosition();

            if (!HasPieceInPosition(position))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Nao_Existe_Peca_Na_Posicao_Origem);
                return result;
            }

            Peca peca = Board.GetPiece(position);

            if (CurrentPlayer != peca.Color)
            {
                result.Exception = new TabuleiroException(MessageGame.msg_A_Peca_De_Origem_Escolhida_Nao_E_Sua);
                return result;
            }

            if (!peca.HasPossibleMoves())
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Nao_Ha_Movimentos_Possiveis_Para_A_Peca_De_Origem_Escolhida);
                return result;
            }

            result.Item = position;
            return result;
        }
        #endregion

        #region "Validar posicao de destino
        public ResultInfo<Posicao> PositionDestinyIsValid(Posicao positionOrigin, string inputDestino)
        {
            var result = new ResultInfo<Posicao>();

            if (!PositionIsValid(inputDestino))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Posicao_Invalida);
                return result;
            }
                
            var detinyChess = new PosicaoXadrez(inputDestino[0], int.Parse(inputDestino[1] + ""));
            var positionDestiny = detinyChess.GetPosition();

            if (!Board.GetPiece(positionOrigin).MoveIsPossble(positionDestiny))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Posicao_De_Destino_Invalida);
                return result;
            }

            result.Item = positionDestiny;
            return result;
        }
        #endregion
        
        /// <summary>
        /// Valid if position inputed in board is valid
        /// </summary>
        /// <param name="inputPosicao">postion inputed by user</param>
        /// <returns>true is ok</returns>
        private bool PositionIsValid(string inputPosicao)
        {
            return inputPosicao.Length == 2 &&
                   inputPosicao.Substring(0, 1).In(Tela.BOARD_HEADER.Split(' '))&&
                   int.TryParse(inputPosicao.Substring(1, 1), out _);
        }

        /// <summary>
        /// check if have piece in the position
        /// </summary>
        /// <param name="posicao">postion inputed by user</param>
        /// <returns>true is ok</returns>
        private bool HasPieceInPosition(Posicao position)
        {
            return Board.GetPiece(position) != null;
        }

        #endregion

        #region "Alterar jogador"
        /// <summary>
        /// change player game
        /// </summary>
        private void ChangePlayer()
        {
            CurrentPlayer = CurrentPlayer.Equals(Cor.Branca) ? Cor.Preta : Cor.Branca;
        }
        #endregion

        #region "Pecas capturadas"
        /// <summary>
        /// Return captured pieces
        /// </summary>
        /// <param name="cor">cor osf current player</param>
        /// <returns>Return captured pieces</returns>
        public HashSet<Peca> GetArrastedPieces(Cor cor)
        {
            HashSet<Peca> hashPlayerPieces = new HashSet<Peca>();
            
            foreach (Peca piece in HashPiecesArrasted.Where(x => x.Color.Equals(cor)).ToList())
            {
                hashPlayerPieces.Add(piece);
            }

            return hashPlayerPieces;
        }
        #endregion

        #region "Pecas em jogo"
        /// <summary>
        /// get pieces of player in game
        /// </summary>
        /// <param name="cor"></param>
        /// <returns>Hash of pieces in game</returns>
        public HashSet<Peca> GetPiecesInGame(Cor cor)
        {
            HashSet<Peca> hashPlayerPieces = new HashSet<Peca>();

            foreach (var piecePlayer in from Peca piece in HashPiecesInGame
                                        where piece.Color.Equals(cor)  
                                        select piece)
            {
                hashPlayerPieces.Add(piecePlayer);
            }

            hashPlayerPieces.ExceptWith(GetArrastedPieces(cor));
            return hashPlayerPieces;
        }
        #endregion

        #region "Cor joagador adversario"
        /// <summary>
        /// return opponent player color
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Cor EnemyColor(Cor color)
        {
            return color.Equals(Cor.Branca) ? Cor.Preta : Cor.Branca;
        }
        #endregion

        #region "Valida rei"
        /// <summary>
        /// Get Rei pieces in the game
        /// </summary>
        /// <param name="cor">Color of player</param>
        /// <returns>Rei of cor</returns>
        private Peca GetRei(Cor cor)
        {
            foreach (Peca piece in GetPiecesInGame(cor))
            {
                if (piece is Rei)
                    return piece;
            }

            return null;
        }
        #endregion

        #region "Adicionar peca na partida"
        public void SetNewPiece(char column, int line, Peca piece)
        {
            Board.AddNewPiece(piece, new PosicaoXadrez(column, line).GetPosition());
            HashPiecesInGame.Add(piece);
        }

        private void AddPieceInGame()
        {
            SetNewPiece('A', 1, new Torre(Board, Cor.Branca));
            SetNewPiece('B', 1, new Cavalo(Board, Cor.Branca));
            SetNewPiece('C', 1, new Bispo(Board, Cor.Branca));
            SetNewPiece('E', 1, new Dama(Board, Cor.Branca));
            SetNewPiece('D', 1, new Rei(Board, Cor.Branca, this));
            SetNewPiece('F', 1, new Bispo(Board, Cor.Branca));
            SetNewPiece('G', 1, new Cavalo(Board, Cor.Branca));
            SetNewPiece('H', 1, new Torre(Board, Cor.Branca));
            SetNewPiece('A', 2, new Peao(Board, Cor.Branca, this));
            SetNewPiece('B', 2, new Peao(Board, Cor.Branca, this));
            SetNewPiece('C', 2, new Peao(Board, Cor.Branca, this));
            SetNewPiece('D', 2, new Peao(Board, Cor.Branca, this));
            SetNewPiece('E', 2, new Peao(Board, Cor.Branca, this));
            SetNewPiece('F', 2, new Peao(Board, Cor.Branca, this));
            SetNewPiece('G', 2, new Peao(Board, Cor.Branca, this));
            SetNewPiece('H', 2, new Peao(Board, Cor.Branca, this));

            SetNewPiece('A', 8, new Torre(Board, Cor.Preta));
            SetNewPiece('B', 8, new Cavalo(Board, Cor.Preta));
            SetNewPiece('C', 8, new Bispo(Board, Cor.Preta));
            SetNewPiece('D', 8, new Dama(Board, Cor.Preta));
            SetNewPiece('E', 8, new Rei(Board, Cor.Preta, this));
            SetNewPiece('F', 8, new Bispo(Board, Cor.Preta));
            SetNewPiece('G', 8, new Cavalo(Board, Cor.Preta));
            SetNewPiece('H', 8, new Torre(Board, Cor.Preta));
            SetNewPiece('A', 7, new Peao(Board, Cor.Preta, this));
            SetNewPiece('B', 7, new Peao(Board, Cor.Preta, this));
            SetNewPiece('C', 7, new Peao(Board, Cor.Preta, this));
            SetNewPiece('D', 7, new Peao(Board, Cor.Preta, this));
            SetNewPiece('E', 7, new Peao(Board, Cor.Preta, this));
            SetNewPiece('F', 7, new Peao(Board, Cor.Preta, this));
            SetNewPiece('G', 7, new Peao(Board, Cor.Preta, this));
            SetNewPiece('H', 7, new Peao(Board, Cor.Preta, this));
        }
        #endregion
    }
}
