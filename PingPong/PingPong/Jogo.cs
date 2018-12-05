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
        string caminhoResultadosPartidas = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "resultadosPartidas.txt";

        public Jogo()
        {
            partidaIniciada = false;

            Console.CursorVisible = false;
            Console.WindowWidth = widthJanela;
            Console.WindowHeight = heightJanela;
            Console.BufferWidth = widthJanela;
            Console.BufferHeight = heightJanela;
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
                
                    inicioJogo(obterInfosPartida());
                }

                List<String> infosPartida = obterInfosPartida();
                int quantidadePontos = Convert.ToInt32(infosPartida[4]);
                int quantidadeRodadas = Convert.ToInt32(infosPartida[5]);

                int numeroRodada = 1;
                while (numeroRodada <= quantidadeRodadas)
                {
                    lock (Jogo._lock)
                    {
                        Console.Clear();
                        inicioJogo(obterInfosPartida());
                    }

                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        partidaIniciada = true;

                        Console.Clear();
                        renderizaObjetos();
                    
                        Thread threadPlacar = new Thread(atualizarPlacar);
                        threadPlacar.Start();
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
                                threadPlacar.Abort();
                                incrementarPlacarRodada();
                                reiniciaPartida();
                            }
                        }
                        
                        numeroRodada++;
                    }
                }
                lock (Jogo._lock)
                {
                    salvarResultadosPartida();
                    File.Delete(caminhoInfos);
                    Console.Clear();
                    desenhar(Console.WindowWidth / 2 - 7, Console.WindowHeight / 2 - 2, playerVencedor() + " Venceu!");
                    desenhar(Console.WindowWidth / 2 - 18, Console.WindowHeight / 2, "Aperte qualquer tecla para reiniciar.");
                    Console.ReadKey();
                }
            }
        }

        private void salvarResultadosPartida()
        {
            String resultadoPartida = "Resultado: " + player1.nome + " " + placar.rodadasVencidas_player1 + " x " + placar.rodadasVencidas_player2 + " " + player2.nome;

            StreamWriter resultados = new StreamWriter(caminhoResultadosPartidas,true);
            resultados.WriteLine(resultadoPartida);
            resultados.Close();
        }

        private void atualizarPlacar()
        {
            while (true)
            {
                if ((bola.x >= player2.x))
                {
                    placar.placar_player1++;
                    placar.atualizarPlacar();
                    while ((bola.x >= player2.x)) { };
                }

                if ((bola.x <= player1.x))
                {
                    placar.placar_player2++;
                    placar.atualizarPlacar();
                    while ((bola.x <= player1.x)) { };
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

        private void inicioJogo(List<String> infosPartida)
        {
            Console.SetCursorPosition((Console.WindowWidth - 33) / 2, 1);
            Console.Write("Aperte Enter para começar o jogo!");

            placar = new Placar();
            player1 = new Player(infosPartida[0],infosPartida[1]);
            player2 = new Player(infosPartida[2], infosPartida[3],true);
            bola = new Bola(player1, player2, placar);

            renderizaObjetos();
        }

        private void renderizaObjetos()
        {
            placar.renderizarPlacar();
            player1.renderizarPlayer();
            player2.renderizarPlayer();
            bola.desenhar(bola.x, bola.y);
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

            return linha;
        }

        private void salvarInfosPartida()
        {
            StreamWriter infos;
            infos = File.CreateText(caminhoInfos);

            Console.Write("Digite o apelido do Player 1: ");
            string nomeP1 = Console.ReadLine();

            Console.Write("Escolha uma cor(azul,vermelho ou branco): ");
            string corPlayer1 = Console.ReadLine();

            Console.Write("Digite o apelido do Player 2: ");
            string nomeP2 = Console.ReadLine();

            Console.Write("Escolha uma cor(azul,vermelho ou branco): ");
            string corPlayer2 = Console.ReadLine();

            Console.Write("Digite a quantidade de pontos por rodada: ");
            string numero_pontos = Console.ReadLine();

            Console.Write("Digite a quantidade de rodadas: ");
            string numero_rodadas = Console.ReadLine();

            infos.WriteLine(nomeP1);
            infos.WriteLine(corPlayer1);
            infos.WriteLine(nomeP2);
            infos.WriteLine(corPlayer2);
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