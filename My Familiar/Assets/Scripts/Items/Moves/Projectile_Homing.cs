using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Homing : Item
{
    // Damage is set in Elements as it does the calculations

    internal Transform Target;
    public float ProjectileForce;

    // Update is called once per frame
    void Update()
    {
        // Aim and head towards target using force
        Vector3 dir = Target.position - transform.position;
        gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * ProjectileForce * Time.deltaTime, ForceMode.Force);
    }

    // Player calls this when Projectile collides with it
    public override void Interact(GameObject characterToDamage)
    {
        if(characterToDamage.tag == "Player")
        {
            // Deal damage
            characterToDamage.GetComponent<Character>().ChangeHealth(-Damage);
        }
        else // Enemy
        {
            // Deal damage
            characterToDamage.GetComponent<Enemy>().ChangeHealth(-Damage);
        }        
        
        // Destroy self
        Destroy(gameObject);
    }
}
