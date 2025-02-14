using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public TextMeshProUGUI  roundText, popupfinalScoreText;
    public GameObject playAgainButton, finalScorePanel;
    public Transform ballSpawnpoint;
    public List<GameObject> metalBallButtons, rubberBallButtons;

    private int totalScore = 0;
    private int roundScore = 0;
    private int currentRound = 0, maxRound = 5;
    public string userName;
    public string currentBallType { get; private set; }
    private List<ScoreRecord> scoreRecords = new List<ScoreRecord>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        popupfinalScoreText.text = $"{0}";
        userName = PlayerPrefs.GetString("Username", "Player");
        Debug.Log($"Username loaded: {userName}");
        UpdateUI();
    }

    public void SetUsername(string username)
    {
        userName = username;
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.Save();
    }

    public void StartNewRound(string ballType)
    {
        if (currentRound >= maxRound)
        {
            EndGame();
            return;
        }

        currentBallType = ballType;
        roundScore = 0;
        currentRound++;
        
        // Create new record for this round
        ScoreRecord record = new ScoreRecord
        {
            round = currentRound,
            score = 0,
            ballType = ballType == "MetalBall" ? "Metal" : "Rubber",
            pinsCollapsed = 0
        };
        scoreRecords.Add(record);
        
        ScoringUI.Instance.ResetRoundScore();
        UpdateUI();
    }

    public void UpdateScore(bool isCollapsed)
    {
        int scorePerCollapse = currentBallType == "MetalBall" ? 10 : 20;
        int scorePerTouch = currentBallType == "MetalBall" ? 5 : 15;

        int points = isCollapsed ? scorePerCollapse : scorePerTouch;
        roundScore += points;
        totalScore += points;
        
        // Update current round's record
        if (scoreRecords.Count > 0)
        {
            var currentRecord = scoreRecords[scoreRecords.Count - 1];
            currentRecord.score = roundScore;
            if (isCollapsed)
            {
                currentRecord.pinsCollapsed++;
            }
        }
        
        ScoringUI.Instance.UpdateScores(roundScore, totalScore);
    }

    public void NextRound()
    {
        if (currentRound >= maxRound)
        {
            EndGame();
        }
        else
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        roundText.text = "Round: " + currentRound + " / " + maxRound;
    }

    public void EndGame()
    {
        playAgainButton.SetActive(true);
        ShowFinalScore();
        
        // Disable all ball buttons
        BallController ballController = FindFirstObjectByType<BallController>();
        if (ballController != null)
        {
            ballController.DisableAllButtons();
        }
    }

    void ShowFinalScore()
    {
        finalScorePanel.SetActive(true);
        popupfinalScoreText.text = $"{totalScore}";
        ScoringUI.Instance.DisplayScoreHistory(scoreRecords);
    }

    public void RestartGame()
    {
        scoreRecords.Clear();
        totalScore = 0;
        roundScore = 0;
        currentRound = 0;

        playAgainButton.SetActive(false);
        finalScorePanel.SetActive(false);
        
        UpdateUI();
    }
}
