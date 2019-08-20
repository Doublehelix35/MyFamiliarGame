using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_WaterFall : Item
{
    // Two object have this script the gun and the water projectiles
    // The gun object manages uses, shooting and self-destruction
    public GameObject GunRef;
    public GameObject WaterProjectile;
    public Transform FirePoint;
    public float ShootFrequency;
    float LastShootTime;
    public float ShootMagnitude;

    void Start()
    {
        LastShootTime = Time.time; // Init last shoot time
    }

    void Update()
    {
        if (GunRef == gameObject && LastShootTime <= Time.time - ShootFrequency) // Is this gameobject the gun?
        {
            // Shoot projectiles
            GameObject GO = Instantiate(WaterProjectile, FirePoint.position, Quaternion.identity);

            // Give projectile velocity
            GO.GetComponent<Rigidbody>().velocity = (FirePoint.position - GunRef.transform.position).normalized * ShootMagnitude;

            // Set watergun ref in projectile spawned
            GO.GetComponent<Item_WaterFall>().GunRef = gameObject;

            // Reset last shoot time
            LastShootTime = Time.time;
        }
    }

    // Player calls this when Water Fall collides with it
    public override void Interact(GameObject player)
    {
        // Deal damage
        player.GetComponent<Character>().ChangeHealth(-Damage);

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Give spec points
        player.GetComponent<Character>().GainElementSpecPoints(Elements.ElementType.Water, SpecPointsGiven);

        // Give exp
        player.GetComponent<Character>().GainExp(ExpPointsGiven);

        // Destroy self
        if (GunRef.GetComponent<Item_WaterFall>().Uses <= 0)
        {
            Destroy(GunRef.transform.parent.gameObject);
        }
        else
        {
            GunRef.GetComponent<Item_WaterFall>().Uses--;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GunRef != gameObject) // Only water projectile should do this
        {
            // Notify observers
            GunRef.GetComponent<Item_WaterFall>().Notify(gameObject, Observer.Events.ItemUsed);

            // Destroy self if collison detected
            Destroy(gameObject);
        }
    }
}

