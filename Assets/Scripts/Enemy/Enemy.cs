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

    [SerializeField] private float _searchingDistance;
    [SerializeField] private GameObject _viewZone;
    private bool _isFounded;
    private Player _player;

    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _player.GetComponent<PlayerController>().GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _player.GetComponent<PlayerController>().GameOver -= OnGameOver;
    }

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

        _patrolPoints[0] = Instantiate(_patrolPointTemplate, GetPatrolPoint(), Quaternion.identity);
        _patrolPoints[1] = Instantiate(_patrolPointTemplate, GetPatrolPoint(), Quaternion.identity);

        GetTarget(_patrolPoints[1].gameObject);

        transform.position = _patrolPoints[0].transform.position;
        DrawViewCircle();
    }

    private void Update()
    {
        if (!_isFounded && DistanceToPlayer() <= _searchingDistance)
        {
            PlayerDetected();
            _isFounded = true;
        }
    }

    private float DistanceToPlayer()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        return distance;
    }

    private void DrawViewCircle()
    {
        _viewZone.transform.localScale = new Vector3(_searchingDistance * 6.33f, _searchingDistance * 6.33f);
    }

    private void OnGameOver(bool over)
    {
        gameObject.GetComponent<AIMSimpleController2D>().enabled = false;
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

    public void PlayerDetected()
    {
        GetTarget(FindObjectOfType<Player>().gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isFounded && collision.gameObject.TryGetComponent(out PatrolPoint patrolPoint))
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