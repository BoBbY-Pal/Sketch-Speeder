using UnityEngine;
using Random = UnityEngine.Random;

public class Ground : MonoBehaviour
{
    private float groundRight; // Right edge of the ground.
    private float groundHeight; // Height edge of the ground.
    private float screenLeft; // Left edge of the screen.
    private float screenRight; // Right edge of the screen.
    
    [SerializeField] BoxCollider2D collider;
    [SerializeField] Renderer rend;
    private Camera _camera;

    bool didGenerateGround = false;
    public Vector2 xDistanceRange;
    public Vector2 yHeightRange;

    private void Awake()
    {
        _camera = Camera.main;
        groundHeight = transform.position.y + (collider.size.y / 2);
        didGenerateGround = false;
    }

    private void Start()
    {
        groundRight = rend.bounds.max.x;
    }

    private void Update()
    {
        screenLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0)).x;
        screenRight = _camera.ViewportToWorldPoint(new Vector3(1, 0)).x;
        
        if (groundRight < screenLeft)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                GenerateGround();
            }
        }
    }


    void GenerateGround()
    {
        GameObject nextGround = Instantiate(gameObject);
        Vector2 pos;

        float heightOffset = Random.Range(yHeightRange.x, yHeightRange.y);
        pos.y = groundHeight + heightOffset;

        float distanceOffset = Random.Range(xDistanceRange.x, xDistanceRange.y);
        pos.x = groundRight + distanceOffset + collider.size.x / 2; 

        nextGround.transform.position = pos;
    }
}