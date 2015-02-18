// --------------------------------------------------
// Project: Darkness Defender
// Script: ControlCharacterScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------
// 
// Script de Déplacements
// 
// --------------------------------------------------
public class ControlCharacterScript : MonoBehaviour
{
    // Prefab Player
    // Le prafab "player" est constitué de 2 éléments:
    // - Une Caméra
    // - Un modèle 3D:
    //      - une "capsule collider"
    //      - un rigidbody, avec les axes de rotations X,Y,Z bloqués pour éviter la chute du personnage sur un côté
    //--------------------------
    
    [SerializeField]
    Rigidbody _rigibodyPlayer = new Rigidbody();

    // Parametres personnages
    private float speed      = 10.0f; // Vitesse de deplacement
    private float gravity    = 9.81f; // Gravité terrestre 9.81 m/s²
    private float heightJump = 1.0f; // Hauteur des sauts
    private bool  isJumping  = false; // Ce booleen bloque certaines fonctionnalites pendant le saut du personnage, par exemple le deplacement

    // Partie Réseau
    class sPlayerIntents
    {
        public float _horizontal = 0f;
        public float _vertical   = 0f;
        public bool  _jump       = false;
    }

    private Dictionary<NetworkPlayer, sPlayerIntents> _playersIntents;
    private Dictionary<NetworkPlayer, sPlayerIntents> PlayersIntents
    {
        get { return _playersIntents; }
        set { _playersIntents = value; }
    }

    private NetworkView _networkView = null;
    //--------------------------------------

    void Start()
    {
        PlayersIntents = new Dictionary<NetworkPlayer, sPlayerIntents>();
        _networkView = this.gameObject.GetComponent<NetworkView>();
    }

    void OnPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new sPlayerIntents());
        _networkView.RPC("NewPlayerConnected", RPCMode.OthersBuffered, p);
    }

    [RPC]
    void NewPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new sPlayerIntents());
    }

    // Update is called once per frame
    void Update()
    {
        if (Network.isClient)
        {
            _networkView.RPC("PlayerWantToMove", RPCMode.Server, Network.player, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetButton("Jump"));
        }
    }

    void FixedUpdate()
    {
        foreach (var p in PlayersIntents)
        {
            Vector3 direction = new Vector3(0, 0, 0);

            if (isJumping == false) // Bloque le controle du personnage pendant le saut
            {
                // Dans InputManager, recupere les etats de l'axe "Horizontal" (touches 'q' ou 'd') et de l'axe "Horizontal" (touches 'z' ou 's')
                // En fonction des états de Z,Q,S,D, on créé une direction
                direction = new Vector3(p.Value._horizontal, 0, p.Value._vertical);

                direction = transform.TransformDirection(direction) * speed;


                // Calcul du vecteur de déplacement
                Vector3 actualVel = _rigibodyPlayer.velocity;
                Vector3 WantedVel = (direction - actualVel);
                _rigibodyPlayer.AddForce(WantedVel, ForceMode.VelocityChange);

                // Si le joueur appui sur la touche "jump", "space" par defaut
                if ((isJumping == false) && p.Value._jump)
                {
                    // D'après la formule Vitesse = RacineCarré( 2 * Hauteur * Gravité), on obtient la vitesse du saut à l'instant t0
                    _rigibodyPlayer.velocity = new Vector3(actualVel.x, Mathf.Sqrt(2 * heightJump * gravity), actualVel.z);
                    isJumping = true;
                }
            }
        }
    }

    [RPC]
    void PlayerWantToMove(NetworkPlayer p, float Hori, float Vert, bool Jump)
    {
        PlayersIntents[p]._horizontal = Hori;
        PlayersIntents[p]._vertical   = Vert;
        PlayersIntents[p]._jump       = Jump;
        if (Network.isServer)
        {
            _networkView.RPC("PlayerWantToMove", RPCMode.Others, p, Hori, Vert, Jump);
        }
    }


    // Se déclenche dès que le personnage touche un autre objet
    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Ground")||(collision.gameObject.tag == "Barricade"))
        {
            isJumping = false;
        }
    }
}
