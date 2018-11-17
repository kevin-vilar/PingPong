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
            Console.SetWindowSize(70, 20);

            Bola bola = new Bola();
            Thread tb = new Thread(bola.movimentar);
            tb.Start();

            Player player1 = new Player(5);
            Player player2 = new Player(60);


            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.F2)
                {
                    player1.movimentar(1);
                }

                if (Console.ReadKey().Key == ConsoleKey.F1)
                {
                    if (player1.y > 0)
                    {
                        player1.movimentar(-1);
                    }
                }

                if (Console.ReadKey().Key == ConsoleKey.DownArrow)
                {
                    player2.movimentar(1);
                }

                if (Console.ReadKey().Key == ConsoleKey.UpArrow)
                {
                    if (player2.y > 0)
                    {
                        player2.movimentar(-1);
                    }
                }

            }
        }

    }
}