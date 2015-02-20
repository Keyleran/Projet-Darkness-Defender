﻿// --------------------------------------------------
// Project: Darkness Defender
// Script: SoldierScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Unit 1 - Soldier
// 
// --------------------------------------------------
public class SoldierScript : MonoBehaviour 
{
    [SerializeField]
    public int health;

    [SerializeField]
    EnemyPoolScript poolRappel;

    IEnumerator OnTriggerEnter(Collider _EnCol)
    {
        if (_EnCol.tag == "Projectile")
        {
            yield return new WaitForFixedUpdate();
            AmmoScript projectile = (AmmoScript) _EnCol.gameObject.GetComponent("AmmoScript");
            health -= projectile.damage;
        }

        if (health == 0)
            killEnemy();
    }

    void killEnemy()
    {
        EnemiesScript enemy = (EnemiesScript)this.gameObject.GetComponent("EnemiesScript");
        poolRappel.ReturnEnemy(enemy);
    }
}