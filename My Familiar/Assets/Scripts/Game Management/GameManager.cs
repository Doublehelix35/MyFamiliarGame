using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // Object refs
    public Load_Character LoadRef;    
    GameObject CharacterRef;
    public GameObject CameraRef;

    // Texts
    public Text CharacterNameText;
    public Text ExpText;
    public Text HappinessText;
    public Text HealthText;

    // Spec point Texts
    public Text AirText;
    public Text EarthText;
    public Text FireText;
    public Text NatureText;
    public Text WaterText;


    bool MoveRagdoll = false;
    float DistFromCamera;
    Transform Ragdoll;
    Vector3 DragOffset;

    void Awake()
    {
        // Load character based on current save slot in use        
        CharacterRef = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot())); // Get slot no. then character name then load character
        UpdateText_CharacterName(CharacterRef.name); // Name originally set during loading in above line
        CharacterRef.transform.position += new Vector3(0f, 4f, 0f); // Spawn above ground
        

        CameraRef.GetComponent<CameraFollow>().SetPlayerRef(CharacterRef);        
    }

    void Start ()
    {
        

    }
	
	void Update ()
    {
        // Move character with touch
        if (Input.touchCount == 1) // user is touching the screen
        {
            Touch touch = Input.GetTouch(0); // get the touch

            Vector3 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began) // check for the first touch
            {
                // Cast a ray
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100))
                {
                    if(hit.transform.name == "Face") // Ray hits Character's face
                    {
                        MoveRagdoll = true; 
                        Ragdoll = hit.transform; // Set ref to character's face so it can move it
                        DistFromCamera = hit.transform.position.z - Camera.main.transform.position.z;
                        Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera);
                        newPos = Camera.main.ScreenToWorldPoint(newPos); // Set new pos equal to touch
                        DragOffset = Ragdoll.position - newPos; 

                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (MoveRagdoll)
                {
                    // Move ragdoll to touch pos
                    Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera);
                    newPos = Camera.main.ScreenToWorldPoint(newPos);
                    Ragdoll.position = newPos + DragOffset;
                }
            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Stop moving the character
                MoveRagdoll = false;
            }
        }

    }

    // Text update methods
    /// <summary>
    /// Alternative idea: Could have one updateText method that takes 2 params 1) String value 2) enum TextToChange
    /// then use a switch
    /// </summary>
    public void UpdateText_CharacterName(string name)
    {
        CharacterNameText.text = name;
    } 

    public void UpdateText_Exp(string currentExp)
    {
        ExpText.text = currentExp;
    }

    public void UpdateText_Happiness(string currentHappiness)
    {
        HappinessText.text = currentHappiness;
    }

    public void UpdateText_Health(string currentHealth)
    {
        HealthText.text = currentHealth;
    }

    // Updates element spec texts
    public void UpdateText_AllElements(string currentAirPoints, string currentEarthPoints, string currentFirePoints, string currentNaturePoints, string currentWaterPoints)
    {
        AirText.text = currentAirPoints;
        EarthText.text = currentEarthPoints;
        FireText.text = currentFirePoints;
        NatureText.text = currentNaturePoints;
        WaterText.text = currentWaterPoints;
    }
}
