namespace Hangman
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    public class HangmanGame
    {
        static void Main()
        {
            int tries = 8;
            Random rnd = new Random();
            string[] dictionary = File.ReadAllLines("wordsEn.txt");
            List<char> wordToFind = new List<char>(dictionary[rnd.Next(0, dictionary.Length - 1)]);
            Console.WriteLine(string.Join(string.Empty, wordToFind.ToArray()));
            int distinctChars = wordToFind.Distinct().Count();
            char charToSearch;

            List<char> used = new List<char>();
            List<char> guessed = new List<char>();
            string endMessage;

            // game loop
            while (true)
            {
                // interface
                DrawInterface(tries, wordToFind, used, guessed);

                // input validation
                charToSearch = Validation(used);

                used.Add(charToSearch);
                if (wordToFind.Contains(charToSearch))
                {
                    guessed.Add(charToSearch);

                    // win condition
                    if (guessed.Count == distinctChars)
                    {
                        endMessage = "Браво, успяхте да познаете думата:" + string.Join(string.Empty, wordToFind.ToArray());
                        ////Console.Clear();
                        break;
                    }
                }
                else
                {
                    tries--;

                    // loss
                    if (tries == 0)
                    {
                        endMessage = "Опитите ви свършиха думата беше: " + string.Join(string.Empty, wordToFind.ToArray());
                        ////Console.Clear();
                        break;
                    }
                }
            }

            Console.WriteLine("\n\n");
            Console.WriteLine(endMessage);
        }

        private static char Validation(List<char> used)
        {
            char charToSearch;
            while (true)
            {
                charToSearch = Console.ReadKey(true).KeyChar;
                Console.Write(charToSearch);
                Thread.Sleep(300);  // added for clarity
                if ((charToSearch >= 97 && charToSearch <= 122) && !used.Contains(charToSearch)) // ASCII a-z
                {
                    break;
                }
                else
                {
                    Console.Write("\nВъведената буква е невалидна или вече използвана, моля опитайте отново: ");
                }
            }

            return charToSearch;
        }

        private static void DrawInterface(int tries, List<char> wordToFind, List<char> used, List<char> guessed)
        {
            Console.Clear();
            Console.WriteLine("Думата е:");
            foreach (char x in wordToFind)
            {
                if (guessed.Contains(x))
                {
                    Console.Write(x + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }

            Console.WriteLine("\n");
            Console.WriteLine("Оставащи опити: " + tries);
            DrawingNoose(tries);
            Console.Write("Използвани букви: ");
            foreach (char x in used)
            {
                Console.Write(x + " ");
            }

            Console.WriteLine("\n");
            Console.Write("Въведете буква:");
        }

        private static void DrawingNoose(int tries)
        {
            switch (tries)
            {
                case 7:
                    Console.WriteLine(" -------" +
                                    "\n |     |" +
                                    "\n |" +
                                    "\n |" +
                                    "\n |" +
                                    "\n |" +
                                    "\n---");
                    break;
                case 6:
                    Console.WriteLine(" -------" +
                                    "\n |     |" +
                                    "\n |     O" +
                                    "\n |" +
                                    "\n |" +
                                    "\n |" +
                                    "\n---");
                    break;
                case 5:
                    Console.WriteLine(" -------" +
                                    "\n |     |" +
                                    "\n |     O" +
                                    "\n |     |" +
                                    "\n |" +
                                    "\n |" +
                                    "\n---");
                    break;
                case 4:
                    Console.WriteLine(" -------" +
                                    "\n |     |" +
                                    "\n |     O" +
                                    "\n |    /|" +
                                    "\n |" +
                                    "\n |" +
                                    "\n---");
                    break;
                case 3:
                    Console.WriteLine(" -------" +
                                    "\n |     |" +
                                    "\n |     O" +
                                    "\n |    /|\\" +
                                    "\n |" +
                                    "\n |" +
                                    "\n---");
                    break;
                case 2:
                    Console.WriteLine(" -------" +
                                    "\n |     |" +
                                    "\n |     O" +
                                    "\n |    /|\\ " +
                                    "\n |    / " +
                                    "\n |" +
                                    "\n---");
                    break;
                case 1:
                    Console.WriteLine(" -------" +
                                    "\n |     |" +
                                    "\n |     O " +
                                    "\n |    /|\\ " +
                                    "\n |    / \\" +
                                    "\n |" +
                                    "\n---");
                    break;
                default:
                    break;
            }
        }
    }
}
