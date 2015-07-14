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
	PlayerScript[] _players;

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
			_players[IdOpeningPlayer].IncreaseHealth(1);
		if (GUI.Button (new Rect (70, 200, 150, 80), "Acheter : Armure de niveau 3"))
			_players[IdOpeningPlayer].IncreaseHealth(2);
		if (GUI.Button (new Rect (70, 350, 150, 80), "Acheter : Armure de niveau 4"))
			_players[IdOpeningPlayer].IncreaseHealth(3);
		if (GUI.Button (new Rect (70, 500, 150, 80), "Acheter : Armure de niveau 5"))
			_players[IdOpeningPlayer].IncreaseHealth(4);
		
		if (GUI.Button (new Rect (170, 50, 150, 80), "Améliorer : Tours au niveau 2"))
			_tower.UpgradeTower(1);
		if (GUI.Button (new Rect (170, 200, 150, 80), "Améliorer : Tours au niveau 3"))
			_tower.UpgradeTower(2);
		if (GUI.Button (new Rect (170, 350, 150, 80), "Améliorer : Tours au niveau 4"))
			_tower.UpgradeTower(3);
		if (GUI.Button (new Rect (170, 500, 150, 80), "Améliorer : Tours au niveau 5"))
			_tower.UpgradeTower(4);
	}

}
