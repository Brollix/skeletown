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
    public Vector2Int topLeft;
    public int width = 2;
    public int height = 3;
    public int floorNumber;

    [Header("Options")]
    public bool isOpen = false;
    public bool showPreviewInEditor = true;

    private Vector3Int[] positions;

    // ================= EDITOR ONLY =================

    void OnValidate()
    {
#if UNITY_EDITOR
        if (tilemap == null) return;

        GeneratePositions();

        if (showPreviewInEditor)
            ApplyEditorPreview();
        else
            tilemap.ClearAllEditorPreviewTiles();
#endif
    }

#if UNITY_EDITOR
    private void ApplyEditorPreview()
    {
        tilemap.ClearAllEditorPreviewTiles();

        if (positions == null) return;

        TileBase[] arr = isOpen ? openTiles : closedTiles;
        if (arr == null || arr.Length == 0) return;

        for (int i = 0; i < positions.Length; i++)
        {
            tilemap.SetEditorPreviewTile(positions[i], arr[i]);
        }
    }
#endif

    // ================= RUNTIME =================

    void Awake()
    {
        if (tilemap == null)
        {
            Debug.LogError("Door missing Tilemap!", this);
            return;
        }

        GeneratePositions();
    }

    void Start()
    {
        StartCoroutine(RegisterWhenReady());

        // Apply actual gameplay tiles (not preview)
        ApplyTiles();
        ApplyCollision();
    }

    private IEnumerator RegisterWhenReady()
    {
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.RegisterDoor(floorNumber, this);
    }

    private void GeneratePositions()
    {
        positions = new Vector3Int[width * height];
        int index = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                positions[index++] = new Vector3Int(topLeft.x + x, topLeft.y - y, 0);
            }
        }
    }

    public void SetOpen(bool open)
    {
        isOpen = open;

        ApplyTiles();
        ApplyCollision();
    }

    public void ToggleDoor()
    {
        SetOpen(!isOpen);
    }

    private void ApplyTiles()
{
    if (positions == null) return;

    TileBase[] arr = isOpen ? openTiles : closedTiles;

    for (int i = 0; i < positions.Length; i++)
    {
        TileBase source = arr[i];

        // clear tile if null
        if (source == null)
        {
            tilemap.SetTile(positions[i], null);
            continue;
        }

        // ---- FORCE TILE UPDATE ----
        // Create a new Tile instance (required so Unity refreshes the sprite)
        Tile newTile = ScriptableObject.CreateInstance<Tile>();

        if (source is Tile srcTile)
        {
            newTile.sprite       = srcTile.sprite;
            newTile.color        = srcTile.color;
            newTile.transform    = srcTile.transform;
            newTile.gameObject   = srcTile.gameObject;
            newTile.flags        = TileFlags.None;
            newTile.colliderType = Tile.ColliderType.None; // collider controlled elsewhere
        }

        tilemap.SetTile(positions[i], newTile);
    }
}



    private void ApplyCollision()
    {
        TilemapCollider2D col = tilemap.GetComponent<TilemapCollider2D>();
        if (col != null)
            col.enabled = !isOpen;

        CompositeCollider2D comp = tilemap.GetComponent<CompositeCollider2D>();
        if (comp != null)
            comp.enabled = !isOpen;
    }
}
