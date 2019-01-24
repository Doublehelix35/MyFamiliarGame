using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public GameObject SpawnableObject;
    public Transform SpawnLocation;

	
    // Use both spawnable object and spawn location
    public void Spawn()
    {
        Instantiate(SpawnableObject, SpawnLocation.position, Quaternion.identity);
    }

    // Just use spawn location
    public void Spawn(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, SpawnLocation.position, Quaternion.identity);
    }

    // Only use parameters
    public void Spawn(GameObject objectToSpawn, Transform SpawnPos)
    {
        Instantiate(objectToSpawn, SpawnPos.position, Quaternion.identity);
    }

    
}
