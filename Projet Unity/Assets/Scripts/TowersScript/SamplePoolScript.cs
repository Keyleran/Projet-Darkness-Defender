// --------------------------------------------------
// Project: Darkness Defender
// Script: SamplePoolScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;


// --------------------------------------------------
// 
// Script de gestion, gère les samples. Les samples sont des "images" des tours, ils permettent de voir où le joueur va placer la tour
// 
// --------------------------------------------------
public class SamplePoolScript : MonoBehaviour
{
    [SerializeField]
    SamplesScript[] _samples;

    // _samples[0] => barricade
    // _samples[1] => shooter
    // _samples[2] => canon
    // _samples[3] => fire
    // _samples[4] => ice
    // _samples[5] => poison
    // _samples[6] => magic
    // _samples[7] => detector

    int index = 0;

    public SamplesScript GetSample(int type)
    {
        return _samples[type];
    }
}
