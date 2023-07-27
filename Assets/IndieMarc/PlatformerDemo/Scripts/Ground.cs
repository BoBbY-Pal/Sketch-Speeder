using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ground : MonoBehaviour
{
    public float groundRight;
    public float groundHeight;
    private float screenLeft; // Left edge of the screen.
    private float screenRight; // Right edge of the screen.
    BoxCollider2D collider;
    Renderer rend;

    bool didGenerateGround = false;

    public Vector2 xDistanceRange = new Vector2(-4, 10);
    public Vector2 yHeightRange = new Vector2(-4, 10);

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rend = GetComponent<Renderer>(); 
        
        groundHeight = transform.position.y + (collider.size.y / 2);
        didGenerateGround = false;
    }

    private void Start()
    {
        groundRight = rend.bounds.max.x;
    }

    private void Update()
    {
        screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x;
        screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x;
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