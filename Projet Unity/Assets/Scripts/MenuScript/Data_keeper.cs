// --------------------------------------------------
// Project: Darkness Defender
// Script: Data_keeper.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

public class Data_keeper : MonoBehaviour
{
    public string gameType = "";
    public string gameName = "";
    public string gameMode = "Solo";
    public string difficulty = "";
    public bool isServer     = false;
    public int Nb_Player     = 0;

    public HostData hostData;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetSolo()
    {
        gameMode = "Solo";
		Application.LoadLevel ("New_Map");
    }

    public void SetMulti()
    {
        gameMode = "Multi";
    }
}
