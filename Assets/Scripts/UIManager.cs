using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Text _scoreText;
    [SerializeField] private Transform _canvas;
    public event EventHandler ResetUIBackGround;

    private void Awake()
    {
        _scoreText.text = "0";
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        CameraFollow.OnGameOverUI += OnGameOverUI;
    }


    private void Start()
    {
       // Debug.LogError("OnEnable UI manage");
        ScoreManager.Instance.OnScoreUpdate += OnScoreUpdate;
    }


    private void OnDisable()
    {
        ScoreManager.Instance.OnScoreUpdate -= OnScoreUpdate;
        CameraFollow.OnGameOverUI -= OnGameOverUI;
    }

    private void OnScoreUpdate(object sender, string score)
    {
     //   Debug.LogError("OnEnable UI manage invoke");
        _scoreText.text = score;
    }


    private void OnGameOverUI(object sender, EventArgs e)
    {
        Debug.Log("OnGameOverUI is Calling");
        GameObject popup = Instantiate(Resources.Load("Popups/GameOverUI"), _canvas) as GameObject;
        popup.name = "GameOverUI";
        popup.SetActive(true);
        _scoreText.text = "0";
        ResetUIBackGround?.Invoke(this, EventArgs.Empty);
    }


}
