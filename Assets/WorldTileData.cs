using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileData
{
    public Vector3Int position;
    public int baseTurnsToTransform;
    public int turnsAlive;
    public int currentWater;

    public WorldTileData(Vector3Int _position, int _baseTurnsToTransform)
    {
        position = _position;
        baseTurnsToTransform = _baseTurnsToTransform;
        turnsAlive = 0;
    }

    public void AdvanceTurn()
    {
        turnsAlive++;
    }

    public void UpdateTile(int _baseTurnsToTransform)
    {
        baseTurnsToTransform = _baseTurnsToTransform;
        turnsAlive = 0;
    }
}
