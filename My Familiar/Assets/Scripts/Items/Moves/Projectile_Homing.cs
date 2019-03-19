using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Homing : Item
{
    // Damage is set in Elements as it does the calculations

    internal Transform Target;
    public float ProjectileForce;

    Elements ElementsRef;
    

    void Start()
    {
        ElementsRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<Elements>();

        if(ElementsRef == null)
        {
            Debug.Log("Error! Projectile couldnt find Game Controller!");
        }
    }

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

        // Do the same thing for player and enemy but use their own script
        if(characterToDamage.tag == "Player")
        {
            Character charRef = characterToDamage.GetComponent<Character>();
            
            // Loop through character types to determine damage modifier
            foreach (Elements.ElementType t in charRef.CharactersElementTypes)
            {
                if(t == itemType) // If they are same type
                {
                    // Reduce damage modifier by 50%
                    damageModifier *= 0.5f;
                }
                else if(ElementsRef.CheckTypeRelationship(t, itemType)) // Check for type relationship
                {
                    // If item type is stronger than t then deal more damage
                    if (ElementsRef.ReturnStrongTypeInRelationship(t, itemType) == itemType) 
                    {
                        damageModifier *= 2f;
                    }
                }
            }
            // Deal damage
            characterToDamage.GetComponent<Character>().ChangeHealth((int)(-(Damage * damageModifier)));
        }
        else // Enemy
        {
            Enemy enemyRef = characterToDamage.GetComponent<Enemy>();

            // Loop through character types to determine damage modifier
            foreach (Elements.ElementType t in enemyRef.CharactersElementTypes)
            {
                if (t == itemType) // If they are same type
                {
                    // Reduce damage modifier by 50%
                    damageModifier *= 0.5f;
                }
                else if (ElementsRef.CheckTypeRelationship(t, itemType)) // Check for type relationship
                {
                    // If item type is stronger than t then deal more damage
                    if (ElementsRef.ReturnStrongTypeInRelationship(t, itemType) == itemType)
                    {
                        damageModifier *= 2f;
                    }
                }
            }           
            // Deal damage
            characterToDamage.GetComponent<Enemy>().ChangeHealth((int)(-(Damage * damageModifier)));
        }        
        
        // Destroy self
        Destroy(gameObject);
    }
}
