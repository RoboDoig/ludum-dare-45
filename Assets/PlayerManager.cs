using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{

    public Tilemap gameTiles;
    public Tilemap selectorTiles;

    public WorldTile[] availableTiles;
    public WorldTile[,] worldTiles;

    public HUD hud;

    // Start is called before the first frame update
    void Start()
    {
        int tileCount = 0;
        foreach (Vector3Int pos in gameTiles.cellBounds.allPositionsWithin)
        {
            tileCount++;
            Debug.Log(pos);
        }
        Debug.Log(tileCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Get clicked location
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Get tile at that point
                Vector3Int selectedCell = SelectedCell(point);
                WorldTile tile = GetTileAtPoint(selectedCell);
                Debug.Log(tile);

                // Change to a different tile
                gameTiles.SetTile(selectedCell, availableTiles[2]);

                // Make change to UI
                hud.TileSelected(tile);
            }
        }
    }

    public Vector3Int SelectedCell(Vector3 point)
    {
        return gameTiles.WorldToCell(point);
    }

    public WorldTile GetTileAtPoint(Vector3Int selectedCell)
    {
        return (WorldTile)gameTiles.GetTile(selectedCell);
    }
}
