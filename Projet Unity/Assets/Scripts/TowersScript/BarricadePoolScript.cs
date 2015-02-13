// --------------------------------------------------
// Project: Darkness Defender
// Script: BarricadePoolScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;


// --------------------------------------------------
// 
// Script de gestion, gère la Pool de Barricades
// 
// --------------------------------------------------
public class BarricadePoolScript : MonoBehaviour
{
    [SerializeField]
    BaricadeScript[] _barricade;

    int index = 0;

    // Renvoi une barricade à chaque appel
    public BaricadeScript GetBarricade()
    {
        if(index < _barricade.Length)
        {
            BaricadeScript barricade = _barricade[index];
            index++;
            return barricade;
        }

        // Si on a atteint le nombre max de barricade
        return null;
    }
}
