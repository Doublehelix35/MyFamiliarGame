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

        // Init arrays
        Vector3[] VertsTemp = new Vector3[GameObjectToSave.GetComponent<MeshFilter>().mesh.vertexCount];
        data.Vertices_x = new float[GameObjectToSave.GetComponent<MeshFilter>().mesh.vertexCount];
        data.Vertices_y = new float[GameObjectToSave.GetComponent<MeshFilter>().mesh.vertexCount];
        data.Vertices_z = new float[GameObjectToSave.GetComponent<MeshFilter>().mesh.vertexCount];

        VertsTemp = GameObjectToSave.GetComponent<MeshFilter>().mesh.vertices;

        for (int i = 0; i < VertsTemp.Length; i++)
        {
            data.Vertices_x[i] = VertsTemp[i].x;
            data.Vertices_y[i] = VertsTemp[i].y;
            data.Vertices_z[i] = VertsTemp[i].z;
        }

        // Save Triangles
        data.Triangles = GameObjectToSave.GetComponent<MeshFilter>().mesh.triangles;

        // Save Material
        data.MaterialName = "Yellow";

        // Write the object to file and close it
        bf.Serialize(file, data);
        file.Close();
    }
}

[System.Serializable]
class CharacterData
{
    public string CharacterName;
    public string CharacterPart;

    public string MaterialName;

    public float[] Vertices_x, Vertices_y, Vertices_z;
    public int[] Triangles;
}