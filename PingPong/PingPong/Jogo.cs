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
                    salvarNomesPlayers();
                    Console.Clear();
                
                    inicioJogo();
                }

                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    partidaIniciada = true;

                    Console.Clear();
                    renderizaObjetosInicio();
                    renderizaNomePlayers();
                    
                    Thread threadTeclado = new Thread(HandleTeclado);
                    threadTeclado.Start();
                    Thread threadBola = new Thread(bola.movimentar);
                    threadBola.Start();

                    while(partidaIniciada)
                    {
                        if(placar.placar_player1 >= 1 || placar.placar_player2 >= 1)
                        {
                            partidaIniciada = false;
                            threadTeclado.Abort();
                            threadBola.Abort();
                            reiniciaPartida();
                        }
                    }
                }
            }
        }

        private void reiniciaPartida()
        {
            Console.Clear();

            File.Delete(caminhoInfos);

            bola.x = Console.WindowWidth/2;
            bola.y = Console.WindowHeight/2;

            player1.x = 5;
            player1.y = ((Console.WindowHeight / 2) - 2);

            player2.x = Console.WindowWidth - 5;
            player2.y = ((Console.WindowHeight / 2) - 2);

            placar.placar_player1 = 0;
            placar.placar_player2 = 0;
        }

        private void verificaTerminoPartida()
        {
            if(placar.placar_player1 >= 2 || placar.placar_player2 >= 2)
            {
                partidaIniciada = false;               
            }
        }

        private void inicioJogo()
        {
            Console.SetCursorPosition((Console.WindowWidth - 33) / 2, 1);
            Console.Write("Aperte Enter para começar o jogo!");

            renderizaObjetosInicio();
        }

        private void renderizaObjetosInicio()
        {
            placar.renderizarPlacar();
            player1.renderizarPlayer();
            player2.renderizarPlayer();
            bola.desenhar('O', bola.x, bola.y);
        }

        private void renderizaNomePlayers()
        {
            StreamReader infosR;
            infosR = File.OpenText(caminhoInfos);
            List<String> linha = new List<String>();
            while (!infosR.EndOfStream)
            {
                linha.Add(infosR.ReadLine());
            }

            desenhar(2, 2, linha[0]);
            desenhar(Console.WindowWidth - 10, 2, linha[1]);
            infosR.Close();
        }

        private void salvarNomesPlayers()
        {
            StreamWriter infos;
            infos = File.CreateText(caminhoInfos);

            Console.Write("Digite o apelido do Player 1: ");
            string infoP1 = Console.ReadLine();

            Console.WriteLine();
            Console.Write("Digite o apelido do Player 2: ");
            string infoP2 = Console.ReadLine();

            infos.WriteLine(infoP1);
            infos.WriteLine(infoP2);

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