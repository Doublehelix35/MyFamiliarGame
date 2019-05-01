using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTips : MonoBehaviour
{
    public GameObject ToolTipPanel;
    public Text ToolTipTextRef;

    public List<string> ToolTipTexts = new List<string>();

    // Delays are in seconds
    float UpdateDelay = 15f; // How long until the panel next appears
    float DisappearDelay = 5f; // Delay until panel disappears

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        // Start CycleDayNight coroutine
        coroutine = ActivateToolTip();
        StartCoroutine(coroutine);
    }

    // After every update delay, increase day progress, loop day progress back to 0 if its > 1
    IEnumerator ActivateToolTip()
    {
        while (true)
        {
            yield return new WaitForSeconds(UpdateDelay);

            ToolTipPanel.SetActive(true);

            // Select a random text to display
            int rand = Random.Range(0, ToolTipTexts.Count);
            ToolTipTextRef.text = ToolTipTexts[rand];

            yield return new WaitForSeconds(DisappearDelay);

            ToolTipPanel.SetActive(false);            
        }
    }
}
