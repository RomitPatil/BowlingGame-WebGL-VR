using UnityEngine;
using TMPro;

public class Pin : MonoBehaviour
{
    private bool isHit = false; // Prevents multiple triggers
    private bool isCollapsed = false;
    private bool isScored = false;  // Track if pin has been scored
    private Rigidbody rb;
    private AudioSource audioSource; // Reference to AudioSource
    public AudioClip pinHitSound; // Assign in Inspector

    [SerializeField] private float collapseThreshold = 0.5f; // Adjust in inspector

    // UI popup prefab for showing score
    [SerializeField] private GameObject scorePopupPrefab;
    private Vector3 scoreOffset = new Vector3(0, 2f, 0); // Offset above pin

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check for collapse without direct hit
        if (!isScored && transform.up.y < collapseThreshold)
        {
            isScored = true;
            isCollapsed = true;
            int score = GameManager.Instance.currentBallType == "MetalBall" ? 10 : 20;
            ScoringUI.Instance.UpdatePinCollapsed();  // Update collapsed count
            GameManager.Instance.UpdateScore(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isHit) return;

        if (collision.gameObject.CompareTag("MetalBall") || collision.gameObject.CompareTag("RubberBall"))
        {
            isHit = true;
            
            if (!isScored)  // Only score if not already scored from falling
            {
                int score = GameManager.Instance.currentBallType == "MetalBall" ? 5 : 15;
                GameManager.Instance.UpdateScore(false);
            }

            if (audioSource != null && pinHitSound != null)
            {
                audioSource.PlayOneShot(pinHitSound);
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
}
