using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public GameObject SkyBox;

    float DayProgress = 0f; // progress between day and night (%)  0.5f is midnight
    float DayProgressStep = 0.002f; // increment by
    float UpdateDelay = 1f; // delay in seconds

    IEnumerator coroutine;
    
    void Start()
    {
        // Init skybox to 0
        SkyBox.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(DayProgress, 0));

        // Start CycleDayNight coroutine
        coroutine = CycleDayNight();
        StartCoroutine(coroutine);
    }

    // After every update delay, increase day progress, loop day progress back to 0 if its > 1
    IEnumerator CycleDayNight()
    {
        while (true)
        {
            yield return new WaitForSeconds(UpdateDelay);
            DayProgress += DayProgressStep;
            if (DayProgress > 1f)
            {
                DayProgress = 0f;
            }
            SkyBox.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(DayProgress, 0));
        }
    }
}
