using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    // Parent class for all items

    // Potential stats    
    protected int Health = 1;
    protected int Uses = 1;
    protected int ExpPointsGiven = 0;

    // Item type
    protected Elements.ElementType itemType;

    // Constructor
    protected Item() { }


    // Define how the item interacts with the familiar
    public abstract void Interact(GameObject player);

    protected virtual void Destroy()
    {
        // Default way to destroy item
        // Override if a unique destruction is needed
    }

    public Elements.ElementType GetItemType()
    {
        return itemType;
    }

}
