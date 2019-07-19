using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Football : Item
{
    // Player calls this when Football collides with it
    public override void Interact(GameObject player)
    {
        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        // Destroy self
        if (Uses <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Uses--;
        }

    }
}
