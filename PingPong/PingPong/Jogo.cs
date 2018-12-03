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

        public static object _lock = new Object();

        public Jogo()
        {
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
            Thread threadTeclado = new Thread(HandleTeclado);
            Thread threadBola = new Thread(bola.movimentar);
            Thread threadPlacar = new Thread(placar.atualizarPlacar);
        
            inicioJogo();

            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Console.Clear();
                renderizaObjetosInicio();

                threadPlacar.Start();
                threadBola.Start();
                threadTeclado.Start();
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