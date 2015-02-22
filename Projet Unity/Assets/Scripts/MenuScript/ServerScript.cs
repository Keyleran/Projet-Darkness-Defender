// --------------------------------------------------
// Project: Darkness Defender
// Script: LoadGame.cs
// Author: Théo Couvert 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

public class ServerScript : MonoBehaviour
{
	private const string typeName = "DarknessDefender";
	private const string gameName = "Room de Test";

	private string gameType;
	private string difficulty = null;
	
	private bool isRefreshing = false;
	private HostData[] hostList;

	[SerializeField]
	GameObject _parent;

	[SerializeField]
	GameObject _destination;
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(70, 100, 150, 80), "Démarrer un serveur"))
				StartServer();
			
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
		Network.InitializeServer(2, 6500, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName, gameType + difficulty);
		_destination.gameObject.SetActive (true);
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
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
		gameType = gameMode;
		Debug.Log(gameType);
	}

	public void Difficulty(string chosenDifficulty)
	{
		difficulty = chosenDifficulty;
		Debug.Log(difficulty);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
}
