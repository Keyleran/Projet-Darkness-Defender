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
	private const string typeName = "UniqueGameName";
	private const string gameName = "RoomName";
	
	private bool isRefreshingHostList = false;
	private HostData[] hostList;

	[SerializeField]
	GameObject _parent;
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(70, 100, 150, 80), "Démarrer un serveur"))
				print ("Start");
			
			if (GUI.Button(new Rect(70, 200, 150, 80), "Rafraichir"))
				print ("Refresh");
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						print ("Connexion");
				}
			}
			if (GUI.Button(new Rect(500, 500, 120, 50), "Retour"))
			{
				this.gameObject.SetActive(false);
				_parent.SetActive(true);
			}
		}
	}
}
