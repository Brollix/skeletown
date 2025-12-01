using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    [Header("Tilemap & Tiles")]
    public Tilemap tilemap;
    public TileBase[] closedTiles;
    public TileBase[] openTiles;

    [Header("Door Settings")]
    public Vector2Int topLeft; // Top-left corner in tile coordinates
    public int width = 2;
    public int height = 3;
    public int floorNumber;

    [Header("State")]
    public bool isOpen = false;

    private TilemapCollider2D tilemapCollider;

    void Awake()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is missing on Door!", this);
            return;
        }

        tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();

        // Force draw the current state immediately
        ApplyTiles();
        ApplyCollision();
    }

    void Start()
    {

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterDoor(floorNumber, this);
        }
        else
        {
            Debug.LogError("Door.Start: GameManager instance not found! Door won't register.", this);
        }
        Debug.Log($"[DEBUG] Door.Start registering floor {floorNumber} - isOpen={isOpen} - tilemapAssigned={(tilemap != null)}", this);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;   // ← FORCE STATE UPDATE, NO EARLY RETURN

        ApplyTiles();
        ApplyCollision();
    }

    public void ToggleDoor()
    {
        SetOpen(!isOpen);
    }

    private void ApplyTiles()
    {
        if (tilemap == null) return;

        TileBase[] tilesToUse = isOpen ? openTiles : closedTiles;

        tilemap.ClearAllTiles();

        int index = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TileBase tile = (tilesToUse != null && index < tilesToUse.Length) ? tilesToUse[index] : null;
                tilemap.SetTile(new Vector3Int(topLeft.x + x, topLeft.y - y, 0), tile);
                index++;
            }
        }
    }

    private void ApplyCollision()
    {
        if (tilemapCollider != null)
            tilemapCollider.enabled = !isOpen;
    }
}
