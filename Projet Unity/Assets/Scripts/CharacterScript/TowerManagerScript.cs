// --------------------------------------------------
// Project: Darkness Defender
// Script: TowerManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// Define
static class Constants
{
    public const short Shooter  = 1;
    public const short Canon    = 2;
    public const short Fire     = 3;
    public const short Ice      = 4;
    public const short Poison   = 5;
    public const short Magic    = 6;
    public const short Detector = 7;
}

// --------------------------------------------------
// 
// Script de sélection et création des Tours
// 
// --------------------------------------------------
public class TowerManagerScript : MonoBehaviour
{
    private bool constructMode = false;
    private int  constructChoose = 0;

    #region [SerializeField] des differents poolScripts
    [SerializeField]
    SamplePoolScript _samplesPoolScript;

    [SerializeField]
    BarricadePoolScript _barricadePoolScript;

    [SerializeField]
    TowersPoolScript _shooterPoolScript;

    [SerializeField]
    TowersPoolScript _canonPoolScript;

    [SerializeField]
    TowersPoolScript _firePoolScript;

    [SerializeField]
    TowersPoolScript _icePoolScript;

    [SerializeField]
    TowersPoolScript _poisonPoolScript;

    [SerializeField]
    TowersPoolScript _magicPoolScript;

    [SerializeField]
    TowersPoolScript _detectorPoolScript;
    #endregion

    void FixedUpdate()
    {
        if (constructMode)
        {
            // Pointeur - Permet de positioner un sample ou une tour à l'endroit où le curseur pointe
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                // Récupère les coordonnées de l'objet visé
                float positionX = hit.transform.position.x;
                float positionY = hit.transform.position.y + 0.5f;
                float positionZ = hit.transform.position.z;

                _samplesPoolScript.ReturnSample();

                // Sample = Aperçu, permet de voir où seront placés les élèments
                // Le code semble identique, mais il permet de ne pas afficher une barricade sur une autre barricade
                if ((hit.collider.tag == "Ground") && (constructChoose == 0))
                {
                    SamplesScript sample = _samplesPoolScript.GetSample(constructChoose);
                    sample.Transform.position = new Vector3(positionX, positionY, positionZ);
                }
                else if ((hit.collider.tag == "Barricade") && (constructChoose > 0))
                {
                    SamplesScript sample = _samplesPoolScript.GetSample(constructChoose);
                    sample.Transform.position = new Vector3(positionX, positionY, positionZ);
                }
                
                // Si le joueur clique, acheter une tour
                if(Input.GetMouseButtonDown(0))
                {
                    BuyTower(hit, constructChoose);
                }
            }
        }

        // Touche "E" par défaut, permet de vendre une tour/barricade
        if (Input.GetButtonDown("SellTower"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                SellTower(hit);
            }            
        }

        // Touche "A" par défaut, permet de passer en mode contruction
        if (Input.GetButtonDown("ConstructMode"))
        {
            _samplesPoolScript.ReturnSample();
            constructMode = !constructMode;
        }

        // Choix de la tour à construire
        #region Choix de la tour à construire
        if (Input.GetButtonDown("SelectBarricade"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 0;
        }

        if (Input.GetButtonDown("SelectShooter"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 1;
        }

        if (Input.GetButtonDown("SelectCanon"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 2;
        }

        if (Input.GetButtonDown("SelectFire"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 3;
        }

        if (Input.GetButtonDown("SelectIce"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 4;
        }

        if (Input.GetButtonDown("SelectPoison"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 5;
        }

        if (Input.GetButtonDown("SelectMagic"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 6;
        }

        if (Input.GetButtonDown("SelectDetector"))
        {
            _samplesPoolScript.ReturnSample();
            constructChoose = 7;
        }
        #endregion
    }



    void BuyTower(RaycastHit hit, int constructChoose)
    {
        float positionX = hit.transform.position.x;
        float positionY = hit.transform.position.y + 0.5f;
        float positionZ = hit.transform.position.z;

        if ((hit.collider.tag == "Ground") && (constructChoose == 0))
        {
            BarricadeScript bar = _barricadePoolScript.GetBarricade(hit.collider.gameObject);
            if (bar != null)
            {
                bar.Transform.position = new Vector3(positionX, positionY, positionZ);
                hit.collider.tag = "GroundUse";
            }
            else
            {
                print("Barricade Limit Reach");
            }
        }
        else if ((hit.collider.tag == "Barricade") && (constructChoose > 0))
        {
            TowersScript tower = null;
            switch(constructChoose)
            {
                case Constants.Shooter:
                    tower = _shooterPoolScript.GetTower(hit.collider.gameObject);
                    break;
                case Constants.Canon:
                    tower = _canonPoolScript.GetTower(hit.collider.gameObject);
                    break;
                case Constants.Fire:
                    tower = _firePoolScript.GetTower(hit.collider.gameObject);
                    break;
                case Constants.Ice:
                    tower = _icePoolScript.GetTower(hit.collider.gameObject);
                    break;
                case Constants.Poison:
                    tower = _poisonPoolScript.GetTower(hit.collider.gameObject);
                    break;
                case Constants.Magic:
                    tower = _magicPoolScript.GetTower(hit.collider.gameObject);
                    break;
                case Constants.Detector:
                    tower = _detectorPoolScript.GetTower(hit.collider.gameObject);
                    break;
            }

            if (tower != null)
            {
                tower.Transform.position = new Vector3(positionX, positionY, positionZ);
                hit.collider.tag = "BarricadeUse";
            }
            else
            {
                print("Tower Limit Reach");
            }
        }
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
                _shooterPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                break;
            case "Canon":
                _canonPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                break;
            case "Fire":
                _firePoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                break;
            case "Ice":
                _icePoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                break;
            case "Poison":
                _poisonPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                break;
            case "Magic":
                _magicPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                break;
            case "Detector":
                _detectorPoolScript.ReturnTower((TowersScript)hit.collider.gameObject.GetComponent("TowersScript"));
                break;
        }
    }
}
