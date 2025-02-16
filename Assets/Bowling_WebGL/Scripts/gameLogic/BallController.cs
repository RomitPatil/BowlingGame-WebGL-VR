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
        Debug.Log("BallController: Starting game setup");
        ResetButtons();
        
        // Auto-select first metal ball after a short delay
        if (metalBallButtons != null && metalBallButtons.Count > 0)
        {
            Debug.Log("BallController: Scheduling auto-select of first metal ball");
            Invoke("AutoSelectFirstMetalBall", 0.1f);
        }
    }

    private void AutoSelectFirstMetalBall()
    {
        Debug.Log("BallController: Attempting to auto-select first metal ball");
        Button firstMetalButton = metalBallButtons[0];
        if (firstMetalButton != null && firstMetalButton.interactable)
        {
            OnMetalBallSelect(firstMetalButton);
        }
    }

    private void ResetButtons()
    {
        Debug.Log($"BallController: Resetting buttons. Previous balls used: {totalBallsUsed}");
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
            Debug.Log($"BallController: Metal ball selected. Ball #{totalBallsUsed + 1} of {MAX_BALLS}");
            PinArrangement.Instance.ResetPins();
            ScoringUI.Instance.ResetPinCount();
            ScoringUI.Instance.UpdateBallType("MetalBall");
            SpawnBall(metalBallPrefab);
            clickedButton.interactable = false;
            totalBallsUsed++;
            
            isLastBall = (totalBallsUsed >= MAX_BALLS);
            Debug.Log($"BallController: Is this the last ball? {isLastBall}");
            
            GameManager.Instance.StartNewRound("MetalBall");
            isRoundActive = true;
        }
        else
        {
            Debug.Log($"BallController: Metal ball selection blocked. Round active: {isRoundActive}, Balls used: {totalBallsUsed}");
        }
    }

    public void OnRubberBallSelect(Button clickedButton)
    {
        if (!isRoundActive && totalBallsUsed < MAX_BALLS)
        {
            Debug.Log($"BallController: Rubber ball selected. Ball #{totalBallsUsed + 1} of {MAX_BALLS}");
            PinArrangement.Instance.ResetPins();
            ScoringUI.Instance.ResetPinCount();
            ScoringUI.Instance.UpdateBallType("RubberBall");
            SpawnBall(rubberBallPrefab);
            clickedButton.interactable = false;
            totalBallsUsed++;
            
            isLastBall = (totalBallsUsed >= MAX_BALLS);
            Debug.Log($"BallController: Is this the last ball? {isLastBall}");
            
            GameManager.Instance.StartNewRound("RubberBall");
            isRoundActive = true;
        }
        else
        {
            Debug.Log($"BallController: Rubber ball selection blocked. Round active: {isRoundActive}, Balls used: {totalBallsUsed}");
        }
    }

    public void EndRound()
    {
        Debug.Log($"BallController: Ending round. Last ball: {isLastBall}, Total balls used: {totalBallsUsed}");
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
        Debug.Log("BallController: Spawning new ball");
        if (currentBall != null) 
            Destroy(currentBall);

        currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
    }

    public void DisableAllButtons()
    {
        Debug.Log("BallController: Disabling all ball selection buttons");
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
