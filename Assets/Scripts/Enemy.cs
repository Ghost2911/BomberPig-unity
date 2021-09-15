using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour, IDamagable, IMovable
{
    public CustomGrid grid;
    public float speed = 1.5f;
    public int health = 3;

    private Vector2Int _gridCoordinate;
    private Transform _waypoint;
    private Animator _animator;
    private SpriteRenderer _renderer;

    public Vector2Int GridPosition { get { return _gridCoordinate; }  set { _gridCoordinate = value; } }
    public int Health { get { return health; } set { health = value; if (health <= 0) Destroy(gameObject); } }


    private void Start()
    {
        GetStartPosition();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        FindNewTarget();
    }

    void GetStartPosition()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.2f);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<Cell>() != null)
                GridPosition = hit.GetComponent<Cell>().index;
        }
    }

    private void Update()
    {
        Move(Vector2.MoveTowards(transform.position, _waypoint.position, Time.deltaTime * speed));
    }

    public void Move(Vector2 movement)
    {
        if (Vector2.Distance(transform.position, _waypoint.position) < 0.01f)
            FindNewTarget();
        AnimationDirectionChange();
        transform.position = movement;
    }

    private void FindNewTarget()
    {
        List<Cell> neighbours = grid.GetNeighbours(grid.cells[GridPosition.x, GridPosition.y]);
        _waypoint = neighbours[Random.Range(0, neighbours.Count)].transform;
    }

    private void AnimationDirectionChange()
    {
        Vector3 direction = _waypoint.position - transform.position;
        _animator.SetFloat("horisontalDirection", direction.x);
        _animator.SetFloat("verticalDirection", direction.y);
    }

    public void Damage(int value)
    {
        DamageColor();
        _animator.SetBool("isDirty", true);
        Health -= value;
    }

    IEnumerator DamageColor()
    {
        _renderer.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        _renderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            SceneManager.LoadScene(0);
    }
}
