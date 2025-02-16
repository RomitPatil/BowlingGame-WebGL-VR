using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ScoringUI : MonoBehaviour
{
    public static ScoringUI Instance;

    [SerializeField] private TextMeshProUGUI playerNameText, scoreText, totalScoreText, BallTypeText, pinCollapsedText, messageText;
    [SerializeField] private GameObject finalScorePanel, scoreRecordPrefab, messageObject;    // scoreRecordPrefab: Prefab for each score record row

    private Coroutine messageCoroutine;


    [Header("Message Animation")]
    [SerializeField] private float messageDisplayTime = 3f, fadeInDuration = 0.5f, fadeOutDuration = 0.5f, panelFadeInDuration = 0.5f, scaleUpDuration = 0.5f, panelScaleUpDuration = 0.5f;
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 endScale = Vector3.one;

    [Header("Final Score Panel Animation")]
    [SerializeField] private Vector3 panelStartScale = Vector3.zero;
    [SerializeField] private Transform scoreRecordContainer;  // Parent object for score records
    [SerializeField] private Vector3 panelEndScale = Vector3.one;
    private CanvasGroup finalScorePanelGroup;
    private int pinsCollapsedCount = 0;

    public bool IsShowingMessage { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            finalScorePanelGroup = finalScorePanel.GetComponent<CanvasGroup>();
            if (finalScorePanelGroup == null)
                finalScorePanelGroup = finalScorePanel.AddComponent<CanvasGroup>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerNameText.text = $"{GameManager.Instance.userName}";
        scoreText.text = "0";
        totalScoreText.text = "0";
        pinCollapsedText.text = "0";
        pinsCollapsedCount = 0;
        BallTypeText.text = "";
        messageObject.transform.localScale = startScale;
        messageObject.SetActive(false);
        finalScorePanel.transform.localScale = panelStartScale;
        finalScorePanel.SetActive(false);
        finalScorePanelGroup.alpha = 0f;
    }

    public void UpdateScores(int roundScore, int totalScore)
    {
        scoreText.text = $"{roundScore}";
        totalScoreText.text = $"{totalScore}";
    }

    public void ResetRoundScore()
    {
        scoreText.text = "0";
    }

    public void UpdatePinCollapsed()
    {
        pinsCollapsedCount++;
        pinCollapsedText.text = $"{pinsCollapsedCount}";
    }

    public void ResetPinCount()
    {
        pinsCollapsedCount = 0;
        pinCollapsedText.text = "0";
    }

    public void HideFinalScore()
    {
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Join(finalScorePanel.transform.DOScale(panelStartScale, panelScaleUpDuration).SetEase(Ease.InBack));
        hideSequence.Join(finalScorePanelGroup.DOFade(0f, panelFadeInDuration));
        hideSequence.OnComplete(() => finalScorePanel.SetActive(false));
    }

    public void UpdateBallType(string ballType)
    {
        if (BallTypeText != null)
        {
            string displayText = ballType == "MetalBall" ? "Metal" : "Rubber";
            BallTypeText.text = displayText;
        }
    }

    public void ResetBallType()
    {
        if (BallTypeText != null)
        {
            BallTypeText.text = "";
        }
    }

    public void DisplayScoreHistory(List<ScoreRecord> records)
    {
        // Clear existing records
        foreach (Transform child in scoreRecordContainer)
        {
            Destroy(child.gameObject);
        }

        // Reset panel state
        finalScorePanel.transform.localScale = panelStartScale;
        finalScorePanelGroup.alpha = 0f;
        finalScorePanel.SetActive(true);

        // Animate panel
        Sequence panelSequence = DOTween.Sequence();
        panelSequence.Join(finalScorePanel.transform.DOScale(panelEndScale, panelScaleUpDuration).SetEase(Ease.OutBack));
        panelSequence.Join(finalScorePanelGroup.DOFade(1f, panelFadeInDuration));

        // Create record rows
        foreach (var record in records)
        {
            GameObject recordObj = Instantiate(scoreRecordPrefab, scoreRecordContainer);
            recordObj.GetComponent<ScoreRecordRow>().SetData(record);
        }
    }

    public void ShowMessage(string message)
    {
        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);
            
        messageCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        IsShowingMessage = true;
        
        // Reset state
        messageObject.transform.localScale = startScale;
        messageText.alpha = 0f;
        messageObject.SetActive(true);
        messageText.text = message;

        // Animate in
        Sequence showSequence = DOTween.Sequence();
        showSequence.Join(messageObject.transform.DOScale(endScale, scaleUpDuration).SetEase(Ease.OutBack));
        showSequence.Join(messageText.DOFade(1f, fadeInDuration));

        yield return new WaitForSeconds(messageDisplayTime);

        // Animate out
        Sequence hideSequence = DOTween.Sequence();
        hideSequence.Join(messageObject.transform.DOScale(startScale, scaleUpDuration).SetEase(Ease.InBack));
        hideSequence.Join(messageText.DOFade(0f, fadeOutDuration));

        yield return hideSequence.WaitForCompletion();
        messageObject.SetActive(false);
        IsShowingMessage = false;
    }

    private void OnDestroy()
    {
        // Kill all tweens when destroyed
        messageObject.transform.DOKill();
        messageText.DOKill();
        if (finalScorePanel != null)
        {
            finalScorePanel.transform.DOKill();
            finalScorePanelGroup.DOKill();
        }
    }
}


 [System.Serializable]

    public class ScoreRecord

    {

        public int round;

        public int score;

        public string ballType;

        public int pinsCollapsed;

    }
