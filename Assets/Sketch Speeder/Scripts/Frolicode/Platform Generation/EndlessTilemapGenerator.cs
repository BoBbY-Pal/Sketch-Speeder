using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EndlessTilemapGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject[] tilemapChunks;

    [SerializeField] private float chunkSize; // The size of each tilemap chunk
    public List<GameObject> activeChunks = new List<GameObject>();
    [SerializeField] private int initialChunksCount = 1; // Number of initial chunks to load
    [SerializeField] private float gapBetweenChunks = 5f; // The distance between each tilemap chunk
    public Transform paretnTransform;
    
    [SerializeField] private Vector2 chunkHeightRange = new Vector2(-2, 2); // The y position range for each tilemap chunk
    [SerializeField] private Vector2 gapRange = new Vector2(3, 5); // The range of the gap between each tilemap chunk
    

    private void OnEnable()
    {
        LoadInitialChunks();
    }

    private void OnDisable()
    {
        UnLoadAllChunks();
    }

    private void LoadInitialChunks()
    {
        for (int i = 0; i < initialChunksCount; i++)
        {
            LoadChunk(i);
        }

        player.localPosition = new Vector3(0, activeChunks[0].transform.position.y + 3, 0);
        player.gameObject.SetActive(true);
    }

    private void Update()
    {
        // Check if player has reached the end of the active chunks
        if (player.position.x > activeChunks[activeChunks.Count - 1].transform.position.x)
        {
            LoadChunk(activeChunks.Count);
        }
    }

    private void LoadChunk(int chunkIndex)
    {
        int randomNum = Random.Range(0, tilemapChunks.Length);
        GameObject newChunk = Instantiate(tilemapChunks[randomNum], paretnTransform);
        newChunk.SetActive(true);
        float lastChunkSize = chunkSize;
        chunkSize = CalculateChunkSize(newChunk.GetComponent<Tilemap>());

        float xPos;
        float randomGap = Random.Range(gapRange.x, gapRange.y);
        if (activeChunks.Count == 0)
        {
            xPos = chunkIndex * chunkSize + chunkIndex * randomGap;
        }
        else
        {
             // Random gap between chunks
            xPos = activeChunks[activeChunks.Count - 1].transform.position.x + lastChunkSize + randomGap;
        }

        float yPos = Random.Range(chunkHeightRange.x, chunkHeightRange.y); // Random height for chunk

        newChunk.transform.position = new Vector3(xPos, yPos, 0f);
        activeChunks.Add(newChunk);

        // Remove the first chunk when there are too many active chunks
        if (activeChunks.Count > initialChunksCount + 1)
        {
            UnloadChunk(0);
        }
    }

    public Vector3 FindNearestChunk(Vector3 targetPos)
    {
        Vector3 nearestChunkPosition = Vector3.zero;
        float smallestDistance = float.MaxValue;

        foreach (var chunk in activeChunks)
        {
            float distance = Mathf.Abs(chunk.transform.position.x - targetPos.x);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                nearestChunkPosition = chunk.transform.position;
            }
        }

        return nearestChunkPosition;
    }


    private void UnloadChunk(int chunkIndex)
    {
        Destroy(activeChunks[chunkIndex]);
        activeChunks.RemoveAt(chunkIndex);
    }
    private void UnLoadAllChunks()
    {
        foreach (var chunk in activeChunks)
        {
            Destroy(chunk);
        }
        activeChunks.Clear();
    }
    float CalculateChunkSize(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // Initialize min and max to extreme values
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    // Calculate world position
                    Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(bounds.x + x, bounds.y + y, bounds.z));

                    if (worldPos.x < minX)
                        minX = worldPos.x;

                    if (worldPos.x > maxX)
                        maxX = worldPos.x;
                }
            }
        }

        // Chunk size
        float chunkSize = maxX - minX;
    
        Debug.Log($"MinX: {minX}, MaxX: {maxX}, Chunk size: {chunkSize}");
        return chunkSize;
    }
}