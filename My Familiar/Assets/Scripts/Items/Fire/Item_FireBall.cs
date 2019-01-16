using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FireBall :  Item {

    // Stats
    public float Speed = 1f;
    public float lifeSpan = 6f;
    public int Damage = 1;
    public int HealthMax = 1;
    public int UsesMax = 1;
    public int ExpPointsGivenMax = 5;

	void Start ()
    {
        // Init stats
        Health = HealthMax;
        Uses = UsesMax;
        ExpPointsGiven = ExpPointsGivenMax;
        itemType = Elements.ElementType.Fire;

        Destroy(gameObject, lifeSpan);
    }
	
	void Update ()
    {
        if(Health <= 0 || Uses <= 0)
        {
            Destroy(gameObject);
        }		
	}

    // Player calls this when fireball collides with it
    public override void Interact(GameObject player)
    {
        // Deal damage
        player.GetComponent<Character>().ChangeHealth(-Damage);

        // Destroy self
        Destroy(gameObject);
    }
}
