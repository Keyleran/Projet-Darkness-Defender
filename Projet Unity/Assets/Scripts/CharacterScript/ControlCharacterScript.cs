﻿// --------------------------------------------------
// Project: Darkness Defender
// Script: ControlCharacterScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Script de Déplacements
// 
// --------------------------------------------------
public class ControlCharacterScript : MonoBehaviour
{
    // --------------------------
    // Prefab Player
    // Le prafab "player" est constitué de 2 éléments:
    // - Une Caméra
    // - Un modèle 3D:
    //      - une "capsule collider"
    //      - un rigidbody, avec les axes de rotations X,Y,Z bloqués pour éviter la chute du personnage sur un côté
    // --------------------------
    
    [SerializeField]
    Rigidbody _rigibodyPlayer = new Rigidbody();

    [SerializeField]
    NetworkView _network;

    [SerializeField]
    Animator _walk;

    // Parametres personnages
    private float speed      = 10.0f; // Vitesse de deplacement
    private float gravity    = -9.81f; // Gravité terrestre 9.81 m/s²
    private float heightJump = 1.0f; // Hauteur des sauts
    private bool isJumping = false; // Ce booleen bloque certaines fonctionnalites pendant le saut du personnage, par exemple le deplacement
    private Data_keeper _data;

    void Start()
    {
        GameObject Network_Data = GameObject.Find("Network_Data");
        _data = (Data_keeper)Network_Data.GetComponent("Data_keeper");
    }

    void FixedUpdate()
    {
        if ((isJumping == false) && ((_network.isMine) || (_data.gameMode == "Solo")))// Bloque le controle du personnage pendant le saut
        {
            Vector3 direction = new Vector3(0, 0, 0);

            // Dans InputManager, recupere les etats de l'axe "Horizontal" (touches 'q' ou 'd') et de l'axe "Horizontal" (touches 'z' ou 's')
            // En fonction des états de Z,Q,S,D, on créé une direction
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if ((Input.GetAxis("Horizontal") > 0)||(Input.GetAxis("Vertical") >0))
                _walk.SetBool("Walk", true);
            else
                _walk.SetBool("Walk", false);

            direction = transform.TransformDirection(direction) * speed;

            // Calcul du vecteur de déplacement
            Vector3 actualVel = new Vector3(_rigibodyPlayer.velocity.x, 0,_rigibodyPlayer.velocity.z);
            Vector3 WantedVel = (direction - actualVel);
            _rigibodyPlayer.AddForce(WantedVel, ForceMode.VelocityChange);
            _rigibodyPlayer.AddForce(0, gravity, 0);

            // Si le joueur appui sur la touche "jump", "space" par defaut
            if (Input.GetButton("Jump"))
            {
                // D'après la formule Vitesse = RacineCarré( 2 * Hauteur * Gravité), on obtient la vitesse du saut à l'instant t0
                _rigibodyPlayer.velocity = new Vector3(actualVel.x, Mathf.Sqrt(2 * heightJump * -gravity), actualVel.z);
                isJumping = true;
            }
        }
    }

    // Se déclenche dès que le personnage touche un autre objet
    void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
    }
}
