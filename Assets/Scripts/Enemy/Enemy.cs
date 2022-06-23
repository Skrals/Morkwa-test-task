using UnityEngine;
using Polarith.AI.Move;

public class Enemy : MonoBehaviour
{
    [Header("Templates")]
    [SerializeField] private GameObject _target;
    [SerializeField] private PatrolPoint _patrolPointTemplate;

    [Header("Patrol points count")]
    [SerializeField] private int _patrolPointsCount;

    [Header("Avoid objects")]
    [SerializeField] private Block[] _blocks;
    [SerializeField] private Wall[] _walls;

    [Header("Polarith AI")]
    [SerializeField] private AIMSeek _seek;
    [SerializeField] private AIMAvoid _avoid;

    [Header("Search settings")]
    [SerializeField] private float _searchingDistance;

    [Header("Search view settings")]
    [SerializeField] private GameObject _viewZone;
    [SerializeField] private float _viewZoneScaleFactor;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private float _colorCycleTime;

    private MazeSpawner _spawner;
    private MazeGeneratorCell[,] _maze;

    private PatrolPoint[] _patrolPoints;
    private Vector2 _startPoint;
    private int _pointNumber;
    private bool _switchPatrolDirection;

    private SpriteRenderer _spriteRenderer;

    private bool _isFounded;
    private Player _player;

    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _player.GetComponent<PlayerController>().GameOver += OnGameOver;
        _player.GetComponent<Noise>().Detected += PlayerDetected;
    }

    private void OnDisable()
    {
        _player.GetComponent<PlayerController>().GameOver -= OnGameOver;
        _player.GetComponent<Noise>().Detected -= PlayerDetected;
    }

    private void Start()
    {
        _patrolPoints = new PatrolPoint[_patrolPointsCount];
        _pointNumber = 0;

        _spriteRenderer = GetComponent<SpriteRenderer>();

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

        _startPoint = GetStartPoint();
        _patrolPoints[0] = Instantiate(_patrolPointTemplate, _startPoint, Quaternion.identity);

        for (int i = 1; i < _patrolPointsCount; i++)
        {
            _patrolPoints[i] = Instantiate(_patrolPointTemplate, GetPatrolPoint(), Quaternion.identity);
        }

        GetTarget(_patrolPoints[_pointNumber + 1].gameObject);

        transform.position = _patrolPoints[_pointNumber].transform.position;
        DrawViewCircle();
    }

    private void Update()
    {
        if (!_isFounded && DistanceToPlayer() <= _searchingDistance)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.transform.position);
            Debug.DrawLine(transform.position, _player.transform.position, Color.white);

            if (hit.collider.gameObject == _player.gameObject)
            {
                PlayerDetected();
            }
        }

        if (_player.gameObject.GetComponent<PlayerController>().IsFinished)
        {
            OnGameOver(true);
        }

        if (_isFounded)
        {
            _spriteRenderer.color = _gradient.Evaluate(Mathf.PingPong(Time.time, _colorCycleTime) / _colorCycleTime);
        }
    }

    private float DistanceToPlayer()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        return distance;
    }

    private void DrawViewCircle()
    {
        _viewZone.transform.localScale = new Vector3(_searchingDistance * _viewZoneScaleFactor, _searchingDistance * _viewZoneScaleFactor);
    }

    private void OnGameOver(bool over)
    {
        gameObject.GetComponent<AIMSimpleController2D>().enabled = false;
    }

    private Vector2 GetStartPoint()
    {
        System.Random rand = new();

        while (true)
        {
            int x = rand.Next(_maze.GetLength(0) - 2);
            int y = rand.Next(_maze.GetLength(1) - 2);

            if (_maze[x, y].BlockEnabled == false && _maze[x, y].PatrolPoint == false)
            {
                _maze[x, y].PatrolPoint = true;
                return new Vector2(x, y);
            }
        }
    }

    private Vector2 GetPatrolPoint()
    {
        var current = _startPoint;
        int x = (int)current.x;
        int y = (int)current.y;

        if (x > 0 && !_maze[x - 1, y].BlockEnabled && !_maze[x - 1, y].PatrolPoint)
        {
            _maze[x - 1, y].PatrolPoint = true;
            _startPoint = new Vector2(x - 1, y);
        }
        else if (y > 0 && !_maze[x, y - 1].BlockEnabled && !_maze[x, y - 1].PatrolPoint)
        {
            _maze[x, y - 1].PatrolPoint = true;
            _startPoint = new Vector2(x, y - 1);
        }
        else if (x < _maze.GetLength(0) - 2 && !_maze[x + 1, y].BlockEnabled && !_maze[x + 1, y].PatrolPoint)
        {
            _maze[x + 1, y].PatrolPoint = true;
            _startPoint = new Vector2(x + 1, y);
        }
        else if (y < _maze.GetLength(1) - 2 && !_maze[x, y + 1].BlockEnabled && !_maze[x, y + 1].PatrolPoint)
        {
            _maze[x, y + 1].PatrolPoint = true;
            _startPoint = new Vector2(x, y + 1);
        }

        return _startPoint;
    }

    private void MoveNextPoint()
    {
        PatrolPoint next = null;

        if (_switchPatrolDirection == false && _pointNumber < _patrolPoints.Length - 1)
        {
            next = _patrolPoints[_pointNumber + 1];
            _pointNumber++;
        }
        else
        {
            _switchPatrolDirection = true;
        }

        if (_switchPatrolDirection == true && _pointNumber > 0)
        {
            next = _patrolPoints[_pointNumber - 1];
            _pointNumber--;
        }
        else
        {
            if (_pointNumber <= 0)
            {
                next = _patrolPoints[_pointNumber + 1];
                _pointNumber++;
            }
            _switchPatrolDirection = false;
        }

        if (next != null)
        {
            GetTarget(next.gameObject);
        }
    }

    private void GetTarget(GameObject target)
    {
        _target = target;
        _seek.GameObjects.Clear();
        _seek.GameObjects.Add(_target);
    }

    private void PlayerDetected()
    {
        _isFounded = true;
        GetTarget(FindObjectOfType<Player>().gameObject);
        _viewZone.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isFounded && collision.gameObject.TryGetComponent(out PatrolPoint patrolPoint))
        {
            MoveNextPoint();
        }
    }
}