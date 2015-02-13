// --------------------------------------------------
// Project: Darkness Defender
// Script: SamplesScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;


// --------------------------------------------------
// 
// Script ajoutant des champs aux samples
// 
// --------------------------------------------------
public class SamplesScript : MonoBehaviour 
{
    [SerializeField]
    Transform _transform;

    // Champs permettant de placer un sample
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
}

