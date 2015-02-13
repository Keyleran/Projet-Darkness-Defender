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
    [SerializeField]
    BarricadePoolScript _barricadePoolScript;

    [SerializeField]
    SamplePoolScript _samplesPoolScript;

    private bool constructMode   = false;
    private int  constructChoose = 0;

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

                    SamplesScript sample = _samplesPoolScript.GetSample(constructChoose);
                    sample.Transform.position = new Vector3(positionX, positionY + 0.25f, positionZ);

                    if((Input.GetMouseButtonDown(0))&&(constructChoose == 0))
                    {
                        BaricadeScript bar = _barricadePoolScript.GetBarricade();
                        if(bar != null)
                        {
                            bar.gameObject.SetActive(true);

                            bar.Transform.position = new Vector3(positionX, positionY + 0.25f, positionZ);
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

        if(Input.GetButtonDown("ConstructMode"))
        {
            constructMode = !constructMode;
        }





        if (Input.GetButtonDown("SelectBaricade"))
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
    }
}
