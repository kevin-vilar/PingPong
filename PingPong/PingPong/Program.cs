using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;


namespace PingPong
{
    class Program
    {

        static void Main(string[] args)
        {

            Player player1 = new Player(5);
            Player player2 = new Player(60);

            Console.WindowHeight = 20;
            Console.WindowWidth = 70;
            Console.BufferHeight = 20;
            Console.BufferWidth = 70;

            Bola bola = new Bola();
            Thread threadBola = new Thread(bola.movimentar);
            //threadBola.Start();

            Thread threadTeclado = new Thread(HandleTeclado);
            threadTeclado.Start();
            while (true) ;

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
}