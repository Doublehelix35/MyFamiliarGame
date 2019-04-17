using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Egg : Item
{
    // Egg that player hatches from when first created

    public Sprite[] EggSprites;
    int i; // increment through egg sprites

    // GM calls this when this is touched
    public override void Interact(GameObject GM)
    {
        // Change egg sprite
        i++;
        Mathf.Clamp(i, 0f, EggSprites.Length); // Clamp between 0 and length of egg sprites
        gameObject.GetComponent<SpriteRenderer>().sprite = EggSprites[i];

        // Reduce uses
        if (Uses <= 0)
        {
            // Spawn player
            GM.GetComponent<GameManager>().ReloadCharacter();

            // Destroy self
            Destroy(gameObject);
        }
        else
        {
            Uses--;
        }
    }
}
