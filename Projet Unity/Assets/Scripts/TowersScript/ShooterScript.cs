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

    private List<Transform> _enemiesTransform = new List<Transform>();

    private IEnumerator ShootCoroutine = null;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            _enemiesTransform.Add(col.transform);
            if (_enemiesTransform.Count == 1 && ShootCoroutine == null)
            {
                ShootCoroutine = TryToShoot();
                StartCoroutine(ShootCoroutine);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            _enemiesTransform.Remove(col.transform);
            if (_enemiesTransform.Count == 0 && ShootCoroutine != null)
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
            AmmoScript ps = _ammoShooter.GetProjectile();

            if (ps != null)
            {
                ps.gameObject.SetActive(true);

                ps.Transform.position = _transform.position + new Vector3(0, 2, 0);

                // TEMPORAIRE !!!
                if (_enemiesTransform.Count == 0)
                {
                    StopCoroutine(ShootCoroutine);
                    ShootCoroutine = null;
                }
                else if (_enemiesTransform[0].position == new Vector3(0, -5, 0))
                    _enemiesTransform.Remove(_enemiesTransform[0]);
                else
                {
                    ps.Rigidbody.velocity = (_enemiesTransform[0].position - ps.Transform.position).normalized * _projectileSpeed;

                    yield return new WaitForSeconds(_shootDelay);
                }

                //---------------

            }
        }
    }
}
