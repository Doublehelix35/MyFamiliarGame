using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AI : MonoBehaviour
{
    /// Should be attached to the body and apply forces from there ///


    // Force of movement
    public float IdleForce = 600f;
    public float WanderForce = 1000f;
    
    // Timers
    float LastIdleTime; // Last idle action
    float IdleWait = 4f; // Time to wait between idle actions
    float LastWanderTime; // Last wander action
    float WanderWait = 4f; // Time to wait between wander actions

    bool CanMove = true;

    Rigidbody rigid; // Store a ref to reduce GetComponent() calls

    void Start()
    {
        // Init timers
        LastIdleTime = Time.time;
        LastWanderTime = Time.time;

        // Init rigidbody
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Check if already moving and if so CanMove = false
        CanMove = rigid.velocity.x > 0.1f ? false : true;
        CanMove = rigid.velocity.y > 0.1f ? false : true;

        if (CanMove)
        {
            if (LastIdleTime + IdleWait < Time.time) // Idle
            {
                // Add force upwards * idle force
                rigid.AddForce(Vector3.up *  IdleForce);

                // Reset timer
                LastIdleTime = Time.time;
            }
            
            if(LastWanderTime + WanderWait < Time.time) // Wander
            {
                // Direction
                int dir = Random.Range(-1, 2); // Returns -1 or 0 or 1

                // Add force
                rigid.AddForce(Vector3.right * WanderForce * dir);

                // Reset timer
                LastWanderTime = Time.time;
            }
        }
    }

    // Stop or resume all movement
    internal void StopOrResumeMoving(bool shouldMove)
    {
        CanMove = shouldMove;
    }
}
