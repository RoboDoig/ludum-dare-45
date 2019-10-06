using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New World Tile", menuName = "Tiles/World Tile")]
public class WorldTile : Tile
{
    // Hierarchy for later sublassing? //

    // All tiles
    public string type = "World tile";
    public string description = "A tile in the game";
    public bool openForPlacement = true;

    // Actionable tiles
    public int cost;

    // Plant tiles
    public WorldTile turnsInto;
    public WorldTile degradesInto;
    public int startingWater = 10;
    public int waterToTransform = 20;
    public int waterToDegrade = 0;
    public int waterDrain = 0;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    public void DoSomeAction()
    {
        Debug.Log("Some action");
    }
}
