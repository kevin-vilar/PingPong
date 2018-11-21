using System;
using System.Timers;
namespace PingPong
{
    internal class Bola
    {
        int x = 55;
        int y = 15;

        int direcaoX = 1;
        int direcaoY = 1;

        private static System.Timers.Timer tempo = new System.Timers.Timer(200);

        const char bola = 'O';

        public void movimentar()
        {
            tempo.Enabled = true;
            tempo.Elapsed += new ElapsedEventHandler(timer);                        
        }

        private void timer(object sender, EventArgs e)
        {
            lock (Jogo._lock)
            {
                Console.CursorVisible = false;
                desenhar(' ', x, y);

                x += 2 * direcaoX;
                y += 1 * direcaoY;
                desenhar(bola, x, y);
                if (y >= Console.WindowHeight - 1)
                {
                    direcaoY *= -1;
                }

                if (y <= Console.WindowTop + 1)
                {
                    direcaoY *= -1;
                }

                if (x >= Console.WindowWidth - 1)
                {
                    direcaoX *= -1;
                }

                if (x <= Console.WindowLeft + 1)
                {
                    direcaoX *= -1;
                }
            }
        }
        
        public void desenhar(char desenho, int x = 0, int y = 0)
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
}