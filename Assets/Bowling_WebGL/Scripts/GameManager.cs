using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public TextMeshProUGUI scoreText, roundText, finalScoreText;
    public GameObject playAgainButton, finalScorePanel;
    public Transform ballSpawnpoint;
    public GameObject metalBallPrefab, rubberBallPrefab;

    private int totalScore = 0, currentRound = 1, maxRound = 5;
    private int metalBallCount = 3, rubberBallCount = 2;
    private GameObject currentBall;
    private string userName;

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
        userName = PlayerPrefs.GetString("Username", "Player");
        Debug.Log($"Username loaded: {userName}");
        scoreText.text = $"Player: {userName}";
        UpdateUI();
        SpawnBall();
    }

    public void SetUsername(string username)
    {
        userName = username;
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.Save();
    }

    public void UpdateScore(string ballType, bool isCollapsed)
    {
        int scorePerCollapse = ballType == "MetalBall" ? 10 : 20;
        int scorePerTouch = ballType == "MetalBall" ? 5 : 15;

        totalScore += isCollapsed ? scorePerCollapse : scorePerTouch;
        scoreText.text = $"Score: {totalScore}";
    }

    public void NextRound()
    {
        Destroy(currentBall);
        currentRound++;

        if (currentRound > maxRound)
        {
            EndGame();
        }
        else
        {
            SpawnBall();
            UpdateUI();
        }
    }

    void SpawnBall()
    {
        if (currentBall != null) Destroy(currentBall);

        if (metalBallCount > 0)
        {
            currentBall = Instantiate(metalBallPrefab, ballSpawnpoint.position, Quaternion.identity);
            metalBallCount--;
        }
        else if (rubberBallCount > 0)
        {
            currentBall = Instantiate(rubberBallPrefab, ballSpawnpoint.position, Quaternion.identity);
            rubberBallCount--;
        }
        else
        {
            Debug.LogWarning("No balls remaining to spawn!");
        }
    }

    void UpdateUI()
    {
        roundText.text = "Round: " + currentRound + " / " + maxRound;
    }

    void EndGame()
    {
        playAgainButton.SetActive(true);
        ShowFinalScore();
    }

    void ShowFinalScore()
    {
        finalScorePanel.SetActive(true);
        finalScoreText.text = $"Final Score: {totalScore}\nPlayer: {userName}";
    }

    public void RestartGame()
    {
        totalScore = 0;
        currentRound = 1;
        metalBallCount = 3;
        rubberBallCount = 2;

        playAgainButton.SetActive(false);
        finalScorePanel.SetActive(false);
        
        SpawnBall();
        UpdateUI();
    }
}
