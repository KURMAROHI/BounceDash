using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player; // Reference to the player's transform
    [SerializeField] private float _smoothSpeed = 0.125f; // Smoothing factor for camera movement
    [SerializeField] private float _verticalThreshold = 2f; // Threshold for detecting vertical progress
    private Vector3 _offset; // Initial offset between camera and player
    private float _currentCameraY; // Tracks the camera's current Y position
    private Vector3 _targetPosition; // Target position for the camera
    private float _cameraYSize;

    public static event EventHandler OnGameOverUI;


    private void Start()
    {
        if (_player == null)
        {
            Debug.LogError("Player Transform not assigned in CameraFollow script!");
            return;
        }
        // Initialize offset and camera's Y position
        _offset = transform.position - _player.position;
        _currentCameraY = _player.position.y;
        _targetPosition = transform.position;

        _cameraYSize = Camera.main.orthographicSize;
    }

    private void LateUpdate()
    {
        if (_player == null) return;

        // Get the player's current Y position
        float playerY = _player.position.y;

        // Update camera only if player moves higher than the current camera Y position
        // or falls significantly below, considering the offset
        if (playerY > _currentCameraY + _verticalThreshold)
        {
            _currentCameraY = playerY;
            _targetPosition = new Vector3(_offset.x, playerY + _offset.y, transform.position.z);
        }
        else if (playerY < transform.position.y - _cameraYSize)
        {
            Debug.LogError("you lose Buddy!!!");
            _player.GetComponent<PlayerAudioControl>().PlayDisappearAudio();
            ResetPlayerproperties();
            ResetCameraproperties();
            OnGameOverUI?.Invoke(this, EventArgs.Empty);
        }

        // Smoothly move the camera to the target position, keeping X and Z from the offset
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed);
        transform.position = new Vector3(_offset.x, smoothedPosition.y, transform.position.z);
    }


    private void ResetPlayerproperties()
    {
        Rigidbody2D rb = _player.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        _player.position = Vector2.zero;
    }
    private void ResetCameraproperties()
    {
        transform.position = new Vector3(0, 0, -10);
        _currentCameraY = _player.position.y;
        _targetPosition = transform.position;
    }


}
