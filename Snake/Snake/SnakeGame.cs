namespace Snake
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class SnakeGame
    {
        private static bool isGameOver = false;
        private static Random rng = new Random();

        private static int score = 0;
        private static int speed = 100;

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

            // Game Loop
            while (!isGameOver)
            {
                ResetWindowSize();
                Console.ForegroundColor = ConsoleColor.Black; // just so you cant write in the console
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

            // for replay
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
            else
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

        private static void Input(Coords direction)
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

        private static int ChangeSpeed(int score, int speed)
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

        private static void DrawBoarder()
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
            public Coords AppleCoords;
            public bool Exists = false;
            private const char APPLE = '@';
            private int timeSinceLastSpawn = Environment.TickCount;
            private int respawnTime = 5000;

            public Apple()
            {
                this.AppleCoords = new Coords(0, 0);
                this.Exists = false;
            }

            public void Draw()
            {
                if (this.Exists)
                {
                    Console.SetCursorPosition(this.AppleCoords.x, this.AppleCoords.y);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(APPLE);
                    Console.ResetColor();
                }
            }

            public void Delete()
            {
                if (this.Exists)
                {
                    Console.SetCursorPosition(this.AppleCoords.x, this.AppleCoords.y);
                    Console.WriteLine(" ");
                }
            }

            public void Update(Snake snake, Rocks rock)
            {
                if (this.timeSinceLastSpawn + this.respawnTime < Environment.TickCount)
                {
                    do
                    {
                        this.AppleCoords.x = rng.Next(2, Console.WindowWidth - 2);
                        this.AppleCoords.y = rng.Next(2, Console.WindowHeight - 2);
                    }
                    while (this.CollideswithElements(snake, rock));
                    this.timeSinceLastSpawn = Environment.TickCount;
                }
            }

            public bool CollideswithElements(Snake snake, Rocks rock)
            {
                bool flag = false;
                for (int i = 0; i < snake.SnakeBody.Count; i++)
                {
                    if (this.AppleCoords.EqualCoords(snake.SnakeBody[i]))
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == false)
                {
                    for (int i = 0; i < rock.AllRocks.Count; i++)
                    {
                        if (this.AppleCoords.EqualCoords(rock.AllRocks[i]))
                        {
                            flag = true;
                            break;
                        }
                    }
                }

                this.Exists = true;
                return flag;
            }
        }

        public class Rocks
        {
            public Coords RockCoords;
            public List<Coords> AllRocks;
            private const char ROCK = 'R';
            private int timeSinceLastSpawn = Environment.TickCount;
            private int respawnTime = 12000;

            public Rocks()
            {
                this.AllRocks = new List<Coords>();
                this.RockCoords = new Coords(
                    rng.Next(2, Console.WindowWidth - 2),
                    rng.Next(2, Console.WindowHeight - 2));
                this.AllRocks.Add(new Coords(
                    this.RockCoords.x, this.RockCoords.y));
            }

            public void Draw()
            {
                Console.SetCursorPosition(this.RockCoords.x, this.RockCoords.y);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(ROCK);
                Console.ResetColor();
            }

            public void Update(Snake snake, Apple apple)
            {
                if (this.timeSinceLastSpawn + this.respawnTime < Environment.TickCount)
                {
                    do
                    {
                        this.RockCoords.x = rng.Next(2, Console.WindowWidth - 2);
                        this.RockCoords.y = rng.Next(2, Console.WindowHeight - 2);
                    }
                    while (this.CollideswithElements(snake, apple));
                    this.timeSinceLastSpawn = Environment.TickCount;
                }
            }

            public bool CollideswithElements(Snake snake, Apple apple)
            {
                if (this.RockCoords.EqualCoords(apple.AppleCoords))
                {
                    for (int i = 0; i < snake.SnakeBody.Count; i++)
                    {
                        if (this.RockCoords.EqualCoords(snake.SnakeBody[i]))
                        {
                            for (int y = 0; y < this.AllRocks.Count; y++)
                            {
                                if (this.RockCoords.EqualCoords(this.AllRocks[y]))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                this.AllRocks.Add(new Coords(
                    this.RockCoords.x, this.RockCoords.y));
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

            public bool EqualCoords(Coords newCoords)
            {
                return this.x == newCoords.x &&
                    this.y == newCoords.y;
            }
        }

        public class Snake
        {
            public List<Coords> SnakeBody;

            public Snake()
            {
                Coords snakeHeadRng = new Coords(rng.Next(2, Console.WindowWidth - 2), rng.Next(2, Console.WindowHeight - 8));
                this.SnakeBody = new List<Coords>
                {
                    snakeHeadRng,
                    new Coords(snakeHeadRng.x, snakeHeadRng.y + 1),
                    new Coords(snakeHeadRng.x, snakeHeadRng.y + 2),
                    new Coords(snakeHeadRng.x, snakeHeadRng.y + 3),
                    new Coords(snakeHeadRng.x, snakeHeadRng.y + 4)
                };
            }

            public void Draw(Coords direction)
            {
                const char HEAD = ' ';
                Console.SetCursorPosition(this.SnakeBody[0].x, this.SnakeBody[0].y);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(HEAD);
                Console.ResetColor();
                for (int i = 1; i < this.SnakeBody.Count; i++)
                {
                    Console.SetCursorPosition(this.SnakeBody[i].x, this.SnakeBody[i].y);
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(' ');
                    Console.ResetColor();
                }
            }

            public void Delete()
            {
                for (int i = 0; i < this.SnakeBody.Count; i++)
                {
                    Console.SetCursorPosition(this.SnakeBody[i].x, this.SnakeBody[i].y);
                    Console.Write(" ");
                }
            }

            public void Update(Coords direction, Apple apple, Rocks rock)
            {
                for (int i = this.SnakeBody.Count - 1; i > 0; i--)
                {
                    this.SnakeBody[i].x = this.SnakeBody[i - 1].x;
                    this.SnakeBody[i].y = this.SnakeBody[i - 1].y;
                }

                this.SnakeBody[0].x += direction.x;
                this.SnakeBody[0].y += direction.y;

                ////to end if snake hits a wall
                //if (snakeBody[0].x<1|| 
                //    snakeBody[0].x>Console.WindowWidth||
                //    snakeBody[0].y<1||
                //    snakeBody[0].y>Console.WindowHeight)
                //{
                //    isGameOver = true;
                //}

                //to pass through walls
                if (this.SnakeBody[0].x == Console.BufferWidth - 2)
                {
                    this.SnakeBody[0].x = 2;
                }

                if (this.SnakeBody[0].y == Console.BufferHeight - 2)
                {
                    this.SnakeBody[0].y = 2;
                }

                if (this.SnakeBody[0].x == 1)
                {
                    this.SnakeBody[0].x = Console.BufferWidth - 3;
                }

                if (this.SnakeBody[0].y == 1)
                {
                    this.SnakeBody[0].y = Console.BufferHeight - 3;
                }

                // apple collision
                if (this.SnakeBody[0].EqualCoords(apple.AppleCoords))
                {
                    this.EatApple(apple);
                }

                // rock collision
                for (int i = 0; i < rock.AllRocks.Count; i++)
                {
                    if (this.SnakeBody[0].EqualCoords(rock.AllRocks[i]))
                    {
                        isGameOver = true;
                        break;
                    }
                }

                // tail collison
                for (int i = 1; i < this.SnakeBody.Count; i++)
                {
                    if (this.SnakeBody[0].EqualCoords(this.SnakeBody[i]))
                    {
                        isGameOver = true;
                        break;
                    }
                }
            }

            public void EatApple(Apple apple)
            {
                this.SnakeBody.Add(new Coords(
                    this.SnakeBody[this.SnakeBody.Count - 1].x,
                    this.SnakeBody[this.SnakeBody.Count - 1].y));
                apple.Exists = false;
                score++;
            }
        }
    }
}
