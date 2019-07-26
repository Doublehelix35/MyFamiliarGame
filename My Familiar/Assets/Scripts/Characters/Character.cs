using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Subject
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

    
    // Objects that are attached
    internal List<GameObject> AttachedBalloonObjects = new List<GameObject>();

    // Stats
    internal int HealthMax = 60; // Health maximum
    internal int Health = 0; // Current health
    internal int Happiness = 50; // 0 min, start at 50, max 100
    internal int HappinessMax = 100;
    float InvincibilityTimer = 0.5f;
    float DamageTakenTime;

    // Battle stats
    internal int Attack; // Damage of an attack
    internal float Accuracy; // Determines if a move hits or misses
    internal float CritChance; // Chance to get a critical hit
    internal int Defence; // Amount of damage to ignore
    internal float DodgeChance; // Chance to dodge an incoming attack
    internal float Speed; // How quickly the character attacks

    // Stomach/Fullness (%)
    internal int CurrentFullness = 50;
    int MinFullness = 0;
    int MaxFullness = 100;
    float FullnessLastTickTime; // Time of the last tick
    public float FullnessTickFrequency = 5f; // How often fullness decreases
    public int FullnessTickPower = 1; // How much to remove from fullness every tick

    // Mouth
    internal Material MouthMat;
    internal Texture MouthHappy;
    internal Texture MouthNormal;
    internal Texture MouthSad;

    // Leveling
    public int Level = 1;
    internal int Experience = 0;
    int ExpToLevelUp = 1;
    int[] LevelsToEvolveAt = { 3, 6, 9 }; // Levels that character evolves at
    internal int CurrentEvolutionStage = 0; // How many times has it evolved?

    // Elemental spec points
    internal int AirPoints = 0;
    internal int EarthPoints = 0;
    internal int FirePoints = 0;
    internal int NaturePoints = 0;
    internal int WaterPoints = 0;

    // Elemental typing
    internal List<Elements.ElementType> CharactersElementTypes = new List<Elements.ElementType>();

    // Moves known
    internal Elements.ElementalMoves[] MoveSlots; // Max 3 moves

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

        // Init move slots
        MoveSlots = new Elements.ElementalMoves[3];

        // Set in battle mode
        InBattleMode = GameManagerRef.GetComponent<BattleManager>() != null ? true : false;
    }

    void Start()
    {
        // Init stats
        DamageTakenTime = Time.time;
        ExpToLevelUp = ExpNeededForNextLevel();
        FullnessLastTickTime = Time.time;
        MouthMat.mainTexture = MouthNormal;

        // Init Element typing
        if(CharactersElementTypes.Count == 0) // no element type found
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
            GameManagerRef.GetComponent<GameManager>().Update_Exp(Experience, ExpToLevelUp);
            GameManagerRef.GetComponent<GameManager>().Update_Happiness(Happiness, HappinessMax, true);
            GameManagerRef.GetComponent<GameManager>().UpdateText_Health(Health.ToString());
            GameManagerRef.GetComponent<GameManager>().Update_Fullness(CurrentFullness, MaxFullness, true);
            GameManagerRef.GetComponent<GameManager>().UpdateText_AllElements(AirPoints.ToString(), EarthPoints.ToString(), FirePoints.ToString(), NaturePoints.ToString(), WaterPoints.ToString());
        }
    }

    void Update()
    {
        // Fullness decrease
        if(FullnessLastTickTime + FullnessTickFrequency < Time.time && !InBattleMode)
        {
            // Reduce fullness
            ChangeFullness(-FullnessTickPower);

            // Reset last tick time
            FullnessLastTickTime = Time.time;
        }
    }

    // Sets move slot equal to the move that is passed in
    public void LearnNewMove(int moveSlot, Elements.ElementalMoves moveToLearn)
    {
        switch (moveSlot)
        {
            case 1:
                MoveSlots[0] = moveToLearn;
                break;
            case 2:
                MoveSlots[1] = moveToLearn;
                break;
            case 3:
                MoveSlots[2] = moveToLearn;
                break;
            default:
                break;
        }
    }

    // To gain health pass in positive value. To lose health pass in negative value
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
            if (InBattleMode)
            {
                // Player loses battle
                GameManagerRef.GetComponent<BattleManager>().LoseBattle();
            }
        }

        if (value < 0) // Damage was taken
        {
            DamageTakenTime = Time.time;
            Notify(gameObject, Observer.Events.CharacterHurt);
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

        // Clamp happiness between zero and happiness max
        Happiness = Mathf.Clamp(Happiness, 0, HappinessMax);

        // Notify that happiness is max
        if (Happiness == HappinessMax) { Notify(gameObject, Observer.Events.HappinessMax); }

        // Update facial features
        if (Happiness <= 25)
        {
            // Sad
            MouthMat.mainTexture = MouthSad;
        }
        else if (Happiness >= 75)
        {
            // Happy
            MouthMat.mainTexture = MouthHappy;
        }
        else
        {
            // Normal
            MouthMat.mainTexture = MouthNormal;
        }

        // Update ui
        if (!InBattleMode)
        {            
            // If value positive
            if(value > 0)
            {
                // Update happiness and flash up arrow
                GameManagerRef.GetComponent<GameManager>().Update_Happiness(Happiness, HappinessMax, true);
            }
            else
            {
                // Update happiness and flash down arrow
                GameManagerRef.GetComponent<GameManager>().Update_Happiness(Happiness, HappinessMax, false);
            }
        }        
    } 

    // To gain fullness pass in positive value, to lose fullness pass in negative value
    public void ChangeFullness(int value)
    {
        CurrentFullness += value;

        // Clamp current fullness between min and max
        CurrentFullness = Mathf.Clamp(CurrentFullness, MinFullness, MaxFullness);

        if (CurrentFullness == MaxFullness) { Notify(gameObject, Observer.Events.EnergyFull); }

        // Update ui 
        if (!InBattleMode)
        {
            if(value > 0)
            {
                GameManagerRef.GetComponent<GameManager>().Update_Fullness(CurrentFullness, MaxFullness, true);
            }
            else
            {
                GameManagerRef.GetComponent<GameManager>().Update_Fullness(CurrentFullness, MaxFullness, false);
            }
            
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
            // Level up
            Level++;
            LevelUpStats(); // Increase stats
            GameManagerRef.GetComponent<GameManager>().FlashLevelUp(); // Flash level up UI

            Experience = Experience - ExpToLevelUp; // Carry over leftover exp
            ExpToLevelUp = ExpNeededForNextLevel(); // Increase exp needed to lvl up

            // Evolve
            for(int i = 0; i < LevelsToEvolveAt.Length; i++) // Loop through levels to evolve at to search for a match
            {
                if (Level == LevelsToEvolveAt[i]) // Lvl is an evolution lvl
                {
                    // Detach from balloons
                    DetachFromAllBalloons();

                    // Increase count of evolutions
                    CurrentEvolutionStage++;

                    if (CurrentEvolutionStage == 1) // First evolution
                    {
                        CharactersElementTypes.Remove(Elements.ElementType.NonElemental); // Remove non-elemental
                    }

                    // Add new type based on spec points
                    Elements.ElementType newType = CalculateNewTypeForEvolution();
                    CharactersElementTypes.Add(newType);

                    // Add new move based on type to evolve to
                    Elements.ElementalMoves newMove = Elements.ElementalMoves.EmptyMoveSlot;

                    switch (newType)
                    {
                        case Elements.ElementType.Air:
                            newMove = Elements.ElementalMoves.AirStrike;
                            break;
                        case Elements.ElementType.Earth:
                            newMove = Elements.ElementalMoves.EarthQuake;
                            break;
                        case Elements.ElementType.Fire:
                            newMove = Elements.ElementalMoves.FireBlaze;
                            break;
                        case Elements.ElementType.Nature:
                            newMove = Elements.ElementalMoves.NaturesWrath;
                            break;
                        case Elements.ElementType.Water:
                            newMove = Elements.ElementalMoves.WaterBlast;
                            break;
                        default:
                            Debug.Log("Error choosing move based on type " + newType);
                            break;
                    }

                    // Chose move slot based on evolution count
                    switch (CurrentEvolutionStage)
                    {
                        case 1:
                            // Add new move to slot 2
                            MoveSlots[1] = newMove;
                            break;
                        case 2:
                            // Add new move to slot 3
                            MoveSlots[2] = newMove;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            // Add new move to slot 1
                            MoveSlots[0] = newMove;
                            break;
                        default:
                            Debug.Log("Error adding new move to move slot");
                            break;
                    }                   

                    // Reset spec points
                    AirPoints = 0; EarthPoints = 0; FirePoints = 0;
                    NaturePoints = 0; WaterPoints = 0;
                    
                    // Save evolution and reload      
                    ThisCharacterIsActive = false; // Dont interact with anything else
                    Notify(gameObject, Observer.Events.Evolve); // Evolved event notification
                    GameManagerRef.GetComponent<GameManager>().FlashEvolved(); // Flash evolved UI
                    GameManagerRef.GetComponent<GameManager>().EvolveToNextStage(gameObject);
                    return;
                }
            }               
        }
        // Update UI
        if (!InBattleMode)
        {
            GameManagerRef.GetComponent<GameManager>().Update_Exp(Experience, ExpToLevelUp);
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
                Attack = (int)(Attack + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent));
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence = (int)(Defence + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent));
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);

            }
            else if (type == Elements.ElementType.Air) // Air stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Air bonuses
                Attack = (int)(Attack + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent));
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence = (int)(Defence + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent));
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
            }
            else if (type == Elements.ElementType.Earth) // Earth stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Earth bonuses
                Attack = (int)(Attack + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall);
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence = (int)(Defence + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostLarge);
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
            }
            else if (type == Elements.ElementType.Fire) // Fire stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Fire bonuses
                Attack = (int)(Attack + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium);
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
                Defence = (int)(Defence + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent));
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
            }
            else if (type == Elements.ElementType.Nature) // Nature stats
            {
                // Increase stats by factoring in magnitude and level^exponent and Nature bonuses
                Attack = (int)(Attack + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall);
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall;
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence = (int)(Defence + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall);
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostSmall;
            }
            else if (type == Elements.ElementType.Water) // Water stats
            {
                // Increase stats by factoring in magnitude and level^exponent
                Attack = (int)(Attack + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent));
                Accuracy += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                CritChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent);
                Defence = (int)(Defence + baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent));
                DodgeChance += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
                Speed += baseStatIncrease * Magnitude * Mathf.Pow(Level, exponent) * statBoostMedium;
            }
        }        
    }

    // Detach from balloons
    void DetachFromAllBalloons()
    {
        for (int i = 0; i < AttachedBalloonObjects.Count; i++)
        {
            if (AttachedBalloonObjects[i] != null) // Object exists
            {
                AttachedBalloonObjects[i].GetComponent<Item_Balloon>().Detach();
                AttachedBalloonObjects.Remove(AttachedBalloonObjects[i]);
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
