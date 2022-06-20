using UnityEngine;

public class MazeGeneratorCell
{
    public int X;
    public int Y;

    public bool BlockEnabled = true;
}

public class MazeGenerator
{
    public int _width = 10;
    public int _height = 10;

    public MazeGeneratorCell [,] GenerateMaze()
    {
        MazeGeneratorCell [,] maze = new MazeGeneratorCell[_width,_height];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }
        return maze;
    }
}
