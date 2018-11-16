using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Load_Character : MonoBehaviour {

    public Material FaceMat;

    float SeperationMultipler = 0.03f;
    float ScaleMultiplier = 0.2f;

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

        // Load Body
        GameObject Body = Load(CharacterName, "Body");

        // Define Part seperation offset
        Vector3 baseSize = Body.GetComponent<Renderer>().bounds.size;
        float PartSeperationOffset = (((baseSize.x * baseSize.y) / 2) * SeperationMultipler); // Get average of x and y axis then * by scale multipler

        // Scale Body size down
        Body.transform.localScale *= (2 * ScaleMultiplier);
        // Move body left
        //Body.transform.position = new Vector3(Body.transform.position.x - PartSeperationOffset, Body.transform.position.y, Body.transform.position.z);
        // Set parent
        Body.transform.parent = CharacterToReturn.transform;

        // Load Face
        GameObject Face = Load(CharacterName, "Face");
        // Scale size down
        Face.transform.localScale *= ScaleMultiplier;
        // Move face up
        Face.transform.position = new Vector3(Face.transform.position.x, Face.transform.position.y + (4 * PartSeperationOffset), Face.transform.position.z);
        // Set parent
        Face.transform.parent = CharacterToReturn.transform;
        

        // Load Arm1
        GameObject Arm1 = Load(CharacterName, "Arm1");
        // Scale size down
        Arm1.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move arm left and up
        Arm1.transform.position = new Vector3(Arm1.transform.position.x - (2 * PartSeperationOffset), Arm1.transform.position.y + (2 * PartSeperationOffset), Arm1.transform.position.z);
        // Set parent
        Arm1.transform.parent = CharacterToReturn.transform;

        // Load Arm2
        GameObject Arm2 = Load(CharacterName, "Arm2");
        // Scale size down
        Arm2.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move arm right and up
        Arm2.transform.position = new Vector3(Arm2.transform.position.x + (2 * PartSeperationOffset), Arm2.transform.position.y + (2 * PartSeperationOffset), Arm2.transform.position.z);
        // Set parent
        Arm2.transform.parent = CharacterToReturn.transform;

        // Load Leg1
        GameObject Leg1 = Load(CharacterName, "Leg1");
        // Scale size down
        Leg1.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move leg left and down
        Leg1.transform.position = new Vector3(Leg1.transform.position.x - PartSeperationOffset, Leg1.transform.position.y - (2 * PartSeperationOffset), Leg1.transform.position.z);
        // Set parent
        Leg1.transform.parent = CharacterToReturn.transform;

        // Load Leg2
        GameObject Leg2 = Load(CharacterName, "Leg2");
        // Scale size down
        Leg2.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move leg right and down
        Leg2.transform.position = new Vector3(Leg2.transform.position.x + PartSeperationOffset, Leg2.transform.position.y - (2 * PartSeperationOffset), Leg2.transform.position.z);
        // Set parent
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

    void SetUpCharacterComponents(GameObject body, GameObject face, GameObject arm1, GameObject arm2, GameObject leg1, GameObject leg2)
    {
        // Set up rigidbodies
        body.AddComponent<Rigidbody>();
        face.AddComponent<Rigidbody>();
        arm1.AddComponent<Rigidbody>();
        arm2.AddComponent<Rigidbody>();
        leg1.AddComponent<Rigidbody>();
        leg2.AddComponent<Rigidbody>();

        // Character joints (All starting from body and going out to other parts)
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();

        CharacterJoint[] CharacterJoints = GetComponents<CharacterJoint>();

        // Body to face
        CharacterJoints[0].connectedBody = face.GetComponent<Rigidbody>();

        // Body to arm1
        CharacterJoints[1].connectedBody = arm1.GetComponent<Rigidbody>();

        // Body to arm2
        CharacterJoints[2].connectedBody = arm2.GetComponent<Rigidbody>();

        // Body to leg1
        CharacterJoints[3].connectedBody = leg1.GetComponent<Rigidbody>();

        // Body to leg2
        CharacterJoints[4].connectedBody = leg2.GetComponent<Rigidbody>();

    }
}
