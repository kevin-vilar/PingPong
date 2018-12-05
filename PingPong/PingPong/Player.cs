using System;

namespace PingPong
{
    internal class Player
    {
        public int x = 0;
        public int y = 0;
        public string nome;
        private string cor;

        private bool isPlayer2;
        
        public Player(string nome, string cor, bool player2 = false)
        {
            this.nome = nome;
            this.cor = cor;
            if (!player2)
            {
                this.x = 5;
            }
            else
            {
                this.x = Console.WindowWidth - 5;
                isPlayer2 = true;
            }

            this.y = ((Console.WindowHeight / 2) - 2);
            nome = "";
        }

        public void renderizarPlayer()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.CursorVisible = false;
                switch (cor)
                {
                case "azul":
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                case "vermelho":
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
                desenhar("|", x, y+i);
                Console.ResetColor();
            }
            if (isPlayer2)
            {
                desenhar(nome,Console.WindowWidth - 10, 2);
            }
            else
            {
                desenhar(nome,2, 2);
            }

        }

        private void limparPlayer()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.CursorVisible = false;
                desenhar(" ", x, y + i);
            }
        }

        public void movimentar(String direcao)
        {
            lock (Jogo._lock)
            {
                if (direcao == "cima" && y > Console.WindowTop + 4)
                {
                    limparPlayer();
                    y -= 2;
                    renderizarPlayer();
                }

                if (direcao == "baixo" && y < Console.WindowHeight - 4)
                {
                    limparPlayer();
                    y += 2;
                    renderizarPlayer();
                }
            }
        }

        public void desenhar(string desenho = "|", int x = 0, int y = 0)
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