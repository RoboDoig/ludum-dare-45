using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public UnityEngine.UI.Image selectedImage;
    public Text selectedDescription;

    public void TileSelected(WorldTile tile)
    {
        selectedImage.sprite = tile.sprite;
        selectedDescription.text = tile.description;
    }
}
