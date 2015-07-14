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
	
	private bool isRefreshing = false;
	private HostData[] hostList;

	[SerializeField]
	GameObject _parent;

    [SerializeField]
    GameObject _name;

    [SerializeField]
    Data_keeper _data;

	[SerializeField]
	public GUISkin Skin_Button;

	[SerializeField]
	public GUISkin Skin_Server;

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			GUI.skin = Skin_Button;

			if (GUI.Button(new Rect(70, 100, 160, 100), "Démarrer un serveur"))
			{
				_name.SetActive(true);
				this.gameObject.SetActive(false);
			}
			
			if (GUI.Button(new Rect(70, 200, 160, 100), "Rafraichir"))
				RefreshHostList();

			GUI.skin = Skin_Server;

			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(250, 100 + (110 * i), 600, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
				}
			}

			if (GUI.Button(new Rect(500, 460, 100, 40), "Retour"))
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
		Application.LoadLevel ("New_Map");
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
		Application.LoadLevel ("New_Map");
		this.gameObject.SetActive(false);
	} 
}
