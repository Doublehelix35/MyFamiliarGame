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
    public Observer QuestObserver;
    public AudioSource TapSound; // Object holding the tap sound

    // UI
    public UIFlashing Happiness_DownArrow; // Flashes down when happiness lost
    public UIFlashing Happiness_UpArrow; // Flashes up when happiness gained
    public UIFlashing Fullness_DownArrow; // Flashes down when fullness lost
    public UIFlashing Fullness_UpArrow; // Flashes up when fullness gained
    public UIFlashing LevelUpFlash; // Flashes on level up
    public UIFlashing EvolvedFlash; // Flashes on evolution
    
    // Texts
    public Text CharacterNameText;
    public Text LevelText;
    public Text HealthText;

    // Spec point Texts
    public Text AirText;
    public Text EarthText;
    public Text FireText;
    public Text NatureText;
    public Text WaterText;

    // Buttons
    public Button BattleModeButton;
    public Button ShowButton;

    // Misc UI Elements
    public Slider Exp_ProgressBar;
    public Material HappinessMat; // Background for happiness icon
    public Material FullnessMat; // Background for fullness icon

    // Touch movement
    bool MoveRagdoll = false;
    float DistFromCamera;
    GameObject Ragdoll;
    Vector3 DragOffset;
    float RagdollMaxVelocity = 1500f;
    float FollowStopDistance = 0.05f;
    float RagdollMaxDist = 10f;

    // Egg Spawning
    public GameObject EggPrefab;
    Vector3 EggSpawnPos = new Vector3(0f, 5f, 0f);
    bool EggActive = false;

    void Awake ()
    {
        // Will either spawn egg (1st time spawn) or reload character from file
        if (LoadRef.CheckFirstTimeLoad(LoadRef.Load(LoadRef.LoadCurrentSlot())))
        {
            // Spawn egg        
            gameObject.GetComponent<GameManager>().SpawnEgg();
            EggActive = true;
            ShowButton.interactable = false;
        }
        else
        {
            ReloadCharacter();
        }
    }
	
	void Update ()
    {
        // Move object with touch //
        if (Input.touchCount == 1) // User is touching the screen
        {
            Touch touch = Input.GetTouch(0); // Get touch
            Vector3 touchPos = touch.position; // Get touch position

            if (touch.phase == TouchPhase.Began) // Check for the first touch
            {
                // Tap sound
                TapSound.Play();

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
                    
                    // Egg code
                    if(hit.transform.tag == "Egg")
                    {
                        // Interact with egg and pass it ref of Game manager
                        hit.transform.gameObject.GetComponent<Item_Egg>().Interact(gameObject);
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
        if(!EggActive) { CharacterRef.GetComponentInChildren<Character_AI>().StopOrResumeMoving(!MoveRagdoll); }        
    }

    // Evolve character and update save
    internal void EvolveToNextStage(GameObject ObjectToEvolve)
    {
        // Save stats
        SaveCharaterStats(ObjectToEvolve);
        
        // Delete old character
        CharacterRef = new GameObject();
        Destroy(ObjectToEvolve.transform.parent.gameObject); // ObjectToEvolve is the body object so we need to delete the parent

        // Reload character
        ReloadCharacter();
    }

    // Object to save needs to have the character script attached
    internal void SaveCharaterStats(GameObject ObjectToSave = null)
    {
        // Default object is character ref's child (body)
        if(ObjectToSave == null) { ObjectToSave = CharacterRef.transform.GetChild(0).gameObject; }

        // Save character's stats
        SaveRef.Save(LoadRef.Load(LoadRef.LoadCurrentSlot()), ObjectToSave); // Load name from current slot to ensure names line up for saving and loading
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

        // Set egg active to false
        EggActive = false;

        // Set show button to interactable
        ShowButton.interactable = true;

        // Add observer to character subject
        CharacterRef.GetComponentInChildren<Subject>().AddObserver(QuestObserver);
    }

    internal void SpawnEgg()
    {
        Instantiate(EggPrefab, EggSpawnPos, Quaternion.identity);
    }

    // Reward for completing quest
    internal void QuestReward(int expGain)
    {
        // Tell character to gain exp
        CharacterRef.GetComponentInChildren<Character>().GainExp(expGain);
    }

    // Set value on exp progress bar (value should be between 0 and 1)
    public void Update_Exp(float currentExp, float maxExp)
    {
        Exp_ProgressBar.value = currentExp / maxExp;
    }

    // Set happiness background mat equal to a point in the transition between red and green based on happiness
    public void Update_Happiness(float currentHappiness, float maxHappiness, bool isUp)
    {
        // lerp between red and green based on happiness
        HappinessMat.color = Color.Lerp(new Color(0.97f, 0.6f, 0.59f) , new Color(0.64f, 0.97f, 0.59f), currentHappiness / maxHappiness);

        if (isUp)
        {
            Happiness_UpArrow.Flash();
        }
        else
        {
            Happiness_DownArrow.Flash();
        }
    }

    // Set fullness background mat equal to a point in the transition between red and green based on fullness
    public void Update_Fullness(float currentFullness, float maxFullness, bool isUp)
    {
        // Lerp between red and green based on fullness
        FullnessMat.color = Color.Lerp(Color.red, Color.green, currentFullness / maxFullness);

        if (isUp)
        {
            Fullness_UpArrow.Flash();
        }
        else
        {
            Fullness_DownArrow.Flash();
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

    public void UpdateText_Level(string currentLevel)
    {
        LevelText.text = currentLevel;
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

    // Flash UI elements
    public void FlashLevelUp()
    {
        LevelUpFlash.Flash();
    }

    public void FlashEvolved()
    {
        EvolvedFlash.Flash();
    }
}
