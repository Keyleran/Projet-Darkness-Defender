// --------------------------------------------------
// Project: Darkness Defender
// Script: MainManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// --------------------------------------------------
// 
// Script Manager: Gére le jeu
// 
// --------------------------------------------------
public class MainManagerScript : MonoBehaviour 
{
    [SerializeField]
    EnemyManager _enemyCreator;

    [SerializeField]
    Text _message;

    private bool levelStart = false;
    void FixedUpdate()
    {
        if ((levelStart == false) && (Input.GetKeyDown(KeyCode.G)))
        {
            levelStart = true;
            _message.text = "";
            StartCoroutine(_enemyCreator.LaunchGame());
        }
    }
}
