using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    bool TransitioningScene = false;

    Material TransitionMat;

    IEnumerator coroutine;

    string SceneString = "";
    int SceneInt;
    bool IsSceneByName;

    void Start()
    {
        coroutine = Transition();
    }

    public void LoadScene(string SceneName, bool firstCall = true)
    {
        // Set SceneString
        SceneString = SceneName;

        // Play transiton effects before loading next scene
        if(firstCall)
        {
            IsSceneByName = true;
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

    public void LoadScene(int buildIndex, bool firstCall = true)
    {
        // Set SceneInt
        SceneInt = buildIndex;

        // Play transiton effects before loading next scene
        if (firstCall)
        {
            IsSceneByName = false;
            TransitionScene();
            return;
        }

        SceneManager.LoadScene(buildIndex);
    }

    public void TransitionScene()
    {
        TransitioningScene = true;

        StartCoroutine(coroutine);
    }

    IEnumerator Transition()
    {
        float step = 0f;

        while (true)
        {
            // Execute every 0.1 seconds
            yield return new WaitForSeconds(0.1f);
            if (TransitioningScene)
            {
                // Increase step
                step += 0.1f;

                TransitionMat.SetFloat("_Cutoff", step);

                if (step >= 1f)
                {
                    // Load next scene
                    if (IsSceneByName)
                    {
                        LoadScene(SceneString, false);
                    }
                    else
                    {
                        LoadScene(SceneInt, false);
                    }
                }
            }            
        }
    }
}
