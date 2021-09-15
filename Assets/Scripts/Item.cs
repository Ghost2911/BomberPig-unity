using UnityEngine;

public class Item : MonoBehaviour
{
    public int points = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Pig>().AddPoints(points);
            Destroy(gameObject);
        }
    }
}
