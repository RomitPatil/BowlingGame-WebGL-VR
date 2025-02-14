using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    public Button playAgainButton;

    private void Start()
    {
        playAgainButton.onClick.AddListener(RestartGame);
    }

    private void RestartGame(){
        SceneManager.LoadScene("MainScene"); 
    }
}