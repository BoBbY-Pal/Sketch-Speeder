using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private Vector3 direction;
    public bool shouldMove = false;
    private float startingX;
   
    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        startingX = transform.position.x;
    }

    private void OnDisable()
    {
        shouldMove = false;
        transform.position = new Vector3(startingX, transform.position.y,0);
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
        if(collision.CompareTag("Border")) 
        {    
            gameObject.SetActive(false);
        }
        else if(collision.CompareTag("Player")) 
        {
            collision.GetComponent<PlayerCharacter>().Kill();
        }
    }

    public void MoveSpikes(bool status)
    {
        shouldMove = status;
    }
}