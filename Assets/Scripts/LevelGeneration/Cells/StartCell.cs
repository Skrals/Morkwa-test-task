using UnityEngine;

public class StartCell : Cell
{
    [SerializeField] private Player _playerTemplate;

    private void Awake()
    {
        Instantiate(_playerTemplate, transform.position,Quaternion.identity);
    }
}