using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private Cell[] _cellTemplates;

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        MazeGeneratorCell[,] maze = generator.GenerateMaze();

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                Instantiate(_cellTemplates[1], new Vector2(x, y),Quaternion.identity);
            }
        }
    }
}
