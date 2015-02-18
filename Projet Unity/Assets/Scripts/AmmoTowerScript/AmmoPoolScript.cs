// --------------------------------------------------
// Project: Darkness Defender
// Script: AmmoPoolScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// --------------------------------------------------
// 
// Script de gestion, gère les pools des projectiles
// 
// --------------------------------------------------
public class AmmoPoolScript : MonoBehaviour 
{
    private List<int> idProjectileUnset = new List<int>();
    private int index = 0;

    [SerializeField]
    AmmoScript[] _projectiles;

    // Renvoi une tour à chaque appel
    public AmmoScript GetProjectile()
    {
        if ((idProjectileUnset.Count != 0) || (index < _projectiles.Length))
        {
            AmmoScript tower = null;
            if (idProjectileUnset.Count != 0)
            {
                int id = idProjectileUnset[0];
                idProjectileUnset.Remove(id);
                tower = _projectiles[id];
            }
            else if (index < _projectiles.Length)
            {
                tower = _projectiles[index];
                tower.id = index;
                index++;
            }
            tower.gameObject.SetActive(true);
            return tower;
        }

        // Si on a atteint le nombre max de barricade
        return null;
    }

    public void ReturnProjectile(AmmoScript tower)
    {
        tower.Transform.position = this.transform.position;
        idProjectileUnset.Add(tower.id);
        tower.gameObject.SetActive(false);
    }
	
}
