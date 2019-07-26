using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    // Enum of events of interest
    public enum Events { BattleWon, CharacterHurt, EnergyFull, Evolve, HappinessMax, ItemUsed, LevelUp, LogIn, Tap};

    // Subject calls this to notify observer of certain event
    public virtual void OnNotify(GameObject GO, Events _event) { }
}
