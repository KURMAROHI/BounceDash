using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour, IObstacle
{
    private Rigidbody2D playerRb = null;
    private Collider2D _obstacleCollider; // Cached obstacle collider

    private void Awake()
    {
        _obstacleCollider = transform.GetComponent<Collider2D>();
    }
    public void HandlePlayerCollision(Collider2D playerCollider)
    {
        // Get player's Rigidbody2D to check velocity
        if (playerRb == null)
        {
            playerRb = playerCollider.GetComponent<Rigidbody2D>();
        }

        if (playerRb == null)
        {
            Debug.LogError("Player Rigidbody2D not found!");
            return;
        }

        // Ignore collision if player is moving upward (jumping from below)
        if (playerRb.linearVelocity.y > 0)
        {
            Debug.Log("Player hit obstacle from below, ignoring.");
            return;
        }

        // Game over for side or top collision
        Debug.Log("Game Over!");
        StartCoroutine(nameof(GameOverSequence));

    }

    private IEnumerator GameOverSequence()
    {
        if (playerRb.TryGetComponent<Player>(out Player player))
        {
            player.SetPlayerStatus = false;
        }


        _obstacleCollider.isTrigger = false;
        playerRb.GetComponent<PlayerAudioControl>().PlayHurtAudio();
        playerRb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.25f);
        playerRb.GetComponent<CircleCollider2D>().enabled = false;
        playerRb.GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HandlePlayerCollision(collision);
        }
    }
}
