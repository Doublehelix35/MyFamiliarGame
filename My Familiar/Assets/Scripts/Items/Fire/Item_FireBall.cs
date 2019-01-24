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

    float DistFromCamera;
    Vector3 DragOffset;
    public bool ControlledByPlayer = false;
    float FollowSpeed = 1000f;
    float FollowStopDistance = 0.5f;


	void Start ()
    {
        // Init stats
        ExpPointsGiven = ExpPointsGivenMax;
        SpecPointsGiven = SpecPointsGivenMax;
        HappinessChangeValue = HappinessChangeValueMax;
        itemType = Elements.ElementType.Fire;

        // Destroy self after lifespan runs out
        //Destroy(gameObject, lifeSpan);

        // Temp code
        //GetComponent<Rigidbody>().useGravity = false;
    }

    void Update()
    {
        // Move character with touch
        if (Input.touchCount == 1) // user is touching the screen
        {
            Touch touch = Input.GetTouch(0); // get the touch

            Vector3 touchPos = touch.position;
            Vector3 lastPos = new Vector3(0f,0f,0f);

            if (touch.phase == TouchPhase.Began) // check for the first touch
            {
                // Cast a ray
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.gameObject == gameObject) // Ray hits this fireball
                    {
                        //GetComponent<Rigidbody>().useGravity = false; // Turn gravity off
                        ControlledByPlayer = true; // Fireball is being controlled by player
                        DistFromCamera = hit.transform.position.z - Camera.main.transform.position.z; // Keep z consistant
                        Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera);
                        newPos = Camera.main.ScreenToWorldPoint(newPos); // Set new pos equal to touch

                        if (Vector3.Distance(newPos, transform.position) > FollowStopDistance)
                        {
                            GetComponent<Rigidbody>().velocity = (newPos - transform.position).normalized * FollowSpeed * Time.deltaTime; // Move fireball
                        }
                        else
                        {
                            GetComponent<Rigidbody>().velocity = Vector3.zero;
                        }
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (ControlledByPlayer)
                {
                    // Move fireball to touch pos
                    Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera);
                    newPos = Camera.main.ScreenToWorldPoint(newPos);

                    if(Vector3.Distance(newPos, transform.position) > FollowStopDistance)
                    {
                        GetComponent<Rigidbody>().velocity = (newPos - transform.position).normalized * FollowSpeed * Time.deltaTime; // Move fireball
                    }                    
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Stop moving the fireball
                ControlledByPlayer = false;

                // Turn kinematic off
                //ObjectToFollow.GetComponent<Rigidbody>().isKinematic = false;
                
                // Turn gravity back on
                //GetComponent<Rigidbody>().useGravity = true;
            }
        }
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
