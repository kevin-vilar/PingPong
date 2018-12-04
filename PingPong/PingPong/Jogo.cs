using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace PingPong
{
    internal class Jogo
    {

        private int widthJanela = 70;
        private int heightJanela = 20;

        private Player player1;
        private Player player2;
        private Bola bola;
        private Placar placar;

        public static bool partidaIniciada;

        public static object _lock = new Object();

        string caminhoInfos = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "infos.txt";

        public Jogo()
        {
            partidaIniciada = false;

            Console.CursorVisible = false;
            Console.WindowWidth = widthJanela;
            Console.WindowHeight = heightJanela;
            Console.BufferWidth = widthJanela;
            Console.BufferHeight = heightJanela;

            placar = new Placar();
            player1 = new Player();
            player2 = new Player(true);
            bola = new Bola(player1,player2,placar);
        }

        public void run()
        {
            while (true)
            {
                lock (Jogo._lock)
                {
                    Console.Clear();
                    salvarInfosPartida();
                    Console.Clear();
                
                    inicioJogo();
                }

                List<String> infosPartida = obterInfosPartida();
                int quantidadePontos = Convert.ToInt32(infosPartida[0]);
                int quantidadeRodadas = Convert.ToInt32(infosPartida[1]);

                int numeroRodada = 1;
                while (numeroRodada <= quantidadeRodadas)
                {
                    lock (Jogo._lock)
                    {
                        Console.Clear();
                        inicioJogo();
                    }

                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        partidaIniciada = true;

                        Console.Clear();
                        renderizaObjetos();
                    
                        Thread threadTeclado = new Thread(HandleTeclado);
                        threadTeclado.Start();
                        Thread threadBola = new Thread(bola.movimentar);
                        threadBola.Start();

                        while(partidaIniciada)
                        {
                            if(placar.placar_player1 >= quantidadePontos || placar.placar_player2 >= quantidadePontos)
                            {
                                partidaIniciada = false;
                                threadTeclado.Abort();
                                threadBola.Abort();
                                incrementarPlacarRodada();
                                reiniciaPartida();
                            }
                        }
                        
                        numeroRodada++;
                    }
                }
                lock (Jogo._lock)
                {
                    Console.Clear();
                    desenhar(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2 - 2, playerVencedor() + " Venceu!");
                    desenhar(Console.WindowWidth / 2 - 18, Console.WindowHeight / 2, "Aperte qualquer tecla para reiniciar.");
                    Console.ReadKey();
                    reiniciarJogo();
                }
            }
        }

        private void incrementarPlacarRodada()
        {
            if (placar.placar_player1 > placar.placar_player2)
            {
                placar.rodadasVencidas_player1++;
            }
            else
            {
                placar.rodadasVencidas_player2++;
            }
        }

        private string playerVencedor()
        {
            string playerVencedor;

            if (placar.rodadasVencidas_player1 == placar.rodadasVencidas_player2)
            {
                return "Ninguém";
            }

            if (placar.rodadasVencidas_player1 > placar.rodadasVencidas_player2)
            {
                playerVencedor = player1.nome;
            }
            else
            {
                playerVencedor = player2.nome;
            }
            return playerVencedor;
        }

        private void reiniciaPartida()
        {
            Console.Clear();

            bola.x = Console.WindowWidth/2;
            bola.y = Console.WindowHeight/2;

            player1.x = 5;
            player1.y = ((Console.WindowHeight / 2) - 2);

            player2.x = Console.WindowWidth - 5;
            player2.y = ((Console.WindowHeight / 2) - 2);

            placar.placar_player1 = 0;
            placar.placar_player2 = 0;
        }

        private void reiniciarJogo()
        {
            placar = new Placar();
            player1 = new Player();
            player2 = new Player(true);
            bola = new Bola(player1, player2, placar);
        }

        private void inicioJogo()
        {
            Console.SetCursorPosition((Console.WindowWidth - 33) / 2, 1);
            Console.Write("Aperte Enter para começar o jogo!");

            renderizaObjetos();
        }

        private void renderizaObjetos()
        {
            atribuirNomePlayers();
            placar.renderizarPlacar();
            player1.renderizarPlayer();
            player2.renderizarPlayer();
            bola.desenhar('O', bola.x, bola.y);
        }

        private void atribuirNomePlayers()
        {
            StreamReader infosR;
            infosR = File.OpenText(caminhoInfos);
            List<String> linha = new List<String>();
            while (!infosR.EndOfStream)
            {
                linha.Add(infosR.ReadLine());
            }

            player1.nome = linha[0];
            player2.nome = linha[1];

            infosR.Close();
        }

        private List<String> obterInfosPartida()
        {
            StreamReader infosR;
            infosR = File.OpenText(caminhoInfos);
            List<String> linha = new List<String>();
            while (!infosR.EndOfStream)
            {
                linha.Add(infosR.ReadLine());
            }
            infosR.Close();

            List<String> infosPartida = new List<String>();
            infosPartida.Add(linha[2]);
            infosPartida.Add(linha[3]);
            return infosPartida;
        }

        private void salvarInfosPartida()
        {
            StreamWriter infos;
            infos = File.CreateText(caminhoInfos);

            Console.Write("Digite o apelido do Player 1: ");
            string infoP1 = Console.ReadLine();

            Console.Write("Digite o apelido do Player 2: ");
            string infoP2 = Console.ReadLine();

            Console.Write("Digite a quantidade de pontos por rodada: ");
            string numero_pontos = Console.ReadLine();

            Console.Write("Digite a quantidade de rodadas: ");
            string numero_rodadas = Console.ReadLine();

            infos.WriteLine(infoP1);
            infos.WriteLine(infoP2);
            infos.WriteLine(numero_pontos);
            infos.WriteLine(numero_rodadas);

            infos.Close();
        }

        private void desenhar(int x, int y, string desenho)
        {
            lock (Jogo._lock)
            {
                try
                {
                    if (x >= 0 && y >= 0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(desenho);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void HandleTeclado()
        {
            while (true)
            {
                var key = Console.ReadKey(true).KeyChar;
                Thread.Sleep(10);

                switch (key)
                {
                    case 'w':
                        player1.movimentar("cima");
                        break;
                    case 's':
                        player1.movimentar("baixo");
                        break;
                    case 'i':
                        player2.movimentar("cima");
                        break;
                    case 'k':
                        player2.movimentar("baixo");
                        break;
                }
            }
        }
    }
}