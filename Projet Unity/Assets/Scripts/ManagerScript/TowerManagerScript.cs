// --------------------------------------------------
// Project: Darkness Defender
// Script: TowerManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using UnityEngine.UI;
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
    private bool constructMode   = false;
    private int  constructChoose = 0;
    private int  buildingMoney   = 600;
    public int BuildingMoney
    {
        get { return buildingMoney; }
        set { buildingMoney = value; }
    }

    [SerializeField]
    Text _buildingMoney;

    [SerializeField]
    Image[] _towers;


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

    /*
    #region [SerializeField] des differents poolAmmoScripts
    [SerializeField]
    AmmoPoolScript _shooterAmmoPoolScript;

    [SerializeField]
    AmmoPoolScript _canonAmmoPoolScript;

    [SerializeField]
    AmmoPoolScript _poisonAmmoPoolScript;

    [SerializeField]
    AmmoPoolScript _magicAmmoPoolScript;
    #endregion
    */
    
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
            RazUITowers();
            _towers[0].color = new Color(0, 180, 0);
            constructChoose = 0;
        }

        // Choix de la tour à construire
        #region Choix de la tour à construire
        if (constructMode)
        {
            if (Input.GetButtonDown("SelectBarricade"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                _towers[0].color = new Color(0, 180, 0);
                constructChoose = 0;
            }

            if (Input.GetButtonDown("SelectShooter"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                _towers[1].color = new Color(0, 180, 0);
                constructChoose = 1;
            }
            /*
            if (Input.GetButtonDown("SelectCanon"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                //_towers[2].color = new Color(0, 180, 0);
                constructChoose = 2;
            }

            if (Input.GetButtonDown("SelectFire"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                //_towers[3].color = new Color(0, 180, 0);
                constructChoose = 3;
            }

            if (Input.GetButtonDown("SelectIce"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                //_towers[4].color = new Color(0, 180, 0);
                constructChoose = 4;
            }

            if (Input.GetButtonDown("SelectPoison"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                //_towers[5].color = new Color(0, 180, 0);
                constructChoose = 5;
            }

            if (Input.GetButtonDown("SelectMagic"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                //_towers[6].color = new Color(0, 180, 0);
                constructChoose = 6;
            }

            if (Input.GetButtonDown("SelectDetector"))
            {
                RazUITowers();
                _samplesPoolScript.ReturnSample();
                //_towers[7].color = new Color(0, 180, 0);
                constructChoose = 7;
            }*/
        }
        else
        {
            RazUITowers();
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
            if ((bar != null)&&(BuildingMoney - 50 >= 0))
            {
                bar.Transform.position = new Vector3(positionX, positionY, positionZ);
                hit.collider.tag = "GroundUse";
                BuildingMoney -= 50;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
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
                    if (BuildingMoney - 100 >= 0)
                    {
                        tower = _shooterPoolScript.GetTower(hit.collider.gameObject);
                        BuildingMoney -= 100;
                        _buildingMoney.text = "Matériaux: " + BuildingMoney;
                    }
                    break;
                case Constants.Canon:
                    if (BuildingMoney - 150 >= 0)
                    {
                        tower = _canonPoolScript.GetTower(hit.collider.gameObject);
                        BuildingMoney -= 150;
                        _buildingMoney.text = "Matériaux: " + BuildingMoney;
                    }
                    break;
                case Constants.Fire:
                    if (BuildingMoney - 200 >= 0)
                    {
                        tower = _firePoolScript.GetTower(hit.collider.gameObject);
                        BuildingMoney -= 200;
                        _buildingMoney.text = "Matériaux: " + BuildingMoney;
                    }
                    break;
                case Constants.Ice:
                    if (BuildingMoney - 100 >= 0)
                    {
                        tower = _icePoolScript.GetTower(hit.collider.gameObject);
                        BuildingMoney -= 100;
                        _buildingMoney.text = "Matériaux: " + BuildingMoney;
                    }
                    break;
                case Constants.Poison:
                    if (BuildingMoney - 100 >= 0)
                    {
                        tower = _poisonPoolScript.GetTower(hit.collider.gameObject);
                        BuildingMoney -= 100;
                        _buildingMoney.text = "Matériaux: " + BuildingMoney;
                    }
                    break;
                case Constants.Magic:
                    if (BuildingMoney - 200 >= 0)
                    {
                        tower = _magicPoolScript.GetTower(hit.collider.gameObject);
                        BuildingMoney -= 200;
                        _buildingMoney.text = "Matériaux: " + BuildingMoney;
                    }
                    break;
                case Constants.Detector:
                    if (BuildingMoney - 150 >= 0)
                    {
                        tower = _detectorPoolScript.GetTower(hit.collider.gameObject);
                        BuildingMoney -= 150;
                        _buildingMoney.text = "Matériaux: " + BuildingMoney;
                    }
                    break;
            }

            if (tower != null)
            {
                tower.Transform.position = new Vector3(positionX, positionY, positionZ);
                hit.collider.tag = "BarricadeUse";
            }
            else
            {
                print("Tower Limit Reach or not enough money");
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
                BuildingMoney += 50;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
            case "Shooter":
                _shooterPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                BuildingMoney += 100;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
            case "Canon":
                _canonPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                BuildingMoney += 150;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
            case "Fire":
                _firePoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                BuildingMoney += 200;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
            case "Ice":
                _icePoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                BuildingMoney += 100;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
            case "Poison":
                _poisonPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                BuildingMoney += 100;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
            case "Magic":
                _magicPoolScript.ReturnTower((TowersScript) hit.collider.gameObject.GetComponent("TowersScript"));
                BuildingMoney += 200;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
            case "Detector":
                _detectorPoolScript.ReturnTower((TowersScript)hit.collider.gameObject.GetComponent("TowersScript"));
                BuildingMoney += 150;
                _buildingMoney.text = "Matériaux: " + BuildingMoney;
                break;
        }
    }

    public void AddBuildingMoney(int add)
    {
        BuildingMoney += add;
        _buildingMoney.text = "Matériaux: " + BuildingMoney;
    }

    void RazUITowers()
    {
        /*
        for (int i = 0; i < 7; i++)
            _towers[i].color = new Color(255, 255, 255);*/

        _towers[0].color = new Color(255, 255, 255);
        _towers[1].color = new Color(255, 255, 255);
        _towers[2].color = new Color(180, 0, 0);
        _towers[3].color = new Color(180, 0, 0);
        _towers[4].color = new Color(180, 0, 0);
        _towers[5].color = new Color(180, 0, 0);
        _towers[6].color = new Color(180, 0, 0);
        _towers[7].color = new Color(180, 0, 0);
    }
}
