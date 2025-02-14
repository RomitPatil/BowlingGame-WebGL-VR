using UnityEngine;
using TMPro;

public class ScoreRecordRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI ballTypeText;
    [SerializeField] private TextMeshProUGUI pinsCollapsedText;

    public void SetData(ScoreRecord record)
    {
        roundText.text = record.round.ToString();
        scoreText.text = record.score.ToString();
        ballTypeText.text = record.ballType;
        pinsCollapsedText.text = record.pinsCollapsed.ToString();
    }
} 