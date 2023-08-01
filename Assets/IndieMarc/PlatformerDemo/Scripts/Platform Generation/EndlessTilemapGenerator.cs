using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EndlessTilemapGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject[] tilemapChunks;

    [SerializeField] private float chunkSize; // The size of each tilemap chunk
    private List<GameObject> activeChunks = new List<GameObject>();
    [SerializeField] private int initialChunksCount = 1; // Number of initial chunks to load
    [SerializeField] private float gapBetweenChunks = 5f; // The distance between each tilemap chunk
    public Transform paretnTransform;

    private void Start()
    {
        // chunkSize = tilemapChunks[0].GetComponent<Tilemap>().localBounds.size.x; // Assuming all chunks have the same size
        LoadInitialChunks();
    }

    private void LoadInitialChunks()
    {
        for (int i = 0; i < initialChunksCount; i++)
        {
            LoadChunk(i);
        }
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
        if (activeChunks.Count == 0)
        {
            xPos = chunkIndex * chunkSize + chunkIndex * gapBetweenChunks;
        }
        else
        {
            xPos = activeChunks[activeChunks.Count - 1].transform.position.x + lastChunkSize + gapBetweenChunks;
        }

        newChunk.transform.position = new Vector3(xPos, 0f, 0f);
        activeChunks.Add(newChunk);
    
        // Remove the first chunk when there are too many active chunks
        if (activeChunks.Count > initialChunksCount + 1)
        {
            UnloadChunk(0);
        }
    }


    private void UnloadChunk(int chunkIndex)
    {
        Destroy(activeChunks[chunkIndex]);
        activeChunks.RemoveAt(chunkIndex);
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