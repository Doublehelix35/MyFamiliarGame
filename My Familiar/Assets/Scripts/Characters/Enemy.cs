using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Object refs
    GameObject EnemyManagerRef;
    public GameObject PlayerRef;
    Elements element;

    // Stats
    internal int HealthMax = 60;
    internal int HealthInitial = 20;
    int Health = 0;
    float InvincibilityTimer = 0.5f;
    float DamageTakenTime;

    // Battle stats
    internal float Attack = 2f;
    float Accuracy = 1f; // Determines if a move hits or misses
    float CritChance = 1f; // Chance to get a critical hit
    float Defence = 1f;
    float DodgeChance = 1f; // Chance to dodge an incoming attack
    float Speed = 1f;

    // Leveling
    public int Level = 1;
    internal int CurrentEvolutionStage = 0; // How many times has it evolved?
    

    // Elemental typing
    internal List<Elements.ElementType> CharactersElementTypes = new List<Elements.ElementType>();

    // Moves known
    internal Elements.ElementalMoves MoveSlot1 = Elements.ElementalMoves.Tackle;
    internal Elements.ElementalMoves MoveSlot2 = Elements.ElementalMoves.EarthQuake;
    internal Elements.ElementalMoves MoveSlot3 = Elements.ElementalMoves.WaterBlast;

    // Move usage timer
    float LastMoveUseTime;
    float MoveUsageDelay = 4f;
    
    void Start()
    {
        // init object refs
        EnemyManagerRef = GameObject.FindGameObjectWithTag("GameController");
        element = EnemyManagerRef.GetComponent<Elements>();

        // Init stats
        Health = HealthInitial;
        DamageTakenTime = Time.time;
        LastMoveUseTime = Time.time;

        // Init type
        CharactersElementTypes.Add(Elements.ElementType.Nature);

        // Update ui
        EnemyManagerRef.GetComponent<EnemyManager>().UpdateText_EnemyHealth(Health.ToString());
    }
    
    void Update()
    {
        if(LastMoveUseTime + MoveUsageDelay < Time.time)
        {
            // Randomly choose move
            int randMove = Random.Range(1, 3);
            MoveSelect(randMove);

            // Reset timer
            LastMoveUseTime = Time.time;
        }
    }

    // Move selection
    void MoveSelect(int moveNum)
    { 
        bool giveTypeBoost = false;
        Vector3 spawnOffset = new Vector3(-2f, 1f, 0f);

        switch (moveNum)
        {
            case 1: // Move slot 1
                // Use the move dictionary to get the move type and then check if the char has that type
                if (CharactersElementTypes.Contains(element.MoveDictionary[MoveSlot1]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(MoveSlot1, giveTypeBoost, Attack, PlayerRef.transform, transform.position + spawnOffset);
                break;

            case 2: // Move slot 2
                // Use the move dictionary to get the move type and then check if the char has that type
                if (CharactersElementTypes.Contains(element.MoveDictionary[MoveSlot2]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(MoveSlot2, giveTypeBoost, Attack, PlayerRef.transform, transform.position + spawnOffset);
                break;

            case 3: // Move slot 3
                // Use the move dictionary to get the move type and then check if the char has that type
                if (CharactersElementTypes.Contains(element.MoveDictionary[MoveSlot3]))
                {
                    giveTypeBoost = true;
                }
                element.UseMove(MoveSlot3, giveTypeBoost, Attack, PlayerRef.transform, transform.position + spawnOffset);
                break;

            default:
                Debug.Log("Button num isn't valid. There are only 3 buttons!");
                break;
        }
    }

    // To gain health pass in positive value, to lose health pass in negative value
    public void ChangeHealth(int value)
    {
        // Exit if invicible
        if (DamageTakenTime >= Time.time - InvincibilityTimer) { return; }

        // Change health based on value
        Health += value;


        Debug.Log("Damage!" + Health + "Value" + value);

        if (Health > HealthMax)
        {
            Health = HealthMax;
        }
        else if (Health <= 0)
        {
            // Enemy dead or exhausted
            EnemyManagerRef.GetComponent<BattleManager>().WinBattle();
        }

        if (value < 0) // Damage was taken
        {
            DamageTakenTime = Time.time;
        }

        // Update ui
        EnemyManagerRef.GetComponent<EnemyManager>().UpdateText_EnemyHealth(Health.ToString());

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Item>())
        {
            col.gameObject.GetComponent<Item>().Interact(gameObject);
        }
    }
}
