using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Egg : Item
{
    // Egg that player hatches from when first created

    public Sprite[] EggSprites;
    int i = 0; // increment through egg sprites

    // GM calls this when this is touched
    public override void Interact(GameObject GM)
    {
        // Change egg sprite
        i++;
        i = Mathf.Clamp(i, 0, EggSprites.Length - 1); // Clamp between 0 and length of egg sprites - 1
        gameObject.GetComponent<SpriteRenderer>().sprite = EggSprites[i];

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        // Reduce uses
        if (Uses <= 0)
        {
            // Spawn player
            GM.GetComponent<GameManager>().ReloadCharacter();
            GM.GetComponent<GameManager>().HideNest();

            // Destroy self
            Destroy(gameObject);
        }
        else
        {
            Uses--;
        }
    }
}
