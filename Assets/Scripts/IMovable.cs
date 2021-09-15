using UnityEngine;

public interface IMovable
{
    Vector2Int GridPosition { get; set; }
    void Move(Vector2 movement);
}
