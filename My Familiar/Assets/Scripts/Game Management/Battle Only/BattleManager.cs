using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // Object refs
    public Save_Character SaveRef;
    public Load_Character LoadRef;
    GameObject CharacterRef; // This is the parent object
    GameObject EnemyRef;
    public GameObject CameraRef;
    Elements element;
    public GameObject PlayerAnchorRef; // G.O. that anchors player in pos with spring joint

    public string SandboxSceneName;

    // Texts
    public Text CharacterNameText;
    public Text HealthText;
    public Text LevelText;
    public Text MoveButtonText1;
    public Text MoveButtonText2;
    public Text MoveButtonText3;

    // Buttons
    public Button[] MoveButtons;    

    // Move usage timer
    float LastMoveUseTime;
    float MoveUsageDelay = 2f;
    float MoveSpeedBase = 5f;
    float MoveStarvingPenalty = 0f;
    float MoveRefreshedBonus = 0f;
    float MoveHappinessBonus = 0f;

    // Spawn offset
    Vector3 PlayerSpawnOffset = new Vector3(-6f, 2f, 0f); // Spawn to the left and up

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
        // Init objects
        element = GetComponent<Elements>();
        ReloadCharacter();

        // Init timer so moves can be used from the start
        LastMoveUseTime = Time.time - MoveUsageDelay;

        // Set anchor
        PlayerAnchorRef.GetComponent<SpringJoint>().connectedBody = CharacterRef.GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        if(LastMoveUseTime + MoveUsageDelay > Time.time) // If move cooling down then disable buttons
        {
            foreach(Button b in MoveButtons)
            {
                b.interactable = false;
            }
        }
        else // Reactivate buttons
        {
            foreach(Button b in MoveButtons)
            {                
                b.interactable = true;
            }
        }

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
                    if (hit.transform.GetComponent<Rigidbody>() && hit.transform.tag == "Player" || hit.transform.tag == "Enemy") // Ray hits a rigidbody thats allowed to be moved
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

        // Tell character ai whether to move or not (Only move if player is not moving ragdoll)
        CharacterRef.GetComponentInChildren<Character_AI>().StopOrResumeMoving(!MoveRagdoll);
    }

    internal void ReloadCharacter()
    {
        // Reload character
        CharacterRef = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot())); // Get slot no. then character name then load character
        Character CharTemp = CharacterRef.GetComponentInChildren<Character>(); // Temp ref to char script

        // Update UI and give camera new ref
        UpdateText_CharacterName(CharacterRef.name);
        UpdateText_Level(CharTemp.Level.ToString());

        // Update move slot buttons (Looks at char ref's children to find character script)
        UpdateText_Moves(1, CharTemp.MoveSlots[0]); // Move 1
        UpdateText_Moves(2, CharTemp.MoveSlots[1]); // Move 2
        UpdateText_Moves(3, CharTemp.MoveSlots[2]); // Move 3

        // Set move bonuses and penalties
        MoveStarvingPenalty = CharTemp.CurrentFullness <= 25 ? 1f : 0f; // If fullness < 25% then turn penalty on
        MoveRefreshedBonus = CharTemp.CurrentFullness >= 75 ? 1f : 0f; // If fullness > 75% then turn bonus on
        MoveHappinessBonus = CharTemp.Happiness >= 75 ? 1f : 0f; // If happiness > 75% then turn bonus on

        // Calculate move delay (Factor in bonuses and penalties)
        MoveUsageDelay = (MoveSpeedBase + MoveStarvingPenalty - MoveRefreshedBonus - MoveHappinessBonus) 
                            / CharTemp.Speed;

        // Player scene setup
        CharacterRef.transform.position += PlayerSpawnOffset; // Spawn above ground and to the left
        CameraRef.GetComponent<CameraFollow>().SetPlayerRef(CharacterRef); // Set player ref in camera   
        gameObject.GetComponent<EnemyManager>().SetPlayerRef(CharacterRef); // Set player ref in enemy manager
    }

    // Object to save needs to have the character script attached
    internal void SaveCharaterStats(GameObject ObjectToSave = null)
    {
        // Default object is character ref's child (body)
        if (ObjectToSave == null) { ObjectToSave = CharacterRef.transform.GetChild(0).gameObject; }

        // Save character's stats
        SaveRef.Save(LoadRef.Load(LoadRef.LoadCurrentSlot()), ObjectToSave); // Load name from current slot to ensure names line up for saving and loading
    }

    // Move buttons
    public void MoveButton(int buttonNum)
    {
        Character charScript = CharacterRef.GetComponentInChildren<Character>();
        bool giveTypeBoost = false;
        Vector3 spawnOffset = new Vector3(3f, 1f, 0f);

        switch (buttonNum)
        {
            case 1: // Move slot 1
                // Use the move dictionary to get the move type and then check if the char has that type
                if (charScript.CharactersElementTypes.Contains(element.MoveDictionary[charScript.MoveSlots[0]]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(charScript.MoveSlots[0], giveTypeBoost, charScript.Attack, charScript.Accuracy,
                                charScript.CritChance, EnemyRef, true);
                break;

            case 2: // Move slot 2
                // Use the move dictionary to get the move type and then check if the char has that type
                if (charScript.CharactersElementTypes.Contains(element.MoveDictionary[charScript.MoveSlots[1]]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(charScript.MoveSlots[1], giveTypeBoost, charScript.Attack, charScript.Accuracy,
                                charScript.CritChance, EnemyRef, true);
                break;

            case 3: // Move slot 3
                // Use the move dictionary to get the move type and then check if the char has that type
                if (charScript.CharactersElementTypes.Contains(element.MoveDictionary[charScript.MoveSlots[2]]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(charScript.MoveSlots[2], giveTypeBoost, charScript.Attack, charScript.Accuracy,
                                charScript.CritChance, EnemyRef, true);
                break;

            default:
                Debug.Log("Button num isn't valid. There are only 3 buttons!");
                break;
        }

        // Reset timer
        LastMoveUseTime = Time.time;
    }

    // Call when enemy is defeated
    public void WinBattle()
    {
        // Give exp

        // Save stat changes i.e. exp

        // Load sandbox
        gameObject.GetComponent<Menu>().LoadScene(SandboxSceneName);
    }

    // Call when player is defeated
    public void LoseBattle()
    {
        // Save stat changes i.e. exp

        // Load sandbox
        gameObject.GetComponent<Menu>().LoadScene(SandboxSceneName);
    }

    public void SetEnemyRef(GameObject enemyRef)
    {
        EnemyRef = enemyRef;
    }

    public GameObject GetCharacterParentRef()
    {
        return CharacterRef;
    }

    // Text update methods
    public void UpdateText_CharacterName(string name)
    {
        CharacterNameText.text = name;
    }

    public void UpdateText_Health(string currentHealth)
    {
        HealthText.text = currentHealth;
    }

    public void UpdateText_Moves(int moveSlotNum, Elements.ElementalMoves move)
    {
        switch (moveSlotNum)
        {
            case 1: // Move Slot 1
                MoveButtonText1.text = element.ElementalMovesToString(move);
                break;
            case 2: // Move Slot 2
                MoveButtonText2.text = element.ElementalMovesToString(move);
                break;
            case 3: // Move Slot 3
                MoveButtonText3.text = element.ElementalMovesToString(move);
                break;
            default:
                Debug.Log("Move slot num not recognized: " + moveSlotNum);
                break;
        }
    }
    void UpdateText_Level(string currentLevel)
    {
        LevelText.text = currentLevel;
    }
}
