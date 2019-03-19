using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Balloon : Item
{
    public float FloatForce = 100f;
    public float AttachedObjectForceModifierMax;
    float AttachedObjectForceModifier = 1f;
    bool IsPlayerAttached = false;
    public float MinDistFromBalloon;
    public float MaxDistFromBalloon;

    public float MaxHeight = 1f;
    public GameObject Balloon_Bottom;
    public GameObject Balloon_String;

    // Player calls this when Balloon collides with it
    public override void Interact(GameObject player)
    {
        // If player isn't attached then attach it
        if (!IsPlayerAttached)
        {
            Attach(player);
        }

        // Give Happiness value
        player.GetComponent<Character>().ChangeHappiness(HappinessChangeValue);

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

    void Update()
    {
        // Stay under max height
        if(transform.position.y < MaxHeight)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * FloatForce * AttachedObjectForceModifier * Time.deltaTime);
        }

        // If player is attached then set up line render with positions
        if (IsPlayerAttached)
        {
            // Set pos 0 to balloon bottom
            Balloon_Bottom.GetComponent<LineRenderer>().SetPosition(0, Balloon_Bottom.transform.position);
            // Set pos 1 to pos of object that balloon bottom is connected to
            Balloon_Bottom.GetComponent<LineRenderer>().SetPosition(1, Balloon_Bottom.GetComponent<SpringJoint>().connectedBody.transform.position);
        }
    }

    void Attach(GameObject objectToAttachTo)
    {
        // Attach to Balloon Bottom and set up spring joint
        Balloon_Bottom.AddComponent<SpringJoint>(); // Add joint to balloon bottom
        Balloon_Bottom.GetComponent<SpringJoint>().connectedBody = objectToAttachTo.GetComponent<Rigidbody>(); // Attach bottom to object
        Balloon_Bottom.GetComponent<SpringJoint>().axis = new Vector3(0f, 0f, 1f); // Set axis of joint

        // Set up Spring joint distances
        Balloon_Bottom.GetComponent<SpringJoint>().minDistance = MinDistFromBalloon;
        Balloon_Bottom.GetComponent<SpringJoint>().maxDistance = MaxDistFromBalloon;

        // Turn off string object
        Balloon_String.SetActive(false);

        // Set attached object force modifier
        AttachedObjectForceModifier = AttachedObjectForceModifierMax;

        // Set is player attached to true
        IsPlayerAttached = true;
    }

    void Detach(GameObject objectToDetachFrom)
    {
        // Detach to String
        Destroy(Balloon_Bottom.GetComponent<SpringJoint>()); // Delete joint

        // Turn off string line render
        // Set pos 0 to balloon bottom
        Balloon_Bottom.GetComponent<LineRenderer>().SetPosition(0, Balloon_Bottom.transform.position);
        // Set pos 1 to balloon bottom
        Balloon_Bottom.GetComponent<LineRenderer>().SetPosition(1, Balloon_Bottom.transform.position);

        // Turn on string object
        Balloon_String.SetActive(true);

        // Reset attached object force modifier
        AttachedObjectForceModifier = 1f;

        // Set is player attached to false
        IsPlayerAttached = false;
    }
}