using System;

namespace PingPong
{
    internal class Placar
    {
        public int placar_player1 = 0;
        public int placar_player2 = 0;

        private static int posicaoPlacar_x = (Console.WindowWidth / 2) - 6;
        private static int posicaoPlacar_y = 2;
        private static int posicaoP1_x = posicaoPlacar_x + 7;
        private static int posicaoP2_x = posicaoP1_x + 4;

        public void atualizarPlacar()
        {
            lock (Jogo._lock)
            {
                limparPlacar();
                renderizarPlacar();
            }
        }

        public void renderizarPlacar()
        {
            desenhar(posicaoPlacar_x, posicaoPlacar_y, "Placar");
            desenhar(posicaoP1_x, posicaoPlacar_y, placar_player1.ToString());
            desenhar(posicaoP1_x + 1, posicaoPlacar_y, " : ");
            desenhar(posicaoP2_x, posicaoPlacar_y, placar_player2.ToString());
        }

        private void limparPlacar()
        {
            for (int i = 0; i <= 11; i++)
            {
                desenhar(posicaoPlacar_x,posicaoPlacar_y, " ");
            }
        }

        public void desenhar(int x, int y, string desenho)
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