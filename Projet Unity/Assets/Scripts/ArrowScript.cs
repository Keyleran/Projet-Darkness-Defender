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
// Script of Arrow
// 
// --------------------------------------------------
public class ArrowScript : MonoBehaviour 
{
    [SerializeField]
    Transform _transform;

    public Transform Transform
    {
        get { return _transform; }
        set { _transform = value; }
    }

    [SerializeField]
    Rigidbody _rigidbody;

    [SerializeField]
    float _desactivateDelay;

    // Projectile
    public GameObject arrow;

    // Target
    public Collider target;

    // Speed
    public int speed;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        // If target is an Enemy
        if (target.tag == "Enemy")
        {
            arrow.rigidbody.velocity = Vector3.zero; // Reset all forces
            arrow.rigidbody.AddForce(new Vector3(
                    target.transform.position.x - arrow.transform.position.x,
                    target.transform.position.y - arrow.transform.position.y,
                    target.transform.position.z - arrow.transform.position.z
                    ).normalized * speed, ForceMode.VelocityChange); // Add new force
        }
        else // if the target is destroy
        {
            Destroy(arrow);
        }
	}
   

    void OnTriggerEnter(Collider _EnCol)
    {
        // When the arrow enter in the target's collider
        if (_EnCol.tag == "Enemy")
        {
            Destroy(arrow);
        }
    }
}
