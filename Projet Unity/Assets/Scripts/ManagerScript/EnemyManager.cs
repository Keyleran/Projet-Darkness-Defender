// --------------------------------------------------
// Project: Darkness Defender
// Script: EnemyManager.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField]
    Text _uiEnemies;

    [SerializeField]
    Text _message;

    [SerializeField]
    NetworkView _network;
    
    [RPC]
    public void LaunchGameNet()
    {
        StartCoroutine(LaunchGame());
    }

    private IEnumerator LaunchGame()
    {
        yield return StartCoroutine(spawnSoldiers(10, 1));
        yield return StartCoroutine(transition(10));
        yield return StartCoroutine(spawnSoldiers(10, 0));
        yield return StartCoroutine(transition(10));
        StartCoroutine(spawnSoldiers(8, 0));
        yield return StartCoroutine(spawnSoldiers(8, 1));
        yield return StartCoroutine(transition(16));
        StartCoroutine(spawnSoldiers(16, 0));
        yield return StartCoroutine(spawnSoldiers(16, 1));

        // Fin du niveau
        while (_soldiersPool.countEnemiesUse != 0)
        {
            if (_soldiersPool.countEnemiesUse == 1)
                _uiEnemies.text = "Ennemi: " + _soldiersPool.countEnemiesUse;
            else
                _uiEnemies.text = "Ennemis: " + _soldiersPool.countEnemiesUse;

            yield return new WaitForSeconds(1);
        }
        _message.text = "Victoire !";
    }
     
    public IEnumerator spawnSoldiers(int Number, int idSpawner)
    {
        for(int i = 0 ; i < Number ; i++)
        {
            EnemiesScript soldier = _soldiersPool.GetEnemy();
            soldier.InitEnemy();
            soldier.transform.position = _spawners[idSpawner].position;
            soldier.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);

            if (_soldiersPool.countEnemiesUse == 1)
                _uiEnemies.text = "Ennemi: " + _soldiersPool.countEnemiesUse;
            else
                _uiEnemies.text = "Ennemis: " + _soldiersPool.countEnemiesUse;
        }
    } 

    public IEnumerator transition(int nbUnit)
    {
        while (_soldiersPool.countEnemiesUse != 0) 
        {
            if (_soldiersPool.countEnemiesUse == 1)
                _uiEnemies.text = "Ennemi: " + _soldiersPool.countEnemiesUse;
            else
                _uiEnemies.text = "Ennemis: " + _soldiersPool.countEnemiesUse;

            yield return new WaitForSeconds(1);
        }
        _uiEnemies.text = "Ennemi: 0";
        _network.RPC("AddMoney", RPCMode.All, nbUnit * 15);
        yield return new WaitForSeconds(5);
        _message.text = "5";
        yield return new WaitForSeconds(1);
        _message.text = "4";
        yield return new WaitForSeconds(1);
        _message.text = "3";
        yield return new WaitForSeconds(1);
        _message.text = "2";
        yield return new WaitForSeconds(1);
        _message.text = "1";
        yield return new WaitForSeconds(1);
        _message.text = "";
    }

    [RPC]
    void AddMoney(int money)
    {
        _manager.AddBuildingMoney(money);
    }
}

