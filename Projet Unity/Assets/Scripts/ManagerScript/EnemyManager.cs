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


    public GameObject[] Players;
    public PlayerScript[] PlayersScript = new PlayerScript[10];
    public int nb_player = 0;
    
    public void LaunchGameNet()
    {
        StartCoroutine(LaunchGame());
    }

    private IEnumerator LaunchGame()
    {
        if(Network.isServer)
        {

            // Wave 1
            int randTarget = Random.Range(0, nb_player);
            yield return StartCoroutine(spawnSoldiers(10, 1, randTarget));
            _network.RPC("LaunchTranstion", RPCMode.Others, 10);
            yield return StartCoroutine(transition(10));

            // Wave 2
            randTarget = Random.Range(0, nb_player);
            yield return StartCoroutine(spawnSoldiers(10, 0, randTarget));
            _network.RPC("LaunchTranstion", RPCMode.Others, 10);
            yield return StartCoroutine(transition(10));

            // Wave 3
            randTarget = Random.Range(0, nb_player);
            StartCoroutine(spawnSoldiers(8, 0, randTarget));
            yield return StartCoroutine(spawnSoldiers(8, 1, randTarget));
            _network.RPC("LaunchTranstion", RPCMode.Others, 16);
            yield return StartCoroutine(transition(16));

            // Wave 4
            randTarget = Random.Range(0, nb_player);
            StartCoroutine(spawnSoldiers(16, 0, randTarget));
            yield return StartCoroutine(spawnSoldiers(16, 1, randTarget));
            _network.RPC("LaunchTranstion", RPCMode.Others, 32);
            yield return StartCoroutine(transition(32));


            /*
            // Fin du niveau
            while (_soldiersPool.countEnemiesUse != 0)
            {
                if (_soldiersPool.countEnemiesUse == 1)
                    _uiEnemies.text = "Ennemi: " + _soldiersPool.countEnemiesUse;
                else
                    _uiEnemies.text = "Ennemis: " + _soldiersPool.countEnemiesUse;

                yield return new WaitForSeconds(1);
            }
            _message.text = "Victoire !";*/
        }
    }

    [RPC]
    public void InitSoldier(int Number, int idSpawner, int Target, int idSoldier)
    {
        EnemiesScript soldier = _soldiersPool._enemies[idSoldier];
        soldier.InitEnemy();
        soldier.transform.position = _spawners[idSpawner].position;
        soldier.SetTarget(Players[Target], PlayersScript[Target]);
        soldier.gameObject.SetActive(true);
        _soldiersPool.countEnemiesUse++;

        if (_soldiersPool.countEnemiesUse == 1)
            _uiEnemies.text = "Ennemi: " + _soldiersPool.countEnemiesUse;
        else
            _uiEnemies.text = "Ennemis: " + _soldiersPool.countEnemiesUse;
    }

    [RPC]
    public void LaunchTranstion(int Number)
    {
        StartCoroutine(transition(Number));
    }

    public IEnumerator spawnSoldiers(int Number, int idSpawner, int Target)
    {
        if (Network.isServer)
        {
            for (int i = 0; i < Number; i++)
            {
                EnemiesScript soldier = _soldiersPool.GetEnemy();
                _network.RPC("InitSoldier", RPCMode.All, Number, idSpawner, Target, soldier.id);

                yield return new WaitForSeconds(1);
            }
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

