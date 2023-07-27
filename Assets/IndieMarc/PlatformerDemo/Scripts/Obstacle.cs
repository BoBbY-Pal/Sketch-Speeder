
using UnityEngine;


public class Obstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed; 
    private void Update()
    {
        transform.position += Vector3.left * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Border")) 
        {    
            gameObject.SetActive(false);
        }
        else if(collision.CompareTag("Player")) 
        {
            // PlayerController.Instance.PlayerDied();
        }
    }
}