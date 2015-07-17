// --------------------------------------------------
// Project: Darkness Defender
// Script: ServerScript.cs
// Author: Théo Couvert 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

public class SetName : MonoBehaviour {

	private string gameName = "";

	[SerializeField]
	GameObject _parent;

	void OnGUI() {

		gameName = GUI.TextField(new Rect(200, 180, 80, 50), gameName, 25);

		GUI.backgroundColor = Color.black;

		if (GUI.Button(new Rect(250, 250, 80, 50), "Valider"))
		{
			this.gameObject.SetActive(false);
			_parent.SetActive(true);
			_parent.SendMessage("SetName", gameName);
		}
		if (GUI.Button(new Rect(500, 500, 120, 50), "Retour"))
		{
			this.gameObject.SetActive(false);
			_parent.SetActive(true);
		}
	}

}
