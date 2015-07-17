// --------------------------------------------------
// Project: Darkness Defender
// Script: ShooterScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------
// 
// Tower 1 - Shooter
// 
// --------------------------------------------------
public class ShooterScript : MonoBehaviour 
{
    [SerializeField]
    Transform _transform;

    [SerializeField]
    AmmoPoolScript _ammoShooter;

    [SerializeField]
    float _projectileSpeed;

    [SerializeField]
    float _shootDelay;

    [SerializeField]
    public Material[] _levels;

    [SerializeField]
    Renderer[] _indicLvl;

    private int damageTower = 2;
    public int levelTower  = 0;

    private List<EnemiesScript> _enemiesScript = new List<EnemiesScript>();

    private IEnumerator ShootCoroutine = null;


    void OnTriggerEnter(Collider enemy)
    {
        if (enemy.tag == "Enemy")
        {
            _enemiesScript.Add((EnemiesScript)enemy.gameObject.GetComponent("EnemiesScript"));
            if (_enemiesScript.Count == 1 && ShootCoroutine == null)
            {
                ShootCoroutine = TryToShoot();
                StartCoroutine(ShootCoroutine);
            }
        }
    }

    void OnTriggerExit(Collider enemy)
    {
        if (enemy.tag == "Enemy")
        {
            _enemiesScript.Remove((EnemiesScript)enemy.gameObject.GetComponent("EnemiesScript"));
            if (_enemiesScript.Count == 0 && ShootCoroutine != null)
            {
                StopCoroutine(ShootCoroutine);
                ShootCoroutine = null;
            }
        }
    }

    IEnumerator TryToShoot()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (_enemiesScript[0].transform.position == new Vector3(0, -100, 0))
                _enemiesScript.Remove(_enemiesScript[0]);

            if (_enemiesScript.Count == 0)
            {
                StopCoroutine(ShootCoroutine);
                ShootCoroutine = null;
            }
            else
            {
                AmmoScript ps = _ammoShooter.GetProjectile();

                if (ps != null)
                { 
                    ps.gameObject.SetActive(true);

                    ps.Transform.position = _transform.position + new Vector3(0, 2, 0);
                    Vector3 Actual = _enemiesScript[0].transform.position;
                    ps.Rigidbody.velocity = (Actual - ps.Transform.position).normalized * _projectileSpeed;

                    yield return new WaitForSeconds(_shootDelay);
                    _enemiesScript[0].impactTower(damageTower);
                    _ammoShooter.ReturnProjectile(ps); 
                }
            } 
        }
    }

    public void RazLevel()
    {
        ChangeColor(0);
        damageTower = 2;
        levelTower  = 1;
    }

    public bool UpgradeTower()
    {
        bool result = false;

        if(levelTower < 4)
        {
            damageTower += 2;
            _shootDelay -= 0.2f;
            levelTower++;
            ChangeColor(levelTower);
            result = true;
        }

        return result;
    }

    private void ChangeColor(int level)
    {
        foreach (Renderer indic in _indicLvl)
        {
            indic.material = _levels[level];
        }
    }
}
