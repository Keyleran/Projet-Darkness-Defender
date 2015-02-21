using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

	[SerializeField]
	string _character;

	void FixedUpdate()
	{
		Application.LoadLevel ("Gameplay");
		print(_character);
	}

}
