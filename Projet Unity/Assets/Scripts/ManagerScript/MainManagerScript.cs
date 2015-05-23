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
    [SerializeField]
    EnemyManager _enemyCreator;

    [SerializeField]
    Text _message;

    //private bool levelStart = false;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        StartCoroutine(InitiateGame());
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            Application.LoadLevel("Menu");
        }
    }

    IEnumerator InitiateGame()
    {
        _message.text = "Voulez-vous lancer le tutoriel ? O/N";
        bool waitPush = true;
        string key = "";
        yield return new WaitForSeconds(1);

        print("choice");
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

        print("Launch");
        _message.text = "Appuyer sur \"G\" pour lancer la partie";
        yield return StartCoroutine(WaitKeyDown(KeyCode.G));
        // levelStart = true;
        _message.text = "";
        StartCoroutine(_enemyCreator.LaunchGame());

    }


    IEnumerator WaitKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }
}
