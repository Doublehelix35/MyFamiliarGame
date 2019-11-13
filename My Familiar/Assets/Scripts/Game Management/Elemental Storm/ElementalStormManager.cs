using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementalStormManager : MonoBehaviour
{
    // Object refs
    public Save_Character SaveRef;
    public Load_Character LoadRef;
    GameObject CharacterRef; // This is the parent object

    public string SandboxSceneName;

    // Texts
    public Text HealthText;
    
    // Spawn offset
    Vector3 PlayerSpawnOffset = new Vector3(0f, 0f, 0f); // Spawn in the center

    // Touch movement
    bool MoveRagdoll = false;
    float DistFromCamera;
    GameObject Ragdoll;
    Vector3 DragOffset;
    float RagdollMaxVelocity = 1500f;
    float FollowStopDistance = 0.05f;
    float RagdollMaxDist = 10f;

    // Hazard prefabs
    public GameObject Air_Hazard;
    public GameObject Earth_Hazard;
    public GameObject Fire_Hazard;
    public GameObject Nature_Hazard;
    public GameObject Water_Hazard;

    int NumOfHazards = 5; // How many hazard prefabs there are

    // Warning UI

    // UI Colours
    Color UIColour_Air = new Color(0.508f, 0.886f, 0.971f);
    Color UIColour_Earth = new Color(0.972f, 0.678f, 0.509f);
    Color UIColour_Fire = new Color(1f, 0.391f, 0.431f);
    Color UIColour_Nature = new Color(0.392f, 1f, 0.451f);
    Color UIColour_Water = new Color(0.443f, 0.392f, 1f);

    // Lanes (Rows start from the top, columns start from the left)
    public GameObject Lane_Row1;
    public GameObject Lane_Row2;
    public GameObject Lane_Row3;
    public GameObject Lane_Row4;
    public GameObject Lane_Column1;
    public GameObject Lane_Column2;
    public GameObject Lane_Column3;
    public GameObject Lane_Column4;

    // Top Arrows ( Left to right order)
    public GameObject Arrow_1T; // 1st top arrow
    public GameObject Arrow_2T; // 2nd top arrow
    public GameObject Arrow_3T; // 3rd top arrow
    public GameObject Arrow_4T; // 4th top arrow

    // Left Arrows ( Top to bottom order)
    public GameObject Arrow_AL; // 1st left arrow
    public GameObject Arrow_BL; // 2nd left arrow
    public GameObject Arrow_CL; // 3rd left arrow
    public GameObject Arrow_DL; // 4th left arrow

    // Bottom Arrows ( Left to right order)
    public GameObject Arrow_1B; // 1st bottom arrow
    public GameObject Arrow_2B; // 2nd bottom arrow
    public GameObject Arrow_3B; // 3rd bottom arrow
    public GameObject Arrow_4B; // 4th bottom arrow

    // Right Arrows ( Top to bottom order)
    public GameObject Arrow_AR; // 1st right arrow
    public GameObject Arrow_BR; // 2nd right arrow
    public GameObject Arrow_CR; // 3rd right arrow
    public GameObject Arrow_DR; // 4th right arrow

    // Array of lane and arrow combos for spawning
    private enum LaneArrows
    {
        // Easy (Top only)
        Lane_Column1_Arrow_1T, Lane_Column2_Arrow_2T,
        Lane_Column3_Arrow_3T, Lane_Column4_Arrow_4T,

        // Medium (Top + Bottom)
        Lane_Column1_Arrow_1B, Lane_Column2_Arrow_2B,
        Lane_Column3_Arrow_3B, Lane_Column4_Arrow_4B,

        // Hard (Top + Bottom + Left)
        Lane_Row1_Arrow_AL, Lane_Row2_Arrow_BL,
        Lane_Row3_Arrow_CL, Lane_Row4_Arrow_DL,

        // Extreme (All)
        Lane_Row1_Arrow_AR, Lane_Row2_Arrow_BR,
        Lane_Row3_Arrow_CR, Lane_Row4_Arrow_DR
    };

    // Difficulty ints (how many arrows to spawn from)
    int CurrentDifficulty; // init at start of game and change as game progresses
    const int EasyDifficulty = 4;
    const int MediumDifficulty = 8;
    const int HardDifficulty = 12;
    const int ExtremeDifficulty = 16;

    // Ui Flashing
    IEnumerator coroutine;
    public float ArrowFlashDelay = 0.1f; // Delay inbetween arrow flashes
    public int ArrowFlashCount = 2; // How many times an arrow flashes
    public float SpawnDelay = 0.5f; // How long after last arrow flash do hazards spawn
    public float TimeTilNextSpawn = 2f; // How long after hazard is spawned does the process repeat

    
    // Flash Ui warnings and spawn hazards
    IEnumerator UIFlashWarnings()
    {
        // Temp arrow and lane objects
        GameObject selectedArrow = new GameObject();
        GameObject selectedLane = new GameObject();
        GameObject selectedHazard = new GameObject();
        Color selectedColour = new Color();


        while (true)
        {
            yield return new WaitForSeconds(TimeTilNextSpawn);

            // Select element
            int randElement = Random.Range(0, NumOfHazards);
            switch (randElement)
            {
                case 0: // Air
                    selectedHazard = Air_Hazard;
                    selectedColour = UIColour_Air;
                    break;

                case 1: // Earth
                    selectedHazard = Earth_Hazard;
                    selectedColour = UIColour_Earth;
                    break;

                case 2: // Fire
                    selectedHazard = Fire_Hazard;
                    selectedColour = UIColour_Fire;
                    break;

                case 3: // Nature
                    selectedHazard = Nature_Hazard;
                    selectedColour = UIColour_Nature;
                    break;

                case 4: // Water
                    selectedHazard = Water_Hazard;
                    selectedColour = UIColour_Water;
                    break;

                default:
                    Debug.Log("Random element not found. Rand = " + randElement);
                    break;
            }    

            // Select lane and arrow (randomly based on difficulty)
            int randArrow = Random.Range(0, CurrentDifficulty);
            switch (randArrow)
            {
                // Easy
                case 0: // Column1_Arrow_1T
                    selectedArrow = Arrow_1T;
                    selectedLane = Lane_Column1;
                    break;
                case 1: // Column2_Arrow_2T
                    selectedArrow = Arrow_2T;
                    selectedLane = Lane_Column2;
                    break;
                case 2: // Column3_Arrow_3T
                    selectedArrow = Arrow_3T;
                    selectedLane = Lane_Column3;
                    break;
                case 3: // Column4_Arrow_4T
                    selectedArrow = Arrow_4T;
                    selectedLane = Lane_Column4;
                    break;
                    
                // Medium
                case 4: // Column1_Arrow_1B
                    selectedArrow = Arrow_1B;
                    selectedLane = Lane_Column1;
                    break;
                case 5: // Column2_Arrow_2B
                    selectedArrow = Arrow_2B;
                    selectedLane = Lane_Column2;
                    break;
                case 6: // Column3_Arrow_3B
                    selectedArrow = Arrow_3B;
                    selectedLane = Lane_Column3;
                    break;
                case 7: // Column4_Arrow_4B
                    selectedArrow = Arrow_4B;
                    selectedLane = Lane_Column4;
                    break;

                // Hard
                case 8: // Row1_Arrow_AL
                    selectedArrow = Arrow_AL;
                    selectedLane = Lane_Row1;
                    break;
                case 9: // Row2_Arrow_BL
                    selectedArrow = Arrow_BL;
                    selectedLane = Lane_Row2;
                    break;
                case 10: // Row3_Arrow_CL
                    selectedArrow = Arrow_CL;
                    selectedLane = Lane_Row3;
                    break;
                case 11: // Row4_Arrow_DL
                    selectedArrow = Arrow_DL;
                    selectedLane = Lane_Row4;
                    break;

                // Extreme
                case 12: // Row1_Arrow_AR
                    selectedArrow = Arrow_AR;
                    selectedLane = Lane_Row1;
                    break;
                case 13: // Row2_Arrow_BR
                    selectedArrow = Arrow_BR;
                    selectedLane = Lane_Row2;
                    break;
                case 14: // Row3_Arrow_CR
                    selectedArrow = Arrow_CR;
                    selectedLane = Lane_Row3;
                    break;
                case 15: // Row4_Arrow_DR
                    selectedArrow = Arrow_DR;
                    selectedLane = Lane_Row4;
                    break;
                default:
                    Debug.Log("Random Arrow/Lane not found. Rand = " + randArrow);
                    break;
            }

            // Turn on warning lane
            selectedLane.SetActive(true);

            // Flash arrow
            for(int i = 0; i < ArrowFlashCount; i++)
            {
                // Arrow on
                selectedArrow.SetActive(true);

                // Wait
                yield return new WaitForSeconds(ArrowFlashDelay);

                // Arrow off
                selectedArrow.SetActive(false);

                // Wait
                yield return new WaitForSeconds(ArrowFlashDelay);
            }
            
            // Wait then spawn hazard
            yield return new WaitForSeconds(SpawnDelay);

            // Spawn hazard

            // Turn off warning lane
            selectedLane.SetActive(false);
        }
    }

    void Update()
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
                    if (hit.transform.GetComponent<Rigidbody>() && hit.transform.tag == "Player") // Ray hits a rigidbody thats allowed to be moved
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
                            Ragdoll.GetComponent<Rigidbody>().velocity = (newPos - Ragdoll.transform.position).normalized * (RagdollMaxVelocity * (dist / RagdollMaxDist) * Time.deltaTime); // Move object
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
                        Ragdoll.GetComponent<Rigidbody>().velocity = (newPos - Ragdoll.transform.position).normalized * (RagdollMaxVelocity * (dist / RagdollMaxDist) * Time.deltaTime); // Move object
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

    internal void ReloadCharacter()
    {
        // Reload character
        CharacterRef = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot())); // Get slot no. then character name then load character

        // Update UI and give camera new ref
        //UpdateText_CharacterName(CharacterRef.name);
        //Update health text

        // Player scene setup
        CharacterRef.transform.position += PlayerSpawnOffset; // Spawn above ground and to the left
        CharacterRef.transform.localScale *= 0.5f; // Half the size of the character
        CharacterRef.GetComponentInChildren<Character>().enabled = false; // Turn off char script
        CharacterRef.GetComponentInChildren<Character_AI>().enabled = false; // Turn off char ai script

        // Turn off gravity in children
        Rigidbody[] rigids = CharacterRef.GetComponentsInChildren<Rigidbody>(); // Create an array of rigidbodies

        foreach(Rigidbody r in rigids) // Loop through and turn gravity off
        {
            r.useGravity = false;
        }
    }

    // Object to save needs to have the character script attached
    internal void SaveCharaterStats(GameObject ObjectToSave = null)
    {
        // Default object is character ref's child (body)
        if (ObjectToSave == null) { ObjectToSave = CharacterRef.transform.GetChild(0).gameObject; }

        // Save character's stats
        SaveRef.Save(LoadRef.Load(LoadRef.LoadCurrentSlot()), ObjectToSave); // Load name from current slot to ensure names line up for saving and loading
    }

    // Call to spawn character and hazards
    public void StartGame()
    {
        // Init objects
        //ReloadCharacter();

        // Set difficulty
        CurrentDifficulty = ExtremeDifficulty;

        // Start hazard spawner
        coroutine = UIFlashWarnings();
        StartCoroutine(coroutine);
    }
    
    // Call when player is defeated
    public void GameOver()
    {
        // Give exp and gold

        // Save stat changes i.e. exp

        // Load sandbox
        gameObject.GetComponent<Menu>().LoadScene(SandboxSceneName);
    }

    public GameObject GetCharacterParentRef()
    {
        return CharacterRef;
    }

    // Text update methods
    
    public void UpdateText_Health(string currentHealth)
    {
        HealthText.text = currentHealth;
    }
}
