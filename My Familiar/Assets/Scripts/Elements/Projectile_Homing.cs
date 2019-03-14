using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Homing : MonoBehaviour
{
    internal Transform Target;
    internal float Damage;
    public Elements.ElementType ProjectileType;
    public float ProjectileForce;


    // Update is called once per frame
    void Update()
    {
        // Aim and head towards target using force
        Vector3 dir = Target.position - transform.position;
        gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * ProjectileForce * Time.deltaTime, ForceMode.Force);
    }
}
