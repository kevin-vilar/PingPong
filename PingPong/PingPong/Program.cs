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
            Jogo jogo = new Jogo();
            jogo.run();
            
        }
    }
}