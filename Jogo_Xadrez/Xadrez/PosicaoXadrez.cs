using tabuleiro;

namespace Xadrez
{
    class PosicaoXadrez
    {
        #region "Variaveis"
        public char Column { get; set; }
        public int Line    { get; set; }
        #endregion

        #region "Construtor"
        public PosicaoXadrez(char column, int line)
        {
            this.Column = column;
            this.Line   = line;
        }
        #endregion

        #region "posicao xadrez"
        /// <summary>
        /// Get Position of board
        /// </summary>
        /// <returns>Position of board</returns>
        public Posicao GetPosition()
        {
            return new Posicao(8 - Line, Column - 'A');
        }
        #endregion

        #region "Override ToString"
        public override string ToString()
        {
            return "" + Column + Line; 
        }
        #endregion
    
    }
}
