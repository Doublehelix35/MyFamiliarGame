using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AI : MonoBehaviour
{
    // Should be attached to the body and apply forces from there


    // Speed of movement
    public float IdleForce = 70f;
    float IdleStartTime;
    internal float WanderSpeed = 1f;

    bool CanMove = true;
    bool IsIdle = true;
    bool IsWandering = false;
    
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (CanMove)
        {
            if (IsIdle) // Idle
            {
                // Add force up and times that by idle force and use ping pong to change force direction smoothly between -1 and 1
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * (Mathf.PingPong(Time.time, 2f) - 1f) * IdleForce);
            }
            else // Wander
            {

            }
        }
    }

    // Stop or resume all movement
    internal void StopOrResumeMoving(bool shouldMove)
    {
        CanMove = shouldMove;
    }
}
