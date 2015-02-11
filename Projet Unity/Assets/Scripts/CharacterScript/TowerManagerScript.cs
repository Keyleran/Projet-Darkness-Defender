// --------------------------------------------------
// Project: Darkness Defender
// Script: ArrowScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Script de sélection et création des Tours
// 
// --------------------------------------------------
public class TowerManagerScript : MonoBehaviour 
{
    private bool constructMode = false;

    void FixedUpdate()
    {
        if (constructMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    float positionX = hit.transform.position.x;
                    float positionY = hit.transform.position.y + 0.5f;
                    float positionZ = hit.transform.position.z;

                    print(positionX + "," + positionY + "," + positionZ);
                }
            }
        }

        if(Input.GetButtonDown("ConstructMode"))
        {
            constructMode = !constructMode;
        }
    }
}
