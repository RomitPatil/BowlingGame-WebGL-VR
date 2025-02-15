using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject metalBallPrefab, rubberBallPrefab;
    [SerializeField] private GameObject currentBall;
    public List<Button> metalBallButtons;
    public List<Button> rubberBallButtons;

    private bool isRoundActive = false;
    private int totalBallsUsed = 0;
    private const int MAX_BALLS = 5;
    private bool isLastBall = false;

    void Start()
    {
        ResetButtons();
        
        // Auto-select first metal ball after a short delay
        if (metalBallButtons != null && metalBallButtons.Count > 0)
        {
            // Small delay to ensure everything is initialized
            Invoke("AutoSelectFirstMetalBall", 0.1f);
        }
    }

    private void AutoSelectFirstMetalBall()
    {
        Button firstMetalButton = metalBallButtons[0];
        if (firstMetalButton != null && firstMetalButton.interactable)
        {
            OnMetalBallSelect(firstMetalButton);
        }
    }

    private void ResetButtons()
    {
        totalBallsUsed = 0;
        isLastBall = false;

        foreach (Button button in metalBallButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnMetalBallSelect(button));
            button.interactable = true;
        }

        foreach (Button button in rubberBallButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnRubberBallSelect(button));
            button.interactable = true;
        }
        isRoundActive = false;
    }

    public void OnMetalBallSelect(Button clickedButton)
    {
        if (!isRoundActive && totalBallsUsed < MAX_BALLS)
        {
            PinArrangement.Instance.ResetPins();
            ScoringUI.Instance.ResetPinCount();
            ScoringUI.Instance.UpdateBallType("MetalBall");
            SpawnBall(metalBallPrefab);
            clickedButton.interactable = false;
            totalBallsUsed++;
            
            isLastBall = (totalBallsUsed >= MAX_BALLS);
            
            GameManager.Instance.StartNewRound("MetalBall");
            isRoundActive = true;
        }
    }

    public void OnRubberBallSelect(Button clickedButton)
    {
        if (!isRoundActive && totalBallsUsed < MAX_BALLS)
        {
            PinArrangement.Instance.ResetPins();
            ScoringUI.Instance.ResetPinCount();
            ScoringUI.Instance.UpdateBallType("RubberBall");
            SpawnBall(rubberBallPrefab);
            clickedButton.interactable = false;
            totalBallsUsed++;
            
            isLastBall = (totalBallsUsed >= MAX_BALLS);
            
            GameManager.Instance.StartNewRound("RubberBall");
            isRoundActive = true;
        }
    }

    public void EndRound()
    {
        isRoundActive = false;
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        Pin.CheckEndRound();  // Check if any pins were hit before ending round

        // Only end game after last ball completes its round
        if (isLastBall)
        {
            GameManager.Instance.EndGame();
            ScoringUI.Instance.ResetBallType();  // Clear ball type text at game end
        }
    }

    void SpawnBall(GameObject ballPrefab)
    {
        if (currentBall != null) 
            Destroy(currentBall);

        currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
    }

    public void DisableAllButtons()
    {
        foreach (Button button in metalBallButtons)
        {
            button.interactable = false;
        }

        foreach (Button button in rubberBallButtons)
        {
            button.interactable = false;
        }
    }
}
