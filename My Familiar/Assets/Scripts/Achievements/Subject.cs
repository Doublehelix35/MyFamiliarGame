using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    // List to store all observers of this subject
    List<Observer> observers = new List<Observer>();

    // Add observer to list
    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    // Remove observer from list
    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }

    // Notify all observers of event
    protected void Notify(GameObject GO, Observer.Events _event)
    {
        // Loop through all observers
        foreach(Observer o in observers)
        {
            // Send notification
            o.OnNotify(gameObject, _event);
        }        
    }
}
