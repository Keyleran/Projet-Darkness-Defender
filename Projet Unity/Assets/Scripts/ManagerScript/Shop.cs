// --------------------------------------------------
// Project: Darkness Defender
// Script: ServerScript.cs
// Author: Théo Couvert 3A 3DJV
// --------------------------------------------------

using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {

	int IdOpeningPlayer = 0;
	
	PlayerScript _player;

	[SerializeField]
	public GUISkin Skin_Button;

	void start()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		_player = (PlayerScript)player.GetComponent ("PlayerScript");
	}
	
	void OnGUI()
	{
		GUI.backgroundColor = Color.gray;

		GUI.skin = Skin_Button;

		Cursor.lockState = CursorLockMode.None;

		if (_player.LevelArmor < 1) {
			if (GUI.Button (new Rect (10, 50, 120, 50), "Armure de niveau 2"))
				_player.IncreaseHealth (1);
		}
		else
		{
			GUI.TextArea(new Rect (70, 50, 120, 50), "Débloqué");
		}
		if (_player.LevelArmor < 2)
		{
			if (GUI.Button (new Rect (70, 50, 120, 50), "Armure de niveau 3"))
				_player.IncreaseHealth (2);
		}
		else
		{
			GUI.TextArea(new Rect (70, 50, 120, 50), "Débloqué");
		}
		if (_player.LevelArmor < 3)
		{
			if (GUI.Button (new Rect (70, 350, 120, 50), "Armure de niveau 4"))
				_player.IncreaseHealth (3);
		}
		else
		{
			GUI.TextArea(new Rect (70, 50, 120, 50), "Débloqué");
		}
		if (_player.LevelArmor < 4)
		{
			if (GUI.Button (new Rect (70, 500, 120, 50), "Armure de niveau 5"))
				_player.IncreaseHealth (4);
		}
		else
		{
			GUI.TextArea(new Rect (70, 50, 120, 50), "Débloqué");
		}
	}

}
