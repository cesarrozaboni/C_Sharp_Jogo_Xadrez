using tabuleiro;

namespace Xadrez
{
    class Rei : Peca
    {
        private readonly PartidaDeXadrez PartidaXadrez;
        
        #region "Construtor"
        /// <summary>
        /// Create new piece Rei
        /// </summary>
        /// <param name="board">Board game</param>
        /// <param name="colorPiece">color piece</param>
        public Rei(Tabuleiro board, Cor color, PartidaDeXadrez partida) : base(board, color)
        {
            this.PartidaXadrez = partida;
        }
        #endregion

        #region "Override To String"
        public override string ToString()
        {
            return PECA_REI;
        }
        #endregion

        #region "Movimentos Possiveis"
        private bool CanMove(Posicao position)
        {
            Peca piece = Board.GetPiece(position);
            return piece == null || piece.Color != Color;
        }
        
        public override bool[,] PossibleMove()
        {
            bool[,] mPossibleMoves = new bool[Board.Line, Board.Column];
            
            //acima
            MovimentosPossiveis(ref mPossibleMoves, -1, 0);
            //ne
            MovimentosPossiveis(ref mPossibleMoves, -1, 1);
            //direita
            MovimentosPossiveis(ref mPossibleMoves, 0, 1);
            //se
            MovimentosPossiveis(ref mPossibleMoves, 1, 1);
            //abaixo
            MovimentosPossiveis(ref mPossibleMoves, 1, 0);
            //so
            MovimentosPossiveis(ref mPossibleMoves, 1, -1);
            //esquerda
            MovimentosPossiveis(ref mPossibleMoves, 0, -1);
            //no
            MovimentosPossiveis(ref mPossibleMoves, -1, -1);
            
            if (RoqueIsValid())
            {
                if (MoveRoqueIsValid(new Posicao(Position.Line, Position.Column + 3)))
                    PlaySmallRoque(ref mPossibleMoves);

                if (MoveRoqueIsValid(new Posicao(Position.Line, Position.Column - 4)))
                    PlayBigRoque(ref mPossibleMoves);
            }

            return mPossibleMoves;
        }

        #region "Jogada especial Roque"
        private bool RoqueIsValid()
        {
            return AmountMoves.Equals(0) && !PartidaXadrez.Xeque;
        }

        private bool MoveRoqueIsValid(Posicao position)
        {
            if (!Board.PositionIsValid(position))
                return false;

            Peca piece = Board.GetPiece(position);
            return piece is Torre && piece.Color == Color && piece.AmountMoves == 0;
        }

        private void PlaySmallRoque(ref bool[,] mMove)
        {
            Posicao position1 = new Posicao(Position.Line, Position.Column + 1);
            Posicao position2 = new Posicao(Position.Line, Position.Column + 2);

            if (Board.GetPiece(position1) == null && Board.GetPiece(position2) == null)
                mMove[Position.Line, Position.Column + 2] = true;
        }

        private void PlayBigRoque(ref bool[,] mMove)
        {
            Posicao p1 = new Posicao(Position.Line, Position.Column - 1);
            Posicao p2 = new Posicao(Position.Line, Position.Column - 2);
            Posicao p3 = new Posicao(Position.Line, Position.Column - 3);

            if (Board.GetPiece(p1) == null && Board.GetPiece(p2) == null && Board.GetPiece(p3) == null)
                mMove[Position.Line, Position.Column - 2] = true;
        }
        #endregion

        private void MovimentosPossiveis(ref bool[,]mMove, int sumLine, int sumColumn)
        {
            Posicao position = new Posicao(0, 0);
            position.SetValue(Position.Line + sumLine, Position.Column + sumColumn);

            if(Board.PositionIsValid(position) && CanMove(position))
                mMove[position.Line, position.Column] = true;
        }

        #endregion
    }
}
