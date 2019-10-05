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

    public void TileSelected(WorldTile tile, WorldTileData tileData)
    {
        selectedImage.sprite = tile.sprite;
        selectedDescription.text = tile.description;
        selectedTurnsAlive.text = "Turns alive: " + tileData.turnsAlive.ToString();
    }
}
