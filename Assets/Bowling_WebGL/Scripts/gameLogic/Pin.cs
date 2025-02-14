using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool isHit = false; // Prevents multiple triggers
  private Rigidbody rb;
 private AudioSource audioSource; // Reference to AudioSource
    public AudioClip pinHitSound; // Assign in Inspector


  void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
        void OnCollisionEnter(Collision collision)
    {
        // Ensure only the first hit is counted
        if (isHit) return;

        if (collision.gameObject.CompareTag("MetalBall") || collision.gameObject.CompareTag("RubberBall"))
        {
            isHit = true; // Mark as hit
            
            bool isCollapsed = transform.up.y < 0.5f; // Check if the pin has fallen
            GameManager.Instance.UpdateScore(collision.gameObject.tag, isCollapsed);
              if (audioSource != null && pinHitSound != null)
            {
                audioSource.PlayOneShot(pinHitSound);
            }
        }
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > 0.1f && transform.up.y < 0.5f) // Detect if the pin has fallen
        {
            isHit = true;
        }
    }
}
