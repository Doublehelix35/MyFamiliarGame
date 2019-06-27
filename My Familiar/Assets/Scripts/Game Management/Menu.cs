using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    bool TransitioningSceneIn = true;
    bool TransitioningSceneOut = false;

    public SimpleBlit TransitionMat;

    IEnumerator coroutineOut;
    IEnumerator coroutineIn;

    string SceneString = "";
    int SceneInt;
    bool IsSceneByName;
    public bool PlayTransition = false;
    float TransitionSpeed = 0.008f;

    void Start()
    {
        coroutineIn = TransitionIn();
        coroutineOut = TransitionOut();

        StartCoroutine(coroutineIn); // Transition in effect
    }

    // Load with string
    public void LoadScene(string SceneName)
    {
        // Set SceneString
        SceneString = SceneName;

        // Play transiton effects before loading next scene
        if(PlayTransition)
        {
            IsSceneByName = true;
            PlayTransition = false;
            TransitionScene();
            return;
        }

        // Save stats
        if(SceneName == "BattleMode") // Sandbox to battle mode
        {
            // Save character stats
            GameManager GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            GM.SaveCharaterStats();
        }
        else if(SceneName == "Sandbox" && SceneManager.GetActiveScene().name == "BattleMode") // Battle mode to sandbox
        {
            // Save character stats
            BattleManager BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleManager>();
            BM.SaveCharaterStats();
        }

        SceneManager.LoadScene(SceneName);
    }

    // Load with int
    public void LoadScene(int buildIndex)
    {
        // Set SceneInt
        SceneInt = buildIndex;

        // Play transiton effects before loading next scene
        if (PlayTransition)
        {
            IsSceneByName = false;
            PlayTransition = false;
            TransitionScene();
            return;
        }

        SceneManager.LoadScene(buildIndex);
    }

    // Start transition effect
    void TransitionScene()
    {
        TransitioningSceneOut = true;

        StartCoroutine(coroutineOut);
    }

    // Gradually increase screen cutoff
    IEnumerator TransitionOut()
    {
        float step = 0f;

        while (true)
        {
            // Execute every x seconds
            yield return new WaitForSeconds(TransitionSpeed);
            if (TransitioningSceneOut)
            {
                // Increase step
                step += 0.05f;
                TransitionMat.TransitionMaterial.SetFloat("_Cutoff", step);

                if (step >= 1f)
                {
                    // Load next scene
                    if (IsSceneByName)
                    {                        
                        LoadScene(SceneString);
                    }
                    else
                    {
                        LoadScene(SceneInt);
                    }
                }
            }            
        }
    }

    // Gradually decrease screen cutoff
    IEnumerator TransitionIn()
    {
        float step = 1f;

        while (true)
        {
            // Execute every x seconds
            yield return new WaitForSeconds(TransitionSpeed);
            if (TransitioningSceneIn)
            {
                // Increase step
                step -= 0.05f;
                TransitionMat.TransitionMaterial.SetFloat("_Cutoff", step);

                if (step <= 0f)
                {
                    TransitioningSceneIn = false;
                    StopCoroutine(coroutineIn); // Exit coroutine
                }
            }
        }
    }
}
