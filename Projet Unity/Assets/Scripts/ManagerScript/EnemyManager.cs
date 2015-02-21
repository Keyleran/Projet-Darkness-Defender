﻿// --------------------------------------------------
// Project: Darkness Defender
// Script: EnemyManager.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Script Manager: Répartition des unités ennemies sur différents points de spawn et gère les pools des unités ennemies
// 
// --------------------------------------------------
public class EnemyManager : MonoBehaviour 
{
    [SerializeField]
    Transform[] _spawners;

    [SerializeField]
    EnemyPoolScript _soldiersPool;

    [SerializeField]
    TowerManagerScript _manager;

    public IEnumerator LaunchGame()
    {
        print("Launch Game");
        yield return StartCoroutine(spawnSoldiers(10, 1));
        yield return StartCoroutine(transition(10));
        yield return StartCoroutine(spawnSoldiers(10, 0));
        yield return StartCoroutine(transition(10));
        StartCoroutine(spawnSoldiers(8, 0));
        yield return StartCoroutine(spawnSoldiers(8, 1));
        yield return StartCoroutine(transition(16));
        StartCoroutine(spawnSoldiers(16, 0));
        yield return StartCoroutine(spawnSoldiers(16, 1));
        yield return StartCoroutine(transition(32));
    }
     
    public IEnumerator spawnSoldiers(int Number, int idSpawner)
    {
        for(int i = 0 ; i < Number ; i++)
        {
            EnemiesScript soldier = _soldiersPool.GetEnemy();
            soldier.transform.position = _spawners[idSpawner].position;
            soldier.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
        }
    } 

    public IEnumerator transition(int nbUnit)
    {
        while (_soldiersPool.countEnemiesUse != 0) 
        {
            yield return new WaitForSeconds(1);
        }
        _manager.BuildingMoney += nbUnit * 15;
        yield return new WaitForSeconds(10);
    }

}
