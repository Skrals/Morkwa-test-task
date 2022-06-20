using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [field: SerializeField] public bool IsFinished { get; set; }

    private void Update()
    {
        if(IsFinished)
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
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }
        if( Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
    }
}