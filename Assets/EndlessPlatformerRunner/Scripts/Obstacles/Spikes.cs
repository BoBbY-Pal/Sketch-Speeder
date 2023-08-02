using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private Vector3 direction;
    public bool shouldMove = false;
    [SerializeField] private PlayerCharacter _playerCharacter;
    private void Update()
    {
        if (shouldMove)
        {
            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
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

    public void MoveSpikes(bool status)
    {
        shouldMove = status;
    }
}