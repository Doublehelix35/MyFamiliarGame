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
    public bool ThisCharacterIsActive = true;
    public bool InBattleMode = false;

    // Stats
    internal int HealthMax = 60;
    internal int HealthInitial = 20;
    internal int HappinessMax = 50;
    int Health = 0;
    int Happiness = 1;
    float InvincibilityTimer = 0.5f;
    float DamageTakenTime;

    // Battle stats
    internal float Attack = 1f;
    float Accuracy = 1f; // Determines if a move hits or misses
    float CritChance = 1f; // Chance to get a critical hit
    float Defence = 1f;
    float DodgeChance = 1f; // Chance to dodge an incoming attack
    float Speed = 1f;

    // Stomach/Fullness (%)
    int CurrentFullness = 20;
    int MinFullness = 0;
    int MaxFullness = 100;
    float FullnessLastTickTime; // Time of the last tick
    public float FullnessTickFrequency = 5f; // How often fullness decreases
    public int FullnessTickPower = 1; // How much to remove from fullness every tick


    // Leveling
    public int Level = 1;
    int Experience = 0;
    int ExpToLevelUp = 1;
    int[] LevelsToEvolveAt = { 2, 5, 10, 15 }; // Levels that character evolves at
    internal int CurrentEvolutionStage = 0; // How many times has it evolved?

    // Elemental spec points
    int AirPoints = 0;
    int EarthPoints = 0;
    int FirePoints = 0;
    int NaturePoints = 0;
    int WaterPoints = 0;

    // Elemental typing
    internal List<Elements.ElementType> CharactersElementTypes = new List<Elements.ElementType>();

    // Moves known
    internal Elements.ElementalMoves MoveSlot1 = Elements.ElementalMoves.Tackle;
    internal Elements.ElementalMoves MoveSlot2 = Elements.ElementalMoves.FireBlaze;
    internal Elements.ElementalMoves MoveSlot3 = Elements.ElementalMoves.NaturesWrath;

    // Constructor
    internal Character()
    {
        // Most construction is done externally during loading.
        // Stats and other data that is used throughout the lifespan of
        // the character should be passed in to be handled internally
    }

    void Awake()
    {
        // Init object refs
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");

        // Set in battle mode?
        if (GameManagerRef.GetComponent<BattleManager>().isActiveAndEnabled)
        {
            InBattleMode = true;
        }
        else
        {
            InBattleMode = false;
        }
    }

    void Start()
    {
        // Init stats
        Health = HealthInitial;
        DamageTakenTime = Time.time;
        ExpToLevelUp = ExpNeededForNextLevel();
        FullnessLastTickTime = Time.time;

        // Init Element typing
        if(CharactersElementTypes.Count >= 0) // no element type found
        {
            CharactersElementTypes.Add(Elements.ElementType.NonElemental);
        }


        // Init UI texts
        if (InBattleMode)
        {
            GameManagerRef.GetComponent<BattleManager>().UpdateText_Health(Health.ToString());
        }
        else
        {
            GameManagerRef.GetComponent<GameManager>().UpdateText_Level(Level.ToString());
            GameManagerRef.GetComponent<GameManager>().UpdateText_Exp(Experience.ToString());
            GameManagerRef.GetComponent<GameManager>().UpdateText_Happiness(Happiness.ToString());
            GameManagerRef.GetComponent<GameManager>().UpdateText_Health(Health.ToString());
            GameManagerRef.GetComponent<GameManager>().UpdateText_Fullness(CurrentFullness.ToString());
            GameManagerRef.GetComponent<GameManager>().UpdateText_AllElements(AirPoints.ToString(), EarthPoints.ToString(), FirePoints.ToString(), NaturePoints.ToString(), WaterPoints.ToString());
        }
    }

    void Update()
    {
        // Fullness decrease
        if(FullnessLastTickTime + FullnessTickFrequency < Time.time && !InBattleMode)
        {
            // Reduce fullness
            CurrentFullness -= FullnessTickPower;

            // Clamp current fullness between min and max
            CurrentFullness = Mathf.Clamp(CurrentFullness, MinFullness, MaxFullness); 

            // Reset last tick time
            FullnessLastTickTime = Time.time;

            // Update ui
            GameManagerRef.GetComponent<GameManager>().UpdateText_Fullness(CurrentFullness.ToString());
        }
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
        if (InBattleMode)
        {
            GameManagerRef.GetComponent<BattleManager>().UpdateText_Health(Health.ToString());
        }
        else
        {
            GameManagerRef.GetComponent<GameManager>().UpdateText_Health(Health.ToString());
        }
        
    }

    // To gain happiness pass in positive value, to lose happiness pass in negative value
    public void ChangeHappiness(int value)
    {
        Happiness += value;

        // Clamp happiness between happiness min and max
        Happiness = Mathf.Clamp(Happiness, -HappinessMax, HappinessMax);

        // Update ui
        if (!InBattleMode)
        {
            GameManagerRef.GetComponent<GameManager>().UpdateText_Happiness(Happiness.ToString());
        }        
    } 

    // To gain fullness pass in positive value, to lose fullness pass in negative value
    public void ChangeFullness(int value)
    {
        CurrentFullness += value;

        // Clamp current fullness between min and max
        CurrentFullness = Mathf.Clamp(CurrentFullness, MinFullness, MaxFullness);

        // Update ui 
        if (!InBattleMode)
        {
            GameManagerRef.GetComponent<GameManager>().UpdateText_Fullness(CurrentFullness.ToString());
        }        
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
        if (!InBattleMode)
        {
            GameManagerRef.GetComponent<GameManager>().UpdateText_AllElements(AirPoints.ToString(), EarthPoints.ToString(), FirePoints.ToString(),
                                                                          NaturePoints.ToString(), WaterPoints.ToString());
        }        
    }

    Elements.ElementType CalculateNewTypeForEvolution()
    {
        Elements.ElementType typeToReturn = Elements.ElementType.NonElemental;

        // Choose type based on type with highest points       
        int[] typePoints = { AirPoints, EarthPoints, FirePoints, NaturePoints, WaterPoints };
        Elements.ElementType[] types = { Elements.ElementType.Air, Elements.ElementType.Earth, Elements.ElementType.Fire, Elements.ElementType.Nature, Elements.ElementType.Water };
        int highestPoint = 0;

        // Loop through all element points and find biggest
        for(int i = 0; i < typePoints.Length; i++)
        {
            if(typePoints[i] > highestPoint) // Is new point higher than the last?
            {
                highestPoint = typePoints[i]; // Update highest point
                typeToReturn = types[i]; // Set type to return to corresponding type
            }
        }

        // Reset all spec points to 0
        AirPoints = 0; EarthPoints = 0; FirePoints = 0; NaturePoints = 0; WaterPoints = 0;

        return typeToReturn;
    }

    public void GainExp(int expPoints)
    {
        Experience += expPoints;

        if (Experience > ExpToLevelUp)
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

                    // Add new type based on spec points
                    CharactersElementTypes.Add(CalculateNewTypeForEvolution()); 
                    if(CurrentEvolutionStage == 1) // First evolution
                    {
                        CharactersElementTypes.Remove(Elements.ElementType.NonElemental); // Remove non-elemental
                    }
                    // Increase stats
                    LevelUpStats();

                    // Save evolution and reload      
                    ThisCharacterIsActive = false; // Dont interact with anything else
                    GameManagerRef.GetComponent<GameManager>().EvolveToNextStage(gameObject);
                    return;
                }
            }               
        }
        // Update UI
        if (!InBattleMode)
        {
            GameManagerRef.GetComponent<GameManager>().UpdateText_Exp(Experience.ToString());
            GameManagerRef.GetComponent<GameManager>().UpdateText_Level(Level.ToString());
            GameManagerRef.GetComponent<GameManager>().UpdateText_AllElements(AirPoints.ToString(), EarthPoints.ToString(), FirePoints.ToString(),
                                                                              NaturePoints.ToString(), WaterPoints.ToString());
        }        
    }

    int ExpNeededForNextLevel() // Formula for calculating exp needed to reach the next level
    {
        float exponent = 1.5f; // i.e. x^2, 2 would be the exponent
        int baseExp = 10;
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
        if (col.gameObject.GetComponent<Item>() && ThisCharacterIsActive)
        {
            col.gameObject.GetComponent<Item>().Interact(gameObject);
        }
    }
}
