using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainControl : MonoBehaviour
{
    public Tilemap gameTiles;
    public Tilemap selectorTiles;

    // Start is called before the first frame update
    void Start()
    {
        int tileCount = 0;
        foreach (Vector3Int pos in gameTiles.cellBounds.allPositionsWithin)
        {
            //Debug.Log(pos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get camera to world point
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // what is index of this cell
            Vector3Int selectedCell = gameTiles.WorldToCell(point);
            Debug.Log(selectedCell);

            // get tile object occupied by this cell
            Debug.Log(gameTiles.GetTile(selectedCell));

            WorldTile tile = (WorldTile) gameTiles.GetTile(selectedCell);
            Debug.Log(tile.cost);

            tile.DoSomeAction();
        }
    }
}
