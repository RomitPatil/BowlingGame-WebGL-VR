using UnityEngine;
using TMPro;
using System.Collections;

public class Pin : MonoBehaviour
{
    private bool isHit = false; // Prevents multiple triggers
    private bool isCollapsed = false;
    private bool isScored = false;  // Track if pin has been scored
    private Rigidbody rb;
    private AudioSource audioSource; // Reference to AudioSource
    public AudioClip pinHitSound; // Assign in Inspector

    [SerializeField] private float collapseThreshold = 0.5f;  // Angle threshold
    [SerializeField] private float minYPosition = -0.5f;      // Minimum Y position before counting as fallen
    private Vector3 startPosition;                            // Initial pin position

    // UI popup prefab for showing score
    [SerializeField] private GameObject scorePopupPrefab;
    private Vector3 scoreOffset = new Vector3(0, 2f, 0); // Offset above pin

    private static int totalPinsCollapsed = 0;
    private const int TOTAL_PINS = 10;
    private static bool anyPinHit = false;  // Track if any pin was hit this round
    private static string roundEndMessage = "";  // Store message for end of round

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
        // Wait a frame before setting the start position to ensure proper physics initialization
        StartCoroutine(InitializePin());
    }

    private IEnumerator InitializePin()
    {
        yield return new WaitForFixedUpdate();
        
        startPosition = transform.position;
        
        // Reset the pin's physics state
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        if (!isHit)
        {
            totalPinsCollapsed = 0;
            anyPinHit = false;
            roundEndMessage = "";
            Debug.Log($"Pin initialized at position: {startPosition}");
        }
    }

    void Update()
    {
        if (!isScored)
        {
            bool isAngleCollapsed = transform.up.y < collapseThreshold;
            bool isBelowLevel = transform.position.y < startPosition.y + minYPosition;

            if (isAngleCollapsed || isBelowLevel)
            {
                isScored = true;
                isCollapsed = true;
                totalPinsCollapsed++;
                
                Debug.Log($"Pin collapsed! Angle: {transform.up.y}, Height: {transform.position.y}. Total pins collapsed: {totalPinsCollapsed}");
                
                // Play pin sound
                SoundManager.singleton.PlayPinSound();
                
                int score = GameManager.Instance.currentBallType == "MetalBall" ? 10 : 20;
                ScoringUI.Instance.UpdatePinCollapsed();
                GameManager.Instance.UpdateScore(true);

                // Store message instead of showing immediately
                if (totalPinsCollapsed == TOTAL_PINS)
                {
                    roundEndMessage = $"{GameManager.Instance.userName}, You have won!";
                }
                else if (string.IsNullOrEmpty(roundEndMessage))  // Only set if not already set
                {
                    roundEndMessage = $"Congratulations {GameManager.Instance.userName}!";
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;

        if (collision.gameObject.CompareTag("MetalBall") || collision.gameObject.CompareTag("RubberBall"))
        {
            isHit = true;
            anyPinHit = true;
            
            Debug.Log($"Pin hit by {collision.gameObject.tag} at velocity: {collision.relativeVelocity.magnitude}");
            
            if (!isScored)
            {
                int score = GameManager.Instance.currentBallType == "MetalBall" ? 5 : 15;
                GameManager.Instance.UpdateScore(false);
                
                // Play pin sound
                SoundManager.singleton.PlayPinSound();
            }
        }
    }

    private void ShowScorePopup(int score)
    {
        if (scorePopupPrefab != null)
        {
            Vector3 popupPosition = transform.position + scoreOffset;
            GameObject popup = Instantiate(scorePopupPrefab, popupPosition, Quaternion.identity);
            popup.GetComponent<TextMeshProUGUI>().text = $"+{score}";
            Destroy(popup, 1f); // Destroy after 1 second
        }
    }

    public static void CheckEndRound()
    {
        Debug.Log($"Checking end round. Any pins hit: {anyPinHit}, Total pins collapsed: {totalPinsCollapsed}");
        
        if (!anyPinHit)
        {
            roundEndMessage = $"Oops {GameManager.Instance.userName}!";
        }
        
        // Show stored message at end of round
        if (!string.IsNullOrEmpty(roundEndMessage))
        {
            Debug.Log($"End round message: {roundEndMessage}");
            ScoringUI.Instance.ShowMessage(roundEndMessage);
        }
    }
}
