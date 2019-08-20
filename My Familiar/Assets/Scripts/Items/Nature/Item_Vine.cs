using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Vine : Item
{
    public GameObject HighestParentVine; // Highest vine in parent heirarchy

    // Player calls this when vine collides with it
    public override void Interact(GameObject player)
    {
        // Deal damage
        player.GetComponent<Character>().ChangeHealth(-Damage);

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Give spec points
        player.GetComponent<Character>().GainElementSpecPoints(Elements.ElementType.Nature, SpecPointsGiven);

        // Give exp
        player.GetComponent<Character>().GainExp(ExpPointsGiven);

        // Notify observers
        HighestParentVine.GetComponent<Item_Vine>().Notify(gameObject, Observer.Events.ItemUsed);

        // Destroy whole vine
        if (HighestParentVine.GetComponent<Item_Vine>().Uses <= 0)
        {
            Destroy(HighestParentVine);
        }
        else
        {
            HighestParentVine.GetComponent<Item_Vine>().Uses--; // Only one vine part track the uses
        }
        
    }
}
