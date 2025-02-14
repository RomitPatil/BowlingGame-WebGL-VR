using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject metalBallPrefab, rubberBallPrefab;
    [SerializeField] private GameObject currentBall;
    private Rigidbody rb;

    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float spinForce = 2f; // Add some spin effect

     private AudioSource audioSource; 
 public AudioClip throwSound; // Assign in Inspector
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            SpawnBall(metalBallPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            SpawnBall(rubberBallPrefab);
        }

        if (currentBall != null && Input.GetKeyDown(KeyCode.Space)){
            ThrowBall();
        }
    }

    void SpawnBall(GameObject ballPrefab)
    {
        if (currentBall != null) 
            Destroy(currentBall);

        currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        rb = currentBall.GetComponent<Rigidbody>();
    }

    void ThrowBall()
    {
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            rb.AddTorque(Vector3.right * spinForce, ForceMode.Impulse); 
            if (audioSource != null && throwSound != null)
            {
                audioSource.PlayOneShot(throwSound);
            }
        }
    }
    
}
