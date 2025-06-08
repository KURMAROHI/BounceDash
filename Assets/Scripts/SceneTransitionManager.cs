using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [SerializeField] private Button _playButton; // Play button in HomeScreen
    [SerializeField] private string _gameplaySceneName = "GamePlayScreen"; // Gameplay scene name
    [SerializeField] private Image _progressBar; // Progress bar in Loading scene
    [SerializeField] private Text _progressText; // Percentage text in Loading scene

    private bool _isLoading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (_playButton != null)
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("Play button not assigned in SceneTransitionManager!");
        }

        // Find and initialize UI elements in the Loading scene
        InitializeLoadingUI();
    }

    private void OnPlayButtonClicked()
    {
        if (!_isLoading)
        {
            StartCoroutine(LoadGameplaySceneAsync());
        }
    }

    private IEnumerator LoadGameplaySceneAsync()
    {
        _isLoading = true;

        // Start loading the GameplayScreen scene
        AsyncOperation gameplaySceneOp = SceneManager.LoadSceneAsync(_gameplaySceneName);
        gameplaySceneOp.allowSceneActivation = false;

        // Update progress until the GameplayScreen is loaded
        while (!gameplaySceneOp.isDone)
        {
            float progress = Mathf.Clamp01(gameplaySceneOp.progress / 0.9f); // Normalize 0-0.9 to 0-1
            UpdateLoadingUI(progress);
            if (gameplaySceneOp.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f); // Minimum 1-second display
                gameplaySceneOp.allowSceneActivation = true;
            }
            yield return null;
        }

        _isLoading = false;
    }

    private void InitializeLoadingUI()
    {
    
        // Initialize UI elements
        if (_progressBar != null)
        {
            _progressBar.fillAmount = 0f;
        }
        else
        {
            Debug.LogError("Progress bar not found in Loading scene!");
        }

        if (_progressText != null)
        {
            _progressText.text = "Loading.....0%";
        }
        else
        {
            Debug.LogWarning("Progress text not found in Loading scene!");
        }
    }

    private void UpdateLoadingUI(float progress)
    {
        if (_progressBar != null)
        {
            _progressBar.fillAmount = progress;
        }

        if (_progressText != null)
        {
            _progressText.text = $"Loading.....{Mathf.FloorToInt(progress * 100)}%";
        }
    }

    private void OnDestroy()
    {
        if (_playButton != null)
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
    }
}