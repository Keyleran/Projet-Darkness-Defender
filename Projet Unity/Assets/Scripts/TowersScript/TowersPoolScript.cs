// --------------------------------------------------
// Project: Darkness Defender
// Script: TowersPoolScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// --------------------------------------------------
// 
// Script de gestion, gère les pools des tours
// 
// --------------------------------------------------
public class TowersPoolScript : MonoBehaviour 
{
    private List<int> idTowersUnset = new List<int>();

    [SerializeField]
    public TowersScript[] _towers;

    void Start()
    {
        int index = 0;
        foreach (TowersScript tower in _towers)
        {
            tower.id = index;
            idTowersUnset.Add(tower.id);
            index++;
        }
    }

    // Renvoi une tour à chaque appel
    public TowersScript GetTower()
    {
        if (idTowersUnset.Count != 0)
        {
            TowersScript tower = null;

            int id = idTowersUnset[0];
            idTowersUnset.Remove(id);
            tower = _towers[id];

            tower.gameObject.SetActive(true);
            return tower;
        }

        // Si on a atteint le nombre max de barricade
        return null;
    }

    public void ReturnTower(int id)
    {
        _towers[id].Transform.position = this.transform.position;
        idTowersUnset.Add(id);
        _towers[id].gameObject.SetActive(false);
    }

    public void Active(int id)
    {
        _towers[id].gameObject.SetActive(true);
    }

    public void Desactive(int id)
    {
        _towers[id].gameObject.SetActive(false);
    }
}
