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
    public WorldTile[] weedTiles;

    public WorldTileData[,] worldTilesData;

    public WorldTileSelector defaultSelector;
    private Vector3Int currentSelected;

    public HUD hud;

    public int baseActionPoints = 10;
    private int actionPointsLeft;
    private int turnsUsed = 0;

    private int sizeXTiles;
    private int xMin;
    private int xMax;
    private int sizeYTiles;
    private int yMin;
    private int yMax;
    private int totalTiles;

    public AudioClip placeTileSuccess;
    public AudioClip endTurn;
    public AudioClip placeTileFail;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        // components
        audio = GetComponent<AudioSource>();

        // populate player parameters
        actionPointsLeft = baseActionPoints;

        // populate tile data
        Vector3Int dimensions = gameTiles.size;
        sizeXTiles = dimensions[0];
        xMin = gameTiles.cellBounds.xMin;
        xMax = gameTiles.cellBounds.xMax;
        sizeYTiles = dimensions[1];
        yMin = gameTiles.cellBounds.yMin;
        yMax = gameTiles.cellBounds.yMax;
        totalTiles = sizeXTiles * sizeYTiles;

        worldTilesData = new WorldTileData[sizeXTiles, sizeYTiles];

        foreach (Vector3Int pos in gameTiles.cellBounds.allPositionsWithin)
        {
            int x = pos[0] - xMin;
            int y = pos[1] - yMin;

            WorldTile tile = GetTileAtPoint(pos);
            worldTilesData[x, y] = new WorldTileData(new Vector3Int(pos[0], pos[1], pos[2]), tile.baseTurnsToTransform);
        }

        // update HUD with starting values
        hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed);
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
                Debug.Log(selectedCell);
                Debug.Log(TilePointToIndex(selectedCell));
                WorldTile tile = GetTileAtPoint(selectedCell);

                // Make change to UI
                hud.TileSelected(tile, GetTilePointData(selectedCell));

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

    public Vector2Int TilePointToIndex(Vector3Int selectedCell)
    {
        return new Vector2Int(selectedCell[0] - xMin, selectedCell[1] - yMin);
    }

    public WorldTileData GetTilePointData(Vector3Int selectedCell)
    {
        Vector2Int index = TilePointToIndex(selectedCell);
        return worldTilesData[index[0], index[1]];
    }

    public void EndTurn()
    {
        // Loop through all tiles in the game
        foreach (Vector3Int cell in gameTiles.cellBounds.allPositionsWithin)
        {
            // Base tile type
            WorldTile tile = GetTileAtPoint(cell);

            // Position in data matrix
            int x = cell[0] - xMin;
            int y = cell[1] - yMin;
            // Tile has been alive for one more turn
            worldTilesData[x, y].AdvanceTurn();

            // Growing / decaying logic
            if (worldTilesData[x, y].turnsAlive >= tile.baseTurnsToTransform)
            {
                if (tile.turnsInto)
                {
                    gameTiles.SetTile(cell, tile.turnsInto);
                    worldTilesData[x, y].UpdateTile(tile.baseTurnsToTransform);
                }
            }
        }

        // Place random danger tiles
        PlaceTileRandomly(weedTiles[0]);

        // Update turns used
        turnsUsed++;
        actionPointsLeft = baseActionPoints;

        // Play audio
        audio.clip = endTurn;
        audio.Play();

        // Update HUD
        hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed);
    }

    public void PlaceTile(WorldTile tile)
    {
        if (actionPointsLeft >= tile.cost)
        {
            gameTiles.SetTile(currentSelected, tile);
            Debug.Log(currentSelected);
            actionPointsLeft -= tile.cost;

            hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed);

            audio.clip = placeTileSuccess;
            audio.Play();
        } else
        {
            // play deny sound
            audio.clip = placeTileFail;
            audio.Play();
        }
    }

    public void PlaceTileRandomly(WorldTile tile)
    {
        int xPos = Random.Range(xMin, xMax);
        int yPos = Random.Range(yMin, yMax);
        Vector3Int placePosition = new Vector3Int(xPos, yPos, 0);
        Debug.Log(placePosition);

        gameTiles.SetTile(placePosition, tile);
        Vector2Int dataIndex = TilePointToIndex(placePosition);
        worldTilesData[dataIndex[0], dataIndex[1]].UpdateTile(tile.baseTurnsToTransform);
    }
}
