// --------------------------------------------------
// Project: Darkness Defender
// Script: EnemyPoolScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// --------------------------------------------------
// 
// Script de gestion, gère les pools des ennemies
// 
// --------------------------------------------------
public class EnemyPoolScript : MonoBehaviour {

    private List<int> idEnemiesUnset = new List<int>();
    private int index = 0;

    [SerializeField]
    EnemiesScript[] _enemies;

    public int countEnemiesUse = 0; // Nombre d'enemis libérés par la pool

    // Renvoi une tour à chaque appel
    public EnemiesScript GetEnemy()
    {
        countEnemiesUse++;
        if ((idEnemiesUnset.Count != 0) || (index < _enemies.Length))
        {
            EnemiesScript enemy = null;
            if (idEnemiesUnset.Count != 0)
            {
                int id = idEnemiesUnset[0];
                idEnemiesUnset.Remove(id);
                enemy = _enemies[id];
            }
            else if (index < _enemies.Length)
            {
                enemy = _enemies[index];
                enemy.id = index;
                index++;
            }
            return enemy;
        }

        return null;
    }

    // ----------
    //
    // Fonction retour de l'unité enemie à la pool
    //
    // ----------
    public void ReturnEnemy(EnemiesScript enemy)
    {
        countEnemiesUse--;

        enemy.Transform.position = this.transform.position; // Replace le projectile dans la pool
        idEnemiesUnset.Add(enemy.id); // Retire l'objet de la liste des projectiles utilisés

        enemy.gameObject.SetActive(false); // Désactive le gameobject
    }
}
