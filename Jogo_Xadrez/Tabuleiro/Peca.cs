using System;
using tabuleiro;

namespace tabuleiro
{
    abstract class Peca
    {
        public const string PECA_BISPO  = "B ";
        public const string PECA_CAVALO = "C ";
        public const string PECA_DAMA   = "D ";
        public const string PECA_REI    = "R ";
        public const string PECA_TORRE  = "T ";
        public const string PECA_PEAO   = "P ";

        public Posicao Posicao     { get; set; }
        public Cor Cor             { get; protected set; }
        public int QtdMovimentos   { get; protected set; }
        public Tabuleiro Tabuleiro { get; protected set; }

        public Peca(Tabuleiro tabuleiro, Cor cor)
        {
            this.Posicao       = null;
            this.Tabuleiro     = tabuleiro;
            this.Cor           = cor;
            this.QtdMovimentos = 0;
        }

        public void IncrementarQtdMovimentos()
        {
            QtdMovimentos++;
        }

        public void DecrementarQtdMovimentos()
        {
            QtdMovimentos--;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for (int i = 0; i < Tabuleiro.Linhas; i++)
            {
                for (int j = 0; j < Tabuleiro.Colunas; j++)
                {
                    if (mat[i, j])
                       return true;
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();
    }

    
}
