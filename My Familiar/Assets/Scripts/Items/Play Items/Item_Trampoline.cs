using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Trampoline : Item
{
    public float BounceMagnitude = 100f;

    // Player calls this when Trampoline collides with it
    public override void Interact(GameObject player)
    {
        // Bounce player up
        player.GetComponent<Rigidbody>().AddExplosionForce(BounceMagnitude, transform.position, 0f, 1f, ForceMode.Impulse);

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        // Destroy self
        if (Uses <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Uses--;
        }
        
    }
}
