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

    // Move particles
    public ParticleSystem AirStrikeParticleLeft;
    public ParticleSystem EarthQuakeParticleLeft;
    public ParticleSystem FireBlazeParticleLeft;
    public ParticleSystem NaturesWrathParticleLeft;
    public ParticleSystem TackleParticleLeft;
    public ParticleSystem WaterBlastParticleLeft;

    public ParticleSystem AirStrikeParticleRight;
    public ParticleSystem EarthQuakeParticleRight;
    public ParticleSystem FireBlazeParticleRight;
    public ParticleSystem NaturesWrathParticleRight;
    public ParticleSystem TackleParticleRight;
    public ParticleSystem WaterBlastParticleRight;

    // Move power
    float AirStrikeMovePower = 2f;
    float EarthQuakeMovePower = 2f;
    float FireBlazeMovePower = 2f;
    float NaturesWrathMovePower = 2f;
    float TackleMovePower = 1f;
    float WaterBlastMovePower = 2f;

    float CritPower = 4f;
    int DamageMin = 1;

    // Type relationships
    class TypeRelationship
    {        
        public ElementType StrongType;
        public ElementType WeakType;

        public TypeRelationship(ElementType strongType, ElementType weakType )
        {            
            StrongType = strongType;
            WeakType = weakType;
        }
    }

    // Define relationships between elements
    TypeRelationship Relationship_WaterFire = new TypeRelationship(ElementType.Water, ElementType.Fire);
    TypeRelationship Relationship_FireNature = new TypeRelationship(ElementType.Fire, ElementType.Nature);
    TypeRelationship Relationship_NatureEarth = new TypeRelationship(ElementType.Nature, ElementType.Earth);
    TypeRelationship Relationship_EarthAir = new TypeRelationship(ElementType.Earth, ElementType.Air);
    TypeRelationship Relationship_AirWater = new TypeRelationship(ElementType.Air, ElementType.Water);

    // Store all relationships in a list
    List<TypeRelationship> Relationships = new List<TypeRelationship>();

    void Awake()
    {
        // Init move dictionary
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
        // Init relationships
        Relationships.Add(Relationship_WaterFire);
        Relationships.Add(Relationship_FireNature);
        Relationships.Add(Relationship_NatureEarth);
        Relationships.Add(Relationship_EarthAir);
        Relationships.Add(Relationship_AirWater);
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

    public void UseMove(ElementalMoves moveToUse, bool isThereTypeBoost, int attackValue, float accuracy, float critChance, GameObject target, bool isLeft)
    {
        // Use selected move
        switch (moveToUse)
        {
            case ElementalMoves.EmptyMoveSlot:
                Debug.Log("No move in this slot!");
                break;

            case ElementalMoves.AirStrike:
                // Play Airstrike
                if (isLeft) { AirStrikeParticleLeft.Play(); }
                else { AirStrikeParticleRight.Play(); }

                break;

            case ElementalMoves.EarthQuake:
                // Play Earthquake
                if (isLeft) { EarthQuakeParticleRight.Play(); }
                else { EarthQuakeParticleLeft.Play(); }

                break;

            case ElementalMoves.FireBlaze:
                // Play Fire blaze
                if (isLeft) { FireBlazeParticleLeft.Play(); }
                else { FireBlazeParticleRight.Play(); }

                break;

            case ElementalMoves.NaturesWrath:
                // Play Natures wrath
                if (isLeft) { NaturesWrathParticleLeft.Play(); }
                else { NaturesWrathParticleRight.Play(); }

                break;

            case ElementalMoves.Tackle:
                // Play Tackle
                if (isLeft) { TackleParticleLeft.Play(); }
                else { TackleParticleRight.Play(); }

                break;

            case ElementalMoves.WaterBlast:
                // Play Water blast
                if (isLeft) { FireBlazeParticleLeft.Play(); }
                else { FireBlazeParticleRight.Play(); }

                break;

            default:
                Debug.Log("Error! Move not implemented!");
                break;
        }

        // Accuracy check
        float randAcc = Random.Range(0f, 1f);
        if (randAcc > accuracy)
        {
            // Move misses
            Debug.Log("Move Missed!");
            return;
        }

        // Calculate damage        
        float typeBoost = isThereTypeBoost ? 1.5f : 1f; // Set type boost
        int Damage = attackValue;
        float damageModifier = 1f;
        float randCrit = Random.Range(0f, 1f);
        damageModifier = randCrit < critChance ? CritPower : 1f; // Check if attack will deal critical damage

        // Do the same thing for player and enemy but use their own scripts
        if (target.tag == "Enemy")
        {
            Enemy enemyRef = target.GetComponentInChildren<Enemy>();

            // Loop through character types to determine damage modifier
            foreach (ElementType t in enemyRef.CharactersElementTypes)
            {
                if (t == MoveDictionary[moveToUse]) // If they are same type
                {
                    // Reduce damage modifier by 50%
                    damageModifier *= 0.5f;
                }
                else if (CheckTypeRelationship(t, MoveDictionary[moveToUse])) // Check for type relationship
                {
                    // If item type is stronger than t then deal more damage
                    if (ReturnStrongTypeInRelationship(t, MoveDictionary[moveToUse]) == MoveDictionary[moveToUse])
                    {
                        damageModifier *= 2f;
                    }
                }
            }
            // Calculate and then deal damage
            Damage = (int)(Damage * damageModifier); // Damage * modifier
            Damage = Damage - enemyRef.Defence <= DamageMin ? DamageMin : Damage - enemyRef.Defence; // Damage - defence
            enemyRef.ChangeHealth(-Damage);
        }
        else // Player
        {
            Character charRef = target.GetComponentInChildren<Character>();

            // Loop through character types to determine damage modifier
            foreach (ElementType t in charRef.CharactersElementTypes)
            {
                if (t == MoveDictionary[moveToUse]) // If they are same type
                {
                    // Reduce damage modifier by 50%
                    damageModifier *= 0.5f;
                }
                else if (CheckTypeRelationship(t, MoveDictionary[moveToUse])) // Check for type relationship
                {
                    // If item type is stronger than t then deal more damage
                    if (ReturnStrongTypeInRelationship(t, MoveDictionary[moveToUse]) == MoveDictionary[moveToUse])
                    {
                        damageModifier *= 2f;
                    }
                }
            }
            // Calculate and then deal damage
            Damage = (int)(Damage * damageModifier); // Damage * modifier
            Damage = Damage - charRef.Defence <= DamageMin ? DamageMin : Damage - charRef.Defence; // Damage - defence
            charRef.ChangeHealth(-Damage); // Deal damage            
        }
    }

    // Check if there is a type relationship
    public bool CheckTypeRelationship(ElementType type1, ElementType type2)
    {
        // Loop through relationships
        foreach(TypeRelationship t in Relationships)
        {
            // if both types are in the relationship return true
            if(t.StrongType == type1 && t.WeakType == type2 || t.WeakType == type1 && t.StrongType == type2)
            {
                return true;
            }
        }
        return false;
    }

    // Return strong type in a relationship (Only call if there is a relationship between type1 and type 2)
    public ElementType ReturnStrongTypeInRelationship(ElementType type1, ElementType type2)
    {
        ElementType typeToReturn = ElementType.NonElemental;
        
        // Loop through relationships
        foreach (TypeRelationship t in Relationships)
        {
            // if both types are in the relationship return true
            if (t.StrongType == type1 && t.WeakType == type2 || t.WeakType == type1 && t.StrongType == type2)
            {
                typeToReturn = t.StrongType;
            }
        }

        // Check for error
        if(typeToReturn == ElementType.NonElemental)
        {
            Debug.Log("Error! Type relationship doesnt exist for strong type to be returned");
        }

        return typeToReturn;
    }
}
