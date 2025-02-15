using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GameEndManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button playAgainButton;
    
    [Header("Animation Settings")]
    [SerializeField] private float buttonScaleDuration = 0.3f;
    private Vector3 buttonOriginalScale;

    void Start()
    {   
        // Set up button listeners
        if (backButton != null)
        {
            buttonOriginalScale = backButton.transform.localScale;
            backButton.onClick.AddListener(OnBackClicked);
            SetupButtonAnimation(backButton);
        }
        
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(OnPlayAgainClicked);
            SetupButtonAnimation(playAgainButton);
        }
    }

    private void SetupButtonAnimation(Button button)
    {
        button.onClick.AddListener(() => AnimateButton(button));
        
        var eventTrigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        var buttonTransform = button.transform;

        // Hover enter
        var enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enterEntry.callback.AddListener((data) => buttonTransform.DOScale(buttonOriginalScale * 1.1f, buttonScaleDuration));
        eventTrigger.triggers.Add(enterEntry);

        // Hover exit
        var exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exitEntry.callback.AddListener((data) => buttonTransform.DOScale(buttonOriginalScale, buttonScaleDuration));
        eventTrigger.triggers.Add(exitEntry);
    }

    private void AnimateButton(Button button)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(button.transform.DOScale(buttonOriginalScale * 0.9f, buttonScaleDuration / 2));
        sequence.Append(button.transform.DOScale(buttonOriginalScale, buttonScaleDuration / 2));
    }

    private void OnBackClicked()
    {
        // Empty sequence not needed if there's no animation
        GameManager.Instance.BackToMainMenu();
    }

    private void OnPlayAgainClicked()
    {
        // Empty sequence not needed if there's no animation
        GameManager.Instance.RestartGame();
    }

    private void OnDestroy()
    {
        // Clean up
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.transform.DOKill();
        }
        
        if (playAgainButton != null)
        {
            playAgainButton.onClick.RemoveAllListeners();
            playAgainButton.transform.DOKill();
        }
    }
}