using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FireBall :  Item
{    
    // Player calls this when fireball collides with it
    public override void Interact(GameObject player)
    {
        // Deal damage
        player.GetComponent<Character>().ChangeHealth(-Damage);

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Give spec points
        player.GetComponent<Character>().GainElementSpecPoints(Elements.ElementType.Fire, SpecPointsGiven);

        // Give exp
        player.GetComponent<Character>().GainExp(ExpPointsGiven);

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        // Destroy self
        Destroy(gameObject);
    }
}
