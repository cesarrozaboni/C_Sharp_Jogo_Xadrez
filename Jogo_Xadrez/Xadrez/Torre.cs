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
            return "T ";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.Linhas, tab.Colunas];
            Posicao pos = new Posicao(0, 0);

            //acima
            pos.definirValores(posicao.Linha - 1, posicao.Coluna);
            while (!tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha--;
            }

            //abaixo
            pos.definirValores(posicao.Linha + 1, posicao.Coluna);
            while (!tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha++;
            }

            //direita
            pos.definirValores(posicao.Linha, posicao.Coluna + 1);
            while (!tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.Coluna++;
            }

            //esquerda
            pos.definirValores(posicao.Linha, posicao.Coluna -1);
            while (!tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;

                if (tab.peca(pos) != null && tab.peca(pos).cor != cor)
                {
                    break;
                }
                pos.Coluna--;
            }
            return mat;
        }
    }
}
