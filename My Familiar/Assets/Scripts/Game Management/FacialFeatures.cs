using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FacialFeatures : MonoBehaviour
{
    // List of all face objects in the scene i.e. eyes
    List<GameObject> FacialObjectsInScene = new List<GameObject>();
    int FacialObjectNum = 0; // Makes sure all facial objects get unique save files

    public Save_Character SaveRef;
    public Load_Character LoadRef;

    GameObject FaceRef;
    public Vector3 FaceOffset;

    public string NextSceneName;

    string CharacterName;

    bool MoveObject = false;
    GameObject ObjectToMove;
    float DistFromCamera;

    public GameObject TestPrefab;

    void Start()
    {
        // Load the face into the scene
        FaceRef = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot()), "Face"); // Load slot no > load character name > load character's face

        // Center face in scene
        FaceRef.transform.position += FaceOffset;

        // Test code
        GameObject eye = Instantiate(TestPrefab, transform.position, Quaternion.identity);
        FacialObjectsInScene.Add(eye);        
    }
    
    void Update()
    {
        // Touch input
        if(Input.touchCount >= 1) // User is touching the sceen
        {
            Touch touch = Input.GetTouch(0); // Get the touch
            Vector3 touchPos = touch.position; // Get touch position

            if (touch.phase == TouchPhase.Began) // Check if first touch
            {
                // Cast a ray
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if(hit.transform.tag == "FacialObject" || hit.transform.tag == "NonMoving")
                    {
                        MoveObject = true;

                        if(hit.transform.tag == "NonMoving") // Spawn facial feature
                        {
                            // Clone object hit
                            ObjectToMove = Instantiate(hit.transform.gameObject, hit.transform.position, Quaternion.identity);
                            // Change tag
                            ObjectToMove.tag = "FacialObject";
                            // Set name
                            ObjectToMove.name = ObjectToMove.name + FacialObjectNum;
                            // Increase facial object num
                            FacialObjectNum++;
                            // Add to list
                            FacialObjectsInScene.Add(ObjectToMove);

                            Debug.Log("Name: " + ObjectToMove);
                        }
                        else // Facial feature selected has been spawned
                        {
                            ObjectToMove = hit.transform.gameObject; // Set object to move equal to object hit by ray
                        }
                        
                        DistFromCamera = ObjectToMove.transform.position.z - Camera.main.transform.position.z; // Keep z consistant
                        Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera); // Pos to move to
                        newPos = Camera.main.ScreenToWorldPoint(newPos); // Convert new pos to world axis

                        // Move object to newPos
                        ObjectToMove.transform.position = newPos;
                    }                    
                }
            }
            else if(touch.phase == TouchPhase.Moved) // Move Facial object
            {
                // Only attempt to move object if there is one currently selected
                if (MoveObject)
                {
                    Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera); // Pos to move to
                    newPos = Camera.main.ScreenToWorldPoint(newPos); // Convert new pos to world axis

                    // Move object to newPos
                    ObjectToMove.transform.position = newPos;
                }

            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) // Drop facial object
            {
                // Stop moving the object
                MoveObject = false;

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
            
            // Create string list of facial objects and save facial parts
            string[] FacialObjectStrings = new string[FacialObjectsInScene.Count];
            int i = 0;

            foreach(GameObject g in FacialObjectsInScene)
            {
                FacialObjectStrings[i] = g.name; // Add name to array
                i++; // Increase incrementor

                // Calcualate position relative to face
                Vector3 posRelativeToFace = g.transform.position - FaceRef.transform.position;

                SaveRef.SaveFacialFeature(CharacterName, g.name, posRelativeToFace);
            }

            // Save facial config
            SaveRef.SaveFacialConfig(CharacterName, FacialObjectStrings);

            // Load next scene
            SceneManager.LoadScene(NextSceneName);
        }
    }
}
