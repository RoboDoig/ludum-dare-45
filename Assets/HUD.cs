using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public UnityEngine.UI.Image selectedImage;
    public Text selectedDescription;
    public Text selectedTurnsAlive;
    public Text selectedWaterLevel;

    public Text turnsUsedIndicator;
    public Text actionPointsIndicator;
    public Text waterIndicator;

    public void TileSelected(WorldTile tile, WorldTileData tileData)
    {
        selectedImage.sprite = tile.sprite;
        selectedDescription.text = tile.description;
        selectedTurnsAlive.text = "Turns alive: " + tileData.turnsAlive.ToString();
        selectedWaterLevel.text = "Water: " + tileData.WaterAmount().ToString();
    }

    public void UpdateStatusIndicators(int actionPoints, int turnsUsed, int waterAvailable)
    {
        turnsUsedIndicator.text = "Turns: " + turnsUsed.ToString();
        actionPointsIndicator.text = "AP: " + actionPoints.ToString();
        waterIndicator.text = "Water: " + waterAvailable.ToString();
    }
}
