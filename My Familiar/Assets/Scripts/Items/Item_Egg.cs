using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Egg : Item
{
    // Egg that player hatches from when first created

    // GM calls this when this is touched
    public override void Interact(GameObject player)
    {
        // Change egg sprite

        // Destroy self
        if (Uses <= 0)
        {
            // Spawn player

            // Destroy
            Destroy(gameObject);
        }
        else
        {
            Uses--;
        }
    }
}
