using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Apple : Item
{
    // Player calls this when Apple collides with it
    public override void Interact(GameObject player)
    {
        // Give Health 
        player.GetComponent<Character>().ChangeHealth(Damage); // Damage is positive and gives health

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Destroy self
        Destroy(gameObject);
    }
}

