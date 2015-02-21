// --------------------------------------------------
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

    public IEnumerator spawnSoldiers(int Number, int idSpawner)
    {
        for(int i = 0 ; i < Number ; i++)
        {
            print(i);
            EnemiesScript soldier = _soldiersPool.GetEnemy();
            soldier.transform.position = _spawners[idSpawner].position;
            yield return new WaitForSeconds(1);
        }
    }


}
