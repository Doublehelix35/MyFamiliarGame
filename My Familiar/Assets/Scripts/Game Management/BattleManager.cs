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

    // Texts
    public Text CharacterNameText;
    public Text HealthText;
    public Text MoveButtonText1;
    public Text MoveButtonText2;
    public Text MoveButtonText3;
    
    void Awake()
    {
        // Init objects
        element = GetComponent<Elements>();
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
        gameObject.GetComponent<EnemyManager>().SetPlayerRef(CharacterRef); // Set player ref in enemy manager and enemy
    }

    // Move buttons
    public void MoveButton(int buttonNum)
    {
        Character charScript = CharacterRef.GetComponentInChildren<Character>();
        bool giveTypeBoost = false;
        Vector3 spawnOffset = new Vector3(1f, 1f, 0f);

        switch (buttonNum)
        {
            case 1: // Move slot 1
                // Use the move dictionary to get the move type and then check if the char has that type
                if (charScript.CharactersElementTypes.Contains(element.MoveDictionary[charScript.MoveSlot1]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(charScript.MoveSlot1, giveTypeBoost, charScript.Attack, EnemyRef.transform, CharacterRef.transform.position + spawnOffset);
                break;

            case 2: // Move slot 2
                // Use the move dictionary to get the move type and then check if the char has that type
                if (charScript.CharactersElementTypes.Contains(element.MoveDictionary[charScript.MoveSlot2]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(charScript.MoveSlot2, giveTypeBoost, charScript.Attack, EnemyRef.transform, CharacterRef.transform.position + spawnOffset);
                break;

            case 3: // Move slot 3
                // Use the move dictionary to get the move type and then check if the char has that type
                if (charScript.CharactersElementTypes.Contains(element.MoveDictionary[charScript.MoveSlot3]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(charScript.MoveSlot3, giveTypeBoost, charScript.Attack, EnemyRef.transform, CharacterRef.transform.position + spawnOffset);
                break;

            default:
                Debug.Log("Button num isn't valid. There are only 3 buttons!");
                break;
        }
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
}
