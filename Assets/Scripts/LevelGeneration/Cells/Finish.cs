using UnityEngine;

public class Finish : Cell
{
    [SerializeField] private GameUI _gameUI;

    private void Start()
    {
        _gameUI = FindObjectOfType<GameUI>();
        _gameUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.gameObject.GetComponent<PlayerController>().IsFinished = true;
            _gameUI.gameObject.SetActive(true);
            _gameUI.OverText(true);
        }
    }
}