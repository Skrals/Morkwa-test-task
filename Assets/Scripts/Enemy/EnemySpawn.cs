using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy _enemyTemplate;
    [SerializeField] private int _enemyCount;

    private MazeSpawner _spawner;
    private MazeGeneratorCell[,] _maze;

    private int _pointX;
    private int _pointY;

    private void Start()
    {
        _spawner = GetComponent<MazeSpawner>();
        _maze = _spawner.GetMaze();

        while (_enemyCount > 0)
        {
            GetSpawnPoint();

            SpawnEnemy(_enemyTemplate, _pointX, _pointY);

            _enemyCount--;
        }
    }

    private void GetSpawnPoint()
    {
        System.Random rand = new();

        while (true)
        {
            _pointX = rand.Next(_maze.GetLength(0) - 2);
            _pointY = rand.Next(_maze.GetLength(1) - 2);

            if (_maze[_pointX, _pointY].BlockEnabled == false)
            {
                break;
            }
        }

    }

    private void SpawnEnemy(Enemy enemy, int x, int y)
    {
        Instantiate(enemy, new Vector2(x, y), Quaternion.identity);
    }
}
