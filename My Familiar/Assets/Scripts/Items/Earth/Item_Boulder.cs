using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Boulder : Item
{
    public Vector3 sizeModifier = new Vector3( 0.5f, 0.2f, 0.5f );

    // Player calls this when character is thrown agaisnt boulder
    public override void Interact(GameObject player)
    {
        // Deal damage
        player.GetComponent<Character>().ChangeHealth(-Damage);

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Give spec points
        player.GetComponent<Character>().GainElementSpecPoints(Elements.ElementType.Earth, SpecPointsGiven);

        // Give exp
        player.GetComponent<Character>().GainExp(ExpPointsGiven);

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        if (Health <= 1)
        {
            // Destroy self
            Destroy(gameObject);
        }
        else
        {
            Health--;
            transform.localScale -= sizeModifier;
        }        
    }
}
