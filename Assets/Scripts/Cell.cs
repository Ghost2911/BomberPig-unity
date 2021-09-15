using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int index;
    public GameObject go;
    public bool isWalkable = true;

    void Start()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Obstacle"))
                isWalkable = false;
            if (hit.tag != null)
                go = hit.gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<IMovable>().GridPosition = index;
        go = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        go = null;
        isWalkable = true;
    }
}
