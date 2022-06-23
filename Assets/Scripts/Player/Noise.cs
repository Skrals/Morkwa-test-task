using UnityEngine;
using UnityEngine.Events;

public class Noise : MonoBehaviour
{
    [field: SerializeField] public float NoiseMeter { get; private set; }
    [field: SerializeField] public float DetectionLevel { get; private set; }

    [Header("Increase settings")]
    [SerializeField] private float _increaseSpeed;

    [Header("Decrease settings")]
    [SerializeField] private float _decreaseSpeed;

    private bool _detected;
    public event UnityAction Detected;

    private void FixedUpdate()
    {
        if (_detected)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            NoiseMeter += _increaseSpeed * Time.fixedDeltaTime;
        }
        else if (NoiseMeter > 0)
        {
            NoiseMeter -= _decreaseSpeed * Time.fixedDeltaTime;
            if(NoiseMeter < 0)
            {
                NoiseMeter = 0;
            }
        }

        if( NoiseMeter >= DetectionLevel)
        {
            _detected = true;
            Detected?.Invoke();
        }
    }
}
