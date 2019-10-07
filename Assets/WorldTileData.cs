using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileData
{
    public Vector3Int position;
    public int turnsAlive;
    private int currentWater;
    private bool openForPlacement;
    private string type;

    public WorldTileData(Vector3Int _position, WorldTile _tile)
    {
        position = _position;
        turnsAlive = 0;
        currentWater = _tile.startingWater + Random.Range(-5, 5); //TODO hard-coded random int
        openForPlacement = _tile.openForPlacement;
        type = _tile.type;
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
        openForPlacement = tile.openForPlacement;
        type = tile.type;
    }

    public void AddWater(int amount)
    {
        if (amount > 0)
        {
            currentWater += amount;
        }
    }

    // Drain water from this tile
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

    public string GetTileType()
    {
        return type;
    }

    public bool OpenForPlacement()
    {
        return openForPlacement;
    }

    public void SetOpenForPlacement(bool state)
    {
        openForPlacement = state;
    }
}
