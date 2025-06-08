using UnityEngine;

public class GemsCollect : MonoBehaviour, ICollect
{

    [SerializeField] private int _rewardPoints = 50; // Points for collecting this item
    [SerializeField] private AudioClip _gemCollectClip;

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
        AudioManager.Instance.PlayClip(_gemCollectClip);
    }

    public void OnUpdateScore()
    {
        ScoreManager.Instance?.AddScore(_rewardPoints, false);
    }
}
