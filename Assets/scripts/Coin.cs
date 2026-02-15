using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PointManager.Point += 10;
            Debug.Log($"I Gay {PointManager.Point}");
            Destroy(this.gameObject); 
        }
       
    }
}
