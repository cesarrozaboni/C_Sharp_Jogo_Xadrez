using tabuleiro;

namespace Xadrez
{
    class Bispo : Peca
    {
        #region "Construtor"
        /// <summary>
        /// Create new piece Bispo
        /// </summary>
        /// <param name="board">Board game</param>
        /// <param name="colorPiece">color piece</param>
        public Bispo(Tabuleiro board, Cor colorPiece) : base(board, colorPiece)
        {
        }
        #endregion

        #region "ToString"
        /// <summary>
        /// Get piece of class
        /// </summary>
        /// <returns>Piece bispo</returns>
        public override string ToString()
        {
            return PECA_BISPO;
        }
        #endregion

        #region "Movimentos Possiveis"
        /// <summary>
        /// can move to position
        /// </summary>
        /// <param name="position">position that check</param>
        /// <returns>true if yes</returns>
        private bool CanMove(Posicao position)
        {
            Peca peca = Board.GetPiece(position);
            return peca == null || peca.Color != base.Color;
        }

        public override bool[,] PossibleMove()
        {
            bool[,] mPossibleMove = new bool[Board.Line, Board.Column];

            //no
            PositionIsValid(ref mPossibleMove, -1, -1);
            //ne
            PositionIsValid(ref mPossibleMove, -1, 1);
            //so
            PositionIsValid(ref mPossibleMove, 1, 1);
            //se
            PositionIsValid(ref mPossibleMove, 1, -1);

            return mPossibleMove;
        }

        /// <summary>
        /// Check if position is valid make increment of line and column
        /// </summary>
        /// <param name="mMove"></param>
        /// <param name="addLine"></param>
        /// <param name="addColumn"></param>
        private void PositionIsValid(ref bool [,] mMove, int addLine, int addColumn)
        {
            var position = new Posicao(0, 0);
            position.SetValue(Position.Line + addLine, Position.Column + addColumn);
            while (Board.PositionIsValid(position) && CanMove(position))
            {
                mMove[position.Line, position.Column] = true;
                if (Board.GetPiece(position) == null)
                {
                    position.SetValue(position.Line + addLine, position.Column + addColumn);
                    continue;
                }
                
                if (Board.GetPiece(position) != null && Board.GetPiece(position).Color != Color)
                    break;
            }
        }
        #endregion
    }
}
