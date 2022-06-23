using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _isOver;
    [field: SerializeField] public bool IsFinished { get; set; }

    public event UnityAction<bool> GameOver;

    private GameUI _gameUI;

    private void OnEnable() => _gameUI = FindObjectOfType<GameUI>();

    private void Update()
    {
        if (IsFinished || _isOver)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            _isOver = true;
            GameOver?.Invoke(_isOver);

            _gameUI.gameObject.SetActive(true);
            _gameUI.OverText(false);
        }
    }
}