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


    //This method initializes tilemap and collider references and applies the initial door state to be closed.
    void Awake()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is missing on Door!", this); //This was a debug made to check that each room had a door.
            return;
        }

        tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();

        ApplyTiles();
        ApplyCollision();
    }


    //This method detects the correct floor number and registers the door with the GameManager. A fix was made that overrides the manual setting in the inspector in case there was a mistake in the editor.
    void Start()
    {
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


    //This method first waits until the GameManager exists, then registers a door. This is called above in start in order to register all the doors in the map.
    private IEnumerator RegisterWhenReady()
    {
        while (GameManager.Instance == null)
            yield return null;

        GameManager.Instance.RegisterDoor(floorNumber, this);
    }


    //Once all enemies die, doors should open, so this method makes sure to set isOpen to its corresponding value and then it updates the tiles and the collision of the door.
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


    //This is the method that toggles the state of the door between open and closed.
    public void ToggleDoor()
    {
        SetOpen(!isOpen);
    }


    //This method updates the door visuals depending on whether it's open or closed.
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


    //Here it enables or disables the door's collider based on the state it's in.
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