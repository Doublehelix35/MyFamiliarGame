using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Apple : Item
{
    public int FullnessChangeValue;

    // Player calls this when Apple collides with it
    public override void Interact(GameObject player)
    {
        // Give Health 
        player.GetComponent<Character>().ChangeHealth(Damage); // Damage is positive and gives health

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Give Fullness
        player.GetComponent<Character>().ChangeFullness(FullnessChangeValue);

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        // Destroy self
        Destroy(gameObject);
    }
}

