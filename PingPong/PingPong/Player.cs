using System;

namespace PingPong
{
    internal class Player
    {
        public int x = 0;
        public int y = 0;
        
        public Player(bool player2 = false)
        {
            if (!player2)
            {
                this.x = 5;
            }
            else
            {
                this.x = Console.WindowWidth - 5;
            }
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

        public void movimentar(String direcao)
        {
            if (direcao == "cima" && y > Console.WindowTop + 1)
            {
                limparPlayer();
                y-=2;
                renderizarPlayer();
            }

            if (direcao == "baixo" && y < Console.WindowHeight - 4)
            {
                limparPlayer();
                y+=2;
                renderizarPlayer();
            }
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