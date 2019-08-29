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
        ReloadCharacter();

        // Start hazard spawner
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
