using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource pinHitSound, ballThrowSound;

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

    public void PlayPinHitSound()
    {
        pinHitSound.Play();
    }

    public void PlayBallThrowSound()
    {
        ballThrowSound.Play();
    }
}
