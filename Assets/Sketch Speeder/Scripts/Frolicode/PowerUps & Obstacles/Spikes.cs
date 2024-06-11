using Sketch_Speeder.Managers;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private Vector3 direction;
    private bool shouldMove = false;
    private float startingX;
 
    private void OnEnable()
    {
        startingX = transform.localPosition.x;
    }

    private void OnDisable()
    {
        shouldMove = false;
        // transform.localPosition = new Vector3(startingX, transform.localPosition.y,10);
    }

    private void Update()
    {
        if (shouldMove)
        {
            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) 
        {
            SoundManager.Instance.Play(SoundTypes.GameLose);
            collision.GetComponent<PlayerCharacter>().ProcessHit();
        }
    }
    
    public void SlowDownSpikes()
    {
        moveSpeed = 0.5f;
    }
    
    public void MoveSpikesNormally()
    {
        moveSpeed = 1f;
    }
    
    public void MoveSpikes(bool status)
    {
        shouldMove = status;
    }
}