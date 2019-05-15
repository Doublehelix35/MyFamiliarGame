using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Swing : Item
{
    public GameObject SwingParent; // Highest object in parent heirarchy
    public GameObject ConnectionPoint; // Point that connects player to swing
    public float MinDistFromSwing; // Min distance from swing when player attached
    public float MaxDistFromSwing = 1f; // Max distance from swing when player attached
    bool IsPlayerAttached = false;

    // Player calls this when Swing collides with it
    public override void Interact(GameObject player)
    {
        // If player isn't attached then attach it
        if (!IsPlayerAttached)
        {
            Attach(player);
        }

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

        // Notify observers
        Notify(gameObject, Observer.Events.ItemUsed);

        // Destroy parent object
        if (SwingParent.GetComponent<Item_Swing>().Uses <= 0)
        {
            Destroy(SwingParent);
        }
        else
        {
            SwingParent.GetComponent<Item_Swing>().Uses--; // Only parent object tracks uses.
        }
    }

    void Attach(GameObject objectToAttachTo)
    {
        // Attach to Balloon Bottom and set up spring joint
        ConnectionPoint.AddComponent<SpringJoint>(); // Add joint to balloon bottom
        ConnectionPoint.GetComponent<SpringJoint>().connectedBody = objectToAttachTo.GetComponent<Rigidbody>(); // Attach bottom to object
        ConnectionPoint.GetComponent<SpringJoint>().axis = new Vector3(0f, 0f, 1f); // Set axis of joint
        ConnectionPoint.GetComponent<SpringJoint>().enablePreprocessing = false; // Set preprocessing to false

        // Set up Spring joint distances
        ConnectionPoint.GetComponent<SpringJoint>().minDistance = MinDistFromSwing;
        ConnectionPoint.GetComponent<SpringJoint>().maxDistance = MaxDistFromSwing;

        // Set is player attached to true
        IsPlayerAttached = true;
    }

    void Detach(GameObject objectToDetachFrom)
    {
        // Detach to String
        Destroy(ConnectionPoint.GetComponent<SpringJoint>()); // Delete joint
        // Set is player attached to false
        IsPlayerAttached = false;
    }
}