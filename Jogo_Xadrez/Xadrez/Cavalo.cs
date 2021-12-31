using tabuleiro;

namespace Xadrez
{
    class Cavalo : Peca
    {
        public Cavalo(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return PECA_CAVALO;
        }

        private bool podeMover(Posicao posicao)
        {
            Peca peca = Tabuleiro.peca(posicao);
            return peca == null || peca.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mMovimentosPossiveis = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            Posicao posicao = new Posicao(0, 0);

            posicao.definirValores(Posicao.Linha - 1, Posicao.Coluna -2);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            posicao.definirValores(Posicao.Linha - 2, Posicao.Coluna - 1);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            posicao.definirValores(Posicao.Linha - 2, Posicao.Coluna + 1);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            posicao.definirValores(Posicao.Linha - 1, Posicao.Coluna + 2);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            posicao.definirValores(Posicao.Linha + 1, Posicao.Coluna + 2);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            posicao.definirValores(Posicao.Linha + 2, Posicao.Coluna + 1);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            posicao.definirValores(Posicao.Linha + 2, Posicao.Coluna - 1);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            posicao.definirValores(Posicao.Linha + 1, Posicao.Coluna - 2);
            PosicoesPossiveis(ref mMovimentosPossiveis, posicao);

            return mMovimentosPossiveis;
        }

        private void PosicoesPossiveis(ref bool[,] mMovimentos, Posicao posicao)
        {
            if (!Tabuleiro.posicaoValida(posicao) && podeMover(posicao))
                mMovimentos[Posicao.Linha, posicao.Coluna] = true;
        }
    }
}
