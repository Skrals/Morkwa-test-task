using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _speed;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, GetTargetPosition(), _speed * Time.fixedDeltaTime);
    }

    private Vector3 GetTargetPosition()
    {
        return new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
    }
}