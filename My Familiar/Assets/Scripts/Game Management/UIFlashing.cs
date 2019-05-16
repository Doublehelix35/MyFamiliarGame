using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlashing : MonoBehaviour
{
    public int FlashMax = 4;
    int flashCount = 0;

    bool isFlashing = false;

    IEnumerator coroutine;

    void Start()
    {
        // Start coroutine;
        coroutine = FlashImage();   
        StartCoroutine(coroutine);
    }

    // Activate flashing
    internal void Flash()
    {
        flashCount = 0;
        isFlashing = true;
    }

    // Loop and flash image for flashMax
    IEnumerator FlashImage()
    {
        while (isFlashing)
        {
            // Off
            GetComponent<RawImage>().enabled = false;
            // Wait
            yield return new WaitForSeconds(0.2f);
            // On
            GetComponent<RawImage>().enabled = true;
            // Wait
            yield return new WaitForSeconds(0.2f);
            // Off
            GetComponent<RawImage>().enabled = false;

            flashCount++;

            // Sets isFlashing to false to exit loop
            isFlashing = flashCount >= FlashMax ? false : true;
        }

    }
}
