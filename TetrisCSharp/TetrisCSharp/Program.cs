using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TetrisCSharp
{
    class Program
    {
        public static string sqr = "■";
        public static int gridWidth = 10;
        public static int gridLenght = 23;
        public static int[,] grid = new int[gridLenght, gridWidth];
        public static int[,] droppedTetrominoeLocationGrid = new int[gridLenght, gridWidth];
        public static Stopwatch timer = new Stopwatch();
        public static Stopwatch dropTimer = new Stopwatch();
        public static int dropTime, dropRate;
        public static bool isDropped = false;
        static Tetrominoe tet;
        static Tetrominoe nextTet;
        public static ConsoleKeyInfo key;
        public static bool isKeyPressed = false;
        public static int linesCleared = 0, score = 0, level = 1;

        static void Main()
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = Environment.CurrentDirectory + "\\Original Tetris theme.wav";
            sp.PlayLooping();

            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth = 41;
            Console.BufferHeight = Console.WindowHeight = 30;

            Console.SetCursorPosition(9, 5);
            Console.WriteLine("Up Arrow for rotation");
            Console.SetCursorPosition(8, 7);
            Console.WriteLine("Spacebar to drop Piece");
            Console.SetCursorPosition(8, 9);
            Console.WriteLine("Press any key to start");
            Console.ReadKey(true);
            Console.Clear();

            timer.Start();
            dropTimer.Start();
            long time = timer.ElapsedMilliseconds;
            Console.SetCursorPosition(gridLenght + 2, 0);
            Console.WriteLine("Level " + level);
            Console.SetCursorPosition(gridLenght + 2, 1);
            Console.WriteLine("Score " + score);
            Console.SetCursorPosition(gridLenght + 2, 2);
            Console.WriteLine("LinesCleared " + linesCleared);
            nextTet = new Tetrominoe();
            tet = nextTet;
            tet.Spawn();
            nextTet = new Tetrominoe();

            Update();

            Console.SetCursorPosition(10, gridLenght+3);
            Console.WriteLine("Game Over");
            Console.SetCursorPosition(7, gridLenght + 4);
            Console.WriteLine("Replay ? (Y / N)");
            string input = Console.ReadLine();

            if (input == "y" || input == "Y")
            {
                int[,] grid = new int[gridLenght, gridWidth];
                droppedTetrominoeLocationGrid = new int[gridLenght, gridWidth];
                timer = new Stopwatch();
                dropTimer = new Stopwatch();
                isDropped = false;
                isKeyPressed = false;
                linesCleared = 0;
                score = 0;
                level = 1;
                GC.Collect();
                Console.Clear();
                Main();
            }
            else return;
        }

        private static void ResetWindowSize()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth = 41;
            Console.BufferHeight = Console.WindowHeight = 30;
        }

        private static void Update()
        {
            while (true)
            {
                ResetWindowSize();
                drawBorder();
                dropTime = (int)dropTimer.ElapsedMilliseconds;
                if (dropTime > dropRate)
                {
                    dropTime = 0;
                    dropTimer.Restart();
                    tet.Drop();
                }
                if (isDropped == true)
                {
                    tet = nextTet;
                    nextTet = new Tetrominoe();
                    tet.Spawn();

                    isDropped = false;
                }
                int j;
                for (j = 0; j < gridWidth; j++)
                {
                    if (droppedTetrominoeLocationGrid[0, j] == 1)
                        return;
                }
                Console.SetCursorPosition(gridLenght + 1, gridWidth + 1);
                Input();
                ClearBlock();
            } 
        }
        private static void ClearBlock()
        {
            int combo = 0;
            for (int i = 0; i < gridLenght; i++)
            {
                int j;
                for (j = 0; j < gridWidth; j++)
                {
                    if (droppedTetrominoeLocationGrid[i, j] == 0)
                        break;
                }
                if (j == gridWidth)
                {
                    linesCleared++;
                    combo++;
                    for (j = 0; j < gridWidth; j++)
                    {
                        droppedTetrominoeLocationGrid[i, j] = 0;
                    }
                    int[,] newdroppedtetrominoeLocationGrid = new int[gridLenght, gridWidth];
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < gridWidth; l++)
                        {
                            newdroppedtetrominoeLocationGrid[k + 1, l] = droppedTetrominoeLocationGrid[k, l];
                        }
                    }
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < gridWidth; l++)
                        {
                            droppedTetrominoeLocationGrid[k, l] = 0;
                        }
                    }
                    for (int k = 0; k < gridLenght; k++)
                        for (int l = 0; l < gridWidth; l++)
                            if (newdroppedtetrominoeLocationGrid[k, l] == 1)
                                droppedTetrominoeLocationGrid[k, l] = 1;
                    Draw();
                }
            }
            if (combo == 1)
                score += 40 * level;
            else if (combo == 2)
                score += 100 * level;
            else if (combo == 3)
                score += 300 * level;
            else if (combo > 3)
                score += 300 * combo * level;

            if (linesCleared < 5) level = 1;
            else if (linesCleared < 10) level = 2;
            else if (linesCleared < 15) level = 3;
            else if (linesCleared < 25) level = 4;
            else if (linesCleared < 35) level = 5;
            else if (linesCleared < 50) level = 6;
            else if (linesCleared < 70) level = 7;
            else if (linesCleared < 90) level = 8;
            else if (linesCleared < 110) level = 9;
            else if (linesCleared < 150) level = 10;


            if (combo > 0)
            {
                Console.SetCursorPosition(gridLenght + 2, 0);
                Console.WriteLine("Level " + level);
                Console.SetCursorPosition(gridLenght + 2, 1);
                Console.WriteLine("Score " + score);
                Console.SetCursorPosition(gridLenght + 2, 2);
                Console.WriteLine("LinesCleared " + linesCleared);
            }

            dropRate = 400 - 20 * (level-1);

        }
        private static void Input()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey();
                isKeyPressed = true;
            }
            else
                isKeyPressed = false;

            if (key.Key == ConsoleKey.LeftArrow && !tet.isSomethingLeft() && isKeyPressed)
            {
                for (int i = 0; i < 4; i++)
                {
                    tet.location[i][1] -= 1;
                }
                tet.Update();
            }
            else if (key.Key == ConsoleKey.RightArrow && !tet.isSomethingRight() && isKeyPressed)
            {
                for (int i = 0; i < 4; i++)
                {
                    tet.location[i][1] += 1;
                }
                tet.Update();
            }
            if (key.Key == ConsoleKey.DownArrow && isKeyPressed)
            {
                tet.Drop();
            }
            if (key.Key == ConsoleKey.Spacebar && isKeyPressed)
            {
                for (; tet.isSomethingBelow() != true;)
                {
                    tet.Drop();
                }
            }
            if (key.Key == ConsoleKey.UpArrow && isKeyPressed)
            {
                tet.Rotate();
                tet.Update();
            }
        }
        public static void Draw()
        {
            for (int i = 0; i < gridLenght; ++i)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    Console.SetCursorPosition(1 + 2 * j, i);
                    if (grid[i, j] == 1 || droppedTetrominoeLocationGrid[i, j] == 1)
                    {
                        Console.SetCursorPosition(1 + 2 * j, i);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(sqr);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }

            }
        }
        public static void drawBorder()
        {
            for (int lengthCount = 0; lengthCount <= gridLenght-1; ++lengthCount)
            {
                Console.SetCursorPosition(0, lengthCount);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                Console.ResetColor();
                Console.SetCursorPosition(gridLenght-2, lengthCount);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(" ");
                Console.ResetColor();
            }
            Console.SetCursorPosition(0, gridLenght);
            for (int widthCount = 0; widthCount <= gridWidth; widthCount++)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("  ");
                Console.ResetColor();
            }

        }
    }
}
