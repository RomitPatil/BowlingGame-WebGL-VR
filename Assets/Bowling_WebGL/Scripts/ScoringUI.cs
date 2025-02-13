using UnityEngine;
using TMPro;

public class ScoringUI : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text roundScoreText;
    public TMP_Text totalScoreText;

    private void OnEnable ()
    {
       userNameText.text = GameManager.Instance.Username;
       ScoreManager.Instance.OnScoreChange += UpdateScoreUI;
       UpdateScoreUI();
    }

    private void UpdateScoreUI(){

        roundScoreText.text = $"Round Score: {ScoreManager.Instance.GetRoundScore()}";
        totalScoreText.text = $"Total Score: {ScoreManager.Instance.GetTotalScore()}";
    }
}
