using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public Button playAgainButton;

    private void Start()
    {
        finalScoreText.text = $"{ScoreManager.Instance.GetTotalScore()}";
        playAgainButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame(){
        SceneManager.LoadScene("MainScene"); 
    }
}