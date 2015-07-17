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
        countEnemiesUse++;
        if (idEnemiesUnset.Count != 0)
        {
            EnemiesScript enemy = null;

            int id = idEnemiesUnset[0];
            idEnemiesUnset.Remove(id);
            enemy = _enemies[id];

            enemy.gameObject.SetActive(true);
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
            idEnemiesUnset.Add(_enemies[id].id); // Retire l'objet de la liste des projectiles utilisés
            _network.RPC("ReturnEnemyRPC", RPCMode.All, id);
        }
    }

    [RPC]
    public void ReturnEnemyRPC(int id)
    {
        countEnemiesUse--;
        _enemies[id].Transform.position = this.transform.position; // Replace le projectile dans la pool
        _enemies[id].gameObject.SetActive(false); // Désactive le gameobject
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
