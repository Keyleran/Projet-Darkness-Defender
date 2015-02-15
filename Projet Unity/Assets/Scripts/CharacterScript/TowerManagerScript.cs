// --------------------------------------------------
// Project: Darkness Defender
// Script: TowerManagerScript.cs
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
    private int  constructChoose = 0;

    [SerializeField]
    BarricadePoolScript _barricadePoolScript;

    [SerializeField]
    SamplePoolScript _samplesPoolScript;

    void FixedUpdate()
    {
        if (constructMode)
        {
            // Pointeur - Permet de positioner un sample ou une tour à l'endroit où le curseur pointe
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                float positionX = hit.transform.position.x;
                float positionY = hit.transform.position.y + 0.5f;
                float positionZ = hit.transform.position.z;

                _samplesPoolScript.ReturnSample();
                if ((hit.collider.tag == "Ground")&&(constructChoose == 0))
                {
                    // Sample = Aperçu, permet de voir où seront placés les élèments
                    SamplesScript sample = _samplesPoolScript.GetSample(constructChoose);
                    sample.Transform.position = new Vector3(positionX, positionY , positionZ);

                    if(Input.GetMouseButtonDown(0))
                    {
                        BarricadeScript bar = _barricadePoolScript.GetBarricade(hit.collider.gameObject);
                        if(bar != null)
                        {
                            bar.Transform.position = new Vector3(positionX, positionY , positionZ);
                            hit.collider.tag = "GroundUse";
                        }
                        else
                        {
                            print("Barricade Limit Reach");
                        }
                    }
                }
            }
        }

        if (Input.GetButtonDown("SellTower"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                SellTower(hit);
            }            
        }

        if (Input.GetButtonDown("ConstructMode"))
        {
            _samplesPoolScript.ReturnSample();
            constructMode = !constructMode;
        }

        // Choix de la tour à construire
        #region Choix de la tour à construire
        if (Input.GetButtonDown("SelectBarricade"))
        {
            constructChoose = 0;
        }

        if (Input.GetButtonDown("SelectShooter"))
        {
            constructChoose = 1;
        }

        if (Input.GetButtonDown("SelectCanon"))
        {
            constructChoose = 2;
        }

        if (Input.GetButtonDown("SelectFire"))
        {
            constructChoose = 3;
        }

        if (Input.GetButtonDown("SelectIce"))
        {
            constructChoose = 4;
        }

        if (Input.GetButtonDown("SelectPoison"))
        {
            constructChoose = 5;
        }

        if (Input.GetButtonDown("SelectMagic"))
        {
            constructChoose = 6;
        }

        if (Input.GetButtonDown("SelectDetector"))
        {
            constructChoose = 7;
        }
        #endregion
    }

    // Fonction permettant de vendre les tours/barrcades
    void SellTower(RaycastHit hit)
    {
        switch(hit.collider.tag)
        {
            case "Barricade":
                _barricadePoolScript.ReturnBarricade((BarricadeScript) hit.collider.gameObject.GetComponent("BarricadeScript"));
                break;
            case "Shooter":
                break;
            case "Canon":
                break;
            case "Fire":
                break;
            case "Ice":
                break;
            case "Poison":
                break;
            case "Magic":
                break;
            case "Detector":
                break;
        }
    }
}
