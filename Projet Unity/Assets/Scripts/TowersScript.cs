using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowersScript : MonoBehaviour
{
    [SerializeField]
    private int _ArrowSpeed;

    [SerializeField]
    private GameObject _Tower;

    private List<Transform> _enemiesTransform;
    private bool Reloaded = true;
    private bool noTarget = true;
    private bool outRange;
    private Collider target;


    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1);
        Reloaded = true;
    }

    // Use this for initialization
    void Start()
    {
        _enemiesTransform = new List<Transform>();
    }

    void OnTriggerEnter(Collider col)
    {
        _enemiesTransform.Add(col.transform);
        if (_enemiesTransform.Count == 0)
            StartCoroutine(arrowFire());
    }


    void OnTriggerExit(Collider col)
    {
        _enemiesTransform.Remove(col.transform);
        if (_enemiesTransform.Count == 0)
            StopCoroutine(arrowFire());
    }

    void lockTarget(Collider col)
    {
        noTarget = false;
        target = col;
        StartCoroutine(arrowFire());
    }

    IEnumerator arrowFire()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            int health = (target.GetComponent("EnemyHealth") as EnemyHealth).health;
            while ((health != 0) && (target.tag == "Enemy") && (!outRange))
            {
                GameObject Arrow = (GameObject)Instantiate(Resources.Load("Prefab/WeaponTowers/Arrow"));
                (Arrow.GetComponent("ArrowScript") as ArrowScript).arrow = Arrow;
                (Arrow.GetComponent("ArrowScript") as ArrowScript).target = target;
                (Arrow.GetComponent("ArrowScript") as ArrowScript).speed = _ArrowSpeed;
                Arrow.transform.position = new Vector3(
                        _Tower.transform.position.x,
                        _Tower.transform.position.y + 3,
                        _Tower.transform.position.z);
                Arrow.rigidbody.velocity = Vector3.zero;
                Arrow.rigidbody.AddForce(
                    new Vector3(
                        target.transform.position.x - Arrow.transform.position.x,
                        target.transform.position.y - Arrow.transform.position.y,
                        target.transform.position.z - Arrow.transform.position.z
                        ).normalized * _ArrowSpeed, ForceMode.VelocityChange);

                yield return new WaitForSeconds(1);
            }
            noTarget = true;
        }
    }

}
