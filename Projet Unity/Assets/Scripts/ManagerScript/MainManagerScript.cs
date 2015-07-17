// --------------------------------------------------
// Project: Darkness Defender
// Script: MainManagerScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// --------------------------------------------------
// 
// Script Manager: Gére le jeu
// 
// --------------------------------------------------
public class MainManagerScript : MonoBehaviour
{
    private string ancientMessage = "";
    private float gameState = 1;

    [SerializeField]
    EnemyManager _enemyCreator;

    [SerializeField]
    Text _message;

    [SerializeField]
    NetworkView _network;

    [SerializeField]
    TowerManagerScript TowerManager;

    public bool levelStart = false;

    int nb_player = 0;
    int ready_player = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        StartCoroutine(InitiateGame());
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Application.LoadLevel("Menu");
        }

        if (Input.GetButtonDown("Pause"))
        {
            print("pauseAsked");
            _network.RPC("PauseRestartGame", RPCMode.All);
        }
    }
    
    public void AddPlayer()
    {
        SetId();
        _network.RPC("NewPlayer", RPCMode.All, nb_player);
    }

    [RPC]
    void NewPlayer(int ActualNb_player)
    {
        GameObject[] PlayersFind = GameObject.FindGameObjectsWithTag("Player");

        if(nb_player == 0)
        {
            GameObject[] Players = new GameObject[10];
            PlayerScript[] PlayersScript = new PlayerScript[10];

            PlayerScript initPlayer = (PlayerScript)PlayersFind[0].GetComponent("PlayerScript");
            initPlayer.idPlayer = ActualNb_player;
            initPlayer.init = true;

            Players[ActualNb_player] = PlayersFind[0];
            PlayersScript[ActualNb_player] = initPlayer;
            TowerManager._player = ActualNb_player;
            TowerManager.ciblePlayer = PlayersScript[ActualNb_player];

            for (int i = 0; i < ActualNb_player; i++)
            {
                initPlayer = (PlayerScript)PlayersFind[i+1].GetComponent("PlayerScript");
                initPlayer.idPlayer = i;
                initPlayer.init = true;

                Players[i] = PlayersFind[i+1];
                PlayersScript[i] = initPlayer;
            }

            _enemyCreator.Players = Players;
            _enemyCreator.PlayersScript = PlayersScript;

        }
        else
        {
            PlayerScript initPlayer = (PlayerScript)PlayersFind[ActualNb_player].GetComponent("PlayerScript");
            initPlayer.idPlayer = ActualNb_player;
            initPlayer.init = true;

            _enemyCreator.Players[ActualNb_player] = PlayersFind[ActualNb_player];
            _enemyCreator.PlayersScript[ActualNb_player] = initPlayer;
        }
        nb_player = ActualNb_player + 1;
        _enemyCreator.nb_player = nb_player;
    } 

    void SetId()
    {
        print("id: " + nb_player);
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            print("test");
            PlayerScript initPlayer = (PlayerScript)player.GetComponent("PlayerScript");

            if(initPlayer.init == false)
            {
                initPlayer.idPlayer = nb_player;
                print(nb_player);
                initPlayer.init = true;
            }
        }  
    }

    [RPC] 
    void ReadyPlayer()
    {
        ready_player++;
    }

    IEnumerator InitiateGame()
    {
        _message.text = "Voulez-vous lancer le tutoriel ? O/N";
        bool waitPush = true;
        string key = "";
        yield return new WaitForSeconds(1);

        while (waitPush)
        {
            // Attendre

            if (Input.GetKeyDown(KeyCode.O))
            {
                key = "O";
                waitPush = false;
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                key = "N";
                waitPush = false;
            }
            yield return null;
        }

        #region Tutoriel
        if (key == "O")
        {
            print("Tuto");
            _message.text = "Bienvenue dans le tutoriel de Darkness Defender\n(Appuyez sur \"E\")";
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "Vous pouvez quitter la partie à tout moment en appuyant sur la touche \"Escape\".\n(Appuyez sur \"E\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "Pour se déplacer dans le jeu, utilisez les touches:\nZ Q S D\nLa touche \"Space\" permet de sauter.";
            yield return new WaitForSeconds(5);
            _message.text = "Parcourons l'interface !\nEn haut à gauche de votre écran, vous avez la barre de santé de votre personnage.\nSi elle tombe à 0, la partie est finie,\nGame Over !\n(Appuyez sur \"E\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "En haut à droite se trouve le compteur d'enemis, il vous indique le nombre d'enemis restant sur la carte.\n(Appuyez sur \"E\")";
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "En parlant de carte, vous trouverez une mini-map en bas à droite.\n(Appuyez sur \"E\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "Ce jeu est un Tower Defense !\nVous devez construire des Tours pour détruire vos enemis.\nAppuyez sur \"A\" pour passer en mode construction...";
            yield return StartCoroutine(WaitKeyDown(KeyCode.A));
            _message.text = "Vous êtes maintenant dans le mode de contruction.\nLes cases en bas de votre écran indique les tours que vous pouvez construire.\nLa barricade est sélectionnée par défaut.\n(Appuyez sur \"E\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "La barricade sert de base aux autres Tours, elle est indispensable !\nVous pouvez construire les barricades sur les lieux surélevés.\nVous pouvez revendre les tours en appuyant sur \"E\".\n(Appuyez sur \"E\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "Une fois la barricade construite, vous pouvez acheter une Tour. Sélectionner la tour \"Shooter\".\n(Appuyez sur \"2\" ou \"é\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.Alpha2));
            _message.text = "La construction de tour et de barricade coûte des matériaux, faites attention !\n Le nombre de matériax restant est indiqué en bas à gauche de votre écran.\n(Appuyez sur \"E\")";
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "Pour le moment, vous n'avez accès qu'à 2 tours.\nUn peu de patience, les 6 autres arriveront prochainement\n(Appuyez sur \"E\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
            _message.text = "Ce tutoriel est maintenant fini ! Que le côté obscur soit avec vous !\n(Appuyez sur \"E\")";
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(WaitKeyDown(KeyCode.E));
        }
        #endregion

        _message.text = "Appuyer sur \"G\" pour lancer la partie";
        yield return StartCoroutine(WaitKeyDown(KeyCode.G));
        _network.RPC("ReadyPlayer", RPCMode.All);
        yield return StartCoroutine(WaitAllPlayer());
        _message.text = "";
        levelStart = true;

        if(Network.isServer)
            _enemyCreator.LaunchGameNet();

    }

    IEnumerator WaitKeyDown(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
            while (!Input.GetKeyUp(keyCode))
                yield return null;

        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }

    IEnumerator WaitKeyPause()
    {
        if (Input.GetKeyDown(KeyCode.P))
            while (!Input.GetKeyUp(KeyCode.P))
                yield return null;

        while (!Input.GetKeyDown(KeyCode.P))
            yield return null;
        _network.RPC("PauseRestartGame", RPCMode.All);
    }

    IEnumerator WaitAllPlayer()
    {
        while (ready_player != nb_player)
        {
            _message.text = "En attente des autres joueurs";
            yield return null;
        }
    }

    [RPC]
    void PauseRestartGame()
    {
        gameState = gameState == 1 ? 0 : 1;
        print(gameState);
        if (gameState == 0)
        {
            ancientMessage = _message.text;
            _message.text = "Jeu en Pause !\n";
            StartCoroutine(WaitKeyPause());
        }
        else
        {
            _message.text = ancientMessage;
            StopCoroutine(WaitKeyPause());
        }

        Time.timeScale = gameState;
    }
}
