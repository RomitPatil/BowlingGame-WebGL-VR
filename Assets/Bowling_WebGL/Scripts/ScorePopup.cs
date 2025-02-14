using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    private float moveSpeed = 1f;
    private float fadeSpeed = 1f;
    private TextMeshProUGUI textMesh;
    private Color textColor;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textColor = textMesh.color;
    }

    void Update()
    {
        // Move up
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade out
        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;

        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
} 