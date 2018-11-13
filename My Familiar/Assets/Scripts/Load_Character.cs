using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Load_Character : MonoBehaviour {

    public Material FaceMat;

	// Use this for initialization
	void Start ()
    {
        Load("Bob", "Face");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Load save slot
    internal void Load(int SaveFileSlot)
    {
        // Load data from file slot
    }

    // Load whole character
    internal void Load(string CharacterName)
    {
        // Load character
    }

    // Load only part selected
    internal void Load(string CharacterName, string CharacterPart)
    {
        // Load a character part
        
        // Check file exists
        if(File.Exists(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat"))
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat", FileMode.Open);

            // Create an object to store information from the file in and then close the file
            CharacterData data = (CharacterData)bf.Deserialize(file);
            file.Close();

            // Create mesh
            Mesh newMesh = new Mesh();
            
            // Load vertices            
            Vector3[] meshVerts = new Vector3[data.Vertices_x.Length];            

            for (int i = 0; i < data.Vertices_x.Length; i++)
            {
                meshVerts[i].x = data.Vertices_x[i];
                meshVerts[i].y = data.Vertices_y[i];
                meshVerts[i].z = data.Vertices_z[i];
            }

            newMesh.vertices = meshVerts;

            // Load triangles
            newMesh.triangles = data.Triangles;

            // Recalculate normals and bounds
            newMesh.RecalculateNormals();
            newMesh.RecalculateBounds();

            // Set mesh to game object
            GameObject MeshObject = new GameObject(CharacterPart, typeof(MeshFilter), typeof(MeshRenderer));
            MeshObject.GetComponent<MeshFilter>().mesh = newMesh;

            // Load material
            MeshObject.GetComponent<MeshRenderer>().material = FaceMat;

        }
        else // File not found
        {
            Debug.Log("CHARACTER PART FILE NOT FOUND!");    
        }

    }
}
