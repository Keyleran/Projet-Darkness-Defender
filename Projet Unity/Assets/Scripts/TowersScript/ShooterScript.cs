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
// Script ajoutant des champs aux Tours "Shooter"
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

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            _enemiesTransform.Add(col.transform);
            if (_enemiesTransform.Count == 1)
            {
                StartCoroutine("TryToShoot");
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            _enemiesTransform.Remove(col.transform);
            if (_enemiesTransform.Count == 0)
            {
                StopCoroutine("TryToShoot");
            }
        }
    }

    IEnumerator TryToShoot()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            var ps = _ammoShooter.GetProjectile();

            ps.gameObject.SetActive(true);

            ps.Transform.position = _transform.position;

            ps.Rigidbody.velocity = (_enemiesTransform[0].position - _transform.position).normalized * _projectileSpeed;

            yield return new WaitForSeconds(_shootDelay);
        }
    }
}
