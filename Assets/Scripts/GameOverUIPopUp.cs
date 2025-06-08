using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIPopUp : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _coinsText;
    [SerializeField] private Text _gemsText;

    public static event EventHandler OnRestartButtonClickEvent;
    public static event EventHandler OnGenarateLevel;
    public static event EventHandler OnHomeButtonClickEvent;

    private void Start()
    {
        _scoreText.text = "SCORE:" + ScoreManager.Instance.GetScore();
        _coinsText.text = ScoreManager.Instance.GetTotalNoofCoins();
        _gemsText.text = ScoreManager.Instance.GetTotalNoofGemS();
        ScoreManager.Instance.ResetData();

    }



    public void OnCloseButtonClick(Transform button)
    {
        Debug.Log("OnCloseButtonClick");
        AnimateButton(button, () =>
        {
            OnGenarateLevel?.Invoke(this, EventArgs.Empty);
            OnRestartButtonClickEvent?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject, 0.3f);
        });


    }

    public void OnHomeButtonClick(Transform button)
    {
        Debug.Log("OnHomeButtonClick");
        AnimateButton(button, () =>
        {
            OnHomeButtonClickEvent?.Invoke(this, EventArgs.Empty);
        });
    }

    public void OnRestartButtonClick(Transform button)
    {
        Debug.Log("OnRestartButtonClick");
        AnimateButton(button, () =>
        {
            OnGenarateLevel?.Invoke(this, EventArgs.Empty);
            OnRestartButtonClickEvent?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject, 0.3f);
        });
    }

    private void AnimateButton(Transform button, Action onCompleteAction)
    {
        float currentScale = button.localScale.x;
        Vector2 upScale = new Vector2(currentScale + 0.1f, currentScale + 0.1f);
        Vector2 originalScale = Vector2.one * currentScale;

        button.DOScale(upScale, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
        {
            button.DOScale(originalScale, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                onCompleteAction?.Invoke();
            });
        });
    }
}
