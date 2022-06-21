using UnityEngine;
using Polarith.AI.Move;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private PatrolPoint _patrolPointTemplate;
    [SerializeField] private Block[] _blocks;
    [SerializeField] private Wall[] _walls;

    [SerializeField] private AIMSeek _seek;
    [SerializeField] private AIMAvoid _avoid;

    private MazeSpawner _spawner;
    private MazeGeneratorCell[,] _maze;
    [SerializeField] private PatrolPoint[] _patrolPoints;

    [SerializeField] private Vector2 _patrolPoint1;
    [SerializeField] private Vector2 _patrolPoint2;

    private void Start()
    {
        _patrolPoints = new PatrolPoint[2];
        _spawner = FindObjectOfType<MazeSpawner>();
        _maze = _spawner.GetMaze();

        _blocks = FindObjectsOfType<Block>();
        _walls = FindObjectsOfType<Wall>();

        foreach (var block in _blocks)
        {
            _avoid.GameObjects.Add(block.gameObject);
        }

        foreach (var wall in _walls)
        {
            _avoid.GameObjects.Add(wall.gameObject);
        }

        _patrolPoint1 = GetPatrolPoint();
        _patrolPoint2 = GetPatrolPoint();

        var point1 = Instantiate(_patrolPointTemplate, _patrolPoint1, Quaternion.identity);
        var point2 = Instantiate(_patrolPointTemplate, _patrolPoint2, Quaternion.identity);

        _patrolPoints[0] = point1;
        _patrolPoints[1] = point2;

        GetTarget(_patrolPoints[0].gameObject);
    }

    private Vector2 GetPatrolPoint()
    {
        System.Random rand = new();

        while (true)
        {
            int x = rand.Next(_maze.GetLength(0) - 2);
            int y = rand.Next(_maze.GetLength(1) - 2);

            if (_maze[x, y].BlockEnabled == false)
            {
                return new Vector2(x, y);
            }
        }
    }

    private void GetTarget(GameObject target)
    {
        _target = target;
        _seek.GameObjects.Clear();
        _seek.GameObjects.Add(_target);
    }

    public void PlayerDetectedByNoise()
    {
        GetTarget(FindObjectOfType<Player>().gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PatrolPoint patrolPoint))
        {
            if (patrolPoint == _patrolPoints[0])
            {
                GetTarget(_patrolPoints[1].gameObject);
            }
            else
            {
                GetTarget(_patrolPoints[0].gameObject);
            }
        }
    }
}