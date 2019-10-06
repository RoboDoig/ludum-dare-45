using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileData
{
    public Vector3Int position;
    public int turnsAlive;
    private int currentWater;

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
            //currentWater--;
        }
    }

    public void UpdateTile(WorldTile tile)
    {
        turnsAlive = 0;
    }

    public void AddWater(int amount)
    {
        if (amount > 0)
        {
            currentWater += amount;
        }
    }

    public int DrainWater(int amount)
    {
        if (amount > 0)
        {
            if (amount <= currentWater)
            {
                currentWater -= amount;
                return amount;
            }
            else
            {
                currentWater = 0;
                return currentWater;
            }
        }
        return 0;
    }

    public int WaterAmount()
    {
        return currentWater;
    }
}
