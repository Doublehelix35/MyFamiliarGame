using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    // Enum of events of interest
    public enum Events { ItemUsed, LevelUp, Evolve, EnergyFull, HappinessMax, BattleWon, LogIn};

    // Subject calls this to notify observer of certain event
    public virtual void OnNotify(GameObject GO, Events _event) { }
}
