using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public Button startbutton;

    private void Start()
    {
        if (startbutton != null)
        {
            startbutton.onClick.AddListener(OnStartGame);
        }
    }

    private void OnStartGame()
    {
        if (usernameInput != null && !string.IsNullOrEmpty(usernameInput.text))
        {
            GameManager.Instance.SetUsername(usernameInput.text);
            SceneManager.LoadScene("GameScene");
        }
    }
}
