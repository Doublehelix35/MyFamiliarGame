using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Object refs
    public Save_Character SaveRef;
    public Load_Character LoadRef;    
    GameObject CharacterRef; // This is the parent object
    public GameObject CameraRef;

    // Texts
    public Text CharacterNameText;
    public Text LevelText;
    public Text ExpText;
    public Text HappinessText;
    public Text HealthText;
    public Text FullnessText;

    // Spec point Texts
    public Text AirText;
    public Text EarthText;
    public Text FireText;
    public Text NatureText;
    public Text WaterText;

    // Buttons
    public Button BattleModeButton;

    // Touch movement
    bool MoveRagdoll = false;
    float DistFromCamera;
    GameObject Ragdoll;
    Vector3 DragOffset;
    float RagdollMaxVelocity = 1500f;
    float FollowStopDistance = 0.05f;
    float RagdollMaxDist = 10f;

    void Awake()
    {
        ReloadCharacter();       
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
                        Ragdoll = hit.transform.gameObject; // Set ragdoll equal to object hit
                        MoveRagdoll = true; // Object is being controlled by player
                        DistFromCamera = hit.transform.position.z - Camera.main.transform.position.z; // Keep z consistant
                        Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera); // Pos to move to
                        newPos = Camera.main.ScreenToWorldPoint(newPos); // Convert new pos to world axis

                        float dist = Vector3.Distance(newPos, Ragdoll.transform.position);

                        if (dist > FollowStopDistance)
                        {
                            // Set velocity. Calc direction. Then times direction by follow speed and time.deltatime and factor in distance from target pos
                            Ragdoll.GetComponent<Rigidbody>().velocity = (newPos - Ragdoll.transform.position).normalized * (RagdollMaxVelocity * (dist/RagdollMaxDist) * Time.deltaTime); // Move object
                        }
                        else
                        {
                            Ragdoll.GetComponent<Rigidbody>().velocity = Vector3.zero; // Stop movement
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
                    newPos = Camera.main.ScreenToWorldPoint(newPos); // Convert new pos to world axis

                    float dist = Vector3.Distance(newPos, Ragdoll.transform.position);

                    if (dist > FollowStopDistance)
                    {
                        // Set velocity. Calc direction. Then times direction by follow speed and time.deltatime and factor in distance from target pos
                        Ragdoll.GetComponent<Rigidbody>().velocity = (newPos - Ragdoll.transform.position).normalized * (RagdollMaxVelocity * (dist/RagdollMaxDist) * Time.deltaTime); // Move object
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Stop moving the object
                MoveRagdoll = false;
            }
        }

        // Tell character ai whether to move or not (Only move if player is not moving ragdoll)
        CharacterRef.GetComponentInChildren<Character_AI>().StopOrResumeMoving(!MoveRagdoll);
    }

    // Evolve character and update save
    internal void EvolveToNextStage(GameObject ObjectToEvolve)
    {
        // Evolve and save new type
        SaveRef.Save(LoadRef.Load(LoadRef.LoadCurrentSlot()), ObjectToEvolve); // Load name from current slot to ensure names line up for saving and loading
        
        // Delete old character
        CharacterRef = new GameObject();
        Destroy(ObjectToEvolve.transform.parent.gameObject); // Char Ref is the body object so we need to delete the parent

        // Reload character
        ReloadCharacter();
    }

    internal void ReloadCharacter()
    {
        // Reload character
        CharacterRef = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot())); // Get slot no. then character name then load character

        // Update UI and give camera new ref
        UpdateText_CharacterName(CharacterRef.name);

        CharacterRef.transform.position += new Vector3(0f, 4f, 0f); // Spawn above ground
        CameraRef.GetComponent<CameraFollow>().SetPlayerRef(CharacterRef); // Set player ref in camera 

        // Turn on battle mode button if player is evolution 1 or higher
        BattleModeButton.interactable = CharacterRef.GetComponentInChildren<Character>().CurrentEvolutionStage >= 1 ? true : false;
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

    public void UpdateText_Level(string currentLevel)
    {
        LevelText.text = currentLevel;
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

    public void UpdateText_Fullness(string currentFullness)
    {
        FullnessText.text = currentFullness + "/100 (%)";
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
