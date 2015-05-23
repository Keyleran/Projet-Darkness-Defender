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

    // ----------
    //
    // Fonction retour du projectile à la pool
    //
    // ----------
    public void ReturnProjectile(AmmoScript proj)
    {
        proj.Transform.position = this.transform.position; // Replace le projectile dans la pool
        proj.Rigidbody.velocity = new Vector3(0, 0, 0);
        idProjectileUnset.Add(proj.id); // Retire l'objet de la liste des projectiles utilisés

        proj.gameObject.SetActive(false); // Désactive le gameobject
    }
	
}
