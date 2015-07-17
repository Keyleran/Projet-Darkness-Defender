// --------------------------------------------------
// Project: Darkness Defender
// Script: ServerScript.cs
// Author: Théo Couvert 3A 3DJV
// --------------------------------------------------

using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {

	int IdOpeningPlayer = 0;

	[SerializeField]
	PlayerScript _players;

	[SerializeField]
	ShooterScript _tower;

	void SetIdPlayer(int id)
	{
		IdOpeningPlayer = id;
	}

	void OnGUI()
	{
		GUI.backgroundColor = Color.black;
		if (GUI.Button (new Rect (70, 50, 150, 80), "Acheter : Armure de niveau 2"))
			_players.IncreaseHealth(1);
		if (GUI.Button (new Rect (70, 200, 150, 80), "Acheter : Armure de niveau 3"))
			_players.IncreaseHealth(2);
		if (GUI.Button (new Rect (70, 350, 150, 80), "Acheter : Armure de niveau 4"))
			_players.IncreaseHealth(3);
		if (GUI.Button (new Rect (70, 500, 150, 80), "Acheter : Armure de niveau 5"))
			_players.IncreaseHealth(4);
	}

}
