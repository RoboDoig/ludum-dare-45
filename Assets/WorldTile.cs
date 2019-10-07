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
    public int waterDrain = 0;
    public int waterLeak = 0;
    public int lifeTime = 0;

    // Actionable tiles
    public int cost;

    // Plant tiles
    public WorldTile turnsInto;
    public WorldTile degradesInto;
    public WorldTile diesInto;
    public int startingWater = 10;
    public int waterToTransform = 20;
    public int waterToDegrade = 0;
    public float spreadProbability = 0;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    public void DoSomeAction()
    {
        Debug.Log("Some action");
    }
}
