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
    public GameObject CameraRef;
    Elements element;


    // Texts
    public Text CharacterNameText;
    public Text HealthText;
    public Text EnemyHealthText;
    public Text MoveButtonText1;
    public Text MoveButtonText2;
    public Text MoveButtonText3;

    
    void Awake()
    {
        // Init objects
        element = gameObject.AddComponent<Elements>();
        ReloadCharacter();
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

        // Update move slot buttons (Looks at char ref's children to find character script)
        UpdateText_Moves(1, CharacterRef.GetComponentInChildren<Character>().MoveSlot1); // Move 1
        UpdateText_Moves(2, CharacterRef.GetComponentInChildren<Character>().MoveSlot2); // Move 2
        UpdateText_Moves(3, CharacterRef.GetComponentInChildren<Character>().MoveSlot3); // Move 3

        CharacterRef.transform.position += new Vector3(-6f, 4f, 0f); // Spawn above ground and to the left
        CameraRef.GetComponent<CameraFollow>().SetPlayerRef(CharacterRef); // Set player ref in camera   
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

    public void UpdateText_EnemyHealth(string currentEnemyHealth)
    {
        EnemyHealthText.text = currentEnemyHealth;
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
}
