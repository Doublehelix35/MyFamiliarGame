using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] AnimalPrefabs;
    GameObject[] AnimalPool; // Pool of all animals so everything is only spawned once
    GameObject CurrentAnimal;
    public float MovementSpeedMin;
    public float MovementSpeedMax;
    public float DelayBetweenSpawns;
    public float AnimalLifeTime;

    IEnumerator coroutine;

    void Start()
    {
        // Init animal pool
        AnimalPool = new GameObject[AnimalPrefabs.Length];
        for (int i = 0; i < AnimalPrefabs.Length; i++)
        {
            AnimalPool[i] = Instantiate(AnimalPrefabs[i], transform.position, Quaternion.identity);
            AnimalPool[i].transform.Rotate(0f, 90f, 0f);
            AnimalPool[i].SetActive(false); // Turn off
        }

        // Start SpawnAnimal coroutine
        coroutine = SpawnAnimal();
        StartCoroutine(coroutine);
    }

    IEnumerator SpawnAnimal()
    {
        while (true)
        {
            yield return new WaitForSeconds(DelayBetweenSpawns);
            // Select animal
            int randAnimal = Random.Range(0, AnimalPrefabs.Length);

            // Spawn animal
            CurrentAnimal = AnimalPool[randAnimal];
            CurrentAnimal.SetActive(true); // Turn on
            CurrentAnimal.transform.position = transform.position; // Reset position

            // Make animal run across scene
            float randSpeed = Random.Range(MovementSpeedMin, MovementSpeedMax);

            CurrentAnimal.GetComponent<Rigidbody>().AddForce(Vector3.right * randSpeed, ForceMode.Acceleration);
            yield return new WaitForSeconds(AnimalLifeTime);

            // Destroy animal off screen
            CurrentAnimal.GetComponent<Rigidbody>().velocity = Vector3.zero;
            CurrentAnimal.SetActive(false);

            // Wait then repeat
        }
    }
}
