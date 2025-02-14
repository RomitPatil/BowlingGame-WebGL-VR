using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject metalBallPrefab, rubberBallPrefab;
    [SerializeField] private GameObject currentBall;
     private AudioSource audioSource; 
     public AudioClip throwSound; // Assign in Inspector

     public List<Button> metalBallButtons;
     public List<Button> rubberBallButtons;

    void Start()
    {
        foreach (Button button in metalBallButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnMetalBallSelect);
        }

        foreach (Button button in rubberBallButtons)    
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnRubberBallSelect);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            SpawnBall(metalBallPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            SpawnBall(rubberBallPrefab);
        }
    }

    public void OnMetalBallSelect( )
    {
        SpawnBall(metalBallPrefab);
    }

    public void OnRubberBallSelect( )
    {
        SpawnBall(rubberBallPrefab);
    }
    

    void SpawnBall(GameObject ballPrefab)
    {
        if (currentBall != null) 
            Destroy(currentBall);

        currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
    }
}
