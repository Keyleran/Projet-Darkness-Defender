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

    [SerializeField]
    public BarricadeScript[] _barricade;

    void Start()
    {
        int index = 0;
        foreach (BarricadeScript barricade in _barricade)
        {
            barricade.id = index;
            idBarUnset.Add(barricade.id);
            index++;
        }
    }


    // Renvoi une barricade à chaque appel
    public BarricadeScript GetBarricade()
    {
        if(idBarUnset.Count != 0)
        {
            BarricadeScript barricade = null;

            int id = idBarUnset[0];
            idBarUnset.Remove(id);
            barricade = _barricade[id];

            barricade.gameObject.SetActive(true);
            barricade.tag = "Barricade";
            return barricade;
        }

        // Si on a atteint le nombre max de barricade
        return null;
    }

    public void ReturnBarricade(int id)
    {
        _barricade[id].Transform.position = this.transform.position;
        idBarUnset.Add(id);
        _barricade[id].gameObject.SetActive(false);
    }

    public void Active(int id, Vector3 position)
    {
        _barricade[id].Transform.position = position;
        _barricade[id].gameObject.SetActive(true);
    }

    public void Desactive(int id)
    {
        _barricade[id].gameObject.SetActive(false);
    }
}
