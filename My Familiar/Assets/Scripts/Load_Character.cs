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
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal int LoadCurrentSlot()
    {
        // Create a binary formatter and open the save file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "CurrentSaveSlot" + ".dat", FileMode.Open);

        // Create an object to store information from the file in and then close the file
        CharacterData data = (CharacterData)bf.Deserialize(file);
        file.Close();

        return data.SaveSlotInUse;
    }

    // Load save slot
    internal string Load(int SaveFileSlot)
    {
        // Create a binary formatter and open the save file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveSlot" + SaveFileSlot + ".dat", FileMode.Open);

        // Create an object to store information from the file in and then close the file
        CharacterData data = (CharacterData)bf.Deserialize(file);
        file.Close();

        return data.CharacterName;
    }

    // Load whole character
    internal GameObject Load(string CharacterName)
    {
        // Load character from parts        
        GameObject CharacterToReturn = new GameObject(CharacterName); // Parent to all parts
        if(CharacterToReturn.name == "")
        {
            CharacterToReturn.name = "NO NAME FOUND";
        }
        
        // Load Face
        GameObject Face = Load(CharacterName, "Face");
        Face.transform.parent = CharacterToReturn.transform;

        // Load Body
        GameObject Body = Load(CharacterName, "Body");
        Body.transform.parent = CharacterToReturn.transform;

        // Load Arm1
        GameObject Arm1 = Load(CharacterName, "Arm1");
        Arm1.transform.parent = CharacterToReturn.transform;

        // Load Arm2
        GameObject Arm2 = Load(CharacterName, "Arm2");
        Arm2.transform.parent = CharacterToReturn.transform;

        // Load Leg1
        GameObject Leg1 = Load(CharacterName, "Leg1");
        Leg1.transform.parent = CharacterToReturn.transform;

        // Load Leg2
        GameObject Leg2 = Load(CharacterName, "Leg2");
        Leg2.transform.parent = CharacterToReturn.transform;

        return CharacterToReturn;
        
    }

    // Load only part selected
    internal GameObject Load(string CharacterName, string CharacterPart)
    {
        GameObject GameObjectToReturn;
        
        // Check file exists
        if(File.Exists(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat"))
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat", FileMode.Open);
            Debug.Log("(Load char)" + "/" + CharacterName + CharacterPart + ".dat");

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
            GameObjectToReturn = new GameObject(CharacterPart, typeof(MeshFilter), typeof(MeshRenderer));
            GameObjectToReturn.GetComponent<MeshFilter>().mesh = newMesh;

            // Load material
            GameObjectToReturn.GetComponent<MeshRenderer>().material = FaceMat;

        }
        else // File not found
        {
            Debug.Log("CHARACTER PART FILE NOT FOUND!");

            // Set GO to empty GO
            GameObjectToReturn = new GameObject();
        }

        return GameObjectToReturn;
    }
}
