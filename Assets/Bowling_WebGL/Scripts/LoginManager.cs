using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(usernameInput.text))
        {
            PlayerPrefs.SetString("Username", usernameInput.text);
            SceneManager.LoadScene("BowlingScene"); // Make sure your main game scene is named correctly
        }
    }
}
