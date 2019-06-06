using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    

    public void LoadScene(string SceneName)
    {
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

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
