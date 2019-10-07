using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    // Game params
    public int startingWater = 0;
    public float weedSpawnProbability = 0.1f;

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
    private int waterAvailable = 0;
    private bool placedWeedThisTurn = false;

    private int sizeXTiles;
    private int xMin;
    private int xMax;
    private int sizeYTiles;
    private int yMin;
    private int yMax;
    private int totalTiles;

    public AudioClip placeTileSuccess;
    public AudioClip endTurn;
    public AudioClip fail;
    public AudioClip waterSuccess;
    public AudioClip waterScoop;
    public AudioClip weedAlert;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1024, 768, false);

        // components
        audioSource = GetComponent<AudioSource>();

        // populate player parameters
        actionPointsLeft = baseActionPoints;
        waterAvailable = startingWater;

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
            worldTilesData[x, y] = new WorldTileData(new Vector3Int(pos[0], pos[1], pos[2]), tile);
        }

        // update HUD with starting values
        hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed, waterAvailable);
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

            // Growing / decaying / dying logic
            if (worldTilesData[x, y].WaterAmount() >= tile.waterToTransform)
            {
                if (tile.turnsInto)
                {
                    worldTilesData[x, y].SetOpenForPlacement(true);
                    PlaceTile(tile.turnsInto, cell);
                }
            }

            if (worldTilesData[x, y].WaterAmount() <= tile.waterToDegrade)
            {
                if (tile.degradesInto)
                {
                    worldTilesData[x, y].SetOpenForPlacement(true);
                    PlaceTile(tile.degradesInto, cell);
                }
            }

            if (tile.lifeTime > 0)
            {
                if (worldTilesData[x, y].turnsAlive > tile.lifeTime)
                {
                    if (tile.diesInto)
                    {
                        worldTilesData[x, y].SetOpenForPlacement(true);
                        PlaceTile(tile.diesInto, cell);
                    }
                }
            }

            // Water drain / leak logic
            // Get neighbors
            List<Vector2Int> indexNeighbors = GetIndexNeighbors(TilePointToIndex(cell));
            // Then drain / leak into each one
            foreach (Vector2Int neighbor in indexNeighbors)
            {
                // draining from
                int drainAmount = tile.waterDrain;
                int amountDrained = worldTilesData[neighbor[0], neighbor[1]].DrainWater(drainAmount);
                worldTilesData[x, y].AddWater(drainAmount);
            }
            // leaking to neighbors
            for (int l = 0; l < tile.waterLeak; l++)
            {
                Vector2Int neighbor = indexNeighbors[Random.Range(0, indexNeighbors.Count)];
                if (worldTilesData[x, y].WaterAmount() > worldTilesData[neighbor[0], neighbor[1]].WaterAmount())
                {
                    worldTilesData[neighbor[0], neighbor[1]].AddWater(1);
                    worldTilesData[x, y].DrainWater(1);
                }
            }

        }

        // Place random danger tiles - TODO, adjustment for how often, type
        if (weedSpawnProbability > Random.Range(0f, 1f))
        {
            PlaceTileRandomly(weedTiles[0]);
            audioSource.clip = weedAlert;
            audioSource.Play();
        }

        // Plant spreading

        // Update turns used
        turnsUsed++;
        actionPointsLeft = baseActionPoints;

        // Play audio
        audioSource.clip = endTurn;
        audioSource.Play();

        // Update HUD
        hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed, waterAvailable);
    }

    public bool PlaceTile(WorldTile tile, Vector3Int position)
    {
        Vector2Int dataIndex = TilePointToIndex(position);
        if (worldTilesData[dataIndex[0], dataIndex[1]].OpenForPlacement())
        {
            gameTiles.SetTile(position, tile);
            worldTilesData[dataIndex[0], dataIndex[1]].UpdateTile(tile);
            return true;
        }

        return false;
    }

    public void UserPlaceTile(WorldTile tile)
    {
        bool tilePlaced = ((actionPointsLeft >= tile.cost) && PlaceTile(tile, currentSelected));

        if (tilePlaced)
        {
            actionPointsLeft -= tile.cost;

            hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed, waterAvailable);

            audioSource.clip = placeTileSuccess;
            audioSource.Play();
        } else
        {
            audioSource.clip = fail;
            audioSource.Play();
        }
    }

    public void PlaceTileRandomly(WorldTile tile)
    {
        int xPos = Random.Range(xMin, xMax);
        int yPos = Random.Range(yMin, yMax);
        Vector3Int placePosition = new Vector3Int(xPos, yPos, 0);

        PlaceTile(tile, placePosition);
    }

    // TODO - hard coded water / AP amounts
    public void WaterSelected()
    {
        if (actionPointsLeft >= 1 && waterAvailable >= 10)
        {
            Vector2Int selectedIndex = TilePointToIndex(currentSelected);
            worldTilesData[selectedIndex[0], selectedIndex[1]].AddWater(10);
            waterAvailable -= 10;
            actionPointsLeft -= 1;

            audioSource.clip = waterSuccess;
        } else
        {
            audioSource.clip = fail;
        }
        audioSource.Play();

        hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed, waterAvailable);
    }

    public void ScoopWater()
    {
        if (actionPointsLeft >= 1)
        {
            Vector2Int selectedIndex = TilePointToIndex(currentSelected);
            if (worldTilesData[selectedIndex[0], selectedIndex[1]].GetTileType().Equals("water"))
            {
                int drainAmount = worldTilesData[selectedIndex[0], selectedIndex[1]].DrainWater(10);

                if (drainAmount > 0)
                {
                    waterAvailable += drainAmount;
                    actionPointsLeft -= 1;
                    audioSource.clip = waterScoop;
                } else
                {
                    audioSource.clip = fail;
                }
            }
        } else
        {
            audioSource.clip = fail;
        }

        audioSource.Play();

        hud.UpdateStatusIndicators(actionPointsLeft, turnsUsed, waterAvailable);
    }

    public List<Vector2Int> GetIndexNeighbors(Vector2Int cellIndex)
    {
        List<Vector2Int> outArray = new List<Vector2Int>();

        for (int x = cellIndex[0]-1; x < cellIndex[0]+2; x++)
        {
            for (int y = cellIndex[1]-1; y < cellIndex[1]+2; y++)
            {
                if (!(x == cellIndex[0] && y == cellIndex[1]) && (x>=0 && y>=0) && (x<sizeXTiles && y<sizeYTiles))
                {
                    outArray.Add(new Vector2Int(x, y));
                }
            }
        }

        return outArray;
    }
}
