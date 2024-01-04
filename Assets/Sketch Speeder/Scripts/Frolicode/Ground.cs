using UnityEngine;
using Random = UnityEngine.Random;

public class Ground : MonoBehaviour
{
    [SerializeField] private float groundRight; // Right edge of the ground.
    [SerializeField] private float groundHeight; // Height edge of the ground.
    [SerializeField] private float screenLeft; // Left edge of the screen.
    [SerializeField] private float screenRight; // Right edge of the screen.
    [SerializeField] private float tempGroundRight; 
    
    [SerializeField] Collider2D collider;
    [SerializeField] Renderer rend;
    private Camera _camera;

    bool didGenerateGround = false;
    public Vector2 xDistanceRange = new Vector2(5,10);
    public Vector2 yHeightRange = new Vector2(-1,1);

    private void Awake()
    {
        _camera = Camera.main;
        groundHeight = transform.position.y + (collider.bounds.size.y / 2);
        didGenerateGround = false;
    }

    private void Start()
    {
        groundRight = rend.bounds.max.x;
        tempGroundRight = (transform.position.x + collider.bounds.size.x / 2);
    }

    private void Update()
    {
        screenLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0)).x;
        screenRight = _camera.ViewportToWorldPoint(new Vector3(1, 0)).x;
        
        if (tempGroundRight < screenLeft)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (tempGroundRight < screenRight)
            {
                didGenerateGround = true;
                GenerateGround();
            }
        }
    }


    void GenerateGround()
    {
        // GameObject nextGround = Instantiate(Generator.instance.prefab[Random.Range(0, Generator.instance.prefab.Count)], Generator.instance.transform);
        Debug.Log("Ground instantiated");
        Vector2 pos;

        float heightOffset = Random.Range(yHeightRange.x, yHeightRange.y);
        pos.y = groundHeight + heightOffset;

        float distanceOffset = Random.Range(xDistanceRange.x, xDistanceRange.y);
        pos.x = tempGroundRight + distanceOffset + collider.bounds.size.x / 2; 

        // nextGround.transform.position = pos;
    }
}