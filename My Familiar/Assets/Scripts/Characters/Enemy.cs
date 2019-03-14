using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Object refs
    GameObject EnemyManagerRef;

    // Stats
    internal int HealthMax = 60;
    internal int HealthInitial = 20;
    int Health = 0;
    float InvincibilityTimer = 0.5f;
    float DamageTakenTime;

    // Battle stats
    internal float Attack = 1f;
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
    
    void Start()
    {
        // Enemy manager ref
        EnemyManagerRef = GameObject.FindGameObjectWithTag("GameController");

        // Init stats
        Health = HealthInitial;
        DamageTakenTime = Time.time;

        // Init type
        CharactersElementTypes.Add(Elements.ElementType.Earth);
    }
    
    void Update()
    {
        
    }
}
