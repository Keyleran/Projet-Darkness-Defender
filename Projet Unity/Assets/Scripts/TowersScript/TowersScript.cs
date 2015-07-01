// --------------------------------------------------
// Project: Darkness Defender
// Script: TowersScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;


// --------------------------------------------------
// 
// Script pour l'ensemble des tours
// 
// --------------------------------------------------
public class TowersScript : MonoBehaviour 
{
    public int id = 0;
    public int id_barricade = 0;

    [SerializeField]
    Transform _transform;

    [SerializeField]
    Rigidbody _rigidbody;

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
}
