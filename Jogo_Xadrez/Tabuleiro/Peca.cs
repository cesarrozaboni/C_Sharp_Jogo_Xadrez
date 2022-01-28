namespace tabuleiro
{
    abstract class Peca
    {
        #region "Constantes"
        public const string PECA_BISPO  = "B ";
        public const string PECA_CAVALO = "C ";
        public const string PECA_DAMA   = "D ";
        public const string PECA_REI    = "R ";
        public const string PECA_TORRE  = "T ";
        public const string PECA_PEAO   = "P ";
        #endregion

        #region "Variaveis"
        /// <summary>
        /// position of piece
        /// </summary>
        public Posicao Position { get; set; }
       /// <summary>
       /// Color of piece
       /// </summary>
        public Cor Color        { get; protected set; }
        /// <summary>
        /// Amount move pieces
        /// </summary>
        public int AmountMoves  { get; protected set; }
        /// <summary>
        /// board of game
        /// </summary>
        public Tabuleiro Board  { get; protected set; }
        #endregion

        #region "Construtor"
        /// <summary>
        /// Constructor Piece
        /// </summary>
        /// <param name="board">board game</param>
        /// <param name="color"> color piece</param>
        protected Peca(Tabuleiro board, Cor colorPiece)
        {
            this.Position    = null;
            this.Board       = board;
            this.Color       = colorPiece;
            this.AmountMoves = 0;
        }
        #endregion

        #region "Count Turn Game"
        /// <summary>
        /// Sum amount moves of game
        /// </summary>
        public void IncrementAmountMoves()
        {
            AmountMoves++;
        }

        /// <summary>
        /// Decrease amount moves of game
        /// </summary>
        public void DecreaseAmountMoves()
        {
            AmountMoves--;
        }
        #endregion

        #region "Possible Move"
        /// <summary>
        /// check possible moves of piece
        /// </summary>
        /// <returns>Possible moves of piece</returns>
        public bool HasPossibleMoves()
        {
            bool[,] mMovimentosPossiveis = PossibleMove();
            for (int linha = 0; linha < Board.Line; linha++)
            {
                for (int coluna = 0; coluna < Board.Column; coluna++)
                {
                    if (mMovimentosPossiveis[linha, coluna])
                       return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if position has possible move
        /// </summary>
        /// <param name="posicao">position of test</param>
        /// <returns>true if has possivle move in position</returns>
        public bool MoveIsPossble(Posicao posicao)
        {
            return PossibleMove()[posicao.Line, posicao.Column];
        }

        /// <summary>
        /// Abstract Method to get possible moves
        /// </summary>
        /// <returns>possible Moves of pieces</returns>
        public abstract bool[,] PossibleMove();
        #endregion
    }
}
