// --------------------------------------------------
// Project: Darkness Defender
// Script: LoadGame.cs
// Author: Théo Couvert 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour 
{

    private const string typeName = "DarknessDefender";
    private string gameName = "Darkness Defender";


    [SerializeField]
    Data_keeper _data;

    public void StartSolo()
    {
        _data.gameName = "Solo";
        _data.gameMode = "Multi";
        _data.isServer = true;
        Application.LoadLevel("New_Map");
    }	
}
 