// --------------------------------------------------
// Project: Darkness Defender
// Script: LoadGame.cs
// Author: Théo Couvert 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

	[SerializeField]
	string _character;

	void FixedUpdate()
	{
		Application.LoadLevel ("New_Map");
		print(_character);
	}

}
