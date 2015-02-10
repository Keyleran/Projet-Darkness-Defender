// --------------------------------------------------
// Project: Darkness Defender
// Script: EnemyHealth.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Script of enemy
// 
// --------------------------------------------------
public class EnemyHealth : MonoBehaviour 
{

    [SerializeField]
    private GameObject _TypeOfEnemy;

    [SerializeField]
    public int health;


    IEnumerator OnTriggerEnter(Collider _EnCol)
    {
        if (_EnCol.tag == "Arrow")
        {
            health--;
        }

        if (health == 0)
        {
            _TypeOfEnemy.tag = "Dead";
            yield return new WaitForSeconds(1);
            Destroy(_TypeOfEnemy);
        }

    }
}
