using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{

    public GameObject titleScreen;

    public void GameStart()
    {
        titleScreen.SetActive(false);
    }

}
