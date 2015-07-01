﻿// --------------------------------------------------
// Project: Darkness Defender
// Script: EnemiesScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Script unités
// 
// --------------------------------------------------
public class EnemiesScript : MonoBehaviour 
{
    public int id = 0;

    [SerializeField]
    public int health;

    [SerializeField]
    EnemyPoolScript poolRappel;

    [SerializeField]
    private int actualHealth;

    [SerializeField]
    Transform _transform;

    [SerializeField]
    Rigidbody _rigidbody;

    public Transform Transform
    {
        get
        {
            return _transform;
        }
        set
        {
            _transform = value;
        }
    }

    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }
        set
        {
            _rigidbody = value;
        }
    }

    [SerializeField]
    NavMeshAgent _agent;

    [SerializeField]
    Animator _Move;

    [SerializeField]
    NetworkView _network;

    private GameObject player;
    private PlayerScript playerScript;

    void Start()
    {
        player       = GameObject.FindGameObjectWithTag("Player");
        playerScript = (PlayerScript) player.GetComponent("PlayerScript");
        _Move.SetBool("Walk", true); 
    }

    void FixedUpdate()
    {
        if(Network.isServer)
            _agent.SetDestination(player.transform.position);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            StartCoroutine("TakeDamage");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            StopCoroutine("TakeDamage");
        }
    }

    IEnumerator TakeDamage()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            playerScript.Health -= 2;
        }
    }

    public void impactTower(int damage)
    {
        new WaitForSeconds(0.5f);
        actualHealth -= damage;

        if (actualHealth <= 0)
            killEnemy();
    }

    void killEnemy()
    {
        poolRappel.ReturnEnemy(this);

        actualHealth = health;
    }

    public void InitEnemy()
    {
        actualHealth = health;
    }
}
