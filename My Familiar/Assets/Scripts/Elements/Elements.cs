using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements : MonoBehaviour {

    /// For all data and methods relating to elements

    // All possible elemental types
    public enum ElementType { NonElemental, Air,  Earth, Fire, Nature, Water }

    // Moves
    public enum ElementalMoves { EmptyMoveSlot, AirStrike, EarthQuake, FireBlaze, NaturesWrath, Tackle, WaterBlast } // EmptyMoveSlot must be ignored and shouldnt be used
    public Dictionary<ElementalMoves, ElementType> MoveDictionary;

    // Move prefabs
    public GameObject AirStrikePrefab;
    public GameObject EarthQuakePrefab;
    public GameObject FireBlazePrefab;
    public GameObject NaturesWrathPrefab;
    public GameObject TacklePrefab;
    public GameObject WaterBlastPrefab;

    // Move power
    float AirStrikeMovePower = 2f;
    float EarthQuakeMovePower = 2f;
    float FireBlazeMovePower = 2f;
    float NaturesWrathMovePower = 2f;
    float TackleMovePower = 1f;
    float WaterBlastMovePower = 2f;


    void Awake()
    {
        MoveDictionary = new Dictionary<ElementalMoves, ElementType>()
    {
        {ElementalMoves.EmptyMoveSlot, ElementType.NonElemental },
        {ElementalMoves.AirStrike, ElementType.Air },
        {ElementalMoves.EarthQuake, ElementType.Earth },
        {ElementalMoves.FireBlaze, ElementType.Fire },
        {ElementalMoves.NaturesWrath, ElementType.Nature },
        {ElementalMoves.Tackle, ElementType.NonElemental },
        {ElementalMoves.WaterBlast, ElementType.Water }        
    };
    }

    public string ElementalMovesToString (ElementalMoves move)
    {
        string moveString = "";

        switch (move)
        {
            case ElementalMoves.EmptyMoveSlot:
                moveString = "Empty";
                break;
            case ElementalMoves.AirStrike:
                moveString = "Air Strike";
                break;
            case ElementalMoves.EarthQuake:
                moveString = "Earthquake";
                break;
            case ElementalMoves.FireBlaze:
                moveString = "Fire Blaze";
                break;
            case ElementalMoves.NaturesWrath:
                moveString = "Natures Wrath";
                break;
            case ElementalMoves.Tackle:
                moveString = "Tackle";
                break;
            case ElementalMoves.WaterBlast:
                moveString = "Water Blast";
                break;
            default:
                break;
        }
        return moveString;
    }

    public void UseMove(ElementalMoves moveToUse, bool isThereTypeBoost, float attackValue, Transform target, Vector3 spawnLocation)
    {
        // Set type boost
        float typeBoost = 1f;
        if (isThereTypeBoost)
        {
            typeBoost = 1.5f;
        }

        // Use selected move
        switch (moveToUse)
        {
            case ElementalMoves.EmptyMoveSlot:
                Debug.Log("No move in this slot!");
                break;

            case ElementalMoves.AirStrike:
                // Spawn air strike at spawn location
                GameObject airStrike = Instantiate(AirStrikePrefab, spawnLocation, Quaternion.identity);
                // Set target
                airStrike.GetComponent<Projectile_Homing>().Target = target;
                // Set damage
                airStrike.GetComponent<Projectile_Homing>().Damage = (int)(attackValue * typeBoost * AirStrikeMovePower);
                break;

            case ElementalMoves.EarthQuake:
                // Spawn earthquake at spawn location
                GameObject earthQuake = Instantiate(EarthQuakePrefab, spawnLocation, Quaternion.identity);
                // Set target
                earthQuake.GetComponent<Projectile_Homing>().Target = target;
                // Set damage
                earthQuake.GetComponent<Projectile_Homing>().Damage = (int)(attackValue * typeBoost * EarthQuakeMovePower);
                break;

            case ElementalMoves.FireBlaze:
                // Spawn fire blaze at spawn location
                GameObject fireBlaze = Instantiate(FireBlazePrefab, spawnLocation, Quaternion.identity);
                // Set target
                fireBlaze.GetComponent<Projectile_Homing>().Target = target;
                // Set damage
                fireBlaze.GetComponent<Projectile_Homing>().Damage = (int)(attackValue * typeBoost * FireBlazeMovePower);
                break;

            case ElementalMoves.NaturesWrath:
                // Spawn natures wrath at spawn location
                GameObject naturesWrath = Instantiate(NaturesWrathPrefab, spawnLocation, Quaternion.identity);
                // Set target
                naturesWrath.GetComponent<Projectile_Homing>().Target = target;
                // Set damage
                naturesWrath.GetComponent<Projectile_Homing>().Damage = (int)(attackValue * typeBoost * NaturesWrathMovePower);
                break;

            case ElementalMoves.Tackle:
                // Spawn tackle at spawn location
                GameObject tackle = Instantiate(TacklePrefab, spawnLocation, Quaternion.identity);
                // Set target
                tackle.GetComponent<Projectile_Homing>().Target = target;
                // Set damage
                tackle.GetComponent<Projectile_Homing>().Damage = (int)(attackValue * typeBoost * TackleMovePower);
                break;

            case ElementalMoves.WaterBlast:
                // Spawn water blast at spawn location
                GameObject waterBlast = Instantiate(WaterBlastPrefab, spawnLocation, Quaternion.identity);
                // Set target
                waterBlast.GetComponent<Projectile_Homing>().Target = target;
                // Set damage
                waterBlast.GetComponent<Projectile_Homing>().Damage = (int)(attackValue * typeBoost * WaterBlastMovePower);
                break;

            default:
                Debug.Log("Error! Move not implemented!");
                break;
        }
    }
}
