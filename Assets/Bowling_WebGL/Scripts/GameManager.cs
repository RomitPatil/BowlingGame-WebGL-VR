using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string Username { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUsername(string username)
    {
        Username = username;
    }
}
