using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New World Tile", menuName = "Tiles/World Tile")]
public class WorldTile : Tile
{
    public int cost;
    public string actionText;
    public WorldTile turnsInto;
    public int baseTurnsToTransform = 1;
    public int startingWater = 10;

    public string type;
    public string description = "A tile in the game";

    private int turnsAlive = 0;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    public void DoSomeAction()
    {
        Debug.Log(actionText);
    }
}
