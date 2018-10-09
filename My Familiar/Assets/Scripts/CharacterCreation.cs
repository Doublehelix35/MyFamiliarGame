using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {

    Vector3[] VectorArrayHead = new Vector3[1000];
    private Vector3 FirstTouchPos;   // First touch position
    private Vector3 LastTouchPos;   // Last touch position
    int LinePointCount = 0;
    float MinDistanceBetweenPoints = 0.2f;


    void Start ()
    {
		
	}

	void Update ()
    {
        if(Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) // check for the first touch
            {
                // Update touch positions
                FirstTouchPos = touch.position;
                LastTouchPos = touch.position;

                // Start of line
                GetComponent<LineRenderer>().SetPosition(LinePointCount, FirstTouchPos);
                LinePointCount++;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                // Check distance from last point
                float dist = Vector3.Distance(LastTouchPos, touch.position);

                if (Mathf.Abs(dist) >= MinDistanceBetweenPoints)
                {
                    // Update touch position
                    LastTouchPos = touch.position;

                    // Next point of line
                    GetComponent<LineRenderer>().SetPosition(LinePointCount, LastTouchPos);
                    LinePointCount++;
                }
                
            }
            else if (touch.phase == TouchPhase.Ended) // check if the finger is removed from the screen
            {
                // Last touch position
                LastTouchPos = touch.position;  

                // End of line
                GetComponent<LineRenderer>().SetPosition(LinePointCount, LastTouchPos);
                LinePointCount++;
            }
        }
    }
}
