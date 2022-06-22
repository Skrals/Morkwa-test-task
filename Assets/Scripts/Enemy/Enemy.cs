using UnityEngine;
using Polarith.AI.Move;

public class Enemy : MonoBehaviour
{
    [Header("")]
    [SerializeField] private GameObject _target;
    [SerializeField] private PatrolPoint _patrolPointTemplate;

    [Header("")]
    [SerializeField] private Block[] _blocks;
    [SerializeField] private Wall[] _walls;

    [Header("Polarith AI")]
    [SerializeField] private AIMSeek _seek;
    [SerializeField] private AIMAvoid _avoid;

    private MazeSpawner _spawner;
    private MazeGeneratorCell[,] _maze;
    private PatrolPoint[] _patrolPoints;

    [Header("Search settings")]
    [SerializeField] private float _searchingDistance;
    [SerializeField] private GameObject _viewZone;
    [SerializeField] private float _viewZoneScaleFactor;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private float _colorCycleTime;

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
        _patrolPoints = new PatrolPoint[2];

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _viewZoneScaleFactor = _searchingDistance * 3f + gameObject.transform.localScale.x;

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
        }

        if (_player.gameObject.GetComponent<PlayerController>().IsFinished)
        {
            OnGameOver(true);
        }

        if(_isFounded)
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