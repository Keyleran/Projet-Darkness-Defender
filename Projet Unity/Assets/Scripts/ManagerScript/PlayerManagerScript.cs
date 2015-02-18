// --------------------------------------------------
// Project: Darkness Defender
// Script: PlayerManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Script: Player Manager
// 
// --------------------------------------------------
public class PlayerManagerScript : MonoBehaviour 
{
    [SerializeField]
    PlayerScript[] _players;

    [SerializeField]
    Transform _spawnPlayer;

    private int index = 0;

    // Renvoi une tour à chaque appel
    public PlayerScript SpawnPlayer()
    {
        _players[index].Transform.position = _spawnPlayer.position;
        _players[index].playerId = index;
        index++;

        _players[index].gameObject.SetActive(true);
        return _players[index];
    }
	
}
