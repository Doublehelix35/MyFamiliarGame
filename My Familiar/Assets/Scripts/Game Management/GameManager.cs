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
    float FollowSpeed = 1000f;
    float FollowStopDistance = 0.5f;

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
        // Move object with touch //
        if (Input.touchCount == 1) // User is touching the screen
        {
            Touch touch = Input.GetTouch(0); // get touch
            Vector3 touchPos = touch.position; // get touch position

            if (touch.phase == TouchPhase.Began) // check for the first touch
            {
                // Cast a ray
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.GetComponent<Rigidbody>() && hit.transform.tag != "NonMoving") // Ray hits a rigidbody thats allowed to be moved
                    {
                        MoveRagdoll = true; // Object is being controlled by player
                        DistFromCamera = hit.transform.position.z - Camera.main.transform.position.z; // Keep z consistant
                        Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera); // Pos to move to
                        newPos = Camera.main.ScreenToWorldPoint(newPos); // Set new pos equal to touch

                        if (Vector3.Distance(newPos, transform.position) > FollowStopDistance)
                        {
                            GetComponent<Rigidbody>().velocity = (newPos - transform.position).normalized * FollowSpeed * Time.deltaTime; // Move object
                        }
                        else
                        {
                            GetComponent<Rigidbody>().velocity = Vector3.zero; // Stop movement
                        }
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (MoveRagdoll)
                {
                    // Move object to touch pos
                    Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera);
                    newPos = Camera.main.ScreenToWorldPoint(newPos);

                    if (Vector3.Distance(newPos, transform.position) > FollowStopDistance)
                    {
                        GetComponent<Rigidbody>().velocity = (newPos - transform.position).normalized * FollowSpeed * Time.deltaTime; // Move object
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Stop moving the object
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
