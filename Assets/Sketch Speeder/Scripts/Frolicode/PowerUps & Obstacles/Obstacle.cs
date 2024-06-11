
using UnityEngine;


public class Obstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed; 
    [SerializeField] private Vector3 direction;
    private void Update()
    {
        transform.position += direction * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Border")) 
        {    
            gameObject.SetActive(false);
        }
        else if(collision.CompareTag("Player")) 
        {
            collision.GetComponent<PlayerCharacter>().ProcessHit();
        }
    }
}