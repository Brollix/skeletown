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

    private TilemapCollider2D tilemapCollider;

    void Awake()
    {
        if (tilemap == null)
        {
            return;
        }

        tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();

        ApplyTiles();
        ApplyCollision();
    }

    void Start()
    {
        FloorID id = GetComponentInParent<FloorID>();
        if (id != null)
        {
            if (floorNumber != id.floorNumber)
            {
                floorNumber = id.floorNumber;
            }
        }

        StartCoroutine(RegisterWhenReady());
    }

    private IEnumerator RegisterWhenReady()
    {
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.RegisterDoor(floorNumber, this);
    }

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

    public void ToggleDoor()
    {
        SetOpen(!isOpen);
    }

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

        tilemap.ClearAllTiles();

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

    private void ApplyCollision()
    {
        if (tilemapCollider != null)
        {
            if (isOpen)
            {
                tilemapCollider.enabled = false;
            }
            else
            {
                tilemapCollider.enabled = true;
            }
        }
    }
}