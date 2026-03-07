using UnityEngine;

public class Coin : MonoBehaviour
{
    public Animator coinAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PointManager.Point += 10;
            Debug.Log($"I Gay {PointManager.Point}");
            coinAnimator.SetTrigger("TokenCollected");
            GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject, 0.5f);
        }
    }
}