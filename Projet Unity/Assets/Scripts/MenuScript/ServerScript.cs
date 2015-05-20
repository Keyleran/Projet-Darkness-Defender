// --------------------------------------------------
// Project: Darkness Defender
// Script: ServerScript.cs
// Author: Théo Couvert 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Net;

public class ServerScript : MonoBehaviour
{
	private const string typeName = "DarknessDefender";
	private string gameName = "Partie de test";
	
	private string gameType = "";
	private string difficulty = "";
	
	private bool isRefreshing = false;
	private HostData[] hostList;
	
	[SerializeField]
	private PlayerManagerScript _playerAccess;
	
	[SerializeField]
	GameObject _parent;
	
	[SerializeField]
	GameObject _destination;
	
	[SerializeField]
	GameObject _name;
	
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
					{
						Debug.Log("ça passe ici");
						JoinServer(hostList[i]);
					}
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
		Network.InitializeServer (2, 6500, !Network.HavePublicAddress());
		string name = gameName + " / " + gameType + " / " + difficulty;
		MasterServer.RegisterHost (typeName, name);
		_destination.gameObject.SetActive (true);
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");

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
	}
	
	public void SetName(string name)
	{
		gameName = name;
		StartServer();
	}
	
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
		_playerAccess.SpawnPlayer();
		_destination.gameObject.SetActive (true);
		this.gameObject.SetActive(false);
		Debug.Log("ça passe ici aussi");
	}
}
