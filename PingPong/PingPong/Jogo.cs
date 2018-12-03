using System;
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

        public static bool jogoIniciado;

        public static object _lock = new Object();

        public Jogo()
        {
            jogoIniciado = false;

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
            Thread threadVerificaTerminoPartida = new Thread(verificaTerminoPartida);
            threadVerificaTerminoPartida.Start();
        
            while(true)
            {
                Console.Clear();
                inicioJogo(); 
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    jogoIniciado = true;

                    Console.Clear();
                    renderizaObjetosInicio();

                    Thread threadTeclado = new Thread(HandleTeclado);
                    Thread threadBola = new Thread(bola.movimentar);
                    threadBola.Start();
                    threadTeclado.Start();

                    while(jogoIniciado)
                    {
                        if(placar.placar_player1 >= 2 || placar.placar_player2 >= 2)
                        {
                            jogoIniciado = false;
                            threadTeclado.Abort();
                            threadBola.Abort();
                            Console.Clear();
                            reiniciaPartida();
                        }
                    } 

                }

                
            }
        }
        private void reiniciaPartida()
        {
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
                jogoIniciado = false;               
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