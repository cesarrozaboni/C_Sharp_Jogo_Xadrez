using tabuleiro;

namespace Xadrez
{
    class Bispo : Peca
    {
        public Bispo(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor)
        {
        }

        public override string ToString()
        {
            return PECA_BISPO;
        }

        private bool PodeMover(Posicao posicao)
        {
            Peca peca = Tabuleiro.peca(posicao);
            return peca == null || peca.Cor != base.Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mMovimentos = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            
            //no
            VerificarPosicoes(ref mMovimentos, -1, -1);
            //ne
            VerificarPosicoes(ref mMovimentos, -1, 1);
            //so
            VerificarPosicoes(ref mMovimentos, 1, 1);
            //se
            VerificarPosicoes(ref mMovimentos, 1, -1);

            return mMovimentos;
        }

        private void VerificarPosicoes(ref bool [,] mMovimentos, int somaLinha, int somaColuna)
        {
            var posicao = new Posicao(0, 0);
            posicao.definirValores(Posicao.Linha + somaLinha, Posicao.Coluna + somaColuna);
            while (!Tabuleiro.posicaoValida(posicao) && PodeMover(posicao))
            {
                mMovimentos[posicao.Linha, posicao.Coluna] = true;
                if (Tabuleiro.peca(posicao) == null)
                    break;
                
                if (Tabuleiro.peca(posicao) != null && Tabuleiro.peca(posicao).Cor != Cor)
                    break;
                
                posicao.definirValores(Posicao.Linha + somaLinha, Posicao.Coluna + somaColuna);
            }
        }
    }
}
