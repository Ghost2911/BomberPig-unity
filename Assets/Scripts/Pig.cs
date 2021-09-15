using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pig : MonoBehaviour, IDamagable, IMovable
{
    [Header("Navigation grid")]
    public CustomGrid grid;

    [Header("Interface elements")]
    public FloatingJoystick joystick;
    public Text textPoints;

    [Header("Pig Settings")]
    public float speed = 2f;
    public int health = 3;
    public GameObject bomb;
    public int points = 0;

    private SpriteRenderer _renderer;
    private Animator _animator;
    private Vector2Int _gridCoordinate;

    public int Health { get { return health; } set { health = value; if (health <= 0) SceneManager.LoadScene(0); } }

    public Vector2Int GridPosition { get { return _gridCoordinate; } set { _gridCoordinate = value; } }


    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 direction = joystick.Direction;

        if (direction != Vector2.zero)
        {
            Move(direction);
        }
    }

    public void Move(Vector2 movement)
    {
        movement = (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) ? new Vector2(movement.x, 0f) : new Vector2(0f, movement.y);

        Vector2 newPosition = (grid.GetDirection(movement, _gridCoordinate) - transform.position).normalized;

        _animator.SetFloat("horisontalDirection", newPosition.x);
        _animator.SetFloat("verticalDirection", newPosition.y);

        transform.Translate(newPosition * speed * Time.deltaTime);
    }

    public void PlaceBomb()
    {
        List<Cell> explosiveCells = grid.GetNeighbours(grid.cells[GridPosition.x, GridPosition.y]);
        explosiveCells.Add(grid.cells[GridPosition.x, GridPosition.y]);
        bomb.GetComponent<Bomb>().explosiveCells = explosiveCells;
        Instantiate(bomb, grid.GetCellCoordinate(GridPosition), new Quaternion(0, 0, 0, 0));
    }

    public void Damage(int value)
    {
        Health -= value;
        StartCoroutine(DamageColor());
    }

    IEnumerator DamageColor()
    {
        _renderer.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        _renderer.color = Color.white;
    }

    public void AddPoints(int point)
    {
        points += point;
        textPoints.text = points.ToString();
    }

}
