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

	[SerializeField]
	public GUISkin Skin_Button;

	void OnGUI() {

		gameName = GUI.TextField(new Rect(300, 180, 170, 40), gameName, 25);

		GUI.skin = Skin_Button;

		if (GUI.Button(new Rect(300, 230, 80, 40), "Valider"))
		{
			this.gameObject.SetActive(false);
			_parent.SetActive(true);
			_parent.SendMessage("SetName", gameName);
		}
		if (GUI.Button(new Rect(390, 230, 80, 40), "Retour"))
		{
			this.gameObject.SetActive(false);
			_parent.SetActive(true);
		}
	}

}
