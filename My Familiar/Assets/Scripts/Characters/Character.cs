using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    /// <summary>
    /// Remember most supporting properties like rigidbodies are set up
    /// in the Load_Character script
    /// 
    /// Character script will be on the body object
    /// </summary>

    // Object Refs
    GameObject GameManagerRef;

    // Stats
    public int HealthMax;
    public int HappinessMax = 50;
    int Health = 10;
    int Attack = 1;
    int Defence = 1;
    int Happiness = 1;
    int Experience = 0;

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

    // Constructor
    internal Character()
    {
        // Most construction is done externally during loading.
        // Stats and other data that is used throughout the lifespan of
        // the character should be passed in to be handled internally
    }

    void Start()
    {
        GameManagerRef = GameObject.FindGameObjectWithTag("GameController");
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
        Health += value;

        if(Health > HealthMax)
        {
            Health = HealthMax;
        }
        else if (Health <= 0)
        {
            // Player dead or exhausted

            Destroy(gameObject); // Temporary test code
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
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Item>())
        {
            col.gameObject.GetComponent<Item>().Interact(gameObject);
        }
    }
}
