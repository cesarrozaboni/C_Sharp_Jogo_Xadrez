using Jogo_Xadrez.Util;
using System;
using System.Collections.Generic;
using tabuleiro;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public const string CABECALHO_COLUNAS_TABULEIRO = "  A  B  C  D  E  F  G  H";
        public Tabuleiro Tabuleiro { get; private set; }
        public int Turno           { get; private set; }
        public Cor JogadorAtual    { get; private set; }
        public bool Terminada      { get; private set; }
        public bool Xeque          { get; private set; }
        
        private HashSet<Peca>      pecas;
        private HashSet<Peca>      capturadas;
        
        public PartidaDeXadrez()
        {
            Tabuleiro    = new Tabuleiro(8, 8);
            Turno        = 1;
            JogadorAtual = Cor.Branca;
            Terminada    = false;
            Xeque        = false;
            pecas        = new HashSet<Peca>();
            capturadas   = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao posicaoOrigem, Posicao posicaoDestino)
        {
            Peca pecaOrigem = Tabuleiro.RetirarPeca(posicaoOrigem);
            pecaOrigem.IncrementarQtdMovimentos();

            Peca pecaCapturada = Tabuleiro.RetirarPeca(posicaoDestino);
            
            if (pecaCapturada != null)
                capturadas.Add(pecaCapturada);
            
            
            Tabuleiro.ColocarPeca(pecaOrigem, posicaoDestino);

            //#Jogada especial Roque pequeno
            if (pecaOrigem is Rei && posicaoDestino.Coluna == posicaoOrigem.Coluna + 2)
            {
                Posicao origemT = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna + 3);
                Posicao destinoT = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna + 1);
                Peca t = Tabuleiro.RetirarPeca(origemT);
                t.IncrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(t, destinoT);
            }

            //#Jogada especial Roque Grande
            if (pecaOrigem is Rei && posicaoDestino.Coluna == posicaoOrigem.Coluna - 2)
            {
                Posicao origemT = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna - 4);
                Posicao destinoT = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna - 1);
                Peca t = Tabuleiro.RetirarPeca(origemT);
                t.IncrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(t, destinoT);
            }

            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tabuleiro.RetirarPeca(destino);
            p.DecrementarQtdMovimentos();
            if (pecaCapturada != null)
            {
                Tabuleiro.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            Tabuleiro.ColocarPeca(p, origem);

            //#Jogada especial Roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca t = Tabuleiro.RetirarPeca(destinoT);
                t.DecrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(t, origemT);
            }

            //#Jogada especial Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca t = Tabuleiro.RetirarPeca(destinoT);
                t.IncrementarQtdMovimentos();
                Tabuleiro.ColocarPeca(t, origemT);
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Voce Não pode se por em xeque");
            }

            Xeque = estaEmXeque(Adversaria(JogadorAtual));
            
            if (testeXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
                return;
            }
                        
            Turno++;
            MudaJogador();
        }

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }

            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < Tabuleiro.Linhas; i++)
                {
                    for (int j = 0; j < Tabuleiro.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public ResultInfo<Posicao> ValidarPosicaoDeOrigem(string inputPosicao)
        {
            ResultInfo<Posicao> result = new ResultInfo<Posicao>();

            if (ValidaPosicao(inputPosicao))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Posicao_Invalida);
                return result;
            }
                        
            var posicaoXadrez = new PosicaoXadrez(coluna: inputPosicao[0], linha : int.Parse(inputPosicao[1] + ""));
            var posicao = posicaoXadrez.GetPosicao();

            if (!ExistePecaNaPosicao(posicao))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Nao_Existe_Peca_Na_Posicao_Origem);
                return result;
            }

            Peca peca = Tabuleiro.Peca(posicao);

            if (JogadorAtual != peca.Cor)
            {
                result.Exception = new TabuleiroException(MessageGame.msg_A_Peca_De_Origem_Escolhida_Nao_E_Sua);
                return result;
            }

            if (!peca.ExisteMovimentosPossiveis())
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Nao_Ha_Movimentos_Possiveis_Para_A_Peca_De_Origem_Escolhida);
                return result;
            }

            result.Item = posicao;
            return result;
        }

        public ResultInfo<Posicao> validarPosicaoDeDestino(Posicao origem, string inputDestino)
        {
            var result = new ResultInfo<Posicao>();
            if (ValidaPosicao(inputDestino))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Posicao_Invalida);
                return result;
            }
                
            var destinoXadrez = new PosicaoXadrez(coluna: inputDestino[0], linha: int.Parse(inputDestino[1] + ""));
            var posicaoDestino = destinoXadrez.GetPosicao();

            if (!Tabuleiro.Peca(origem).MovimentoPossivel(posicaoDestino))
            {
                result.Exception = new TabuleiroException(MessageGame.msg_Posicao_De_Destino_Invalida);
                return result;
            }

            result.Item = posicaoDestino;
            return result;
        }

        private void MudaJogador()
        {
            JogadorAtual = JogadorAtual.Equals(Cor.Branca) ? Cor.Preta : Cor.Branca;
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }

            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }

            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        /// <summary>
        /// retorna a cor do jogador adversario
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Cor Adversaria(Cor cor)
        {
            return cor.Equals(Cor.Branca) ? Cor.Preta : Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei) // verifica se X pertence a classe de Rei
                {
                    return x;
                }
            }

            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);

            if (R == null)
            {
                throw new TabuleiroException("Não Possui Rei Da Cor " + cor + "Em Jogo");
            }

            foreach (Peca x in pecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public void colocarNovaPeca
            (char coluna, int linha, Peca peca)
        {
            Tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).GetPosicao());
            pecas.Add(peca);
        }

       
        private void colocarPecas()
        {
            colocarNovaPeca('A', 1, new Torre(Tabuleiro, Cor.Branca));
            colocarNovaPeca('B', 1, new Cavalo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('C', 1, new Bispo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('D', 1, new Dama(Tabuleiro, Cor.Branca));
            colocarNovaPeca('E', 1, new Rei(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('F', 1, new Bispo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('G', 1, new Cavalo(Tabuleiro, Cor.Branca));
            colocarNovaPeca('H', 1, new Torre(Tabuleiro, Cor.Branca));
            colocarNovaPeca('A', 2, new Peao(Tabuleiro, Cor.Branca));
            colocarNovaPeca('B', 2, new Peao(Tabuleiro, Cor.Branca));
            colocarNovaPeca('C', 2, new Peao(Tabuleiro, Cor.Branca));
            colocarNovaPeca('D', 2, new Peao(Tabuleiro, Cor.Branca));
            colocarNovaPeca('E', 2, new Peao(Tabuleiro, Cor.Branca));
            colocarNovaPeca('F', 2, new Peao(Tabuleiro, Cor.Branca));
            colocarNovaPeca('G', 2, new Peao(Tabuleiro, Cor.Branca));
            colocarNovaPeca('H', 2, new Peao(Tabuleiro, Cor.Branca));

            colocarNovaPeca('A', 8, new Torre(Tabuleiro, Cor.Preta));
            colocarNovaPeca('B', 8, new Cavalo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('C', 8, new Bispo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('D', 8, new Dama(Tabuleiro, Cor.Preta));
            colocarNovaPeca('E', 8, new Rei(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('F', 8, new Bispo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('G', 8, new Cavalo(Tabuleiro, Cor.Preta));
            colocarNovaPeca('H', 8, new Torre(Tabuleiro, Cor.Preta));
            colocarNovaPeca('A', 7, new Peao(Tabuleiro, Cor.Preta));
            colocarNovaPeca('B', 7, new Peao(Tabuleiro, Cor.Preta));
            colocarNovaPeca('C', 7, new Peao(Tabuleiro, Cor.Preta));
            colocarNovaPeca('D', 7, new Peao(Tabuleiro, Cor.Preta));
            colocarNovaPeca('E', 7, new Peao(Tabuleiro, Cor.Preta));
            colocarNovaPeca('F', 7, new Peao(Tabuleiro, Cor.Preta));
            colocarNovaPeca('G', 7, new Peao(Tabuleiro, Cor.Preta));
            colocarNovaPeca('H', 7, new Peao(Tabuleiro, Cor.Preta));
        }

        private bool ValidaPosicao(string inputPosicao)
        {
            return inputPosicao.Length != 2
                   || inputPosicao.Substring(0, 1).NotIn(CABECALHO_COLUNAS_TABULEIRO.Split(' '))
                   || !int.TryParse(inputPosicao.Substring(1, 1), out _);
        }

        private bool ExistePecaNaPosicao(Posicao posicao)
        {
            return Tabuleiro.Peca(posicao) != null;
        }
    }
}
