using UnityEngine;

public class CoinCollect : MonoBehaviour, ICollect
{
    [SerializeField] private int _rewardPoints = 50; // Points for collecting this item
    [SerializeField] private AudioClip _coinCollectClip;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlaySound();
            OnUpdateScore();
            Destroy(gameObject);
        }
    }

    public void OnPlaySound()
    {
        AudioManager.Instance.PlayClip(_coinCollectClip);
    }

    public void OnUpdateScore()
    {
        ScoreManager.Instance?.AddScore(_rewardPoints, true);
    }
}
