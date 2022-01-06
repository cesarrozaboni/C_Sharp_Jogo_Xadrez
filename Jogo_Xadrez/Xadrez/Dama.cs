using tabuleiro;

namespace Xadrez
{
    class Dama : Peca
    {
        public Dama(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor)
        {
        }

        public override string ToString()
        {
            return PECA_DAMA;
        }

        private bool podeMover(Posicao pos)
        {
            Peca peca = Tabuleiro.Peca(pos);
            return peca == null || peca.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mMovimentosPossiveis = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

            //up
            VerificarPosicoes(ref mMovimentosPossiveis, -1, 0);
            //down
            VerificarPosicoes(ref mMovimentosPossiveis, 1, 0);
            //right
            VerificarPosicoes(ref mMovimentosPossiveis, 0, 1);
            //left
            VerificarPosicoes(ref mMovimentosPossiveis, 0, -1);
            //no
            VerificarPosicoes(ref mMovimentosPossiveis, -1, -1);
            //ne
            VerificarPosicoes(ref mMovimentosPossiveis, -1, 1);
            //so
            VerificarPosicoes(ref mMovimentosPossiveis, 1, 1);
            //se
            VerificarPosicoes(ref mMovimentosPossiveis, 1, -1);
            
            return mMovimentosPossiveis;
        }

        private void VerificarPosicoes(ref bool[,]mMovimentosPossiveis, int somaLinha, int somaColuna)
        {
            Posicao posicao = new Posicao(0, 0);
            posicao.DefinirValores(Posicao.Linha + somaLinha, Posicao.Coluna + somaColuna);

            while (!Tabuleiro.PosicaoValida(posicao) && podeMover(posicao))
            {
                mMovimentosPossiveis[posicao.Linha, posicao.Coluna] = true;
                if (Tabuleiro.Peca(posicao) == null)
                    break;
                
                if (Tabuleiro.Peca(posicao) != null && Tabuleiro.Peca(posicao).Cor != Cor)
                    break;
                
                posicao.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            }
        }
    }
}
