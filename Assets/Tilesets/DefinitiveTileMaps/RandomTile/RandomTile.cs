using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Random Tile", menuName = "2D/Tiles/Random Tile")]
public class RandomTile : TileBase
{
    [Tooltip("Assign all possible tile sprites here")]
    public Sprite[] sprites;

    [Tooltip("Optional weights for each sprite (same length as sprites)")]
    public float[] weights;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if (sprites == null || sprites.Length == 0)
            return;

        float total = 0f;
        for (int i = 0; i < sprites.Length; i++)
        {
            float w = (weights != null && i < weights.Length) ? weights[i] : 1f;
            total += w;
        }

        float pick = Random.value * total;
        for (int i = 0; i < sprites.Length; i++)
        {
            float w = (weights != null && i < weights.Length) ? weights[i] : 1f;
            if (pick < w)
            {
                tileData.sprite = sprites[i];
                break;
            }
            pick -= w;
        }

        tileData.colliderType = Tile.ColliderType.None;
    }
}