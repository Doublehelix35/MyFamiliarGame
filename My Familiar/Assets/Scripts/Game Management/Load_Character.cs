using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Load_Character : MonoBehaviour
{

    public Material FaceMat;

    float SeperationMultipler = 0.03f;
    float ScaleMultiplier = 0.2f;
    float Drag = 0.3f;

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
        if(CharacterToReturn.name == "") { CharacterToReturn.name = "NO NAME FOUND"; }

        // Load Body
        GameObject Body = Load(CharacterName, "Body");

        // Add character script to body
        Body.AddComponent<Character>();

        // Define Part seperation offset
        Vector3 baseSize = Body.GetComponent<Renderer>().bounds.size;
        float PartSeperationOffset = (((baseSize.x * baseSize.y) / 2) * SeperationMultipler); // Get average of x and y axis then * by scale multipler

        // Scale Body size down
        Body.transform.localScale *= (2 * ScaleMultiplier);
        // Move body left
        //Body.transform.position = new Vector3(Body.transform.position.x - PartSeperationOffset, Body.transform.position.y, Body.transform.position.z);
        // Set parent
        Body.transform.parent = CharacterToReturn.transform;

        // Add capsule collider to body
        Body.AddComponent<CapsuleCollider>();
        Body.GetComponent<CapsuleCollider>().radius = baseSize.x / 4;
        Body.GetComponent<CapsuleCollider>().height = baseSize.y * 2;

        // Load Face
        GameObject Face = Load(CharacterName, "Face");
        // Scale size down
        Face.transform.localScale *= ScaleMultiplier;
        // Add cube collider
        Face.AddComponent<BoxCollider>();
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

        // Set up rididbodies and character joints
        SetUpCharacterAsRagdoll(Body, Face, Arm1, Arm2, Leg1, Leg2);

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
            //Debug.Log("(Load char)" + "/" + CharacterName + CharacterPart + ".dat");

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

    void SetUpCharacterAsRagdoll(GameObject body, GameObject face, GameObject arm1, GameObject arm2, GameObject leg1, GameObject leg2)
    {
        // Set up rigidbodies
        body.AddComponent<Rigidbody>();
        face.AddComponent<Rigidbody>();
        arm1.AddComponent<Rigidbody>();
        arm2.AddComponent<Rigidbody>();
        leg1.AddComponent<Rigidbody>();
        leg2.AddComponent<Rigidbody>();

        // Rigidbody drag
        body.GetComponent<Rigidbody>().drag = Drag;
        face.GetComponent<Rigidbody>().drag = Drag;
        arm1.GetComponent<Rigidbody>().drag = Drag;
        arm2.GetComponent<Rigidbody>().drag = Drag;
        leg1.GetComponent<Rigidbody>().drag = Drag;
        leg2.GetComponent<Rigidbody>().drag = Drag;

        // Set constrains on body
        body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

        // Set parts to character layer
        body.layer = 9; face.layer = 9;
        arm1.layer = 9; arm2.layer = 9;
        leg1.layer = 9; leg2.layer = 9;

        // Character joints (All starting from body and going out to other parts)
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();
        body.AddComponent<CharacterJoint>();

        CharacterJoint[] CharacterJoints = body.GetComponents<CharacterJoint>();

        // Anchor offsets based on body size
        float bodySizeX = body.GetComponent<Renderer>().bounds.size.x;
        float bodySizeY = body.GetComponent<Renderer>().bounds.size.x;

        // Body to face
        CharacterJoints[0].connectedBody = face.GetComponent<Rigidbody>(); // Connect body to face
        CharacterJoints[0].axis = new Vector3(1, 0, 0); // Set axis
        // Set joint anchor y
        CharacterJoints[0].anchor = new Vector3(0f, bodySizeY / 2, 0f);

        // Body to arm1 (left)
        CharacterJoints[1].connectedBody = arm1.GetComponent<Rigidbody>(); // Connect body to arm1
        CharacterJoints[1].axis = new Vector3(0, 1, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[1].anchor = new Vector3(-bodySizeX / 3, bodySizeY / 3, 0f);

        // Body to arm2 (right)
        CharacterJoints[2].connectedBody = arm2.GetComponent<Rigidbody>(); // Connect body to arm2
        CharacterJoints[2].axis = new Vector3(0, -1, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[2].anchor = new Vector3(bodySizeX / 3, bodySizeY / 3, 0f);

        // Body to leg1 (left)
        CharacterJoints[3].connectedBody = leg1.GetComponent<Rigidbody>(); // Connect body to leg1
        CharacterJoints[3].axis = new Vector3(1, 0, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[3].anchor = new Vector3(-bodySizeX * 0.4f, -bodySizeY * 0.4f, 0f);

        // Body to leg2 (right)
        CharacterJoints[4].connectedBody = leg2.GetComponent<Rigidbody>(); // Connect body to leg2
        CharacterJoints[4].axis = new Vector3(1, 0, 0); // Set axis
        // Set joint anchor x and y
        CharacterJoints[4].anchor = new Vector3(bodySizeX * 0.4f, -bodySizeY * 0.4f, 0f);

        // Fixed joint from face to body
        face.AddComponent<FixedJoint>();
        face.GetComponent<FixedJoint>().connectedBody = body.GetComponent<Rigidbody>();
    }
}
