using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Save_Character : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Save to a save slot
    internal void Save(int SaveFileSlot)
    {
        // Save data to slot
    }

    // Save a whole character
    internal void Save(string CharacterName, GameObject GameObjectToSave)
    {

    }

    // Save a character part
    internal void Save(string CharacterName, string CharacterPart, GameObject GameObjectToSave)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Save vertices

        // Save Triangles

        // Save Material
    }
}

[System.Serializable]
class CharacterData
{
    public string CharacterName;
    public string CharacterPart;

    public string MaterialName;

    public Vector3[] Vertices;
    public int[] Triangles;
}