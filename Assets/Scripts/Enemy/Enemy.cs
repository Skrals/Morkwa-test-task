using UnityEngine;
using Polarith.AI.Move;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Block[] _blocks;

    [SerializeField] private AIMSeek _seek;
    [SerializeField] private AIMAvoid _avoid;

    private void Start()
    {
        _target = FindObjectOfType<Player>().gameObject;

        _blocks = FindObjectsOfType<Block>();

        _seek.GameObjects.Add(_target);

        foreach (var block in _blocks)
        {
            _avoid.GameObjects.Add(block.gameObject);
        }
    }

}