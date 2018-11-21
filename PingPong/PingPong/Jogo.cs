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

        public static object _lock = new Object();

        public Jogo()
        {
            Console.CursorVisible = false;
            Console.WindowWidth = widthJanela;
            Console.WindowHeight = heightJanela;
            Console.BufferWidth = widthJanela;
            Console.BufferHeight = heightJanela;

            player1 = new Player();
            player2 = new Player(true);
            bola = new Bola(player1,player2);
        }

        public void run()
        {
            Thread threadTeclado = new Thread(HandleTeclado);
            threadTeclado.Start();

            Thread threadBola = new Thread(bola.movimentar);
            threadBola.Start(); 
            while (true){}
        }

        void HandleTeclado()
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