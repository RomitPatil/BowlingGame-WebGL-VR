using UnityEngine;

public class Pin : MonoBehaviour
{
  private bool hasCollide = false;

  void OnCollisionEnter(Collision collision){

if (!hasCollide && (collision.gameObject.CompareTag("MetalBall") || collision.gameObject.CompareTag("RubberBall")))
    hasCollide = true;
    if(transform.up.y < 0.5f){
        GameManager.Instance.AddScore(collision.gameObject.tag, true);
    }else {
        GameManager.Instance.AddScore(collision.gameObject.tag, false);
    }
  }
}
