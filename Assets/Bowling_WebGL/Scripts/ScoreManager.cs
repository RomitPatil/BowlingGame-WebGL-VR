using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public event Action OnScoreChange;

    [SerializeField]
    private int totalScore;
    [SerializeField]
    private int roundScore;

    private void Awake(){

        if (Instance == null) Instance = this;
    }

    public void UpdateScore(int points){
        roundScore += points;
        totalScore += points;
        OnScoreChange?.Invoke();
    }

    public int GetRoundScore() => roundScore;
    public int GetTotalScore() => totalScore;


}
