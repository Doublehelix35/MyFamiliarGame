using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlashing : MonoBehaviour
{
    public float FlashMax = 2f; // Duration of flashing in seconds total
    public float FlashDelay = 0.2f; // Gap between each flash

    float LastTimeStamp;
    float LastFlashTime;

    RawImage FlashingImage;

    void Start()
    {
        // Init time
        LastTimeStamp = Time.time;
        LastFlashTime = Time.time;

        FlashingImage = GetComponent<RawImage>();
    }

    // Activate flashing
    internal void Flash()
    {
        Debug.Log("Flash");

        LastTimeStamp = Time.time;
        LastFlashTime = Time.time;
    }

    // Loop and flash image for flashMax
    void FixedUpdate()
    {
        // Check if able to be flashing
        if(LastTimeStamp + FlashMax >= Time.time)
        {
            // Check time since last flash
            if(LastFlashTime + FlashDelay <= Time.time)
            {
                // Flash
                FlashingImage.enabled = !FlashingImage.enabled; // Toggle enabled

                // Reset last flash time
                LastFlashTime = Time.time;
            }
        }
        else
        {
            // Make sure image isnt visible when not flashing
            FlashingImage.enabled = false;
        }
    }
}
