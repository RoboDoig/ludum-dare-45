using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileData
{
    public Vector3Int position;
    public int turnsAlive;
    public int currentWater;

    public WorldTileData(Vector3Int _position, WorldTile _tile)
    {
        position = _position;
        turnsAlive = 0;
        currentWater = _tile.startingWater + Random.Range(-5, 5); //TODO hard-coded random int
    }

    public void AdvanceTurn()
    {
        turnsAlive++;
        if (currentWater > 0)
        {
            currentWater--;
        }
    }

    public void UpdateTile(WorldTile tile)
    {
        turnsAlive = 0;
    }
}
