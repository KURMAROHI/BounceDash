using UnityEngine;

public interface IObstacle
{
    void HandlePlayerCollision(Collider2D playerCollider);
}
