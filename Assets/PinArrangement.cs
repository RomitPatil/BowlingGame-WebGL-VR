using UnityEngine;

public class PinArrangement : MonoBehaviour
{
    public GameObject pinPrefab;  // Assign your pin prefab in Inspector
    public Transform pinParent;   // Assign "Pinset" (parent object) in Inspector
    public float spacing = 30f;   // Distance between adjacent pins
    public float yOffset = 0.0f;  // Adjust this in Inspector to tweak pin height
    private int totalPins = 10;   // Total number of pins

    void Start()
    {
        ArrangePins();
    }

    void ArrangePins()
    {
        float rowSpacing = spacing * Mathf.Sqrt(3) / 2; // Vertical offset for rows
        int pinCount = 0;

        // Get the height of the pin to adjust its position correctly
        float pinHeight = pinPrefab.GetComponent<Collider>().bounds.size.y;
        float floorOffset = (pinHeight / 2) + yOffset; // Allows fine-tuning in Inspector

        // Standard bowling pin arrangement (4 rows)
        Vector3[,] pinPositions = new Vector3[4, 4] {
            { new Vector3(0, 0, 0), Vector3.zero, Vector3.zero, Vector3.zero },
            { new Vector3(-0.2f, 0, 0.3f), new Vector3(0.2f, 0, 0.3f), Vector3.zero, Vector3.zero },
            { new Vector3(-0.4f, 0, 0.6f), new Vector3(0, 0, 0.6f), new Vector3(0.4f, 0, 0.6f), Vector3.zero },
            { new Vector3(-0.6f, 0, 0.9f), new Vector3(-0.2f, 0, 0.9f), new Vector3(0.2f, 0, 0.9f), new Vector3(0.6f, 0, 0.9f) }
        };

        // Create all pins
        for (int row = 0; row < 4; row++) {
            for (int col = 0; col < 4; col++) {
                if (pinPositions[row, col] != Vector3.zero) {
                    Vector3 position = pinParent.position + pinPositions[row, col];
                    GameObject pin = Instantiate(pinPrefab, position, Quaternion.identity, pinParent);

      
                  

                    pinCount++;
                }
            }
        }
    }
}
