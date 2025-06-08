using UnityEngine;
using DG.Tweening;

public class MovingObjects : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f; // Speed of horizontal movement
    private float _xMax; // Screen boundary
    private Tween _moveTween;

    private void Start()
    {
        Camera mainCamera = Camera.main;
        _xMax = mainCamera.orthographicSize * mainCamera.aspect;

        // Calculate the target X position (opposite screen edge)
        float startX = transform.position.x;
        float targetX = startX < 0 ? _xMax + 0.5f : -_xMax - 0.5f;
        bool movingRight = startX < 0;
        float distance = Mathf.Abs(targetX - startX);
        float duration = distance / _moveSpeed;
        transform.localScale = movingRight ? new Vector3(-1, 1f, 1f) : Vector3.one;
        // Set up DOTween animation to move back and forth
        _moveTween = transform.DOMoveX(targetX, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo)
            .SetAutoKill(false).OnStepComplete(() =>
            {
                movingRight = !movingRight;
                transform.localScale = movingRight ? new Vector3(-1, 1f, 1f) : Vector3.one;
            });
    }



    private void OnDestroy()
    {
        _moveTween?.Kill();
    }
}
