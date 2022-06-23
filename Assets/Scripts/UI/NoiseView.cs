using UnityEngine;
using UnityEngine.UI;
public class NoiseView : MonoBehaviour
{
    [SerializeField] private Noise _noiseDetector;

    private Slider _noiseSlider;

    private void Start()
    {
        _noiseDetector = FindObjectOfType<Noise>();
        _noiseSlider = GetComponent<Slider>();
        _noiseSlider.maxValue = _noiseDetector.DetectionLevel;
    }

    private void Update()
    {
        _noiseSlider.value = _noiseDetector.NoiseMeter;
    }
}
