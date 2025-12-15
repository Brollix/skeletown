using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("Tilemap & Tiles")]
    public Tilemap tilemap;
    public TileBase[] closedTiles;
    public TileBase[] openTiles;

    [Header("Door Size & Position")]
    public Vector2Int topLeft; // Top-left corner of the door in tile coordinates
    public int width = 2;
    public int height = 3;
    public int floorNumber;

    [Header("Options")]
    public bool isOpen = false;

    private TilemapCollider2D tilemapCollider;


    // Initializes tilemap and collider references.
    void Awake()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is missing on Door!", this);
            return;
        }

        tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();

        ApplyTiles();
        ApplyCollision();
    }


    // Auto-detects floor ID and registers with GameManager.
    void Start()
    {
        // Fix: Auto-detect floor ID from parent if available, overriding manual setting
        FloorID id = GetComponentInParent<FloorID>();
        if (id != null)
        {
            if (floorNumber != id.floorNumber)
            {
                Debug.Log($"[Door] Correcting Floor ID from {floorNumber} to {id.floorNumber} based on parent FloorID.");
                floorNumber = id.floorNumber;
            }
        }

        StartCoroutine(RegisterWhenReady());
    }


    // Waits for GameManager to exist before registering.
    private IEnumerator RegisterWhenReady()
    {
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.RegisterDoor(floorNumber, this);
    }


    // Opens or closes the door and updates visuals/collision.
    public void SetOpen(bool open)
    {
        if (isOpen == open)
            return;

        if (open)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }

        ApplyTiles();
        ApplyCollision();
    }


    // Flips the current state of the door.
    public void ToggleDoor()
    {
        SetOpen(!isOpen);
    }


    // Updates the Tilemap to show open or closed tiles.
    private void ApplyTiles()
    {
        if (tilemap == null)
            return;

        TileBase[] tilesToUse;
        if (isOpen)
        {
            tilesToUse = openTiles;
        }
        else
        {
            tilesToUse = closedTiles;
        }

        // Clear previous tiles
        tilemap.ClearAllTiles();

        // Fill door area based on width and height starting from top-left
        int index = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TileBase tile = null;
                if (tilesToUse != null && index < tilesToUse.Length)
                {
                    tile = tilesToUse[index];
                }

                Vector3Int position = new Vector3Int(topLeft.x + x, topLeft.y - y, 0);
                tilemap.SetTile(position, tile);

                index++;
            }
        }
    }


    // Enables or disables the collider based on state.
    private void ApplyCollision()
    {
        if (tilemapCollider != null)
        {
            if (isOpen)
            {
                tilemapCollider.enabled = false; // Door open → collider off
            }
            else
            {
                tilemapCollider.enabled = true;  // Door closed → collider on
            }
        }
    }
}