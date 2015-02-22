// --------------------------------------------------
// Project: Darkness Defender
// Script: PlayerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------
// 
// Script: Player Script
// 
// --------------------------------------------------
public class PlayerScript : MonoBehaviour 
{
    public int playerId = 0;

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public  float InitialHealth;
    private float health;
    private float AncientHealth;

    [SerializeField]
    Scrollbar _healthBar;

    [SerializeField]
    Text _health;

    [SerializeField]
    Text _message;


    void Start()
    {
        health = InitialHealth;
        AncientHealth = InitialHealth;
    }
    void FixedUpdate()
    {
        if(Health != AncientHealth)
        {
            AncientHealth = Health;
            _healthBar.size = Health / InitialHealth;
            _health.text = Health.ToString() + " / " + InitialHealth.ToString();

            if (Health == 0)
            {
                // GameOver
                _message.text = "Game Over";
            }
        }
    }
}
