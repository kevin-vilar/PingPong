using System;
using System.Timers;
namespace PingPong
{
    internal class Bola
    {
        Player player1;
        Player player2;
        Placar placar;

        public int x = Console.WindowWidth/2;
        public int y = Console.WindowHeight/2;

        int direcaoX = 1;
        int direcaoY = 1;

        private static System.Timers.Timer tempo;

        const char bola = 'O';

        public Bola(Player player1, Player player2, Placar placar)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.placar = placar;
        }

        public void movimentar()
        {
            tempo = new System.Timers.Timer(200);
            tempo.Enabled = true;
            tempo.Elapsed += new ElapsedEventHandler(timer);                        
        }

        private void timer(object sender, EventArgs e)
        {
            lock (Jogo._lock)
            {

                if(Jogo.jogoIniciado)
                {
                    tempo.Start();
                    Console.CursorVisible = false;
                    desenhar(' ', x, y);

                    x += 2 * direcaoX;
                    y += 1 * direcaoY;

                    if ((y >= Console.WindowHeight - 1) || (y <= Console.WindowTop + 4))
                    {
                        direcaoY *= -1;
                    }

                    if (colisaoPlayer())
                    {
                        direcaoX *= -1;
                    }

                    if((x >= Console.WindowWidth - 1))
                    {
                        placar.placar_player1 ++;
                        direcaoX *= -1;
                    }

                    if((x <= Console.WindowLeft + 1))
                    {
                        placar.placar_player2 ++;
                        direcaoX *= -1;
                    }

                    placar.atualizarPlacar();
                    desenhar(bola, x, y);
                }
                else
                {
                    tempo.BeginInit();
                }
            }
        }

        private bool colisaoPlayer()
        {
            bool colisaoP1 = (y >= player1.y && y <= (player1.y + 3)) && (x <= player1.x + 2);
            bool colisaoP2 = (y >= player2.y && y <= (player2.y + 3)) && (x >= player2.x - 2);

            if (colisaoP1||colisaoP2)
            {
                return true;
            }

            return false;
        }
        
        public void desenhar(char desenho = bola, int x = 0, int y = 0)
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