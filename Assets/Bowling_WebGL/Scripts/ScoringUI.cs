using UnityEngine;
using TMPro;

public class ScoringUI : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text roundScoreText;
    public TMP_Text totalScoreText;

    private void Start()
    {
        // Add null checks
        if (userNameText != null && GameManager.Instance != null)
        {
            userNameText.text = GameManager.Instance.Username;
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChange += UpdateScoreUI;
            UpdateScoreUI();
        }
        else
        {
            Debug.LogWarning("ScoreManager instance is null!");
        }
    }

    private void OnDestroy()
    {
        // Clean up the event subscription when the object is destroyed
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChange -= UpdateScoreUI;
        }
    }

    private void UpdateScoreUI()
    {
        if (roundScoreText != null && totalScoreText != null && ScoreManager.Instance != null)
        {
            roundScoreText.text = $"Round Score: {ScoreManager.Instance.GetRoundScore()}";
            totalScoreText.text = $"Total Score: {ScoreManager.Instance.GetTotalScore()}";
        }
    }
}
