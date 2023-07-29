using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformGenerator : MonoBehaviour
{
    public Tilemap tilemap;   // The Tilemap where we'll draw our tiles
    public TileBase tile;     // The tile we'll use

    // Dimensions of the platform we'll generate
    public int width = 10;
    public int height = 1;

    // Position where we'll start generating the platform
    public Vector3Int startPosition = new Vector3Int(0, 0, 0);

    void Start()
    {
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(startPosition.x + x, startPosition.y + y, startPosition.z);
                tilemap.SetTile(position, tile);
            }
        }
    }
}