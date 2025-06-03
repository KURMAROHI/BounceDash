using UnityEngine;

public class CameraFollow : MonoBehaviour
{
[SerializeField] private Transform _player; // Reference to the player's transform
    [SerializeField] private float _smoothSpeed = 0.125f; // Smoothing factor for camera movement
    [SerializeField] private float _verticalThreshold = 2f; // Threshold for detecting vertical progress
    private Vector3 _offset; // Initial offset between camera and player
    private float _currentCameraY; // Tracks the camera's current Y position
    private Vector3 _targetPosition; // Target position for the camera

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
    }

    private void LateUpdate()
    {
        if (_player == null) return;

        // Get the player's current Y position
        float playerY = _player.position.y;

        // Update camera only if player moves higher than the current camera Y position
        // or falls significantly below, considering the offset
        if (playerY > _currentCameraY + _verticalThreshold || playerY < _currentCameraY - _verticalThreshold)
        {
            _currentCameraY = playerY;
            _targetPosition = new Vector3(_offset.x, playerY + _offset.y, transform.position.z);
        }

        // Smoothly move the camera to the target position, keeping X and Z from the offset
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed);
        transform.position = new Vector3(_offset.x, smoothedPosition.y, transform.position.z);
    }
}
