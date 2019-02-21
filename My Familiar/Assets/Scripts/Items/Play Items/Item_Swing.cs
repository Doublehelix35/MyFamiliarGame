using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Swing : Item
{
    public GameObject SwingParent; // Highest object in parent heirarchy
    public GameObject ConnectionPoint; // Point that connects player to swing

    // Player calls this when Swing collides with it
    public override void Interact(GameObject player)
    {
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

    //void Attach(GameObject objectToAttachTo)
    //{
    //    // Attach to Balloon Bottom and set up spring joint
    //    Balloon_Bottom.AddComponent<SpringJoint>(); // Add joint to balloon bottom
    //    Balloon_Bottom.GetComponent<SpringJoint>().connectedBody = objectToAttachTo.GetComponent<Rigidbody>(); // Attach bottom to object
    //    Balloon_Bottom.GetComponent<SpringJoint>().axis = new Vector3(0f, 0f, 1f); // Set axis of joint

    //    // Set up Spring joint distances
    //    Balloon_Bottom.GetComponent<SpringJoint>().minDistance = MinDistFromBalloon;
    //    Balloon_Bottom.GetComponent<SpringJoint>().maxDistance = MaxDistFromBalloon;

    //    // Turn off string object
    //    Balloon_String.SetActive(false);

    //    // Set attached object force modifier
    //    AttachedObjectForceModifier = AttachedObjectForceModifierMax;

    //    // Set is player attached to true
    //    IsPlayerAttached = true;
    //}

    //void Detach(GameObject objectToDetachFrom)
    //{
    //    // Detach to String
    //    Destroy(Balloon_Bottom.GetComponent<SpringJoint>()); // Delete joint

    //    // Turn off string line render
    //    // Set pos 0 to balloon bottom
    //    Balloon_Bottom.GetComponent<LineRenderer>().SetPosition(0, Balloon_Bottom.transform.position);
    //    // Set pos 1 to balloon bottom
    //    Balloon_Bottom.GetComponent<LineRenderer>().SetPosition(1, Balloon_Bottom.transform.position);

    //    // Turn on string object
    //    Balloon_String.SetActive(true);

    //    // Reset attached object force modifier
    //    AttachedObjectForceModifier = 1f;

    //    // Set is player attached to false
    //    IsPlayerAttached = false;
    //}
}