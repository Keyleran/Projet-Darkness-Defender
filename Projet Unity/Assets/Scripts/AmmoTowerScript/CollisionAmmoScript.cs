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
// Script: gère les collisions d'un projectile
// 
// --------------------------------------------------
public class CollisionAmmoScript : MonoBehaviour 
{
    [SerializeField]
    AmmoPoolScript poolRappel;

    IEnumerator OnTriggerEnter(Collider _col)
    {
        yield return new WaitForFixedUpdate();
        AmmoScript proj = (AmmoScript)this.gameObject.GetComponent("AmmoScript");
        poolRappel.ReturnProjectile(proj);
    }
}
