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
// Script of "Players"
// 
// --------------------------------------------------
public class ControlCharacterScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigibodyPlayer = new Rigidbody();

    [SerializeField]
    CapsuleCollider colliderPlayer = new CapsuleCollider();

    // Parametres personnages
    private float speed      = 10.0f; // Vitesse de deplacement
    private float gravity    = 9.81f; // Gravité
    private float heightJump = 1.0f; // Hauteur des sauts
    private bool  isJumping  = false; // Ce booleen bloque certaines fonctionnalites pendant le saut du personnage, par exemple le deplacement


    void FixedUpdate()
    {
        if (isJumping == false) // Bloque le controle du personnage pendant le saut
        {
            // Dans InputManager, recupere les etats de l'axe "Horizontal", si une des touches ('q' ou 'd') est entrée, on applique un mouvement
            if (Input.GetAxis("Vertical") > 0)
            {
                transform.position = new Vector3(0, 0, speed);
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                transform.position = new Vector3(0, 0, -speed);
            }

            // Dans InputManager, recupere les etats de l'axe "Horizontal", si une des touches ('q' ou 'd') est entrée, on applique un mouvement
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.position = new Vector3(speed, 0, 0);
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.position = new Vector3(-speed, 0, 0);
            }

        }
    }
}
