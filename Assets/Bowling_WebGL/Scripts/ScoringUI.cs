using UnityEngine;
using TMPro;

public class ScoringUI : MonoBehaviour
{
    public static ScoringUI Instance;

    [SerializeField] private TextMeshProUGUI playerNameText;
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

    void Start()
    {
        playerNameText.text = $"{GameManager.Instance.userName}";
        scoreText.text = $"{0}";
    }

    public void UpdateScoreUI(int totalScore, string userName)
    {
        playerNameText.text = $"{userName}";
        scoreText.text = $"{totalScore}";
    }

    public void ShowFinalScore(int totalScore)
    {
        finalScorePanel.SetActive(true);
        finalScoreText.text = $"{totalScore}";
    }

    public void HideFinalScore()
    {
        finalScorePanel.SetActive(false);
    }
}
