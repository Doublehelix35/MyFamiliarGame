using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Swing : Item
{
    public GameObject SwingParent; // Highest object in parent heirarchy
    public GameObject ConnectionPoint; // Point that connects player to swing
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
        ConnectionPoint.AddComponent<HingeJoint>(); // Add joint to balloon bottom
        ConnectionPoint.GetComponent<HingeJoint>().connectedBody = objectToAttachTo.GetComponent<Rigidbody>(); // Attach bottom to object
        ConnectionPoint.GetComponent<HingeJoint>().axis = new Vector3(0f, 0f, 1f); // Set axis of joint
        ConnectionPoint.GetComponent<HingeJoint>().enablePreprocessing = false; // Set preprocessing to false
        
        // Set is player attached to true
        IsPlayerAttached = true;
    }

    void Detach(GameObject objectToDetachFrom)
    {
        // Detach to String
        Destroy(ConnectionPoint.GetComponent<HingeJoint>()); // Delete joint
        // Set is player attached to false
        IsPlayerAttached = false;
    }
}