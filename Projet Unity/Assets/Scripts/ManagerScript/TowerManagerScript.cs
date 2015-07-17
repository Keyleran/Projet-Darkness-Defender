// --------------------------------------------------
// Project: Darkness Defender
// Script: TowerManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



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
    private bool constructMode  = false;
    private int constructChoose = 0;
    private int buildingMoney   = 600;
    public int _player;
    bool coroutineState = false;
    private IEnumerator coConstruction;
    bool access = false;

    MuninAlgo Munin;

    [SerializeField]
    Text _buildingMoney;

    [SerializeField]
    Text _ChoosenTower;

    [SerializeField]
    Image _ChoosenTowerFont;

    [SerializeField]
    Image[] _towers;

    [SerializeField]
    NavMeshAgent _gridMaker;

    [SerializeField]
    Material _sampleMat;

    [SerializeField]
    Material[] _sampleMatChange;

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

    [SerializeField]
    NetworkView network;

    [SerializeField]
    public PlayerScript ciblePlayer;

    private Vector3 oldPosition;
    bool constructionWait = true;
    int levelArmor = 0;

    void Start()
    {
        _ChoosenTowerFont.color = new Color(255, 255, 255, 0);

        coConstruction = Construction();

        #region Initialisation Alogorithme de Munin
        Munin = new MuninAlgo(_gridMaker);
        Munin.InitiateGrid();
        #endregion
    }
     
    void FixedUpdate()
    {
        // Touche "A" par défaut, permet de passer en mode contruction
        if (Input.GetButtonDown("ConstructMode"))
        {
            _samplesPoolScript.ReturnSample();
            constructMode = !constructMode;
            RazUITowers();
            _ChoosenTower.text = "Barricade";
            _ChoosenTowerFont.color = new Color(255, 255, 255, 0);
            _towers[0].color = new Color(0, 180, 0);
            constructChoose = 0;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            levelArmor += 1;
            float fPrice = 250 * Mathf.Pow(2, levelArmor);
            int price = (int)fPrice;
            Buy_Upgrade(price, levelArmor);
        }

        // Choix de la tour à construire
        #region Choix de la tour à construire
        if (constructMode)
        {
            if (!coroutineState)
            {
                StartCoroutine(coConstruction);
                coroutineState = true;
            }

            _ChoosenTowerFont.color = new Color(255, 255, 255, 255);
            if (Input.GetButtonDown("SelectBarricade"))
            {
                RazUITowers();
                _ChoosenTower.text = "Barricade";
                _samplesPoolScript.ReturnSample();
                _towers[0].color = new Color(0, 180, 0);
                constructChoose = 0;
            }

            if (Input.GetButtonDown("SelectShooter"))
            {
                RazUITowers();
                _ChoosenTower.text = "Tirreur";
                _samplesPoolScript.ReturnSample();
                _towers[1].color = new Color(0, 180, 0);
                constructChoose = 1;
            }
            /*
            if (Input.GetButtonDown("SelectCanon"))
            {
                RazUITowers();
                _ChoosenTower.text = "Canon";
                _samplesPoolScript.ReturnSample();
                //_towers[2].color = new Color(0, 180, 0);
                constructChoose = 2;
            }

            if (Input.GetButtonDown("SelectFire"))
            {
                RazUITowers();
                _ChoosenTower.text = "Lance-Flammes";
                _samplesPoolScript.ReturnSample();
                //_towers[3].color = new Color(0, 180, 0);
                constructChoose = 3;
            }

            if (Input.GetButtonDown("SelectIce"))
            {
                RazUITowers();
                _ChoosenTower.text = "Géle";
                _samplesPoolScript.ReturnSample();
                //_towers[4].color = new Color(0, 180, 0);
                constructChoose = 4;
            }

            if (Input.GetButtonDown("SelectPoison"))
            {
                RazUITowers();
                _ChoosenTower.text = "Poison";
                _samplesPoolScript.ReturnSample();
                //_towers[5].color = new Color(0, 180, 0);
                constructChoose = 5;
            }

            if (Input.GetButtonDown("SelectMagic"))
            {
                RazUITowers();
                _ChoosenTower.text = "Tour Sorcier";
                _samplesPoolScript.ReturnSample();
                //_towers[6].color = new Color(0, 180, 0);
                constructChoose = 6;
            }

            if (Input.GetButtonDown("SelectDetector"))
            {
                RazUITowers();
                _ChoosenTower.text = "Oeil";
                _samplesPoolScript.ReturnSample();
                //_towers[7].color = new Color(0, 180, 0);
                constructChoose = 7;
            }*/
        }
        else
        {
            RazUITowers();
            if (coroutineState)
            {
                StopCoroutine(coConstruction);
                coroutineState = false;
            }
        }
        #endregion
    }

    IEnumerator Construction()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();

            // Pointeur - Permet de positioner un sample ou une tour à l'endroit où le curseur pointe
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if ((Physics.Raycast(ray, out hit)))
            {
                float positionX = 0, positionY = 0, positionZ = 0;

                positionX = roundNumber(hit.point.x);
                positionY = roundNumber(hit.point.y);
                positionZ = roundNumber(hit.point.z);

                #region SAMPLES
                _samplesPoolScript.ReturnSample();

                // Sample = Aperçu, permet de voir où seront placés les élèments
                // Le code semble identique, mais il permet de ne pas afficher une barricade sur une autre barricade
                if ((hit.collider.tag == "Ground") && (constructChoose == 0))
                {
                    // Récupère les coordonnées de l'objet visé
                    Vector3 position = new Vector3(positionX, positionY, positionZ);
                    if (oldPosition != position || constructionWait)
                    {
                        yield return new WaitForFixedUpdate();
                        constructionWait = false;
                        oldPosition = position;
                        access = Munin.accessRequest(positionX, positionZ, ciblePlayer.transform.position);
                        if (access)
                        {
                            Color new_color = _sampleMatChange[0].color;
                            _sampleMat.color = new Color(new_color.r, new_color.g, new_color.b);
                        }
                        else
                        {
                            Color new_color = _sampleMatChange[1].color;
                            _sampleMat.color = new Color(new_color.r, new_color.g, new_color.b);
                        }
                    }
                    SamplesScript sample = _samplesPoolScript.GetSample(constructChoose);
                    sample.Transform.position = new Vector3(oldPosition.x, oldPosition.y, oldPosition.z);
                }
                else if ((hit.collider.tag == "Barricade") && (constructChoose > 0))
                {
                    Color new_color = _sampleMatChange[0].color;
                    _sampleMat.color = new Color(new_color.r, new_color.g, new_color.b);
                    positionX = hit.transform.position.x;
                    positionY = hit.transform.position.y + 0.5f;
                    positionZ = hit.transform.position.z;

                    SamplesScript sample = _samplesPoolScript.GetSample(constructChoose);
                    sample.Transform.position = new Vector3(positionX, positionY, positionZ);
                }
                #endregion

                // Si le joueur clique, acheter une tour
                if (Input.GetMouseButtonDown(0))
                {
                    yield return WaitMouseUp();
                    if ((hit.collider.tag == "Ground") && access && (constructChoose == 0))
                    {
                        constructionWait = true;
                        if (Network.isServer)
                            BuyBar(new Vector3(oldPosition.x, oldPosition.y, oldPosition.z), buildingMoney, 0);
                        else
                            network.RPC("BuyBar", RPCMode.Server, new Vector3(oldPosition.x, oldPosition.y, oldPosition.z), buildingMoney, _player);
                    }
                    else if ((hit.collider.tag == "Barricade") && (constructChoose > 0))
                    {
                        int id_bar = ((BarricadeScript)hit.collider.gameObject.GetComponent("BarricadeScript")).id;
                        if (Network.isServer)
                            BuyTower(new Vector3(positionX, positionY, positionZ), id_bar, constructChoose, buildingMoney, 0);
                        else
                            network.RPC("BuyTower", RPCMode.Server, new Vector3(positionX, positionY, positionZ), id_bar, constructChoose, buildingMoney, _player);
                    }
                }

                // Si le joueur appui sur U, améliorer une tour
                if ((hit.collider.tag == "Shooter") && (constructChoose > 0))
                {
                    if (Input.GetButtonDown("UpgradeMode"))
                    {
                        yield return WaitKeyUp("UpgradeMode");
                    
                        if (buildingMoney >= 100)
                        {
                            TowersScript tower = ((TowersScript)hit.collider.gameObject.GetComponent("TowersScript"));
                            ShooterScript shooter = (ShooterScript)hit.collider.gameObject.GetComponent("ShooterScript");

                            if (shooter.levelTower < 4)
                            {
                                network.RPC("UpgrageTower", RPCMode.All, tower.id, "Shooter");

                                buildingMoney -= 100;
                                _buildingMoney.text = "Matériaux: " + buildingMoney;
                            }
                        }
                    }
                }
            }

            // Touche "E" par défaut, permet de vendre une tour/barricade
            if (Input.GetButtonDown("SellTower"))
            {
                yield return WaitKeyUp("SellTower");

                if (Physics.Raycast(ray, out hit))
                {
                    SellTower(hit);
                }
            }
        }
    }

    IEnumerator WaitKeyUp(string keyCode)
    {
        if (Input.GetButtonUp(keyCode))
            while (!Input.GetButtonDown(keyCode))
                yield return null;

        while (!Input.GetButtonUp(keyCode))
            yield return null;
    }
    IEnumerator WaitMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
            while (!Input.GetMouseButtonDown(0))
                yield return null;

        while (!Input.GetMouseButtonUp(0))
            yield return null;
    }

    float roundNumber(float value)
    {
        float new_value = 0;

        if(value >= 0)
        {
            new_value = Mathf.Ceil(value);
            if (new_value % 2 != 0)
                new_value--;
        }
        else // Value < 0
        {
            new_value = Mathf.Floor(value);
            if (new_value % 2 != 0)
                new_value++;
        }

        return new_value;
    }

    [RPC]
    void BuyBar(Vector3 position, int BuildingMon, int id_player)
    {
        if (Network.isServer)
        {
            BarricadeScript bar = _barricadePoolScript.GetBarricade();
            if ((bar != null) && (BuildingMon - 50 >= 0))
            {
                network.RPC("ActiveBar", RPCMode.All, bar.id, position);
                network.RPC("UpdateMunin", RPCMode.All, position, "B");
                network.RPC("ModifMoney", RPCMode.All, id_player, -50);
            }
            else
            {
                print("Barricade Limit Reach");
            }
        }
    }

    [RPC]
    void BuyTower(Vector3 position, int id_bar, int constructChoose, int BuildingMon, int id_player)
    {
        int money = 0;
        TowersScript tower = null;
        switch(constructChoose)
        {
            case Constants.Shooter:
                if (BuildingMon - 100 >= 0)
                {
                    tower = _shooterPoolScript.GetTower();
                    money = -100;
                }
                break;
            case Constants.Canon:
                if (BuildingMon - 150 >= 0)
                {
                    tower = _canonPoolScript.GetTower();
                    money = -150;
                }
                break;
            case Constants.Fire:
                if (BuildingMon - 200 >= 0)
                {
                    tower = _firePoolScript.GetTower();
                    money = -200;
                }
                break;
            case Constants.Ice:
                if (BuildingMon - 100 >= 0)
                {
                    tower = _icePoolScript.GetTower();
                    money = -100;
                }
                break;
            case Constants.Poison:
                if (BuildingMon - 100 >= 0)
                {
                    tower = _poisonPoolScript.GetTower();
                    money = -100;
                }
                break;
            case Constants.Magic:
                if (BuildingMon - 200 >= 0)
                {
                    tower = _magicPoolScript.GetTower();
                    money = -200;
                }
                break;
            case Constants.Detector:
                if (BuildingMon - 150 >= 0)
                {
                    tower = _detectorPoolScript.GetTower();
                    money = -150;
                }
                break;
        }

        if (tower != null)
        {
            network.RPC("ActiveTower", RPCMode.All, tower.id, id_bar, constructChoose, position);
            network.RPC("ModifMoney", RPCMode.All, id_player, money);
            network.RPC("UpdateBar", RPCMode.All, id_bar);
        }
        else
        {
            print("Tower Limit Reach or not enough money");
        }
    }

    [RPC]
    void UpdateBar(int id_bar)
    {
        string tag = _barricadePoolScript._barricade[id_bar].tag;
        if (tag == "BarricadeUse")
            _barricadePoolScript._barricade[id_bar].tag = "Barricade";
        else if (tag == "Barricade")
            _barricadePoolScript._barricade[id_bar].tag = "BarricadeUse";
    }

    // Fonction permettant de vendre les tours/barrcades
    void SellTower(RaycastHit hit)
    {
        int id;
        switch(hit.collider.tag)
        {
            case "Barricade":
                id = ((BarricadeScript) hit.collider.gameObject.GetComponent("BarricadeScript")).id;
                Vector3 position = hit.collider.gameObject.transform.position;
                network.RPC("UpdateMunin", RPCMode.All, position, "G");
                network.RPC("SellTowerNetwork", RPCMode.All, "Barricade", id);
                ModifMoney(_player, 50);
                break;
            case "Shooter":
                TowersScript tower = ((TowersScript)hit.collider.gameObject.GetComponent("TowersScript"));
                ModifMoney(_player, 100 * (((ShooterScript) hit.collider.gameObject.GetComponent("ShooterScript")).levelTower + 1));
                network.RPC("SellTowerNetwork", RPCMode.All, "Shooter", tower.id);
                network.RPC("UpdateBar", RPCMode.All, tower.id_barricade);
                break;
        }
    }

    [RPC]
    public void AddBuildingMoney(int add)
    {
        buildingMoney += add;
        _buildingMoney.text = "Matériaux: " + buildingMoney;
    }

    void RazUITowers()
    {
        /*
        for (int i = 0; i < 7; i++)
            _towers[i].color = new Color(255, 255, 255);*/

        _ChoosenTower.text = "";
        _towers[0].color = new Color(255, 255, 255);
        _towers[1].color = new Color(255, 255, 255);
        _towers[2].color = new Color(180, 0, 0);
        _towers[3].color = new Color(180, 0, 0);
        _towers[4].color = new Color(180, 0, 0);
        _towers[5].color = new Color(180, 0, 0);
        _towers[6].color = new Color(180, 0, 0);
        _towers[7].color = new Color(180, 0, 0);
    }

    [RPC]
    void ActiveBar(int id, Vector3 position)
    {
        _barricadePoolScript.Active(id, position);
    }

    [RPC]
    void DesactiveBar(int id)
    {
        _barricadePoolScript.Desactive(id);
    }

    [RPC]
    void ActiveTower(int id, int id_bar, int type, Vector3 position)
    {
        switch(type)
        {
            case Constants.Shooter:
                _shooterPoolScript.Active(id, position);
                _shooterPoolScript._towers[id].id_barricade = id_bar;
                break;
            case Constants.Canon:
                _canonPoolScript.Active(id, position);
                break;
            case Constants.Fire:
                _firePoolScript.Active(id, position);
                break;
            case Constants.Ice:
                _icePoolScript.Active(id, position);
                break;
            case Constants.Poison:
                _magicPoolScript.Active(id, position);
                break;
            case Constants.Magic:
                _magicPoolScript.Active(id, position);
                break;
            case Constants.Detector:
                _detectorPoolScript.Active(id, position);
                break;
        }
    }

    [RPC]
    void DesactiveTower(int id, int type)
    {
        switch (type)
        {
            case Constants.Shooter:
                _shooterPoolScript.Desactive(id);
                _shooterPoolScript._towers[id].id_barricade = 0;
                break;
            case Constants.Canon:
                _canonPoolScript.Desactive(id);
                break;
            case Constants.Fire:
                _firePoolScript.Desactive(id);
                break;
            case Constants.Ice:
                _icePoolScript.Desactive(id);
                break;
            case Constants.Poison:
                _magicPoolScript.Desactive(id);
                break;
            case Constants.Magic:
                _magicPoolScript.Desactive(id);
                break;
            case Constants.Detector:
                _detectorPoolScript.Desactive(id);
                break;
        }
    }

    [RPC]
    void ModifMoney(int idPlayer, int money)
    {
        if (idPlayer == _player)
        {
            buildingMoney += money;
            _buildingMoney.text = "Matériaux: " + buildingMoney;
        }
    }

    [RPC]
    void SellTowerNetwork(string Type, int id)
    {
        switch (Type)
        {
            case "Barricade":
                _barricadePoolScript.ReturnBarricade(id);
                break;
            case "Shooter":
                _shooterPoolScript.ReturnTower(id);
                ShooterScript shooter = (ShooterScript)_shooterPoolScript._towers[id].gameObject.GetComponent("ShooterScript");
                shooter.RazLevel();
                break;
        }
    }

    [RPC]
    void UpdateMunin(Vector3 position, string state)
    {
        Munin.GridMapping[position.x.ToString() + ":" + position.z.ToString()] = state.ToCharArray()[0];
    }

    [RPC]
    void UpgrageTower(int id, string type)
    {
        switch (type)
        {
            case "Shooter":
                ShooterScript shooter = (ShooterScript)_shooterPoolScript._towers[id].gameObject.GetComponent("ShooterScript");
                shooter.UpgradeTower();
                break;
        }
    }

    void Buy_Upgrade(int price, int level)
    {
        if (buildingMoney - price > 0)
        {
            ModifMoney(_player, -price);
            ciblePlayer.IncreaseHealth(level);
        }
    }
}
