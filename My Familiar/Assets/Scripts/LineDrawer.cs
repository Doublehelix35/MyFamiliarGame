using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour {

    /// <summary>
    /// Not currently implemented but considered potentially useful.
    /// </summary>

    private Vector3 FirstTouchPos;   // First touch position
    private Vector3 LastTouchPos;   // Last touch position

    private LineRenderer lineRend;
    int LinePointCount = 1;
    public float MinDistanceBetweenPoints = 0.2f;
    public float LineWidth = 0.2f;
    public Material LineMat;


    void Start()
    {
        lineRend = gameObject.AddComponent<LineRenderer>();
        lineRend.material = LineMat;
        lineRend.widthMultiplier = LineWidth;
        lineRend.positionCount = LinePointCount; // Set line size
        lineRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    void Update()
    {
        if (Input.touchCount >= 1) // user is touching the screen with a touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) // check for the first touch
            {
                // Update touch positions
                FirstTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
                LastTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

                // Start of line
                lineRend.positionCount = LinePointCount; // Set line size
                lineRend.SetPosition(0, new Vector3(FirstTouchPos.x, FirstTouchPos.y, 0f)); // set pos of new line segment
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                // Check distance from last point
                float dist = Vector3.Distance(LastTouchPos, Camera.main.ScreenToWorldPoint(touch.position));

                if (Mathf.Abs(dist) >= MinDistanceBetweenPoints)
                {
                    // Update touch position
                    LastTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

                    // Next point of line
                    LinePointCount++;
                    lineRend.positionCount = LinePointCount; // Increase line size
                    lineRend.SetPosition(LinePointCount - 1, new Vector3(LastTouchPos.x, LastTouchPos.y, 0f)); // set pos of new line segment

                }

            }
            else if (touch.phase == TouchPhase.Ended) // check if the finger is removed from the screen
            {
                // Last touch position
                LastTouchPos = Camera.main.ScreenToWorldPoint(touch.position);

                // End of line
                lineRend.positionCount = LinePointCount; // Set line size
                lineRend.SetPosition(LinePointCount - 1, new Vector3(LastTouchPos.x, LastTouchPos.y, 0f));
                LinePointCount = 1;
            }
        }
    }
}
