using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    public GameObject metalBallPrefab, rubberBallPrefab;
    
    [SerializeField]
    private GameObject currentBall;
    private Rigidbody rb;

    [SerializeField]
    private float throwForce = 15f;

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
        }
    }
}
