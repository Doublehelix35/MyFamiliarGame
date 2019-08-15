using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Subject
{
    // Parent class for all items

    // Potential stats    
    public int Health = 1;
    public int Uses = 1;
    public int Damage = 0;
    public int ExpPointsGiven = 0;
    public int SpecPointsGiven = 0;
    public int HappinessChangeValue = 0;
    public float LifeSpan = 5f;
    public int GoldToGive = 0;

    public int Cost = 0;

    // Item type
    public Elements.ElementType itemType;

    // Constructor
    protected Item() { }

    void Start()
    {
        // Destroy self after lifespan runs out
        //Destroy(gameObject, lifeSpan);
    }

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
