using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private Transform _playerTransform;
    private float _highestYPosition = 0f;
    private int _score = 0;
    private int _coinsCollected = 0;
    private int _gemeCollected = 0;
    private int _scoreMultiplier = 10;
    public event EventHandler<string> OnScoreUpdate;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found! Assign player transform in ScoreManager.");
            }
        }
    }

    private void Update()
    {
        if (_playerTransform != null)
        {
            float currentY = _playerTransform.position.y;
            if (currentY > _highestYPosition)
            {
                //   Debug.Log("Diff|" + (currentY - _highestYPosition));
                float Diff = Mathf.Abs(currentY - _highestYPosition);
                _highestYPosition = currentY;
                _score += Mathf.FloorToInt(Diff * _scoreMultiplier * 2);
                OnScoreUpdate?.Invoke(this, _score.ToString());
            }
        }
    }

    public void AddScore(int points, bool isCoin)
    {
        _score += points;
        OnScoreUpdate?.Invoke(this, _score.ToString());
        Debug.Log($"Score Added: {points}, Total Score: {_score}");

        if (isCoin)
            _coinsCollected += 1;
        else
            _gemeCollected += 1;
    }


    public string GetTotalNoofCoins() => _coinsCollected.ToString();
    public string GetTotalNoofGemS() => _gemeCollected.ToString();
    public string GetScore() => _score.ToString();

    public void ResetData()
    {
        _highestYPosition = 0f;
        _coinsCollected = 0;
        _gemeCollected = 0;
        _score = 0;
    }


}
