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
    private Slider _healthBar;

    [SerializeField]
    private Text _health;

    [SerializeField]
    private Text _message;

	int LevelArmor = 0;

    void Start()
    {
        health = InitialHealth;
        AncientHealth = InitialHealth;

        GameObject Interface = GameObject.Find("Mask_health");
        _healthBar = (Slider)Interface.GetComponent("Slider");

        Interface = GameObject.Find("Health");
        _health = (Text)Interface.GetComponent("Text");

        Interface = GameObject.Find("Message");
        _message = (Text)Interface.GetComponent("Text");
    } 
    void FixedUpdate()
    {
        if(Health != AncientHealth)
        {
            AncientHealth = Health;
            _healthBar.value = 1 - (health / InitialHealth);
            _health.text = Health.ToString() + " / " + InitialHealth.ToString();

            if (Health == 0)
            {
                // GameOver
                StartCoroutine("death");
            }
        }
    }

	public void IncreaseHealth(int level)
	{
		InitialHealth = InitialHealth + (5*(level-LevelArmor));
		health = health + (5*(level-LevelArmor));
		LevelArmor = LevelArmor + level;
	}

    IEnumerator death()
    {
        _message.text = "Game Over";
        _message.fontSize = 30;
        yield return new WaitForSeconds(5);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Application.LoadLevel("Menu");
    }
}
