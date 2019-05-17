using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlashing : MonoBehaviour
{
    public float FlashMax = 2f; // Duration of flashing in seconds total
    public float FlashDelay = 0.2f; // Gap between each flash
    public float StartDelay = 2f; // Delay after scene is loaded
    public bool isImageNotText; // true = image false = text

    float LastTimeStamp;
    float LastFlashTime;

    RawImage FlashingImage;
    Text FlashingText;

    

    void Start()
    {
        // Init time
        LastTimeStamp = Time.time - StartDelay;
        LastFlashTime = Time.time - StartDelay;

        if (isImageNotText)
        {
            FlashingImage = GetComponent<RawImage>();
        }
        else
        {
            FlashingText = GetComponent<Text>();
        }
        
    }

    // Activate flashing
    internal void Flash()
    {
        LastTimeStamp = Time.time;
        LastFlashTime = Time.time - FlashDelay; // Flash immediately
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
                if (isImageNotText)
                {
                    FlashingImage.enabled = !FlashingImage.enabled; // Toggle
                }
                else
                {
                    FlashingText.enabled = !FlashingText.enabled; // Toggle
                    Debug.Log("Flash " + gameObject.ToString());
                }                

                // Reset last flash time
                LastFlashTime = Time.time;
            }
        }
        else
        {
            // Make sure image isnt visible when not flashing
            if (isImageNotText)
            {
                FlashingImage.enabled = false;
            }
            else
            {
                FlashingText.enabled = false;
            }
                
        }
    }
}
