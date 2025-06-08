using UnityEngine;


[RequireComponent(typeof(AudioClip), typeof(Rigidbody2D))]
public class PlayerMainMenuControl : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private Transform[] _fly;
    private Rigidbody2D _rb;
    private AudioSource _audioSorce;
    private float _bounceForce = 7f;

    private float xOffset = 1f;
    private float yOffset = 1f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSorce = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _rb.linearVelocityY = _bounceForce;

        Camera mainCamera = Camera.main;
        float xMax = mainCamera.orthographicSize * mainCamera.aspect;
        float yMax = mainCamera.orthographicSize;
        _ground.position = new Vector3(-xMax + xOffset, -yMax + yOffset, 10);
        transform.position = new Vector3(-xMax + xOffset, -yMax + yOffset * 2, 10);

        if (_fly.Length > 0)
        {
            Transform flyBird = Instantiate(_fly[Random.Range(0, _fly.Length)]);
            int direction = Random.Range(0, 2) == 1 ? 1 : -1;
            flyBird.position = new Vector3(direction * xMax, yMax - yOffset, 10);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // _audioSorce.PlayOneShot(_jumpClip);
        _audioSorce.Play();
        Vector2 velocity = _rb.linearVelocity;
        velocity.y = _bounceForce;
        _rb.linearVelocity = velocity;
    }

}
