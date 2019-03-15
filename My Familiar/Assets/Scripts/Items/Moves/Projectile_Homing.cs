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
        float damageModifier = 1f;

        if(characterToDamage.tag == "Player")
        {
            // Set damage modifier
            if (characterToDamage.GetComponent<Character>().CharactersElementTypes.Contains(itemType))
            {
                // Take half damage as character type matches projectile type
                damageModifier = 0.5f;
            }
            //else if(true)
            //{
            //    damageModifier = 2f;
            //}

            // Deal damage
            characterToDamage.GetComponent<Character>().ChangeHealth((int)(-Damage * damageModifier));
        }
        else // Enemy
        {
            // Set damage modifier

            // Deal damage
            characterToDamage.GetComponent<Enemy>().ChangeHealth((int)(-Damage * damageModifier));
        }        
        
        // Destroy self
        Destroy(gameObject);
    }
}
