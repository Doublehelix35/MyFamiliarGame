using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public GameObject SpawnableObject;
    public Transform SpawnLocation;

    // Observers
    public Observer QuestObserver;
    public Observer SoundObserver;

    public bool IsObservable;

	
    // Use both spawnable object and spawn location
    public void Spawn()
    {
        GameObject GO = Instantiate(SpawnableObject, SpawnLocation.position, Quaternion.identity);

        if (IsObservable)
        {
            GO.GetComponent<Subject>().AddObserver(QuestObserver);
            GO.GetComponent<Subject>().AddObserver(SoundObserver);
        }
    }

    // Just use spawn location
    public void Spawn(GameObject objectToSpawn)
    {
        GameObject GO = Instantiate(objectToSpawn, SpawnLocation.position, Quaternion.identity);

        if (IsObservable)
        {
            if (GO.name.Contains("Water")) // Waterfall exception
            {
                GO.GetComponentInChildren<Subject>().AddObserver(QuestObserver);
                GO.GetComponentInChildren<Subject>().AddObserver(SoundObserver);
            }
            else
            {
                GO.GetComponent<Subject>().AddObserver(QuestObserver);
                GO.GetComponent<Subject>().AddObserver(SoundObserver);
            }            
        }
    }

    // Only use parameters
    public void Spawn(GameObject objectToSpawn, Transform SpawnPos)
    {
        GameObject GO = Instantiate(objectToSpawn, SpawnPos.position, Quaternion.identity);

        if (IsObservable)
        {
            GO.GetComponent<Subject>().AddObserver(QuestObserver);
            GO.GetComponent<Subject>().AddObserver(SoundObserver);
        }
    }

    
}
