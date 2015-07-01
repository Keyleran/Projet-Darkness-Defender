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
    private const string typeName = "DarknessDefender";
    private const string gameName = "Darkness Defender";
    private HostData[] hostList;
    private bool isRefreshingHostList = false;


    public GameObject playerPrefab;
    private Data_keeper _data;

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

    [SerializeField]
    TowerManagerScript towerScript;

    void Start()
    {
        GameObject Network_Data = GameObject.Find("Network_Data");
        _data = (Data_keeper) Network_Data.GetComponent("Data_keeper");

        if (_data.gameMode == "Multi")
            StartServer();
        else
            Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);


    }


    void Update()
    {
        if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            isRefreshingHostList = false;
            hostList = MasterServer.PollHostList();
        }
    }


    void StartServer()
    {
        Application.runInBackground = true;

        if (_data.isServer)
        {
            Network.InitializeServer(2, 25000, true);
            MasterServer.RegisterHost(typeName, gameName);
        }
        else
        {
            Network.Connect(_data.hostData);
        }
    }

    void OnServerInitialized()
    {
        SpawnPlayer();
    }

    void OnConnectedToServer()
    {
        SpawnPlayer();
        Debug.Log("Server Joined");
    }

    private void SpawnPlayer()
    {
        Network.Instantiate(playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }
}
