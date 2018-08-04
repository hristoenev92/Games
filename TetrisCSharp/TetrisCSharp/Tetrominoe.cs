using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisCSharp
{
    public class Tetrominoe
    {
        public static int[,] I = new int[1, 4] { { 1, 1, 1, 1 } };
        public static int[,] square = new int[2, 2] { { 1, 1 }, { 1, 1 } };
        public static int[,] T = new int[2, 3] { { 1, 1, 1 }, { 0, 1, 0 } };
        public static int[,] S = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        public static int[,] Z = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        public static int[,] J = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        public static int[,] L = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        public static List<int[,]> tetrominoes = new List<int[,]>() { I, square, T, S, Z, J, L };

        private bool isErect = false;
        private int[,] shape;
        private int[] pix = new int[2];
        public List<int[]> location = new List<int[]>();

        public Tetrominoe()
        {
            Random rnd = new Random();
            shape = tetrominoes[rnd.Next(0, tetrominoes.Count)];
            for (int i = Program.gridLenght; i < Program.gridLenght + 10; ++i)
            {
                for (int j = 3; j < Program.gridWidth; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("  ");
                }

            }
            for (int i = 0; i < shape.GetLength(0); i++)// draw next piece
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                    {
                        Console.SetCursorPosition(((Program.gridWidth - shape.GetLength(1)) / 2 + j) * 2 + 20, i + 5);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(Program.sqr);
                        Console.ResetColor();
                    }
                }
            }
        }

        public void Spawn()
        {
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                    {
                        location.Add(new int[] { i, (Program.gridWidth - shape.GetLength(1)) / 2 + j });
                    }
                }
            }
            Update();
        }

        public void Drop()
        {

            if (isSomethingBelow())
            {
                for (int i = 0; i < 4; i++)
                {
                    Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1]] = 1;
                }
                Program.isDropped = true;
            }
            else
            {
                for (int numCount = 0; numCount < 4; numCount++)
                {
                    location[numCount][0] += 1;
                }
                Update();
            }
        }

        public void Rotate()
        {
            List<int[]> templocation = new List<int[]>();
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                    {
                        templocation.Add(new int[] { i, (Program.gridWidth - shape.GetLength(1)) / 2 + j });
                    }
                }
            }

            if (shape == tetrominoes[0])
            {
                if (isErect == false)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i] = TransformMatrix(location[i], location[2], "Clockwise");
                    }
                }
                else
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i] = TransformMatrix(location[i], location[2], "Counterclockwise");
                    }
                }
            }

            else if (shape == tetrominoes[3])
            {
                for (int i = 0; i < location.Count; i++)
                {
                    templocation[i] = TransformMatrix(location[i], location[3], "Clockwise");
                }
            }

            else if (shape == tetrominoes[1]) return;
            else
            {
                for (int i = 0; i < location.Count; i++)
                {
                    templocation[i] = TransformMatrix(location[i], location[2], "Clockwise");
                }
            }


            for (int count = 0; isOverlayLeft(templocation) != false || isOverlayRight(templocation) != false || isOverlayBelow(templocation) != false; count++)
            {
                if (isOverlayLeft(templocation) == true)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i][1] += 1;
                    }
                }

                if (isOverlayRight(templocation) == true)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i][1] -= 1;
                    }
                }
                if (isOverlayBelow(templocation) == true)
                {
                    for (int i = 0; i < location.Count; i++)
                    {
                        templocation[i][0] -= 1;
                    }
                }
                if (count == 3)
                {
                    return;
                }
            }

            location = templocation;

        }

        public int[] TransformMatrix(int[] coord, int[] axis, string dir)
        {
            int[] pcoord = { coord[0] - axis[0], coord[1] - axis[1] };
            if (dir == "Counterclockwise")
            {
                pcoord = new int[] { -pcoord[1], pcoord[0] };
            }
            else if (dir == "Clockwise")
            {
                pcoord = new int[] { pcoord[1], -pcoord[0] };
            }

            return new int[] { pcoord[0] + axis[0], pcoord[1] + axis[1] };
        }
        public bool isSomethingBelow()
        {
            for (int i = 0; i < 4; i++)
            {
                if (location[i][0] + 1 >= Program.gridLenght)
                    return true;
                if (location[i][0] + 1 < Program.gridLenght)
                {
                    if (Program.droppedTetrominoeLocationGrid[location[i][0] + 1, location[i][1]] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool? isOverlayBelow(List<int[]> location)
        {
            List<int> ycoords = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                ycoords.Add(location[i][0]);
                if (location[i][0] >= Program.gridLenght)
                    return true;
                if (location[i][0] < 0)
                    return null;
                if (location[i][1] < 0)
                {
                    return null;
                }
                if (location[i][1] > Program.gridWidth - 1)
                {
                    return null;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (ycoords.Max() - ycoords.Min() == 3)
                {
                    if (ycoords.Max() == location[i][0] || ycoords.Max() - 1 == location[i][0])
                    {
                        if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (ycoords.Max() == location[i][0])
                    {
                        if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public bool isSomethingLeft()
        {
            for (int i = 0; i < 4; i++)
            {
                if (location[i][1] == 0)
                {
                    return true;
                }
                else if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1] - 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool? isOverlayLeft(List<int[]> location)
        {
            List<int> xcoords = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                xcoords.Add(location[i][1]);
                if (location[i][1] < 0)
                {
                    return true;
                }
                if (location[i][1] > Program.gridWidth - 1)
                {
                    return false;
                }
                if (location[i][0] >= Program.gridLenght)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if (xcoords.Min() == location[i][1] || xcoords.Min() + 1 == location[i][1])
                    {
                        if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (xcoords.Min() == location[i][1])
                    {
                        if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool isSomethingRight()
        {
            for (int i = 0; i < 4; i++)
            {
                if (location[i][1] == Program.gridWidth - 1)
                {
                    return true;
                }
                else if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1] + 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool? isOverlayRight(List<int[]> location)
        {
            List<int> xcoords = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                xcoords.Add(location[i][1]);
                if (location[i][1] > Program.gridWidth - 1)
                {
                    return true;
                }
                if (location[i][1] < 0)
                {
                    return false;
                }
                if (location[i][0] >= Program.gridLenght)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (xcoords.Max() - xcoords.Min() == 3)
                {
                    if (xcoords.Max() == location[i][1] || xcoords.Max() - 1 == location[i][1])
                    {
                        if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (xcoords.Max() == location[i][1])
                    {
                        if (Program.droppedTetrominoeLocationGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void Update()
        {
            for (int i = 0; i < Program.gridLenght; i++)
            {
                for (int j = 0; j < Program.gridWidth; j++)
                {
                    Program.grid[i, j] = 0;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Program.grid[location[i][0], location[i][1]] = 1;
            }
            Program.Draw();
        }
    }
}
