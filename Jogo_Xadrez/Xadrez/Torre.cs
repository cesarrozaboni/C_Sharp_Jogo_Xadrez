using tabuleiro;

namespace Xadrez
{
    class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return PECA_TORRE;
        }

        private bool podeMover(Posicao pos)
        {
            Peca peca = Tabuleiro.peca(pos);
            return peca == null || peca.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mMovimentosPossiveis = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            
            //acima
            MovimentosPossiveis(ref mMovimentosPossiveis, -1, 0);
            //abaixo
            MovimentosPossiveis(ref mMovimentosPossiveis, 1, 0);
            //direita
            MovimentosPossiveis(ref mMovimentosPossiveis, 0, 1);
            //esquerda
            MovimentosPossiveis(ref mMovimentosPossiveis, 0, -1);
            
            return mMovimentosPossiveis;
        }

        private void MovimentosPossiveis(ref bool[,]mMovimentosPossiveis, int somaLinha, int somaColuna)
        {
            var posicao = new Posicao(0, 0);

            posicao.definirValores(posicao.Linha + somaLinha, posicao.Coluna + somaColuna);

            while (!Tabuleiro.posicaoValida(posicao) && podeMover(posicao))
            {
                mMovimentosPossiveis[posicao.Linha, posicao.Coluna] = true;

                if (Tabuleiro.peca(posicao) != null && Tabuleiro.peca(posicao).Cor != Cor)
                {
                    break;
                }
                posicao.Linha += somaLinha;
                posicao.Coluna += somaColuna;
            }
        }
    }
}
