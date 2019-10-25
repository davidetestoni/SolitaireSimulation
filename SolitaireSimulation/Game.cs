using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolitaireSimulation
{
    public class Game
    {
        public int DifferentCards { get; set; } = 6;
        private int Columns { get { return DifferentCards - 1; } }
        public Card[,] Table { get; set; } // Column, Row
        public List<Card> Side { get; set; } = new List<Card>();
        public Card Hand { get; set; } = null;
        public bool Finished { get { return Side.Count == 4 && Side.All(card => card.FacingUp && card.Value == Side.First().Value); } }
        public bool Won {
            get
            {
                for (var i = 0; i < Columns; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        if (Table[i, j].Value != Table[i, 0].Value)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public void Setup()
        {
            List<Card> cards = new List<Card>();
            for (var i = 1; i <= DifferentCards; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    cards.Add(new Card() { Value = i, FacingUp = false });
                }
            }
            cards.Shuffle();

            for (var i = 0; i < 4; i++)
            {
                Side.Add(cards[0]);
                cards.RemoveAt(0);
            }

            Table = new Card[Columns, 4];
            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    Table[i, j] = cards[0];
                    cards.RemoveAt(0);
                }
            }
        }

        public bool Play(bool print)
        {
            // Take the first card on the top left
            Hand = Side.First();
            Hand.FacingUp = true;
            Side.RemoveAt(0);

            while (!Finished)
            {
                // If we find the same value
                for (var i = 0; i < Columns; i++)
                {
                    if (Table[i, 0].Value == Hand.Value && Table[i, 0].FacingUp)
                    {
                        for (var j = 0; j < 4; j++)
                        {
                            if (!Table[i, j].FacingUp)
                            {
                                var temp = Table[i, j];
                                Table[i, j] = Hand;
                                Hand = temp;
                                Hand.FacingUp = true;
                                goto PRINT;
                            }
                        }
                    }
                }

                // If we find a card facing down in the top place of the column
                for (var i = 0; i < Columns; i++)
                {
                    if (!Table[i, 0].FacingUp)
                    {
                        var temp = Table[i, 0];
                        Table[i, 0] = Hand;
                        Hand = temp;
                        Hand.FacingUp = true;
                        goto PRINT;
                    }
                }

                foreach (Card card in Side)
                {
                    if (!card.FacingUp)
                    {
                        var temp = card;
                        Side.Remove(card);
                        Side.Add(Hand);
                        Hand = temp;
                        Hand.FacingUp = true;
                        goto PRINT;
                    }
                }

                // We reach here if it's the end of the game and we have to put our hand card in the Side column
                Side.Add(Hand);
                Hand = null;

                PRINT:
                if (print)
                {
                    PrintTable();
                    Thread.Sleep(500);
                }
            }

            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (!Table[i, j].FacingUp)
                    {
                        Table[i, j].FacingUp = true;
                    }
                }
            }

            if (print)
            {
                PrintTable();
            }

            return Won;
        }

        private void PrintTable()
        {
            Console.Clear();

            for (var j = 0; j < 4; j++)
            {
                for (var i = 0; i < Columns; i++)
                {
                    Console.Write("|");
                    Console.Write(Table[i,j].FacingUp ? Table[i,j].Value.ToString() : "x");
                }

                Console.Write(" - ");
                if (Side.Count <= j) Console.WriteLine(" ");
                else Console.WriteLine(Side[j].FacingUp ? Side[j].Value.ToString() : "x");
            }

            if (Hand != null)
            {
                Console.WriteLine();
                Console.WriteLine($"Hand: {Hand.Value}");
            }
        }
    }

    static class MyExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
