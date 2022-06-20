using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private Cell _blockTemplate;
    [SerializeField] private Cell _finishTemplate;
    [SerializeField] private Cell _startTemplate;

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        MazeGeneratorCell[,] maze = generator.GenerateMaze();
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                if (maze[x, y].Finish)
                {
                    Instantiate(_finishTemplate, new Vector2(x, y), Quaternion.identity);
                }
                else if (maze[x, y].Start)
                {
                    Instantiate(_startTemplate, new Vector2(x, y), Quaternion.identity);
                }
                else
                {
                    Cell cell = Instantiate(_blockTemplate, new Vector2(x, y), Quaternion.identity);

                    if(!maze[x, y].BlockEnabled)
                    {
                        Destroy(cell.gameObject);
                    }
                }
            }
        }
    }
}
