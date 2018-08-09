namespace TicTacToe
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class TicTacToe
    {
        private static List<Position> allPositions = new List<Position>
        {
            new Position(0, new Coords(0, 0)),
            new Position(1, new Coords(2, 2)),
            new Position(2, new Coords(9, 2)),
            new Position(3, new Coords(16, 2)),
            new Position(4, new Coords(2, 5)),
            new Position(5, new Coords(9, 5)),
            new Position(6, new Coords(16, 5)),
            new Position(7, new Coords(2, 8)),
            new Position(8, new Coords(9, 8)),
            new Position(9, new Coords(16, 8))
        };

        private static bool isGameOver = false;

        static void Main()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth = 50;
            Console.BufferHeight = Console.WindowHeight = 20;

            int turn = 1;
            Position currentInput;
            string result = string.Empty;

            WelcomeMessage();
            DrawBoarder();

            while (!isGameOver)
            {
                ResetWindowSize();
                currentInput = Input();
                // for wrong input
                if (currentInput.position == 0)
                {
                    Console.SetCursorPosition(1, 13);
                    Console.WriteLine("This is not a valid position!");
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(1, 13);
                    Console.WriteLine("                                                    ");
                    continue;
                }

                turn = Update(turn, currentInput);
                result = EndGame();
            }

            Thread.Sleep(1500);
            Console.Clear();
            Console.SetCursorPosition(20, 6);
            Console.WriteLine("GAME OVER!");
            Console.SetCursorPosition(15, 8);
            Console.WriteLine(result);

            // for replay
            Console.SetCursorPosition(17, 10);
            Console.WriteLine("Replay ? (Y / N)");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Y)
            {
                isGameOver = false;
                turn = 1;
                foreach (var position in allPositions)
                {
                    position.isEmpty = true;
                    position.type = string.Empty;
                }

                GC.Collect();
                Console.Clear();
                Main();
            }
            else
            {
                return;
            }
        }

        private static void ResetWindowSize()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth = 50;
            Console.BufferHeight = Console.WindowHeight = 20;
        }

        private static void WelcomeMessage()
        {
            Console.SetCursorPosition(20, 6);
            Console.WriteLine("Tic Tac Toe");
            Console.SetCursorPosition(18, 7);
            Console.WriteLine("three in a row!");
            Console.SetCursorPosition(9, 9);
            Console.WriteLine("Press a key to fill the position.");
            Console.SetCursorPosition(15, 11);
            Console.WriteLine("Press any key to start");
            Console.ReadKey(true);
            Console.Clear();
        }

        private static void DrawBoarder()
        {
            Console.SetCursorPosition(1, 1);
            Console.Write("┌");
            Console.SetCursorPosition(22, 1);
            Console.Write("┐");
            Console.SetCursorPosition(1, 10);
            Console.Write("└");
            Console.SetCursorPosition(22, 10);
            Console.Write("┘");
            for (int i = 2; i < 22; i++)
            {
                Console.SetCursorPosition(i, 1);
                Console.Write("─");
                Console.SetCursorPosition(i, 4);
                Console.Write("─");
                Console.SetCursorPosition(i, 7);
                Console.Write("─");
                Console.SetCursorPosition(i, 10);
                Console.Write("─");
            }

            for (int i = 2; i < 10; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write("│");
                Console.SetCursorPosition(8, i);
                Console.Write("│");
                Console.SetCursorPosition(15, i);
                Console.Write("│");
                Console.SetCursorPosition(22, i);
                Console.Write("│");
            }

            Console.SetCursorPosition(2, 2);
            Console.WriteLine("1");
            Console.SetCursorPosition(9, 2);
            Console.WriteLine("2");
            Console.SetCursorPosition(16, 2);
            Console.WriteLine("3");
            Console.SetCursorPosition(2, 5);
            Console.WriteLine("4");
            Console.SetCursorPosition(9, 5);
            Console.WriteLine("5");
            Console.SetCursorPosition(16, 5);
            Console.WriteLine("6");
            Console.SetCursorPosition(2, 8);
            Console.WriteLine("7");
            Console.SetCursorPosition(9, 8);
            Console.WriteLine("8");
            Console.SetCursorPosition(16, 8);
            Console.WriteLine("9");
            Console.SetCursorPosition(1, 12);
            Console.WriteLine("It's player one's turn!");
        }

        private static int Update(int turn, Position position)
        {
            if (position.isEmpty)
            {
                if (turn % 2 == 0)
                {
                    WriteX(position.posCoords.x, position.posCoords.y);
                    position.isEmpty = false;
                    position.type = "X";
                    Console.SetCursorPosition(1, 12);
                    Console.WriteLine("It's player one's turn!");
                    turn++;
                }
                else
                {
                    WriteO(position.posCoords.x, position.posCoords.y);
                    position.isEmpty = false;
                    position.type = "O";
                    Console.SetCursorPosition(1, 12);
                    Console.WriteLine("It's player two's turn!");
                    turn++;
                }
            }
            else
            {
                Console.SetCursorPosition(1, 13);
                Console.WriteLine("The position is not empty!");
                Thread.Sleep(1000);
                Console.SetCursorPosition(1, 13);
                Console.WriteLine("                                           ");
            }

            return turn;
        }

        private static void WriteX(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write("  \\/");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("  /\\");
            Console.ResetColor();
        }

        private static void WriteO(int x, int y)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(x, y);
            Console.Write("  /\\");
            Console.SetCursorPosition(x, y + 1);
            Console.Write("  \\/");
            Console.ResetColor();
        }

        private static Position Input()
        {
            Console.SetCursorPosition(25, 3);
            Console.ForegroundColor = ConsoleColor.Black;
            ConsoleKeyInfo key = Console.ReadKey();
            Console.ResetColor();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    {
                        return allPositions[1];
                    }

                case ConsoleKey.D2:
                    {
                        return allPositions[2];
                    }

                case ConsoleKey.D3:
                    {
                        return allPositions[3];
                    }

                case ConsoleKey.D4:
                    {
                        return allPositions[4];
                    }

                case ConsoleKey.D5:
                    {
                        return allPositions[5];
                    }

                case ConsoleKey.D6:
                    {
                        return allPositions[6];
                    }

                case ConsoleKey.D7:
                    {
                        return allPositions[7];
                    }

                case ConsoleKey.D8:
                    {
                        return allPositions[8];
                    }

                case ConsoleKey.D9:
                    {
                        return allPositions[9];
                    }

                default:
                    {
                        return allPositions[0];
                    }
            }
        }

        private static string EndGame()
        {
            string player1 = "Winner is player 1!";
            string player2 = "Winner is player 2!";
            string draw = "The game is a draw!";
            List<Position> Xs = allPositions.Where(a => a.type == "X").ToList<Position>();
            List<Position> Os = allPositions.Where(a => a.type == "O").ToList<Position>();

            if (Xs.Count > 2)
            {
                if ((Xs.Contains(allPositions[1]) && Xs.Contains(allPositions[2]) && Xs.Contains(allPositions[3])) ||
                    (Xs.Contains(allPositions[4]) && Xs.Contains(allPositions[5]) && Xs.Contains(allPositions[6])) ||
                    (Xs.Contains(allPositions[7]) && Xs.Contains(allPositions[8]) && Xs.Contains(allPositions[9])) ||
                    (Xs.Contains(allPositions[1]) && Xs.Contains(allPositions[4]) && Xs.Contains(allPositions[7])) ||
                    (Xs.Contains(allPositions[2]) && Xs.Contains(allPositions[5]) && Xs.Contains(allPositions[8])) ||
                    (Xs.Contains(allPositions[3]) && Xs.Contains(allPositions[6]) && Xs.Contains(allPositions[9])) ||
                    (Xs.Contains(allPositions[1]) && Xs.Contains(allPositions[5]) && Xs.Contains(allPositions[9])) ||
                    (Xs.Contains(allPositions[7]) && Xs.Contains(allPositions[5]) && Xs.Contains(allPositions[3])))
                {
                    isGameOver = true;
                    return player2;
                }
            }

            if (Os.Count > 2)
            {
                if ((Os.Contains(allPositions[1]) && Os.Contains(allPositions[2]) && Os.Contains(allPositions[3])) ||
                    (Os.Contains(allPositions[4]) && Os.Contains(allPositions[5]) && Os.Contains(allPositions[6])) ||
                    (Os.Contains(allPositions[7]) && Os.Contains(allPositions[8]) && Os.Contains(allPositions[9])) ||
                    (Os.Contains(allPositions[1]) && Os.Contains(allPositions[4]) && Os.Contains(allPositions[7])) ||
                    (Os.Contains(allPositions[2]) && Os.Contains(allPositions[5]) && Os.Contains(allPositions[8])) ||
                    (Os.Contains(allPositions[3]) && Os.Contains(allPositions[6]) && Os.Contains(allPositions[9])) ||
                    (Os.Contains(allPositions[1]) && Os.Contains(allPositions[5]) && Os.Contains(allPositions[9])) ||
                    (Os.Contains(allPositions[7]) && Os.Contains(allPositions[5]) && Os.Contains(allPositions[3])))
                {
                    isGameOver = true;
                    return player1;
                }
            }

            if (Xs.Count + Os.Count == 9)
            {
                isGameOver = true;
                return draw;
            }

            return string.Empty;
        }

        private class Coords
        {
            public int x;
            public int y;

            public Coords(int X, int Y)
            {
                this.x = X;
                this.y = Y;
            }
        }

        private class Position
        {
            public int position;
            public bool isEmpty = true;
            public string type = string.Empty;
            public Coords posCoords;

            public Position(int pos, Coords coords)
            {
                this.position = pos;
                this.posCoords = coords;
            }
        }
    }
}
