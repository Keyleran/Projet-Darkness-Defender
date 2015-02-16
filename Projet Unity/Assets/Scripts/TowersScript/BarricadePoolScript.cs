// --------------------------------------------------
// Project: Darkness Defender
// Script: BarricadePoolScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// --------------------------------------------------
// 
// Script de gestion, gère la Pool de Barricades
// 
// --------------------------------------------------
public class BarricadePoolScript : MonoBehaviour
{
    private List<int> idBarUnset = new List<int>(); 
    private int index = 0;

    [SerializeField]
    BarricadeScript[] _barricade;

    // Renvoi une barricade à chaque appel
    public BarricadeScript GetBarricade(GameObject ground)
    {
        if((idBarUnset.Count != 0)||(index < _barricade.Length))
        {
            BarricadeScript barricade = null;
            if(idBarUnset.Count != 0)
            {
                int id = idBarUnset[0];
                idBarUnset.Remove(id);
                barricade = _barricade[id];
            }
            else if (index < _barricade.Length)
            {
                barricade = _barricade[index];
                barricade.id = index;
                index++;
            }
            barricade.ground = ground;
            barricade.gameObject.SetActive(true);
            return barricade;
        }

        // Si on a atteint le nombre max de barricade
        return null;
    }

    public void ReturnBarricade(BarricadeScript barricade)
    {
        barricade.Transform.position = this.transform.position;
        idBarUnset.Add(barricade.id);
        barricade.gameObject.SetActive(false);
        barricade.ground.tag = "Ground";



        barricade.ground = null;

        
    }
}
