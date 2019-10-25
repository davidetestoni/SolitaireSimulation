using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            var i = 1;
            var won = 0;
            Game game;

            Console.WriteLine("How many different cards? ");
            var cards = int.Parse(Console.ReadLine());

            do
            {
                Console.Clear();
                game = new Game() { DifferentCards = cards };
                game.Setup();
                if (game.Play(false)) won++;
                Console.WriteLine($"Played: {i}\r\nWon: {won}\r\nPercentage: {(float)won / (float)i * 100.0}%");
                i++;
            }
            while (i <= 1000 * 10);

            Console.ReadLine();
        }
    }
}
