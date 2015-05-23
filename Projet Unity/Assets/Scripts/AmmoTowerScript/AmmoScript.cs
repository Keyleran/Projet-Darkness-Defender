// --------------------------------------------------
// Project: Darkness Defender
// Script: AmmoScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------
// 
// Script des projectiles de la tour Shooter
// 
// --------------------------------------------------
public class AmmoScript : MonoBehaviour 
{
    public int id = 0;

    [SerializeField]
    Transform _transform;

    [SerializeField]
    Rigidbody _rigidbody;

    [SerializeField]
    public int damage;

    [SerializeField]
    AmmoPoolScript poolRappel;

    // Champs permettant de placer une barricade
    public Transform Transform
    {
        get
        {
            return _transform;
        }
        set
        {
            _transform = value;
        }
    }

    // Champs permettant d'appliquer de la physique
    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }
        set
        {
            _rigidbody = value;
        }
    }

    IEnumerator OnTriggerEnter(Collider _col)
    {
        yield return new WaitForFixedUpdate();
        poolRappel.ReturnProjectile(this);
    }
}
