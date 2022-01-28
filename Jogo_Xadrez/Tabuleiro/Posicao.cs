using System;

namespace tabuleiro
{
    class Posicao
    {
        #region "Variaveis"
        /// <summary>
        /// Line Position
        /// </summary>
        public int Line { get; set; }
        /// <summary>
        /// Column Position
        /// </summary>
        public int Column { get; set; }
        #endregion

        #region "Construtor"
        /// <summary>
        /// Constructor Set line and position
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        public Posicao(int line, int column)
        {
            Line   = line;
            Column = column;
        }
        #endregion

        #region "Set Value"
        /// <summary>
        /// set line and column in position
        /// </summary>
        /// <param name="line">line of position</param>
        /// <param name="column">column of position</param>
        public void SetValue(int line, int column)
        {
            Line = line;
            Column = column;
        }
        #endregion

        #region "To String"
        /// <summary>
        /// To string class
        /// </summary>
        /// <returns>Line and Column</returns>
        public override string ToString()
        {
            return Line + ", " + Column;
        }
        #endregion
    }
}
