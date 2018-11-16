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
            while (true) { }
        }

    }
}