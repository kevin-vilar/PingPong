using System;

namespace PingPong
{
    internal class Player
    {
        public int x = 0;
        public int y = 0;
        
        public Player(int x)
        {
            this.x = x;
        }

        private void renderizarPlayer()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.CursorVisible = false;
                desenhar('|', x, y+i);
            }

        }

        private void limparPlayer()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.CursorVisible = false;
                desenhar(' ', x, y + i);
            }
        }

        public void movimentar(int n)
        {
            limparPlayer();
            y += n;
            renderizarPlayer();
        }

        public void desenhar(char desenho, int x = 0, int y = 0)
        {
            try
            {
                if (x >= 0 && y >= 0)
                {
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine(desenho);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}