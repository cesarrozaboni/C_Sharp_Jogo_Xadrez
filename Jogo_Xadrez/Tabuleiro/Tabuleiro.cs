using System;

namespace tabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas  = linhas;
            Colunas = colunas;
            pecas   = new Peca[linhas, colunas];
        }

        public Peca Peca(int linha, int coluna)
        {
            return pecas[linha, coluna];
        }

        public Peca Peca(Posicao posicao)
        {
            return pecas[posicao.Linha, posicao.Coluna];
        }

        public void ColocarPeca(Peca peca, Posicao posicao)
        {
            if (ExistePeca(posicao))
            {
                throw new TabuleiroException("Ja Existe uma peça nesta posição");
            }
            pecas[posicao.Linha, posicao.Coluna] = peca;
            peca.Posicao = posicao;
        }

        /// <summary>
        /// remove piece from board
        /// </summary>
        /// <param name="posicao">position of piece</param>
        /// <returns>piece removed if have</returns>
        public Peca RetirarPeca(Posicao posicao)
        {
            if(Peca(posicao) == null)
            {
                return null;
            }
            Peca aux = Peca(posicao);
            aux.Posicao = null;
            pecas[posicao.Linha, posicao.Coluna] = null;
            return aux;
        }

        public bool PosicaoValida(Posicao posicao)
        {
            return posicao.Linha < 0 || posicao.Linha >= Linhas || posicao.Coluna < 0 || posicao.Coluna >= Colunas;
        }

        public void ValidarPosicao(Posicao pos)
        {
            if (PosicaoValida(pos))
            {
                throw new TabuleiroException("Posição Invalida!");
            }
        }

        public bool ExistePeca(Posicao pos)
        {
            ValidarPosicao(pos);
            return Peca(pos) != null;
        }
    }

}
