using UnityEngine;
using TMPro;

public class ScoringUI : MonoBehaviour
{
    public static ScoringUI Instance;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject finalScorePanel;

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

    public void UpdateScoreUI(int totalScore, string userName)
    {
        scoreText.text = $"Player: {userName} | Score: {totalScore}";
    }

    public void ShowFinalScore(int totalScore)
    {
        finalScorePanel.SetActive(true);
        finalScoreText.text = $"Final Score: {totalScore}";
    }

    public void HideFinalScore()
    {
        finalScorePanel.SetActive(false);
    }
}
