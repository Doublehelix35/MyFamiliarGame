using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /// <summary>
    /// Remember most supporting properties like rigidbodies are set up
    /// in the Load_Character script
    /// 
    /// Character script will be on the body object
    /// </summary>

    // Object Refs
    GameObject GameManagerRef;

    // Stats
    internal int HealthMax;
    internal int HappinessMax = 50;
    int Health = 0;
    int Happiness = 1;
    float InvincibilityTimer = 0.5f;
    float DamageTakenTime;

    // Battle stats
    float Attack = 1f;
    float Accuracy = 1f; // Determines if a move hits or misses
    float CritChance = 1f; // Chance to get a critical hit
    float Defence = 1f;
    float DodgeChance = 1f; // Chance to dodge an incoming attack
    float Speed = 1f;

    // Leveling
    int Level = 1;
    int Experience = 0;
    int ExpToLevelUp = 1;
    int[] LevelsToEvolveAt = { 5, 10, 15 }; // Levels that character evolves at
    internal int CurrentEvolutionStage = 0; // How many times has it evolved?

    // Elemental spec points
    int AirPoints = 0;
    int EarthPoints = 0;
    int FirePoints = 0;
    int NaturePoints = 0;
    int WaterPoints = 0;

    // Moves known
    Elements.ElementalMoves MoveSlot1 = Elements.ElementalMoves.EmptyMoveSlot;
    Elements.ElementalMoves MoveSlot2 = Elements.ElementalMoves.EmptyMoveSlot;
    Elements.ElementalMoves MoveSlot3 = Elements.ElementalMoves.EmptyMoveSlot;

    // Elemental typing
    internal List<Elements.ElementType> CharactersElementTypes = new List<Elements.ElementType>();

    // Constructor
    internal Character()
    {
        // Most construction is done externally during loading.
        // Stats and other data that is used throughout the lifespan of
        // the character should be passed in to be handled internally
    }

    void Start()
    {
        // Init object refs
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");

        // Init stats
        Health = HealthMax;
        DamageTakenTime = Time.time;
        ExpToLevelUp = ExpNeededForNextLevel();

        // Init Element typing
        CharactersElementTypes.Add(Elements.ElementType.NonElemental);

        // Init UI texts
        GameManagerRef.GetComponent<GameManager>().UpdateText_Exp(Experience.ToString());
        GameManagerRef.GetComponent<GameManager>().UpdateText_Happiness(Happiness.ToString());
        GameManagerRef.GetComponent<GameManager>().UpdateText_Health(Health.ToString());
        GameManagerRef.GetComponent<GameManager>().UpdateText_AllElements(AirPoints.ToString(), EarthPoints.ToString(), FirePoints.ToString(), NaturePoints.ToString(), WaterPoints.ToString());
    }

    // Sets move slot equal to the move that is passed in
    public void LearnNewMove(int moveSlot, Elements.ElementalMoves moveToLearn)
    {
        switch (moveSlot)
        {
            case 1:
                MoveSlot1 = moveToLearn;
                break;
            case 2:
                MoveSlot2 = moveToLearn;
                break;
            case 3:
                MoveSlot3 = moveToLearn;
                break;
            default:
                break;
        }
    }

    // To gain health pass in positive value, to lose health pass in negative value
    public void ChangeHealth(int value)
    {
        // Exit if invicible
        if(DamageTakenTime >= Time.time - InvincibilityTimer) { return; }

        // Change health based on value
        Health += value;        

        if(Health > HealthMax)
        {
            Health = HealthMax;
        }
        else if (Health <= 0)
        {
            // Player dead or exhausted
            //Destroy(gameObject.transform.parent.gameObject); // Temporary test code
        }


        if (value < 0) // Damage was taken
        {
            DamageTakenTime = Time.time;
        }

        // Update ui
        GameManagerRef.GetComponent<GameManager>().UpdateText_Health(Health.ToString());
    }

    public void ChangeHappiness(int value)
    {
        Happiness += value;

        // Clamp happiness between happiness min and max
        Mathf.Clamp(Happiness, -HappinessMax, HappinessMax);

        // Update ui
        GameManagerRef.GetComponent<GameManager>().UpdateText_Happiness(Happiness.ToString());
    } 

    public void GainElementSpecPoints(Elements.ElementType ElementToGainPoints, int pointValue)
    {
        switch (ElementToGainPoints)
        {
            case Elements.ElementType.Air:
                AirPoints += pointValue;
                break;
            case Elements.ElementType.Earth:
                EarthPoints += pointValue;
                break;
            case Elements.ElementType.Fire:
                FirePoints += pointValue;
                break;
            case Elements.ElementType.Nature:
                NaturePoints += pointValue;
                break;
            case Elements.ElementType.Water:
                WaterPoints += pointValue;
                break;
            case Elements.ElementType.NonElemental:
                Debug.Log("Cant gain points in non-elemental. This does nothing.");
                break;
            default:
                Debug.Log("ERROR! Element not recongized");
                break;
        }
        // Update UI
        GameManagerRef.GetComponent<GameManager>().UpdateText_AllElements(AirPoints.ToString(), EarthPoints.ToString(), FirePoints.ToString(),
                                                                          NaturePoints.ToString(), WaterPoints.ToString());
    }

    public void GainExp(int expPoints)
    {
        Experience += expPoints;

        if (Experience >= ExpToLevelUp)
        {
            Level++;
            Experience = Experience - ExpToLevelUp; // Carry over leftover exp
            ExpToLevelUp = ExpNeededForNextLevel(); // Increase exp needed to lvl up

            // Evolve
            for(int i = 0; i < LevelsToEvolveAt.Length; i++) // Loop through levels to evolve at to search for a match
            {
                if (Level == LevelsToEvolveAt[i])
                {
                    CurrentEvolutionStage++; // Increase count of evolutions

                    CharactersElementTypes.Add(Elements.ElementType.Air); // Add new type
                    if(i == 0) // First evolution
                    {
                        CharactersElementTypes.Remove(Elements.ElementType.NonElemental); // Remove non-elemental
                    }                    
                    GameManagerRef.GetComponent<GameManager>().EvolveToNextStage();
                }
            }            

            // Increase stats
            LevelUpStats();
        }

        // Update UI
        GameManagerRef.GetComponent<GameManager>().UpdateText_Exp(Experience.ToString());
        GameManagerRef.GetComponent<GameManager>().UpdateText_Level(Level.ToString());
    }

    int ExpNeededForNextLevel() // Formula for calculating exp needed to reach the next level
    {
        float exponent = 1.5f; // i.e. x^2, 2 would be the exponent
        int baseExp = 30;
        int expToReturn = (int)Mathf.Floor(baseExp * Mathf.Pow(Level, exponent));

        return expToReturn;
    }

    void LevelUpStats()
    {
        // Stats are increased based on level, what types it has and each types bonus stats
        float Magnitude = 1f / CharactersElementTypes.Count; // Magnitude of stat increase factoring in No. of types a character has 
        float baseStatIncrease = 1f;
        float exponent = 1.5f; // i.e. x^2, 2 would be the exponent
        float statBoostSmall = 1.05f; // 5% increase
        float statBoostMedium = 1.1f; // 10% increase
        float statBoostLarge = 1.15f; // 15% increase
        
        foreach(Elements.ElementType type in CharactersElementTypes)
        {
            if (type == Elements.ElementType.NonElemental) // Normal stats
            {
                // Increase stats by factoring in magnitude and level^exponent
                Attack += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);

            }
            else if (type == Elements.ElementType.Air) // Air stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Air bonuses
                Attack += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
            }
            else if (type == Elements.ElementType.Earth) // Earth stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Earth bonuses
                Attack += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall;
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostLarge;
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
            }
            else if (type == Elements.ElementType.Fire) // Fire stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Fire bonuses
                Attack += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
                Defence += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
            }
            else if (type == Elements.ElementType.Nature) // Nature stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Nature bonuses
                Attack += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall;
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall;
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall;
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall;
            }
            else if (type == Elements.ElementType.Water) // Water stats
            {
                // Increase stats by factoring in magnitude and level^exponent
                Attack += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
            }
        }        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Item>())
        {
            col.gameObject.GetComponent<Item>().Interact(gameObject);
        }
    }
}
