// --------------------------------------------------
// Project: Darkness Defender
// Script: ServerScript.cs
// Author: Théo Couvert 3A 3DJV
//         Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Net;

public class ServerScript : MonoBehaviour
{
	private const string typeName = "DarknessDefender";
    private string gameName = "Darkness Defender";

	private string gameType = "";
	private string difficulty = "";
	
	private bool isRefreshing = false;
	private HostData[] hostList;

	[SerializeField]
	GameObject _parent;

	[SerializeField]
	GameObject _destination;

    [SerializeField]
    GameObject _name;

    [SerializeField]
    Data_keeper _data;
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(70, 100, 150, 80), "Démarrer un serveur"))
			{
				_name.SetActive(true);
				this.gameObject.SetActive(false);
			}
			
			if (GUI.Button(new Rect(70, 200, 150, 80), "Rafraichir"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(250, 100 + (110 * i), 600, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
				}
			}

			if (GUI.Button(new Rect(500, 500, 120, 50), "Retour"))
			{
				this.gameObject.SetActive(false);
				_parent.SetActive(true);
			}
		}
	}

    
	private void StartServer()
	{
		//string name = gameName + " / " + gameType + " / " + difficulty;

        _data.isServer = true;
        _destination.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
	}	
	 
	void Update()
	{
		if (isRefreshing && MasterServer.PollHostList().Length > 0)
		{
			isRefreshing = false;
			hostList = MasterServer.PollHostList();
		}
	}
	
	private void RefreshHostList()
	{
		if (!isRefreshing)
		{
			isRefreshing = true;
			MasterServer.RequestHostList(typeName);
		}
	}

	public void TypeGame(string gameMode)
	{
        _data.gameType = gameMode;
        gameType = gameMode;
		Debug.Log(gameType);
	}

	public void Difficulty(string chosenDifficulty)
	{
        _data.difficulty = chosenDifficulty;
        difficulty = chosenDifficulty;
	}

	public void SetName(string name)
	{
        _data.gameName = name;
        gameName = name;
        _data.gameMode = "Multi";
		StartServer();
	}

	public void JoinServer(HostData hostData)
	{
        _data.hostData = hostData;
        _data.isServer = false;
        _data.gameMode = "Multi";
        _destination.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
	} 
}
