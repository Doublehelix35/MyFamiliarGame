using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_StormOrb : Item
{
    public float WindMagnitude = 100f;

    // Player calls this when Storm Ball collides with it
    public override void Interact(GameObject player)
    {
        // Push player away
        player.GetComponent<Rigidbody>().AddExplosionForce(WindMagnitude, transform.position, 0f, 0.05f, ForceMode.Impulse);

        // Deal damage
        player.GetComponent<Character>().ChangeHealth(-Damage);

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Give spec points
        player.GetComponent<Character>().GainElementSpecPoints(Elements.ElementType.Air, SpecPointsGiven);

        // Give exp
        player.GetComponent<Character>().GainExp(ExpPointsGiven);

        // Destroy self
        if(Uses <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Uses--;
        }
        
    }
}