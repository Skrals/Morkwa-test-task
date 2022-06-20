using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell
{
    public int X;
    public int Y;

    public bool BlockEnabled = true;
    public bool Visited = false;

    public bool Start = false;
    public bool Finish = false;
}

public class MazeGenerator
{
    public int _width = 11;
    public int _height = 11;

    public MazeGeneratorCell[,] GenerateMaze()
    {
        MazeGeneratorCell[,] maze = new MazeGeneratorCell[_width, _height];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            maze[x, _height - 1].BlockEnabled = false;
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            maze[_width - 1, y].BlockEnabled = false;
        }

        RemoveWalls(maze);

        PlaceExitAndStart(maze);

        FillEmpty(maze);

        return maze;
    }

    private void FillEmpty(MazeGeneratorCell[,] maze)
    {
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            for (int x = 1; x  < maze.GetLength(0); x++)
            {
                if ((x + 2) < maze.GetLength(0))
                {
                    if (maze[x, y].BlockEnabled == true && maze[x + 1, y].BlockEnabled == false && maze[x + 2, y].BlockEnabled == true)
                    {
                        maze[x + 1, y].BlockEnabled = true;
                    }
                }
            }
        }
    }

    private void PlaceExitAndStart(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell furthest = maze[0, 0];
        furthest.Start = true;

        furthest = maze[_height - 2, _width - 2];
        furthest.Finish = true;
    }

    private void RemoveWalls(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell current = maze[0, 0];
        current.Visited = true;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();

        do
        {
            List<MazeGeneratorCell> unvisitedCell = new List<MazeGeneratorCell>();
            int x = current.X;
            int y = current.Y;


            if (x > 0 && !maze[x - 1, y].Visited) unvisitedCell.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedCell.Add(maze[x, y - 1]);
            if (x < _width - 2 && !maze[x + 1, y].Visited) unvisitedCell.Add(maze[x + 1, y]);
            if (y < _height - 2 && !maze[x, y + 1].Visited) unvisitedCell.Add(maze[x, y + 1]);

            if (unvisitedCell.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedCell[Random.Range(0, unvisitedCell.Count)];
                RemoveWall(current, chosen);

                chosen.Visited = true;
                stack.Push(chosen);
                current = chosen;
            }
            else
            {
                current = stack.Pop();
            }
        }
        while (stack.Count > 0);

    }

    private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y)
            {
                a.BlockEnabled = false;
            }
            else
            {
                b.BlockEnabled = false;
            }
        }
        else
        {
            if (a.X > b.X)
            {
                a.BlockEnabled = false;
            }
            else
            {
                b.BlockEnabled = false;
            }
        }
    }
}
