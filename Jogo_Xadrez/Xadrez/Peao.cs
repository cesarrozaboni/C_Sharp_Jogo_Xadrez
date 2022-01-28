using tabuleiro;


namespace Xadrez
{
    class Peao : Peca
    {
        private readonly PartidaDeXadrez PartidaXadrez;

        #region "Construtor"
        /// <summary>
        /// Create new piece Peao
        /// </summary>
        /// <param name="board">Board game</param>
        /// <param name="colorPiece">color piece</param>
        public Peao(Tabuleiro board, Cor colorPiece, PartidaDeXadrez partida) : base(board, colorPiece)
        {
            PartidaXadrez = partida;
        }
        #endregion

        #region "Override To String"
        public override string ToString()
        {
            return PECA_PEAO;
        }
        #endregion

        #region "Movimentos Possiveis"
        /// <summary>
        /// Check if has enemy in position
        /// </summary>
        /// <param name="position">position check</param>
        /// <returns>true if has enemy in position</returns>
        private bool HasEnemy(Posicao position)
        {
            Peca peca = Board.GetPiece(position);
            return peca != null && peca.Color != Color;
        }

        /// <summary>
        /// Check if position is empty
        /// </summary>
        /// <param name="position"></param>
        /// <returns>true if is empty</returns>
        private bool PositionIsEmpty(Posicao position)
        {
            return Board.GetPiece(position) == null;
        }

        public override bool[,] PossibleMove()
        {
            bool[,] mPossibleMove = new bool[Board.Line, Board.Column];
            
            if (Color == Cor.Branca)
            {
                SetPossibleMoveAbove(ref mPossibleMove, -1, 0);
                SetPossibleMoveAbove(ref mPossibleMove, -2, 0, AmountMoves);
                SetPossibleMoveSide(ref mPossibleMove, -1, -1);
                SetPossibleMoveSide(ref mPossibleMove, -1, 1);
                
                if (PlayerOneIsEnPassant(Board.Line))
                    ExecuteMoveEnPassantOne(ref mPossibleMove);
            }
            else
            {
                SetPossibleMoveAbove(ref mPossibleMove, 1, 0);
                SetPossibleMoveAbove(ref mPossibleMove, 2, 0, AmountMoves);
                SetPossibleMoveSide(ref mPossibleMove, 1, -1);
                SetPossibleMoveSide(ref mPossibleMove, 1, 1);
                
                if (PlayerTwoIsEnPassant(Board.Line))
                    ExecuteMoveEnPassantTwo(ref mPossibleMove);
            }

            return mPossibleMove;
        }

        private void SetPossibleMoveAbove(ref bool[,] mMov, int sumLine, int sumColumn, int? amountMoves = null)
        {
            Posicao position = new Posicao(0, 0);

            position.SetValue(Position.Line + sumLine, Position.Column + sumColumn);
         
            if (Board.PositionIsValid(position) && PositionIsEmpty(position) && (amountMoves == null || amountMoves == 0))
                mMov[position.Line, position.Column] = true;
        }

        private void SetPossibleMoveSide(ref bool[,] mMov, int sumLine, int sumColumn)
        {
            Posicao position = new Posicao(0, 0);

            position.SetValue(Position.Line + sumLine, Position.Column + sumColumn);

            if (Board.PositionIsValid(position) && HasEnemy(position))
                mMov[position.Line, position.Column] = true;
        }

        #region "Jogada especial EnPassant"
        private void ExecuteMoveEnPassantOne(ref bool[,] mMov)
        {
            Posicao esquerda = new Posicao(Position.Line, Position.Column - 1);
            if (Board.PositionIsValid(esquerda) && HasEnemy(esquerda) && Board.GetPiece(esquerda) == PartidaXadrez.PieceEnPassant)
                mMov[esquerda.Line - 1, esquerda.Column] = true;
            
            Posicao direita = new Posicao(Position.Line, Position.Column + 1);
            if (Board.PositionIsValid(direita) && HasEnemy(direita) && Board.GetPiece(direita) == PartidaXadrez.PieceEnPassant)
                mMov[direita.Line - 1, direita.Column] = true;
        }

        private void ExecuteMoveEnPassantTwo(ref bool[,] mMov)
        {
            Posicao esquerda = new Posicao(Position.Line, Position.Column+ 1);
            if (Board.PositionIsValid(esquerda) && HasEnemy(esquerda) && Board.GetPiece(esquerda) == PartidaXadrez.PieceEnPassant)
                mMov[esquerda.Line + 1, esquerda.Column] = true;
            
            Posicao direita = new Posicao(Position.Line, Position.Column - 1);
            if (Board.PositionIsValid(direita) && HasEnemy(direita) && Board.GetPiece(direita) == PartidaXadrez.PieceEnPassant)
                mMov[direita.Line + 1, direita.Column] = true;
        }

        private bool PlayerOneIsEnPassant(int line)
        {
            return line.Equals(3);
        }

        private bool PlayerTwoIsEnPassant(int line)
        {
            return line.Equals(4);
        }
        #endregion

        #endregion

    }
}
