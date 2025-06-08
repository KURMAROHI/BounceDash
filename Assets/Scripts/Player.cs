

using System;
using UnityEngine;


[RequireComponent(typeof(PlayerAudioControl), typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Camera _mainCamera;
    [SerializeField] private float _bounceForce = 5f; // Controls vertical bounce strength
    [SerializeField] private float _springForce = 8f; // Controls vertical bounce while jump on Spring
    [SerializeField] private float _gravityForce = -9.8f;
    [SerializeField] private float _horizontalSpeed = 5f; // Controls horizontal movement speed
    [SerializeField] private float _maxHorizontalSpeed = 5f; // Max horizontal speed to prevent overspeeding
    [SerializeField] private float _touchSensitivity = 0.01f; // Sensitivity for touch swipe
    private float _direction = 0f; // Current horizontal movement direction
    private bool _isGrounded; // Tracks if the ball is touching a platform

    // Screen boundary variables
    private float _minX, _maxX;
    private float _ballHalfWidth;
    private CircleCollider2D _playeCollider2D;
    private bool _isplayerAlive = false;
    private PlayerAudioControl _playerAudioControl;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;

        // Calculate screen boundaries in world coordinates
        float camDistance = _mainCamera.transform.position.z - transform.position.z;
        Debug.Log("camDistance|" + camDistance);
        _minX = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, camDistance)).x;
        _maxX = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, camDistance)).x;

        // Get ball's half-width 
        _playeCollider2D = transform.GetComponent<CircleCollider2D>();
        _ballHalfWidth = _playeCollider2D.bounds.extents.x;
        _isplayerAlive = true;

        _playerAudioControl = transform.GetComponent<PlayerAudioControl>();
    }

    private void Start()
    {
        // Apply initial upward velocity for bouncing
        _rb.linearVelocity = new Vector2(0, _bounceForce);
        GameOverUIPopUp.OnRestartButtonClickEvent += OnRestartButtonClickEvent;
    }


    private void OnDisable()
    {

        GameOverUIPopUp.OnRestartButtonClickEvent -= OnRestartButtonClickEvent;
    }

    private void Update()
    {
        if (!_isplayerAlive)
            return;

        float inputDirection = 0f;
#if UNITY_EDITOR
        // Handle keyboard input
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            inputDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            inputDirection = -1f;
        }

#else
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                inputDirection = touch.deltaPosition.x * _touchSensitivity;
                inputDirection = Mathf.Clamp(inputDirection, -1f, 1f); // Normalize swipe input
                Debug.LogError("inputDirection|" + inputDirection +"|"+touch.deltaPosition.x );
            }
        }
#endif

        // Update direction for physics application
        _direction = inputDirection;

        // Clamp position to screen boundaries
        ClampToScreen();
    }

    private void FixedUpdate()
    {
        if (!_isplayerAlive)
            return;
        // Apply horizontal movement
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = _direction * _horizontalSpeed;
        velocity.x = Mathf.Clamp(velocity.x, -_maxHorizontalSpeed, _maxHorizontalSpeed);
        _rb.linearVelocity = velocity;
        // Debug.Log("FixedUpdate|" + _rb.linearVelocity);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isplayerAlive)
            return;

        float playerBottom = transform.position.y - _playeCollider2D.bounds.extents.y;
        float platformTop = collision.bounds.center.y + collision.bounds.extents.y;

        if (collision.gameObject.CompareTag("Spring"))
        {
            Debug.LogError("Spring|" + collision.transform.name + "|" + playerBottom + "|" + platformTop);
        }

        if (playerBottom < platformTop - 0.2f)
            return;

        if (collision.gameObject.CompareTag("PlatForm"))
        {
            //Debug.Log("playerBottom|" + playerBottom + "|" + platformTop);
            if (!IsPlayerOnEdge(collision)) // allow a small margin
            {
                SetVelocity(_bounceForce);
                _playerAudioControl.PlayJumpAudio();
            }
        }
        else if (collision.gameObject.CompareTag("BreakPlatForm"))
        {
            Debug.LogError("BreakPlatForm|");
            collision.transform.GetComponent<PolygonCollider2D>().enabled = false;
            Animator animtor = collision.transform.GetComponent<Animator>();
            animtor.SetTrigger("Break");
            _rb.linearVelocity = Vector2.zero;
        }
        else if (collision.gameObject.CompareTag("Spring"))
        {
            Debug.LogError("Spring|" + collision.transform.name);
            Animator animtor = collision.transform.GetComponent<Animator>();
            animtor.SetTrigger("HighJump");
            SetVelocity(_springForce);
            _playerAudioControl.PlayHighJumpAudio();

        }
    }


    private void OnRestartButtonClickEvent(object sender, EventArgs e)
    {
        Debug.Log("OnRestartButtonClickEvent player|");
        SetPlayerStatus = true;
        _playeCollider2D.enabled = true;
        _rb.gravityScale = 1;
        _rb.linearVelocity = new Vector2(0, _bounceForce);
    }


    private void ClampToScreen()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, _minX + _ballHalfWidth, _maxX - _ballHalfWidth);
        transform.position = pos;
    }



    private void SetVelocity(float yveloCity)
    {
        Vector2 velocity = _rb.linearVelocity;
        velocity.y = yveloCity;
        _rb.linearVelocity = velocity;
        _isGrounded = true;
    }

    private bool IsPlayerOnEdge(Collider2D collision)
    {
        // Debug.Log("Landed on top — bounce triggered.");
        float platformLeft = collision.bounds.min.x;
        float platformRight = collision.bounds.max.x;
        float platformWidth = collision.bounds.size.x;

        float playerCenterX = transform.position.x;
        float distanceFromLeft = Mathf.Abs(playerCenterX - platformLeft);
        float distanceFromRight = Mathf.Abs(playerCenterX - platformRight);
        // Threshold: how close to the edge is considered an "edge"
        float edgeThreshold = platformWidth * 0.05f;
        if (distanceFromLeft < edgeThreshold || distanceFromRight < edgeThreshold)
        {
            Debug.Log("Player landed on the EDGE of the platform!");
        }

        return distanceFromLeft < edgeThreshold || distanceFromRight < edgeThreshold;
    }

    public bool GetPlayerStatus() => _isplayerAlive;
    public bool SetPlayerStatus
    {
        set => _isplayerAlive = value;
    }

}