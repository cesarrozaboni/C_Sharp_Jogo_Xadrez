using tabuleiro;

namespace Xadrez
{
    class Dama : Peca
    {
        #region "Construtor"
        /// <summary>
        /// Create new piece Dama
        /// </summary>
        /// <param name="board">Board game</param>
        /// <param name="colorPiece">color piece</param>
        public Dama(Tabuleiro board, Cor colorPiece) : base(board, colorPiece)
        {
        }
        #endregion

        #region "To String"
        /// <summary>
        /// Get piece of class
        /// </summary>
        /// <returns>Piece Dama</returns>
        public override string ToString()
        {
            return PECA_DAMA;
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
            GetPositionValid(ref mPossibleMove, -1, 0);
            //down
            GetPositionValid(ref mPossibleMove, 1, 0);
            //right
            GetPositionValid(ref mPossibleMove, 0, 1);
            //left
            GetPositionValid(ref mPossibleMove, 0, -1);
            //no
            GetPositionValid(ref mPossibleMove, -1, -1);
            //ne
            GetPositionValid(ref mPossibleMove, -1, 1);
            //so
            GetPositionValid(ref mPossibleMove, 1, 1);
            //se
            GetPositionValid(ref mPossibleMove, 1, -1);
            
            return mPossibleMove;
        }

        private void GetPositionValid(ref bool[,] mPossibleMove, int sumLine, int sumColumn)
        {
            Posicao position = new Posicao(0, 0);
            position.SetValue(Position.Line + sumLine, Position.Column + sumColumn);

            while (Board.PositionIsValid(position) && CanMove(position))
            {
                mPossibleMove[position.Line, position.Column] = true;
                if (Board.GetPiece(position) == null)
                {
                    position.SetValue(position.Line + sumLine, position.Column + sumColumn);
                    continue;
                }
                   
                if (Board.GetPiece(position) != null && Board.GetPiece(position).Color != Color)
                    break;
            }
        }
        #endregion
    }
}
