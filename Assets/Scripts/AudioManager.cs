using System.Collections;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource _audioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _audioSource = transform.GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip, float delay = 0)
    {
        if (clip == null)
            return;
        StartCoroutine(PlayAudioClip(clip, delay));
    }

    private IEnumerator PlayAudioClip(AudioClip clip, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}
