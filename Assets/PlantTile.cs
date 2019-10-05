using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Tile", menuName = "Tiles/Plant Tile")]
public class PlantTile : WorldTile
{

    public PlantTile growsInto;
    public int baseGrowthTurns;

}
