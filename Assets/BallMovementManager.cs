using UnityEngine;

public class BallMovementManager : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float minXPosition = 0f;
    [SerializeField] private float maxXPosition = 10f;
    [SerializeField] private float smoothSpeed = 5f;
    
    [Header("Ball Launch Settings")]
    [SerializeField] private float minForce = 500f;    // Minimum force to apply
    [SerializeField] private float maxForce = 2000f;   // Maximum force to apply
    [SerializeField] private float minDragDistance = 50f;  // Minimum drag distance required to launch

    [Header("Ball Boundaries")]
    [SerializeField] private float minYPosition = -10f; // Height at which ball is destroyed
    [SerializeField] private string wallTag = "Wall";  // Tag for walls that should end the round
    [SerializeField] private float endRoundDelay = 2f; // Delay before ending round after wall hit
    
    [Header("Movement Detection")]
    [SerializeField] private float minMovementSpeed = 0.1f;  // Minimum speed to consider ball moving
    [SerializeField] private float stillDuration = 2f;       // How long ball needs to be still before ending round
    private float stillTimer = 0f;
    private bool isCheckingMovement = false;

    private float targetXPosition;
    [SerializeField] private bool isMouseOver = false;
    private Camera mainCamera;
    [SerializeField] private bool canMove = true;
    
    private Rigidbody rb;
    private Vector3 dragStartPosition;
    private Vector2 dragStartScreen;
    [SerializeField] private bool isDragging = false;
    [SerializeField] private bool hasLaunched = false;

    private BallController ballController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        targetXPosition = transform.position.x;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Start with kinematic enabled
        ballController = FindFirstObjectByType<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLaunched)
        {
            // Check if ball has fallen below minimum height
            if (transform.position.y < minYPosition)
            {
                EndBall();
                return;
            }

            // Check for stopped movement
            if (rb != null && !rb.isKinematic)
            {
                CheckBallMovement();
            }
        }

        HandleSidewaysMovement();
        HandleDragging();
    }

    private void HandleSidewaysMovement()
    {
        if (isMouseOver && canMove && !isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);
            targetXPosition = Mathf.Clamp(worldPosition.x, minXPosition, maxXPosition);
        }

        if (canMove && !isDragging)
        {
            Vector3 currentPos = transform.position;
            currentPos.x = Mathf.Lerp(currentPos.x, targetXPosition, smoothSpeed * Time.deltaTime);
            transform.position = currentPos;
        }
    }

    private void HandleDragging()
    {
        if (isDragging)
        {
            Vector2 currentMousePos = Input.mousePosition;
            Vector2 dragDelta = currentMousePos - dragStartScreen;

            if (Input.GetMouseButtonUp(0))
            {
                // Only launch if drag distance exceeds minimum and moving upward
                if (dragDelta.magnitude >= minDragDistance && dragDelta.y > 0)
                {
                    LaunchBall(dragDelta);
                }
                else
                {
                    isDragging = false;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (hasLaunched || !canMove) return;
        
        isDragging = true;
        dragStartPosition = transform.position;
        dragStartScreen = Input.mousePosition;
    }

    private void CheckBallMovement()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        if (currentSpeed < minMovementSpeed)
        {
            if (!isCheckingMovement)
            {
                isCheckingMovement = true;
                stillTimer = 0f;
            }
            
            stillTimer += Time.deltaTime;
            
            if (stillTimer >= stillDuration)
            {
                EndBall();
            }
        }
        else
        {
            isCheckingMovement = false;
            stillTimer = 0f;
        }
    }

    private void LaunchBall(Vector2 dragDelta)
    {
        isDragging = false;
        hasLaunched = true;
        canMove = false;
        isCheckingMovement = false;  // Reset movement checking
        stillTimer = 0f;
        
        // Convert screen direction to world direction (y becomes z)
        Vector3 dragDirection = new Vector3(dragDelta.x, 0, dragDelta.y);
        dragDirection = dragDirection.normalized;
        
        // Calculate force based on drag distance
        float dragDistance = dragDelta.magnitude;
        float forceMagnitude = Mathf.Lerp(minForce, maxForce, dragDistance / 500f);
        forceMagnitude = Mathf.Clamp(forceMagnitude, minForce, maxForce);
        
        // Enable physics and apply force
        rb.isKinematic = false;
        rb.AddForce(dragDirection * forceMagnitude, ForceMode.Impulse);
    }

    private void OnMouseEnter()
    {
        if (!hasLaunched)
            isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    // Public method to enable/disable movement
    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
    }

    // Method to reset the ball state
    public void ResetBall()
    {
        hasLaunched = false;
        isDragging = false;
        canMove = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLaunched && collision.gameObject.CompareTag(wallTag))
        {
            // End round after a short delay to let physics settle
            Invoke("EndBall", endRoundDelay);
        }
    }

    private void EndBall()
    {
        if (ballController != null)
        {
            ballController.EndRound();
        }
        Destroy(gameObject);
    }
}

