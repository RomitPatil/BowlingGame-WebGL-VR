using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string Username { get; private set; }

    public TextMeshProUGUI scoreText;
    private int totalScore;

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

    public void SetUsername(string username)
    {
        Username = username;
    }

    public void AddScore(string ballType, bool isCollapsed)
    {
        int points = 0;

        if(ballType == "MetalBall"){
            points = isCollapsed ? 10 : 5;
        }else if(ballType == "RubberBall"){
            points = isCollapsed ? 20 : 15;
        }
        totalScore += points;
        scoreText.text = $"Score: {totalScore}";
    }
}
