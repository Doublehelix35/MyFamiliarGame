using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FacialFeatures : MonoBehaviour
{
    // List of all face objects in the scene i.e. eyes
    List<GameObject> FacialObjectsInScene = new List<GameObject>();

    public Save_Character SaveRef;
    public Load_Character LoadRef;

    public string NextSceneName;

    string CharacterName;
    public string PartName;
    public Material PartMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        // Delete all facial objects
        foreach(GameObject g in FacialObjectsInScene)
        {
            // Save g to temp object, remove g from list then destroy it
            GameObject temp = g;
            FacialObjectsInScene.Remove(g);
            Destroy(temp);
        }
    }

    public void Complete()
    {
        if (FacialObjectsInScene.Count > 0)
        {
            // Load name from current save slot
            CharacterName = LoadRef.Load(LoadRef.LoadCurrentSlot());

            // Save part
            //SaveRef.Save(CharacterName, PartName, ActiveMeshObject);

            // Load next scene
            SceneManager.LoadScene(NextSceneName);
        }
    }
}
