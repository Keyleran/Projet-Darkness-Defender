// --------------------------------------------------
// Project: Darkness Defender
// Script: NetworkScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------
// 
// Script: Network
// 
// --------------------------------------------------
public class NetworkScript : MonoBehaviour
{
    private const string typeName = "UniqueGameName";
    private const string gameName = "Darkness Defender";

    [SerializeField]
    private PlayerManagerScript _playerAccess;

    [SerializeField]
    private bool _isServer = true;
    public bool IsServer
    {
        get { return _isServer; }
        set { _isServer = value; }
    }


    [SerializeField]
    private int _nbPlayer = 0;
    public int NbPlayer
    {
        get { return _nbPlayer; }
        set { _nbPlayer = value; }
    }

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;

        if (IsServer)
        {
            Network.InitializeSecurity();
            Network.InitializeServer(2, 6600, true);
            MasterServer.RegisterHost(typeName, gameName);
        }
        else
        {
            Network.Connect("127.0.0.1", 6600);
        }
    }

    void OnServerInitialized()
    {
        _playerAccess.SpawnPlayer();
    }

    void OnConnectedToServer()
    {
        _playerAccess.SpawnPlayer();
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        print("Player Connected");
    }

}
