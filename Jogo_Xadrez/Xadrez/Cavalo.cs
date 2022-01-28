using tabuleiro;

namespace Xadrez
{
    class Cavalo : Peca
    {
        #region "Construtor"
        /// <summary>
        /// Create new piece Cavalo
        /// </summary>
        /// <param name="board">Board game</param>
        /// <param name="colorPiece">color piece</param>
        public Cavalo(Tabuleiro board, Cor colorPiece) : base(board, colorPiece)
        {
        }
        #endregion

        #region "Override To String"
        public override string ToString()
        {
            return PECA_CAVALO;
        }
        #endregion

        #region "Movimentos Possiveis"
        /// <summary>
        /// Check if has piece in position
        /// </summary>
        /// <param name="position">position that valid</param>
        /// <returns>true if is valid</returns>
        private bool CanMove(Posicao position)
        {
            Peca piece = Board.GetPiece(position);
            return piece == null || piece.Color != Color;
        }

        public override bool[,] PossibleMove()
        {
            bool[,] mPossibleMove = new bool[Board.Line, Board.Column];
            Posicao position = new Posicao(0, 0);

            position.SetValue(Position.Line - 1, Position.Column -2);
            PositionIsPossible(ref mPossibleMove, position);

            position.SetValue(Position.Line - 2, Position.Column - 1);
            PositionIsPossible(ref mPossibleMove, position);

            position.SetValue(Position.Line - 2, Position.Column + 1);
            PositionIsPossible(ref mPossibleMove, position);

            position.SetValue(Position.Line - 1, Position.Column + 2);
            PositionIsPossible(ref mPossibleMove, position);

            position.SetValue(Position.Line + 1, Position.Column + 2);
            PositionIsPossible(ref mPossibleMove, position);

            position.SetValue(Position.Line + 2, Position.Column + 1);
            PositionIsPossible(ref mPossibleMove, position);

            position.SetValue(Position.Line + 2, Position.Column - 1);
            PositionIsPossible(ref mPossibleMove, position);

            position.SetValue(Position.Line + 1, Position.Column - 2);
            PositionIsPossible(ref mPossibleMove, position);

            return mPossibleMove;
        }

        private void PositionIsPossible(ref bool[,] mMovimentos, Posicao position)
        {
            if (Board.PositionIsValid(position) && CanMove(position))
                mMovimentos[position.Line, position.Column] = true;
        }
        #endregion
    }
}
