using Jogo_Xadrez.Util;
using System;

namespace tabuleiro
{
    /// <summary>
    /// Operações de tabuleiro
    /// </summary>
    class Tabuleiro
    {
        #region "Variaveis"
        /// <summary>
        /// Line of board
        /// </summary>
        public int Line { get; set; }
        /// <summary>
        /// Column of board
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// Pieces of board
        /// </summary>
        private Peca[,] Piece;
        #endregion

        #region "Construtor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="line">line of board</param>
        /// <param name="column">column of board</param>
        public Tabuleiro(int line, int column)
        {
            Line   = line;
            Column = column;
            Piece = new Peca[line, column];
        }
        #endregion

        #region "Operaçoes Peça"
        /// <summary>
        /// Get peca in position of board
        /// </summary>
        /// <param name="line">line board</param>
        /// <param name="column">column board</param>
        /// <returns>return piece of the position of board</returns>
        public Peca GetPiece(Posicao position)
        {
            return Piece[position.Line, position.Column];
        }

        /// <summary>
        /// Add new Piece in position of board
        /// </summary>
        /// <param name="piece">Piece that add</param>
        /// <param name="position">position of new piece</param>
        /// <exception cref="TabuleiroException">position is ocupped</exception>
        public void AddNewPiece(Peca piece, Posicao position)
        {
            if (HasPieceInPosition(position))
            {
                throw new TabuleiroException(MessageGame.msg_Ja_Existe_Peca_Nesta_Posicao);
            }

            Piece[position.Line, position.Column] = piece;
            piece.Position = position;
        }

        /// <summary>
        /// remove piece from board
        /// </summary>
        /// <param name="position">position of piece</param>
        /// <returns>piece removed if have</returns>
        public Peca RemovePiece(Posicao position)
        {
            if(GetPiece(position) == null)
                return null;
            
            Peca pieceRemoved = GetPiece(position);
            pieceRemoved.Position = null;
            Piece[position.Line, position.Column] = null;
            return pieceRemoved;
        }
        #endregion

        #region "Operações Posição"
        /// <summary>
        /// Check if position is valid
        /// </summary>
        /// <param name="position"></param>
        /// <returns>true if is valid</returns>
        public bool PositionIsValid(Posicao position)
        {
            return position.Line >= 0 && position.Line < Line && position.Column >= 0 && position.Column < Column;
        }

        /// <summary>
        /// Check if position is valid
        /// </summary>
        /// <param name="position"></param>
        /// <exception cref="TabuleiroException"></exception>
        public void PositionIsValdWithException(Posicao position)
        {
            if (!PositionIsValid(position))
                throw new TabuleiroException(MessageGame.msg_Posicao_Invalida);
        }
        
        /// <summary>
        /// check if has piece in position of board
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool HasPieceInPosition(Posicao position)
        {
            PositionIsValdWithException(position);
            return GetPiece(position) != null;
        }
        #endregion
    }
}
