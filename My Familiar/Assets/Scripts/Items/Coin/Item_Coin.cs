using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Coin : Item
{
    // Player calls this when coin collides with it
    public override void Interact(GameObject player)
    {
        // Give gold
        player.GetComponent<Character>().ChangeGold(GoldToGive);

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        // Destroy self
        Destroy(gameObject);
    }
}
