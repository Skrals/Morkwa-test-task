using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private Cell _blockTemplate;
    [SerializeField] private Cell _finishTemplate;
    [SerializeField] private Cell _startTemplate;
    private MazeGeneratorCell[,] _maze;

    private void Awake()
    {
        MazeGenerator generator = new();
        _maze = generator.GenerateMaze();

        for (int x = 0; x < _maze.GetLength(0); x++)
        {
            for (int y = 0; y < _maze.GetLength(1); y++)
            {
                if (_maze[x, y].Finish)
                {
                    Instantiate(_finishTemplate, new Vector2(x, y), Quaternion.identity);
                }
                else if (_maze[x, y].Start)
                {
                    Instantiate(_startTemplate, new Vector2(x, y), Quaternion.identity);
                }
                else
                {
                    Cell cell = Instantiate(_blockTemplate, new Vector2(x, y), Quaternion.identity);

                    if (!_maze[x, y].BlockEnabled)
                    {
                        Destroy(cell.gameObject);
                    }
                }
            }
        }
    }

    public MazeGeneratorCell[,] GetMaze ()
    {
        return _maze;
    }
}
