using System;

namespace M1JogoDaVelha
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool simAI;
            string jogando = "S";
            string[,] espacos;

            //Repete o jogo e pede o modo de jogo ao jogador.
            while (jogando == "S")
            {
                string modoDeJogo;
                espacos = new string[3, 3]
                {
                    {" ", " ", " "},
                    {" ", " ", " "},
                    {" ", " ", " "}
                };

                Console.WriteLine("\nDigite <S> para Singleplayer (vs. AI) ou <M> para Multiplayer Local.");

                do
                {
                    modoDeJogo = Console.ReadLine().ToUpper();
                } while (modoDeJogo != "S" && modoDeJogo != "M");

                if (modoDeJogo == "S")
                {
                    simAI = true;
                }

                else
                {
                    simAI = false;
                }

                jogando = Jogar(espacos, simAI);
            }

        }

        // Faz o quiz.
        static string Jogar(string[,] espacoDeJogo, bool aiEnabled)
        {
            string jogadorAtual = "X";
            int turnos = 0;
            string vencedor = "N/A";
            bool aiTurn = true;

            PrintarJogo(espacoDeJogo);

            //Repete até se ter um vencedor.
            while (vencedor == "N/A")
            {
                
                //Faz as jogadas.
                if(aiEnabled == true && aiTurn == true)
                {
                    espacoDeJogo = FazerJogadaAI(espacoDeJogo, jogadorAtual);
                }

                else
                {
                    espacoDeJogo = FazerJogada(espacoDeJogo, jogadorAtual);
                }
                //Mostra o tabuleiro
                PrintarJogo(espacoDeJogo);

                //Altera o jogador.
                jogadorAtual = MudarJogador(jogadorAtual);

                if (aiEnabled == true)
                {
                    aiTurn = MudarJogadorAI(aiTurn);
                }

                //Checa vencedor
                vencedor = ChecarVitoria(espacoDeJogo);
                
                //Empata a partida se não há vencedor após preencher o tabuleiro.
                if (turnos == 8 && vencedor == "N/A")
                {
                    vencedor = "Empate!";
                }
                turnos++;
            }

            //Abaixo, mostra vencedor e pede para repetir o jogo.
            Console.WriteLine("\n E o resultado é: ");

            if (vencedor != "Empate!")
            {
                Console.WriteLine(vencedor + " venceu!");
            }

            else
            {
                Console.WriteLine(vencedor);
            }

            Console.WriteLine("\n\nGostaria de Jogar Novamente? Digite <S> se SIM, <N> se NÃO.");
            string respostaRejogar;

            do
            {
                respostaRejogar = Console.ReadLine().ToUpper();
            } while (respostaRejogar != "S" && respostaRejogar != "N");

            return respostaRejogar;
        }

        //Faz a jogada do jogador.
        static string[,] FazerJogada(string[,] campo, string jogador)
        {
            int linhaJogada;
            int colunaJogada;


            do
            {
                linhaJogada = 0;
                colunaJogada = 0;
                Console.WriteLine("\nInsira o número da linha e dê ENTER, então insira o número da coluna do espaço de sua jogada.");

                while ((linhaJogada < 1 || linhaJogada > 3) || (colunaJogada < 1 || colunaJogada > 3))
                {
                    Console.Write("Linha: ");
                    linhaJogada = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Coluna: ");
                    colunaJogada = Convert.ToInt32(Console.ReadLine());
                }
            } while (campo[linhaJogada - 1, colunaJogada - 1] == "X" || campo[linhaJogada - 1, colunaJogada - 1] == "O");



            campo[linhaJogada - 1, colunaJogada - 1] = jogador;
            return campo;
        }

        //Faz a Jogada da IA.
        static string[,] FazerJogadaAI(string[,] campoAI, string jogadorAI)
        {
            int linhaGerada;
            int colunaGerada;
            var rngGerador = new Random();
            int valorAleatorio;

            //Checa se a IA tem um lança que ganha o jogo.
            for(int iteradorLinha = 0; iteradorLinha < 3; iteradorLinha++)
            {
                for(int iteradorColuna = 0; iteradorColuna < 3; iteradorColuna++)
                {
                    if (campoAI[iteradorLinha, iteradorColuna] == " ")
                    {
                        campoAI[iteradorLinha, iteradorColuna] = jogadorAI;

                        if (ChecarVitoria(campoAI) == "Jogador 1(X)")
                        {
                            return campoAI;
                        }
                        
                        campoAI[iteradorLinha, iteradorColuna] = " ";
                        
                    }
                }
            }

            // Se não houver nenhum, checa se o oponente consegue ganhar o jogo.
            for (int iteradorLinhaDerrota = 0; iteradorLinhaDerrota < 3; iteradorLinhaDerrota++)
            {
                for (int iteradorColunaDerrota = 0; iteradorColunaDerrota < 3; iteradorColunaDerrota++)
                {
                    if (campoAI[iteradorLinhaDerrota, iteradorColunaDerrota] == " ")
                    {
                        jogadorAI = "O";
                        campoAI[iteradorLinhaDerrota, iteradorColunaDerrota] = jogadorAI;
                        jogadorAI = "X";

                        if (ChecarVitoria(campoAI) == "Jogador 2(O)")
                        {
                            campoAI[iteradorLinhaDerrota, iteradorColunaDerrota] = jogadorAI;
                            return campoAI;
                        }

                        campoAI[iteradorLinhaDerrota, iteradorColunaDerrota] = " ";
                    }
                }
            }

            // Se ambos forem falso, gera um lugar aleatorio vazio.
            do
            {
                valorAleatorio = rngGerador.Next(0, 3);
                linhaGerada = valorAleatorio;
                valorAleatorio = rngGerador.Next(0, 3);
                colunaGerada = valorAleatorio;
            } while (campoAI[linhaGerada, colunaGerada] == "X" || campoAI[linhaGerada, colunaGerada] == "O");

            campoAI[linhaGerada, colunaGerada] = jogadorAI;
            return campoAI;
            
        }

        static string ChecarVitoria(string[,] tabuleiroAtual)
        {
            for(int i = 0; i < tabuleiroAtual.GetLength(0); i++)
            {
                //Checa se algum jogador ganhou horizontalmente
                if (tabuleiroAtual[i,0] == tabuleiroAtual[i,1] && tabuleiroAtual[i,0] == tabuleiroAtual[i, 2] && tabuleiroAtual[i,0] != " ")
                {
                    if (tabuleiroAtual[i,0] == "X")
                    {
                        return "Jogador 1(X)";
                    } 

                    else
                    {
                        return "Jogador 2(O)";
                    }
                }

                // Verticalmente
                else if (tabuleiroAtual[0,i] == tabuleiroAtual[1,i] && tabuleiroAtual[0,i] == tabuleiroAtual[2,i] && tabuleiroAtual[0,i] != " ")
                {
                    if (tabuleiroAtual[0, i] == "X")
                    {
                        return "Jogador 1(X)";
                    }

                    else
                    {
                        return "Jogador 2(O)";
                    }
                }
            }
            // E nas diagonais
            if (tabuleiroAtual[0,0] == tabuleiroAtual[1,1] && tabuleiroAtual[0,0] == tabuleiroAtual[2,2] && tabuleiroAtual[0,0] != " ")
            {
                if (tabuleiroAtual[0, 0] == "X")
                {
                    return "Jogador 1(X)";
                }

                else
                {
                    return "Jogador 2(O)";
                }
            }

            if (tabuleiroAtual[2,0] == tabuleiroAtual[1,1] && tabuleiroAtual[0,2] == tabuleiroAtual[2,0] && tabuleiroAtual[2,0] != " ")
            {
                if (tabuleiroAtual[2,0] == "X")
                {
                    return "Jogador 1(X)";
                }

                else
                {
                    return "Jogador 2(O)";
                }
            }
            return "N/A";

        }

        static string MudarJogador(string jogadorDoTurno)
        {
            bool jogadorMudou = false;

            if (jogadorDoTurno == "X" && jogadorMudou == false)
            {
                jogadorDoTurno = "O";
                jogadorMudou = true;
            }

            if (jogadorDoTurno == "O" && jogadorMudou == false)
            {
                jogadorDoTurno = "X";
            }

            return jogadorDoTurno;
        }

        static bool MudarJogadorAI(bool condicaoDaIA)
        {
            bool turnoMudou = false;

            if (condicaoDaIA == true && turnoMudou == false)
            {
                condicaoDaIA = false;
                turnoMudou = true;
            }

            if (condicaoDaIA == false && turnoMudou == false)
            {
                condicaoDaIA = true;
            }

            return condicaoDaIA;
        }

        //Mostra o tabuleiro
        static void PrintarJogo(string[,] campoAtual)
        {
            Console.Clear();
            Console.WriteLine("Olá! Esse é o simulador de Jogo da Velha");
            Console.WriteLine("Feito por Lorenzo Grando e Pedro Henrique D'Avila");
            Console.WriteLine();
            Console.WriteLine(
               "          COLUNA" +
               "\n        1   2   3" +
               "\n  L  1  " + campoAtual[0,0] + " | " + campoAtual[0,1] + " | " + campoAtual[0,2] + " " +
               "\n  I     ----------" +
               "\n  N  2  " + campoAtual[1,0] + " | " + campoAtual[1,1] + " | " + campoAtual[1,2] + " " +
               "\n  H     ----------" +
               "\n  A  3  " + campoAtual[2,0] + " | " + campoAtual[2,1] + " | " + campoAtual[2,2] + " "
            );
        }
    }
}