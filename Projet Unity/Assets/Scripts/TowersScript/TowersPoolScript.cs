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
    private int index = 0;

    [SerializeField]
    TowersScript[] _towers;


    // Renvoi une tour à chaque appel
    public TowersScript GetTower(GameObject barricade)
    {
        if ((idTowersUnset.Count != 0) || (index < _towers.Length))
        {
            TowersScript tower = null;
            if (idTowersUnset.Count != 0)
            {
                int id = idTowersUnset[0];
                idTowersUnset.Remove(id);
                tower = _towers[id];
            }
            else if (index < _towers.Length)
            {
                tower = _towers[index];
                tower.id = index;
                index++;
            }
            tower.barricade = barricade;
            tower.gameObject.SetActive(true);
            return tower;
        }

        // Si on a atteint le nombre max de barricade
        return null;
    }

    public void ReturnTower(TowersScript tower)
    {
        tower.Transform.position = this.transform.position;
        idTowersUnset.Add(tower.id);
        tower.gameObject.SetActive(false);
        tower.barricade.tag = "Barricade";
        tower.barricade = null;
    }
}
