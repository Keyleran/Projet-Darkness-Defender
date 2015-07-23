// --------------------------------------------------
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
    public NavMeshAgent _agent;

    [SerializeField]
    NetworkView _network;

    [SerializeField]
    public Material[] _levels;

    [SerializeField]
    Renderer[] _indicLvl;

    public int levelEnnemi = 0;
    public int Loop = 0;
    public int actualHealth = 20;
    public int damage = 2;
    private GameObject player;
    private PlayerScript playerScript;

    void FixedUpdate()
    {
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

            playerScript.Health -= damage;
        }
    }

    public void impactTower(int damage)
    {
        if(Network.isServer)
        {
            new WaitForSeconds(0.5f);
            actualHealth -= damage;

            if (actualHealth <= 0)
                killEnemy();
        }
    }

    public void SetTarget(GameObject playerTarget, PlayerScript playerScriptTarget)
    {
        player = playerTarget;
        playerScript = playerScriptTarget;
    }

    void killEnemy()
    {
        if (Network.isServer)
        {
            poolRappel.ReturnEnemy(id);
        }
    }

    public void InitEnemy(int level, int waveLoop)
    {
        levelEnnemi = level;
        Loop = waveLoop;

        ChangeColor(levelEnnemi);

        levelEnnemi = levelEnnemi == 0 ? 1 : levelEnnemi;
        Loop = Loop == 0 ? 1 : Loop;
        damage = 2 * (int) Mathf.Pow(levelEnnemi, 2) * (int) Mathf.Pow(2, Loop);

        actualHealth = health * levelEnnemi * Loop;
    }

    private void ChangeColor(int level)
    {
        foreach (Renderer indic in _indicLvl)
        {
            indic.material = _levels[level];
        }
    }
}
