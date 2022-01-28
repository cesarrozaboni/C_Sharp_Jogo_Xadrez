using tabuleiro;

namespace Xadrez
{
    class Torre : Peca
    {

        #region "Construtor"
        /// <summary>
        /// Create new piece Torre
        /// </summary>
        /// <param name="board">Board game</param>
        /// <param name="colorPiece">color piece</param>
        public Torre(Tabuleiro board, Cor color) : base(board, color)
        {
        }
        #endregion

        #region "Override To String"
        public override string ToString()
        {
            return PECA_TORRE;
        }
        #endregion

        #region "Movimentos Possiveis"
        private bool CanMove(Posicao position)
        {
            Peca peca = Board.GetPiece(position);
            return peca == null || peca.Color != Color;
        }

        public override bool[,] PossibleMove()
        {
            bool[,] mPossibleMove = new bool[Board.Line, Board.Column];

            //up
            MoveIsPossible(ref mPossibleMove, -1, 0);
            //down
            MoveIsPossible(ref mPossibleMove, 1, 0);
            //rigth
            MoveIsPossible(ref mPossibleMove, 0, 1);
            //left
            MoveIsPossible(ref mPossibleMove, 0, -1);
            
            return mPossibleMove;
        }

        private void MoveIsPossible(ref bool[,] mPossibleMove, int sumLine, int sumColumn)
        {
            var position = new Posicao(0, 0);
            position.SetValue(Position.Line + sumLine, Position.Column + sumColumn);

            while (Board.PositionIsValid(position) && CanMove(position))
            {
                mPossibleMove[position.Line, position.Column] = true;
                if (Board.GetPiece(position) != null && Board.GetPiece(position).Color != Color)
                    break;
                
                position.SetValue(position.Line + sumLine, position.Column + sumColumn);
            }
        }
        #endregion

    }
}
