using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    // Stats
    public int HealthMax;
    int Health = 1;
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


    

	void Start ()
    {
		
	}
	

	void Update ()
    {
		
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
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Item>())
        {
            col.gameObject.GetComponent<Item>().Interact(gameObject);
        }
    }
}
