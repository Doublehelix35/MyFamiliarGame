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

    GameObject FaceRef;
    public Vector3 FaceOffset;

    public string NextSceneName;

    string CharacterName;
    public string PartName;
    public Material PartMaterial;

   
    void Start()
    {
        // Load the face into the scene
        FaceRef = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot()), "Face"); // Load slot no > load character name > load character's face

        // Center face in scene
        FaceRef.transform.position += FaceOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // Touch input
        if(Input.touchCount >= 1) // User is touching the sceen
        {
            Touch touch = Input.GetTouch(0); // Get the touch
            if (touch.phase == TouchPhase.Began) // Check if first touch
            {

            }
            else if(touch.phase == TouchPhase.Moved) // Move Facial object
            {

            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) // Drop facial object
            {

            }

        }
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
