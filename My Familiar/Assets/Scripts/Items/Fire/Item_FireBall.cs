using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FireBall :  Item {

    // Stats
    public float lifeSpan = 10f;
    public int Damage = 1;
    public int ExpPointsGivenMax = 5;
    public int SpecPointsGivenMax = 1;
    public int HappinessChangeValueMax = -1;

	void Start ()
    {
        // Init stats
        ExpPointsGiven = ExpPointsGivenMax;
        SpecPointsGiven = SpecPointsGivenMax;
        HappinessChangeValue = HappinessChangeValueMax;
        itemType = Elements.ElementType.Fire;

        // Destroy self after lifespan runs out
        Destroy(gameObject, lifeSpan);
    }
	

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

        // Destroy self
        Destroy(gameObject);
    }
}
