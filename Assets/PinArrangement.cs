using UnityEngine;

public class PinArrangement : MonoBehaviour
{
    public static PinArrangement Instance;
    
    public GameObject pinPrefab;  // Assign your pin prefab in Inspector
    public Transform pinParent;   // Assign "Pinset" (parent object) in Inspector
    public float spacing = 1f;    // Distance between adjacent pins
    public float yOffset = 0.0f;  // Height adjustment
    public Vector3 pinScale = new Vector3(1f, 1f, 1f);  // Added pin scale control

    private GameObject[] currentPins;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ArrangePins();
    }

    public void ResetPins()
    {
        // Destroy existing pins
        if (currentPins != null)
        {
            foreach (GameObject pin in currentPins)
            {
                if (pin != null)
                    Destroy(pin);
            }
        }

        // Create new pins
        ArrangePins();
    }

    void ArrangePins()
    {
        float rowSpacing = spacing * 0.866f; // cos(30°) for proper triangular spacing
        float pinHeight = pinPrefab.GetComponent<Collider>().bounds.size.y;
        float floorOffset = (pinHeight / 2) + yOffset;

        // Pin positions in a triangular formation (4 rows)
        int[] pinsPerRow = { 1, 2, 3, 4 };
        currentPins = new GameObject[10];
        int currentPin = 0;

        // Start from back row (3) to front row (0)
        for (int row = pinsPerRow.Length - 1; row >= 0; row--)
        {
            int pinsInThisRow = pinsPerRow[row];
            float rowZ = row * rowSpacing;
            
            // Calculate starting X position to center the row
            float startX = -(pinsInThisRow - 1) * spacing / 2;

            for (int pin = 0; pin < pinsInThisRow; pin++)
            {
                float xPos = startX + (pin * spacing);
                Vector3 position = pinParent.position + new Vector3(xPos, floorOffset, rowZ);
                
                GameObject newPin = Instantiate(pinPrefab, position, Quaternion.identity, pinParent);
                newPin.transform.localScale = pinScale;  // Apply scale to each pin
                currentPins[currentPin] = newPin;
                currentPin++;
            }
        }
    }
}
