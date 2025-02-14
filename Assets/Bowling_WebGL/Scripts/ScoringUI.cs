using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoringUI : MonoBehaviour
{
    public static ScoringUI Instance;

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI pinCollapsedText;
    [SerializeField] private GameObject finalScorePanel;

    [SerializeField] private TextMeshProUGUI BallTypeText;

    [SerializeField] private Transform scoreRecordContainer;  // Parent object for score records
    [SerializeField] private GameObject scoreRecordPrefab;    // Prefab for each score record row

    private int pinsCollapsedCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        finalScorePanel.SetActive(false);
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

        // Create record rows
        foreach (var record in records)
        {
            GameObject recordObj = Instantiate(scoreRecordPrefab, scoreRecordContainer);
            recordObj.GetComponent<ScoreRecordRow>().SetData(record);
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
