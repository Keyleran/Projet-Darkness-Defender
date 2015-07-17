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

    [SerializeField]
    public EnemiesScript[] _enemies;

    [SerializeField]
    NetworkView _network;

    public int countEnemiesUse = 0; // Nombre d'enemis libérés par la pool

    void Start()
    {
        int index = 0;
        foreach (EnemiesScript enemy in _enemies)
        {
            enemy.id = index;
            idEnemiesUnset.Add(enemy.id);
            index++;
        }
    }

    // Renvoi une tour à chaque appel
    public EnemiesScript GetEnemy()
    {
        if (idEnemiesUnset.Count != 0)
        {
            EnemiesScript enemy = null;

            int id = idEnemiesUnset[0];
            idEnemiesUnset.Remove(id);
            enemy = _enemies[id];

            return enemy;
        }
        return null;
    }


    // ----------
    //
    // Fonctions retour de l'unité enemie à la pool
    //
    // ----------
    public void ReturnEnemy(int id)
    {
        if (Network.isServer)
        {
            _network.RPC("ReturnEnemyRPC", RPCMode.All, id);
        }
    }

    [RPC]
    public void ReturnEnemyRPC(int id)
    {
        countEnemiesUse--;
        print(id); 
        Desactive(id); // Désactive le gameobject 
        _enemies[id].transform.position = this.gameObject.transform.position; // Replace le projectile dans la pool
        idEnemiesUnset.Add(id); // Retire l'objet de la liste des projectiles utilisés
        _enemies[id].InitEnemy(1,1);
    }

    public void Active(int id)
    {
        _enemies[id].gameObject.SetActive(true);
    }

    public void Desactive(int id)
    {
        _enemies[id].gameObject.SetActive(false);
    }
}
