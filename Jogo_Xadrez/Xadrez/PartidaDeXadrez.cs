using Jogo_Xadrez.Util;
using System;
using System.Collections.Generic;
using tabuleiro;
using System.Linq;

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
        public Peca VulneravelEnPassant;
        
        private readonly HashSet<Peca> HashPecasEmJogo;
        private readonly HashSet<Peca> HashPecasCapturadas;

        #region "Constructor"
        public PartidaDeXadrez()
        {
            Tabuleiro    = new Tabuleiro(8, 8);
            Turno        = 1;
            JogadorAtual = Cor.Branca;
            Terminada    = false;
            Xeque        = false;
            VulneravelEnPassant = null;
            HashPecasEmJogo     = new HashSet<Peca>();
            HashPecasCapturadas = new HashSet<Peca>();
            colocarPecas();
        }
        #endregion

        #region "Executa Movimento"
        /// <summary>
        /// execute a moviment of game
        /// </summary>
        /// <param name="posicaoOrigem">input origin of user</param>
        /// <param name="posicaoDestino">input destination of user</param>
        /// <returns>piece captured if have</returns>
        public Peca ExecutaMovimento(Posicao posicaoOrigem, Posicao posicaoDestino)
        {
            Peca pecaOrigem = Tabuleiro.RetirarPeca(posicaoOrigem);
            pecaOrigem.IncrementarQtdMovimentos();

            Peca pecaCapturada = Tabuleiro.RetirarPeca(posicaoDestino);
            
            if (pecaCapturada != null)
                HashPecasCapturadas.Add(pecaCapturada);
                        
            Tabuleiro.ColocarPeca(pecaOrigem, posicaoDestino);

            if (JogadaRoquePequeno(pecaOrigem, posicaoDestino, posicaoOrigem))
                ExecutaRoquePequeno(posicaoOrigem);
            
            if (JogadaRoqueGrande(pecaOrigem, posicaoDestino, posicaoOrigem))
                ExecutaRoqueGrande(posicaoOrigem);

            if (pecaOrigem is Peao)
            {
                if (posicaoOrigem.Coluna != posicaoDestino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if(pecaOrigem.Cor.Equals(Cor.Branca))
                    {
                        posP = new Posicao(posicaoDestino.Linha + 1, posicaoDestino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(posicaoDestino.Linha - 1, posicaoDestino.Coluna);
                    }

                    pecaCapturada = Tabuleiro.RetirarPeca(posP);
                    HashPecasCapturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao posicaoOrigem, Posicao posicaoDestino, Peca pecaCapturada)
        {
            Peca peca = Tabuleiro.RetirarPeca(posicao: posicaoDestino);
            peca.DecrementarQtdMovimentos();

            if (pecaCapturada != null)
            {
                Tabuleiro.ColocarPeca(pecaCapturada, posicaoDestino);
                HashPecasCapturadas.Remove(pecaCapturada);
            }

            Tabuleiro.ColocarPeca(peca, posicaoOrigem);

            if (JogadaRoquePequeno(peca, posicaoDestino, posicaoOrigem))
                VoltarRoquePequeno(posicaoOrigem);

            if (JogadaRoqueGrande(peca, posicaoDestino, posicaoOrigem))
                VoltarRoqueGrande(posicaoOrigem);

            if(peca is Peao)
            {
                if(posicaoOrigem.Coluna != posicaoDestino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tabuleiro.RetirarPeca(posicaoDestino);
                    Posicao posP;
                    if(peca.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, posicaoDestino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, posicaoDestino.Coluna);
                    }

                    Tabuleiro.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (JogadorEstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException(MessageGame.msg_Voce_Nao_Pode_Se_Por_Em_Xeque);
            }

            Peca peca = Tabuleiro.Peca(destino);

            if(peca is Peao)
            {
                if(peca.Cor == Cor.Branca && destino.Linha == 0 ||
                     peca.Cor == Cor.Preta && destino.Linha == 7)
                {
                    peca = Tabuleiro.RetirarPeca(destino);
                    HashPecasEmJogo.Remove(peca);
                    Peca dama = new Dama(Tabuleiro, peca.Cor);
                    Tabuleiro.ColocarPeca(dama, destino);
                    HashPecasEmJogo.Add(dama);
                }
            }

            Xeque = JogadorEstaEmXeque(Adversaria(JogadorAtual));

            if (Xeque && JogadaXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
                return;
            }

            Turno++;
            MudaJogador();

            

            if (peca is Peao && (destino.Linha.Equals(origem.Linha - 2) ||
               destino.Linha.Equals(origem.Linha + 2)))
            {
                VulneravelEnPassant = peca;
            }
            else
            {
                VulneravelEnPassant = null;
            }
        }
        #endregion

        #region "Jogada Especial"
        private bool JogadaRoquePequeno(Peca pecaOrigem, Posicao posicaoDestino, Posicao posicaoOrigem)
        {
            return pecaOrigem is Rei && posicaoDestino.Coluna == posicaoOrigem.Coluna + 2;
        }
        
        private void ExecutaRoquePequeno(Posicao posicaoOrigem)
        {
            Posicao origem = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna + 3);
            Posicao destino = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna + 1);
            Peca peca = Tabuleiro.RetirarPeca(origem);
            peca.IncrementarQtdMovimentos();
            Tabuleiro.ColocarPeca(peca, destino);
        }

        private void VoltarRoquePequeno(Posicao posicaoOrigem)
        {
            Posicao origem = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna + 3);
            Posicao destino = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna + 1);
            Peca peca = Tabuleiro.RetirarPeca(destino);
            peca.DecrementarQtdMovimentos();
            Tabuleiro.ColocarPeca(peca, origem);
        }

        private bool JogadaRoqueGrande(Peca pecaOrigem, Posicao posicaoDestino, Posicao posicaoOrigem)
        {
            return pecaOrigem is Rei && posicaoDestino.Coluna == posicaoOrigem.Coluna - 2;
        }

        private void ExecutaRoqueGrande(Posicao posicaoOrigem)
        {
            Posicao origem = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna - 4);
            Posicao destino = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna - 1);
            Peca peca = Tabuleiro.RetirarPeca(origem);
            peca.IncrementarQtdMovimentos();
            Tabuleiro.ColocarPeca(peca, destino);
        }
        
        private void VoltarRoqueGrande(Posicao posicaoOrigem)
        {
            Posicao origem = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna - 4);
            Posicao destino = new Posicao(posicaoOrigem.Linha, posicaoOrigem.Coluna - 1);
            Peca peca = Tabuleiro.RetirarPeca(destino);
            peca.DecrementarQtdMovimentos();
            Tabuleiro.ColocarPeca(peca, origem);
        }

        #endregion
        
        #region "Valida Check Mate"
        /// <summary>
        /// check if player is check mate
        /// </summary>
        /// <param name="cor"></param>
        /// <returns>true if check mate</returns>
        /// <exception cref="TabuleiroException">not have rei in game</exception>
        public bool JogadorEstaEmXeque(Cor cor)
        {
            Peca rei = GetRei(cor);

            if (rei == null)
                throw new TabuleiroException(MessageGame.msg_Nao_Possui_Rei_Da_Cor_X0_Em_Jogo.ToFormat(cor));

            foreach (Peca peca in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mMovimentosPossiveis = peca.MovimentosPossiveis();
                if (mMovimentosPossiveis[rei.Posicao.Linha, rei.Posicao.Coluna])
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Play is checkMate
        /// </summary>
        /// <param name="cor"></param>
        /// <returns>true if Xeque Mate</returns>
        public bool JogadaXequeMate(Cor cor)
        {
            foreach (Peca peca in PecasEmJogo(cor))
            {
                bool[,] mMovimentosPossiveis = peca.MovimentosPossiveis();
                for (int linha = 0; linha < Tabuleiro.Linhas; linha++)
                {
                    for (int coluna = 0; coluna < Tabuleiro.Colunas; coluna++)
                    {
                        if (mMovimentosPossiveis[linha, coluna])
                        {
                            Posicao posicaoOrigem = peca.Posicao;
                            Posicao posicaoDestino = new Posicao(linha, coluna);
                            Peca pecaCapturada = ExecutaMovimento(posicaoOrigem, posicaoDestino);
                            bool xeque = JogadorEstaEmXeque(cor);
                            DesfazMovimento(posicaoOrigem, posicaoDestino, pecaCapturada);
                            if (!xeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region "Validar Posicao"

        #region "Validar Posicao de origem"
        /// <summary>
        /// validate position origin
        /// </summary>
        /// <param name="inputPosicao">player position origin setted</param>
        /// <returns></returns>
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
        #endregion

        #region "Validar posicao de destino
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
        #endregion
        
        /// <summary>
        /// Valid if position inputed in board is valid
        /// </summary>
        /// <param name="inputPosicao">postion inputed by user</param>
        /// <returns>true is ok</returns>
        private bool ValidaPosicao(string inputPosicao)
        {
            return inputPosicao.Length != 2 ||
                   inputPosicao.Substring(0, 1).NotIn(CABECALHO_COLUNAS_TABULEIRO.Split(' '))||
                   !int.TryParse(inputPosicao.Substring(1, 1), out _);
        }

        /// <summary>
        /// check if have piece in the position
        /// </summary>
        /// <param name="posicao">postion inputed by user</param>
        /// <returns>true is ok</returns>
        private bool ExistePecaNaPosicao(Posicao posicao)
        {
            return Tabuleiro.Peca(posicao) != null;
        }

        #endregion

        #region "Alterar jogador"
        /// <summary>
        /// change player game
        /// </summary>
        private void MudaJogador()
        {
            JogadorAtual = JogadorAtual.Equals(Cor.Branca) ? Cor.Preta : Cor.Branca;
        }
        #endregion

        #region "Pecas capturadas"
        /// <summary>
        /// Return captured pieces
        /// </summary>
        /// <param name="cor">cor osf current player</param>
        /// <returns>Return captured pieces</returns>
        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> listaPecasCapturadas = new HashSet<Peca>();
            
            foreach (Peca peca in HashPecasCapturadas.Where(x => x.Cor.Equals(cor)).ToList())
            {
                listaPecasCapturadas.Add(peca);
            }

            return listaPecasCapturadas;
        }
        #endregion

        #region "Pecas em jogo"
        /// <summary>
        /// get pieces of player in game
        /// </summary>
        /// <param name="cor"></param>
        /// <returns>Hash of pieces in game</returns>
        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> listaPecasJogador = new HashSet<Peca>();

            foreach (var peca in from Peca peca in HashPecasEmJogo
                                 where peca.Cor.Equals(cor)
                                 select peca)
            {
                listaPecasJogador.Add(peca);
            }

            listaPecasJogador.ExceptWith(PecasCapturadas(cor));
            return listaPecasJogador;
        }
        #endregion

        #region "Cor joagador adversario"
        /// <summary>
        /// return opponent player color
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        private Cor Adversaria(Cor cor)
        {
            return cor.Equals(Cor.Branca) ? Cor.Preta : Cor.Branca;
        }
        #endregion

        #region "Valida rei"
        /// <summary>
        /// Get Rei pieces in the game
        /// </summary>
        /// <param name="cor">Color of player</param>
        /// <returns>Rei of cor</returns>
        private Peca GetRei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                    return x;
            }

            return null;
        }
        #endregion

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).GetPosicao());
            HashPecasEmJogo.Add(peca);
        }

        private void colocarPecas()
        {
         //   colocarNovaPeca('A', 1, new Torre(Tabuleiro, Cor.Branca));
         //   colocarNovaPeca('B', 1, new Cavalo(Tabuleiro, Cor.Branca));
        //    colocarNovaPeca('C', 1, new Bispo(Tabuleiro, Cor.Branca));
        //    colocarNovaPeca('D', 1, new Dama(Tabuleiro, Cor.Branca));
            colocarNovaPeca('E', 1, new Rei(Tabuleiro, Cor.Branca, this));
            //     colocarNovaPeca('F', 1, new Bispo(Tabuleiro, Cor.Branca));
            /*     colocarNovaPeca('G', 1, new Cavalo(Tabuleiro, Cor.Branca));
                 colocarNovaPeca('H', 1, new Torre(Tabuleiro, Cor.Branca));
                 colocarNovaPeca('A', 2, new Peao(Tabuleiro, Cor.Branca, this));
                 colocarNovaPeca('B', 2, new Peao(Tabuleiro, Cor.Branca, this));
                 colocarNovaPeca('C', 2, new Peao(Tabuleiro, Cor.Branca, this));
                 colocarNovaPeca('D', 2, new Peao(Tabuleiro, Cor.Branca, this));*/
            colocarNovaPeca('E', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('F', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('G', 2, new Peao(Tabuleiro, Cor.Branca, this));
            colocarNovaPeca('H', 2, new Peao(Tabuleiro, Cor.Branca, this));

       //    colocarNovaPeca('A', 8, new Torre(Tabuleiro, Cor.Preta));
       //     colocarNovaPeca('B', 8, new Cavalo(Tabuleiro, Cor.Preta));
       //     colocarNovaPeca('C', 8, new Bispo(Tabuleiro, Cor.Preta));
      //      colocarNovaPeca('D', 8, new Dama(Tabuleiro, Cor.Preta));
            colocarNovaPeca('E', 8, new Rei(Tabuleiro, Cor.Preta, this));
     //       colocarNovaPeca('F', 8, new Bispo(Tabuleiro, Cor.Preta));
      //      colocarNovaPeca('G', 8, new Cavalo(Tabuleiro, Cor.Preta));
      //      colocarNovaPeca('H', 8, new Torre(Tabuleiro, Cor.Preta));
      ///      colocarNovaPeca('A', 7, new Peao(Tabuleiro, Cor.Preta, this));
      ///      colocarNovaPeca('B', 7, new Peao(Tabuleiro, Cor.Preta, this));
      //      colocarNovaPeca('C', 7, new Peao(Tabuleiro, Cor.Preta, this));
      //      colocarNovaPeca('D', 7, new Peao(Tabuleiro, Cor.Preta, this));
      //      colocarNovaPeca('E', 7, new Peao(Tabuleiro, Cor.Preta, this));
    ///        colocarNovaPeca('F', 7, new Peao(Tabuleiro, Cor.Preta, this));
    ///        colocarNovaPeca('G', 7, new Peao(Tabuleiro, Cor.Preta, this));
            colocarNovaPeca('H', 7, new Peao(Tabuleiro, Cor.Preta, this));
        }
    }
}
