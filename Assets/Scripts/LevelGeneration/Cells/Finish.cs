using UnityEngine;

public class Finish : Cell
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log($"Finish");
            player.gameObject.GetComponent<PlayerController>().IsFinished = true;
        }
    }
}