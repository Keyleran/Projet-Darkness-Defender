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

    public int countEnemiesUse = 0;

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

        // Si on a atteint le nombre max de barricade
        return null;
    }

    public void ReturnEnemy(EnemiesScript enemy)
    {
        countEnemiesUse--;
        enemy.Transform.position = this.transform.position;
        idEnemiesUnset.Add(enemy.id);
        enemy.gameObject.SetActive(false);
    }
}
