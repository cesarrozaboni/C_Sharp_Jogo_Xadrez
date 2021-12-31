using tabuleiro;

namespace Xadrez
{
    class Rei : Peca
    {
        private PartidaDeXadrez partida;

        public Rei(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return PECA_REI;
        }

        private bool PodeMover(Posicao pos)
        {
            Peca peca = Tabuleiro.peca(pos);
            return peca == null || peca.Cor != Cor;
        }

        private bool TesteTorreParaRoque(Posicao pos)
        {
            Peca peca = Tabuleiro.peca(pos);
            return peca is Torre && peca.Cor == Cor && peca.QtdMovimentos == 0;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mMovimentosPossiveis = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
            
            //acima
            MovimentosPossiveis(ref mMovimentosPossiveis, -1, 0);
            //ne
            MovimentosPossiveis(ref mMovimentosPossiveis, -1, 1);
            //direita
            MovimentosPossiveis(ref mMovimentosPossiveis, 0, 1);
            //se
            MovimentosPossiveis(ref mMovimentosPossiveis, 1, 1);
            //abaixo
            MovimentosPossiveis(ref mMovimentosPossiveis, 1, 0);
            //so
            MovimentosPossiveis(ref mMovimentosPossiveis, 1, -1);
            //esquerda
            MovimentosPossiveis(ref mMovimentosPossiveis, 0, -1);
            //no
            MovimentosPossiveis(ref mMovimentosPossiveis, -1, -1);
            
            //#Jogada especial Roque
            if(PodeJogarRoque())
            {
                //#Roque Pequeno
                Posicao posT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);

                if (TesteTorreParaRoque(posT1))
                {
                    Posicao posicao1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao posicao2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);

                    if(Tabuleiro.peca(posicao1) == null && Tabuleiro.peca(posicao2) == null)
                    {
                        mMovimentosPossiveis[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }
                }

                //#Roque Grande
                Posicao posT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);

                if (TesteTorreParaRoque(posT2))
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);

                    if (Tabuleiro.peca(p1) == null && Tabuleiro.peca(p2) == null && Tabuleiro.peca(p3) == null)
                    {
                        mMovimentosPossiveis[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }
                }
            }

            return mMovimentosPossiveis;
        }
        private void MovimentosPossiveis(ref bool[,]mMovimentosPossiveis, int somaLinha, int somaColuna)
        {
            Posicao posicao = new Posicao(0, 0);
            posicao.definirValores(Posicao.Linha + somaLinha, Posicao.Coluna + somaColuna);

            if(!Tabuleiro.posicaoValida(posicao) && PodeMover(posicao))
                    mMovimentosPossiveis[posicao.Linha, posicao.Coluna] = true;
        }

        private bool PodeJogarRoque()
        {
            return QtdMovimentos.Equals(0) && !partida.xeque;
        }
    }
}
