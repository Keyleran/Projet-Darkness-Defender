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
    int NbLoop = 0;
    int wave = 1;
    
    public void LaunchGameNet()
    {
        StartCoroutine(LaunchGame());
    }

    private IEnumerator LaunchGame()
    {
        if(Network.isServer)
        {
            int randTarget;
            int level;
            int spawn;
            int nbUnit;

            while(true)
            {
                // Wave 1
                randTarget = Random.Range(0, nb_player);
                level = 0;
                spawn = 0;
                nbUnit = 10;

                yield return StartCoroutine(spawnSoldiers(nbUnit, spawn, randTarget, level));
                _network.RPC("LaunchTranstion", RPCMode.Others, nbUnit, level + 1);
                yield return StartCoroutine(transition(nbUnit, level + 1));

                // Wave 2
                randTarget = Random.Range(0, nb_player);
                level = 0;
                spawn = 1;
                nbUnit = 10;

                yield return StartCoroutine(spawnSoldiers(nbUnit, spawn, randTarget, level));
                _network.RPC("LaunchTranstion", RPCMode.Others, nbUnit, level + 1);
                yield return StartCoroutine(transition(nbUnit, level + 1));

                // Wave 3
                randTarget = Random.Range(0, nb_player);
                level = 1;
                nbUnit = 16;

                yield return StartCoroutine(spawnSoldiers(nbUnit / 2, 0, randTarget, level));
                yield return StartCoroutine(spawnSoldiers(nbUnit / 2, 1, randTarget, level));
                _network.RPC("LaunchTranstion", RPCMode.Others, nbUnit, level + 1);
                yield return StartCoroutine(transition(nbUnit, level + 1));

                // Wave 4
                randTarget = Random.Range(0, nb_player);
                level = 2;
                nbUnit = 20;

                StartCoroutine(spawnSoldiers(nbUnit / 2, 0, randTarget, level));
                yield return StartCoroutine(spawnSoldiers(nbUnit / 2, 1, randTarget, level));
                _network.RPC("LaunchTranstion", RPCMode.Others, nbUnit, level + 1);
                yield return StartCoroutine(transition(nbUnit, level + 1));

                // Wave 5
                randTarget = Random.Range(0, nb_player);
                level = 3;
                spawn = 0;
                nbUnit = 10;

                yield return StartCoroutine(spawnSoldiers(nbUnit, spawn, randTarget, level));
                _network.RPC("LaunchTranstion", RPCMode.Others, nbUnit, level + 1);
                yield return StartCoroutine(transition(nbUnit, level + 1));

                // Wave 6
                randTarget = Random.Range(0, nb_player);
                level = 3;
                spawn = 1;
                nbUnit = 10;

                yield return StartCoroutine(spawnSoldiers(nbUnit, spawn, randTarget, level));
                _network.RPC("LaunchTranstion", RPCMode.Others, nbUnit, level + 1);
                yield return StartCoroutine(transition(nbUnit, level + 1));

                // Wave 7
                randTarget = Random.Range(0, nb_player);
                level = 4;
                nbUnit = 20;

                StartCoroutine(spawnSoldiers(nbUnit / 2, 0, randTarget, level));
                yield return StartCoroutine(spawnSoldiers(nbUnit / 2, 1, randTarget, level));
                _network.RPC("LaunchTranstion", RPCMode.Others, nbUnit, level + 1);
                yield return StartCoroutine(transition(nbUnit, level + 1));

                NbLoop++;
            }
        }
    }

    [RPC]
    public void InitSoldier(int Number, int idSpawner, int Target, int idSoldier, int level)
    {
        EnemiesScript soldier = _soldiersPool._enemies[idSoldier];
        soldier.InitEnemy(level, NbLoop);
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
    public void LaunchTranstion(int Number, int level)
    {
        StartCoroutine(transition(Number, level));
    }

    public IEnumerator spawnSoldiers(int Number, int idSpawner, int Target, int level)
    {
        if (Network.isServer)
        {
            for (int i = 0; i < Number; i++)
            {
                EnemiesScript soldier = _soldiersPool.GetEnemy();
                _network.RPC("InitSoldier", RPCMode.All, Number, idSpawner, Target, soldier.id, level);

                yield return new WaitForSeconds(1);
            }
        }
    }

    public IEnumerator transition(int nbUnit, int level)
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
        _network.RPC("AddMoney", RPCMode.All, nbUnit * 15 * level);
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
        wave++;
        _message.text = "Vague " + wave.ToString();
        yield return new WaitForSeconds(1);
        _message.text = "";
    }

    [RPC]
    void AddMoney(int money)
    {
        _manager.AddBuildingMoney(money);
    }
}

