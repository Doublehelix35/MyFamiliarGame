using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements : MonoBehaviour {

    /// For all data and methods relating to elements

    // All possible elemental types
    public enum ElementType { NonElemental, Fire, Earth, Water, Air, Nature }

    // Moves
    public enum ElementalMoves { EmptyMoveSlot, Tackle, FireBlaze, AirStrike, WaterBlast, EarthQuake, NaturesWrath } // EmptyMoveSlot must be ignored and shouldnt be used
    public Dictionary<ElementalMoves, ElementType> MoveDictionary;

    void Awake()
    {
        MoveDictionary = new Dictionary<ElementalMoves, ElementType>()
    {
        {ElementalMoves.EmptyMoveSlot, ElementType.NonElemental },
        {ElementalMoves.Tackle, ElementType.NonElemental },
        {ElementalMoves.FireBlaze, ElementType.Fire },
        {ElementalMoves.AirStrike, ElementType.Air },
        {ElementalMoves.WaterBlast, ElementType.Water },
        {ElementalMoves.EarthQuake, ElementType.Earth },
        {ElementalMoves.NaturesWrath, ElementType.Nature }
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
                moveString = "NaturesWrath";
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
}
