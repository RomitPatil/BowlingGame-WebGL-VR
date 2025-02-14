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

    private float targetXPosition;
    [SerializeField] private bool isMouseOver = false;
    private Camera mainCamera;
    [SerializeField] private bool canMove = true;
    
    private Rigidbody rb;
    private Vector3 dragStartPosition;
    private Vector2 dragStartScreen;
    [SerializeField] private bool isDragging = false;
    [SerializeField] private bool hasLaunched = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        targetXPosition = transform.position.x;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Start with kinematic enabled
    }

    // Update is called once per frame
    void Update()
    {
        if (hasLaunched) return;

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

    private void LaunchBall(Vector2 dragDelta)
    {
        isDragging = false;
        hasLaunched = true;
        canMove = false;
        
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
}

