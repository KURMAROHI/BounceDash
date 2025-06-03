

using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Camera _mainCamera;
    [SerializeField] private float _bounceForce = 5f; // Controls vertical bounce strength
    [SerializeField] private float _horizontalSpeed = 5f; // Controls horizontal movement speed
    [SerializeField] private float _maxHorizontalSpeed = 5f; // Max horizontal speed to prevent overspeeding
    [SerializeField] private float _touchSensitivity = 0.01f; // Sensitivity for touch swipe
    private float _direction = 0f; // Current horizontal movement direction
    private bool _isGrounded; // Tracks if the ball is touching a platform

    // Screen boundary variables
    private float _minX, _maxX;
    private float _ballHalfWidth;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;

        // Calculate screen boundaries in world coordinates
        float camDistance = _mainCamera.transform.position.z - transform.position.z;
        Debug.Log("camDistance|" + camDistance);
        _minX = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, camDistance)).x;
        _maxX = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, camDistance)).x;

        // Get ball's half-width (assuming a circular collider for simplicity)
        _ballHalfWidth = GetComponent<CircleCollider2D>().bounds.extents.x;
    }

    private void Start()
    {
        // Apply initial upward velocity for bouncing
        _rb.linearVelocity = new Vector2(0, _bounceForce);
    }

    private void Update()
    {

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
        // Apply horizontal movement
        Vector2 velocity = _rb.linearVelocity;
        velocity.x = _direction * _horizontalSpeed;
        velocity.x = Mathf.Clamp(velocity.x, -_maxHorizontalSpeed, _maxHorizontalSpeed);
        _rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlatForm"))
        {
            // Bounce upward with consistent force
            Vector2 velocity = _rb.linearVelocity;
            velocity.y = _bounceForce;
            _rb.linearVelocity = velocity;
            _isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Game Over logic (you can expand this)
            Debug.Log("Game Over!");
            Time.timeScale = 0; // Pause game for now
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlatForm"))
        {
            _isGrounded = false;
        }
    }

    private void ClampToScreen()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, _minX + _ballHalfWidth, _maxX - _ballHalfWidth);
        transform.position = pos;
    }

    // Optional: Handle collectibles
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            // Add score logic here
            Debug.Log("Collected a coin!");
            Destroy(other.gameObject); // Remove collectible
        }
    }
}