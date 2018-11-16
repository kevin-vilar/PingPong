﻿using System;
using System.Timers;
namespace PingPong
{
    internal class Bola
    {
        int x = 0;
        int y = 0;

        int direcaoX = 1;
        int direcaoY = 1;

        private static System.Timers.Timer tempo = new System.Timers.Timer(50);

        const char bola = 'O';

        public void movimentar()
        {
            tempo.Enabled = true;
            tempo.Elapsed += new ElapsedEventHandler(timer);
            desenhar(bola, x, y);

            desenhar(bola, x, y);
        }

        private void timer(object sender, EventArgs e)
        {
            Console.CursorVisible = false;
            desenhar(' ', x, y);
            x += 1 * direcaoX;
            y += 1 * direcaoY;
            desenhar(bola, x, y);
            if (y >= Console.WindowHeight)
            {
                direcaoY *= -1;
            }

            if (y <= Console.WindowTop)
            {
                direcaoY *= -1;
            }

            if (x >= Console.WindowWidth)
            {
                direcaoX *= -1;
            }

            if (x <= Console.WindowLeft)
            {
                direcaoX *= -1;
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