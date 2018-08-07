using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {
        public static bool isGameOver = false;
        public static Random rng = new Random();

        public static int score = 0;
        public static int speed = 100;

        static void Main()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth = 50;
            Console.BufferHeight = Console.WindowHeight = 30;

            Coords direction = new Coords(0, -1);
            Apple apple = new Apple();
            Rocks rock = new Rocks();
            Snake snake = new Snake();

            WelcomeMessage();
            DrawBoarder();

            //Game Loop
            while (!isGameOver)
            {
                ResetWindowSize();
                Console.ForegroundColor = ConsoleColor.Black; //just so you cant write in the console
                Input(direction);
                rock.Update(snake, apple);
                apple.Update(snake, rock);
                snake.Update(direction, apple, rock);
                apple.Draw();
                rock.Draw();
                snake.Draw(direction);
                speed = ChangeSpeed(score, speed);
                Thread.Sleep(speed);
                snake.Delete();
                apple.Delete();
            }

            Console.Clear();
            Console.SetCursorPosition(20, 6);
            Console.WriteLine("GAME OVER!");
            Console.SetCursorPosition(18, 8);
            Console.WriteLine("Your Score: " + score);

            //for replay
            Console.SetCursorPosition(17, 10);
            Console.WriteLine("Replay ? (Y / N)");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Y)
            {
                direction = new Coords(0, -1);
                apple = new Apple();
                snake = new Snake();
                isGameOver = false;
                score = 0;
                speed = 100;
                GC.Collect();
                Console.Clear();
                Main();
            }
            else if (key.Key == ConsoleKey.N)
            {
                return;
            }
        }

        private static void WelcomeMessage()
        {
            Console.SetCursorPosition(17, 6);
            Console.WriteLine("Eat green apples,");
            Console.SetCursorPosition(15, 7);
            Console.WriteLine("don't eat grey rocks");
            Console.SetCursorPosition(14, 9);
            Console.WriteLine("Press any key to start");
            Console.ReadKey(true);
            Console.Clear();
        }

        private static void ResetWindowSize()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth = 50;
            Console.BufferHeight = Console.WindowHeight = 30;
        }

        static void Input(Coords direction)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.RightArrow && direction.x != -1)
                {
                    direction.x = 1;
                    direction.y = 0;
                }
                else if (key.Key == ConsoleKey.LeftArrow && direction.x != 1)
                {
                    direction.x = -1;
                    direction.y = 0;
                }
                else if (key.Key == ConsoleKey.DownArrow && direction.y != -1)
                {
                    direction.x = 0;
                    direction.y = 1;
                }
                else if (key.Key == ConsoleKey.UpArrow && direction.y != 1)
                {
                    direction.x = 0;
                    direction.y = -1;
                }
            }
        }

        static int ChangeSpeed(int score, int speed)
        {
            switch (score)
            {
                case 3:
                    {
                        speed = 100;
                        break;
                    }
                case 5:
                    {
                        speed = 80;
                        break;
                    }
                case 10:
                    {
                        speed = 65;
                        break;
                    }
                case 20:
                    {
                        speed = 50;
                        break;
                    }
            }
            return speed;
        }

        static void DrawBoarder()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            for (int i = 1; i < Console.WindowWidth - 1; i++)
            {
                Console.SetCursorPosition(i, 1);
                Console.Write(' ');
                Console.SetCursorPosition(i, Console.WindowHeight - 2);
                Console.Write(' ');
            }
            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(' ');
                Console.SetCursorPosition(Console.WindowWidth - 2, i);
                Console.Write(' ');
            }
            Console.ResetColor();
        }

        public class Apple
        {
            char apple = '@';
            public Coords appleCoords;
            public bool exists = false;
            int timeSinceLastSpawn = Environment.TickCount;
            int respawnTime = 5000;

            public Apple()
            {
                appleCoords = new Coords(0, 0);
                exists = false;
            }

            public void Draw()
            {
                if (exists)
                {
                    Console.SetCursorPosition(appleCoords.x, appleCoords.y);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(apple);
                    Console.ResetColor();
                }
            }

            public void Delete()
            {
                if (exists)
                {
                    Console.SetCursorPosition(appleCoords.x, appleCoords.y);
                    Console.WriteLine(" ");
                }
            }

            public void Update(Snake snake, Rocks rock)
            {
                if (timeSinceLastSpawn + respawnTime < Environment.TickCount)
                {
                    do
                    {
                        appleCoords.x = rng.Next(2, Console.WindowWidth - 2);
                        appleCoords.y = rng.Next(2, Console.WindowHeight - 2);
                    }
                    while (CollideswithElements(snake, rock));
                    timeSinceLastSpawn = Environment.TickCount;
                }
            }

            public bool CollideswithElements(Snake snake, Rocks rock)
            {
                bool flag = false;
                for (int i = 0; i < snake.snakeBody.Count; i++)
                {
                    if (appleCoords.equalCoords(snake.snakeBody[i]))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    for (int i = 0; i < rock.allRocks.Count; i++)
                    {
                        if (appleCoords.equalCoords(rock.allRocks[i]))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                exists = true;
                return flag;
            }
        }

        public class Rocks
        {
            char rock = 'G';
            public Coords rockCoords;
            public List<Coords> allRocks;
            int timeSinceLastSpawn = Environment.TickCount;
            int respawnTime = 12000;

            public Rocks()
            {
                allRocks = new List<Coords>();
                rockCoords = new Coords(
                    rng.Next(2, Console.WindowWidth - 2),
                    rng.Next(2, Console.WindowHeight - 2));
                allRocks.Add(new Coords(
                    rockCoords.x, rockCoords.y));
            }

            public void Draw()
            {
                Console.SetCursorPosition(rockCoords.x, rockCoords.y);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(rock);
                Console.ResetColor();
            }

            public void Update(Snake snake, Apple apple)
            {
                if (timeSinceLastSpawn + respawnTime < Environment.TickCount)
                {
                    do
                    {
                        rockCoords.x = rng.Next(2, Console.WindowWidth - 2);
                        rockCoords.y = rng.Next(2, Console.WindowHeight - 2);
                    }
                    while (CollideswithElements(snake, apple));
                    timeSinceLastSpawn = Environment.TickCount;
                }
            }

            public bool CollideswithElements(Snake snake, Apple apple)
            {
                if (rockCoords.equalCoords(apple.appleCoords))
                {
                    for (int i = 0; i < snake.snakeBody.Count; i++)
                    {
                        if (rockCoords.equalCoords(snake.snakeBody[i]))
                        {
                            return true;
                        }
                    }
                }
                allRocks.Add(new Coords(
                    rockCoords.x, rockCoords.y));
                return false;
            }
        }

        public class Coords
        {
            public int x;
            public int y;

            public Coords(int X, int Y)
            {
                this.x = X;
                this.y = Y;
            }

            //public void Add(Coords newCoords)
            //{
            //    x = newCoords.x;
            //    y = newCoords.y;
            //}

            public bool equalCoords(Coords newCoords)
            {
                return x == newCoords.x &&
                    y == newCoords.y;
            }
        }

        public class Snake
        {
            public List<Coords> snakeBody;

            public Snake()
            {
                Coords snakeHeadRng = new Coords(rng.Next(2, Console.WindowWidth - 2), rng.Next(2, Console.WindowHeight - 8));
                snakeBody = new List<Coords>
                {
                    snakeHeadRng,
                    new Coords(snakeHeadRng.x,snakeHeadRng.y+1),
                    new Coords(snakeHeadRng.x,snakeHeadRng.y+2),
                    new Coords(snakeHeadRng.x,snakeHeadRng.y+3),
                    new Coords(snakeHeadRng.x,snakeHeadRng.y+4)
                };
            }

            public void Draw(Coords direction)
            {
                char head = ' ';
                //if (direction.x == 1)
                //{
                //    head = '>';
                //}
                //if (direction.x == -1)
                //{
                //    head = '<';
                //}
                //if (direction.y == 1)
                //{
                //    head = 'V';
                //}
                //if (direction.y == -1)
                //{
                //    head = '^';
                //}
                Console.SetCursorPosition(snakeBody[0].x, snakeBody[0].y);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(head);
                Console.ResetColor();
                for (int i = 1; i < snakeBody.Count; i++)
                {
                    Console.SetCursorPosition(snakeBody[i].x, snakeBody[i].y);
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(' ');
                    Console.ResetColor();
                }
            }

            public void Delete()
            {
                for (int i = 0; i < snakeBody.Count; i++)
                {
                    Console.SetCursorPosition(snakeBody[i].x, snakeBody[i].y);
                    Console.Write(" ");
                }
            }

            public void Update(Coords direction, Apple apple, Rocks rock)
            {
                for (int i = snakeBody.Count - 1; i > 0; i--)
                {
                    snakeBody[i].x = snakeBody[i - 1].x;
                    snakeBody[i].y = snakeBody[i - 1].y;
                }
                //this.snakeBody[0].Add(direction);
                snakeBody[0].x += direction.x;
                snakeBody[0].y += direction.y;

                ////to end if snake hits a wall
                //if (snakeBody[0].x<1|| 
                //    snakeBody[0].x>Console.WindowWidth||
                //    snakeBody[0].y<1||
                //    snakeBody[0].y>Console.WindowHeight)
                //{
                //    isGameOver = true;
                //}

                //to pass through walls
                if (snakeBody[0].x == Console.BufferWidth - 2)
                {
                    snakeBody[0].x = 2;
                }
                if (snakeBody[0].y == Console.BufferHeight - 2)
                {
                    snakeBody[0].y = 2;
                }
                if (snakeBody[0].x == 1)
                {
                    snakeBody[0].x = Console.BufferWidth - 3;
                }
                if (snakeBody[0].y == 1)
                {
                    snakeBody[0].y = Console.BufferHeight - 3;
                }

                //apple collision
                if (snakeBody[0].equalCoords(apple.appleCoords))
                {
                    EatApple(apple);
                }

                //rock collision
                for (int i = 0; i < rock.allRocks.Count; i++)
                {
                    if (snakeBody[0].equalCoords(rock.allRocks[i]))
                    {
                        isGameOver = true;
                        break;
                    }
                }


                //tail collison
                for (int i = 1; i < snakeBody.Count; i++)
                {
                    if (snakeBody[0].equalCoords(snakeBody[i]))
                    {
                        isGameOver = true;
                        break;
                    }
                }
            }

            public void EatApple(Apple apple)
            {
                snakeBody.Add(new Coords(
                    snakeBody[snakeBody.Count - 1].x,
                    snakeBody[snakeBody.Count - 1].y));
                apple.exists = false;
                score++;
            }
        }
    }
}
