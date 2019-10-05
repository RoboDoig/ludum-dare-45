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

    public WorldTileSelector defaultSelector;
    private Vector3Int currentSelected;

    public HUD hud;

    public int actionPointsLeft = 10;

    private int sizeXTiles;
    private int xMin;
    private int sizeYTiles;
    private int yMin;
    private int totalTiles;

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int dimensions = gameTiles.size;
        sizeXTiles = dimensions[0];
        xMin = gameTiles.cellBounds.xMin;
        sizeYTiles = dimensions[1];
        yMin = gameTiles.cellBounds.yMin;
        totalTiles = sizeXTiles * sizeYTiles;

        worldTiles = new WorldTile[sizeXTiles, sizeYTiles];

        foreach (Vector3Int pos in gameTiles.cellBounds.allPositionsWithin)
        {
            int x = pos[0] - xMin;
            int y = pos[1] - yMin;

            worldTiles[x, y] = GetTileAtPoint(pos);
        }
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
                //gameTiles.SetTile(selectedCell, availableTiles[2]);

                // Make change to UI
                hud.TileSelected(tile);

                // Draw selector
                selectorTiles.SetTile(currentSelected, null);
                selectorTiles.SetTile(selectedCell, defaultSelector);
                currentSelected = selectedCell;
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

    public void EndTurn()
    {
        Debug.Log("End turn.");
    }

    // TODO - generalise to all placement?
    public void PlacePlant1(WorldTile tile)
    {
        gameTiles.SetTile(currentSelected, tile);
    }
}
