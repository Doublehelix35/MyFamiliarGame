using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Swing : Item
{
    // Player calls this when Swing collides with it
    public override void Interact(GameObject player)
    {
        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

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