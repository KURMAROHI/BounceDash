using UnityEngine;

public class PlayerAudioControl : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _disappearClip;
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private AudioClip _HightjumpClip;

    private void Awake()
    {
        _audioSource = transform.GetComponent<AudioSource>();
    }

    public void PlayJumpAudio()
    {
        _audioSource.PlayOneShot(_jumpClip);
    }
    public void PlayHighJumpAudio()
    {
        _audioSource.PlayOneShot(_HightjumpClip);
    }
    public void PlayDisappearAudio()
    {
        _audioSource.PlayOneShot(_disappearClip);
    }
    public void PlayHurtAudio()
    {
        _audioSource.PlayOneShot(_hurtClip);
    }


}
