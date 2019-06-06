using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Load_Character : MonoBehaviour
{
    // Materials
    public Material NonElementalMat;
    public Material AirMat;
    public Material EarthMat;
    public Material FireMat;
    public Material NatureMat;
    public Material WaterMat;
    Material MatToApply;

    // Facial prefabs
    public GameObject EyePrefab;
    public GameObject NosePrefab;
    public GameObject MouthPrefab;

    float FacialOffset_Z = 0.1f; // Spawn facial features in front of face
    float FacialOffsetDivison = 4f; // How seperated the facial features are

    float Drag = 0.3f;

    // Default scale and seperation
    float SeperationMultipler = 0.03f;
    float ScaleMultiplier = 0.2f;

    // Evolution modifiers
    float EvolutionModifier = 1.02f; // Base modifier
    float EvolutionCountDivision = 20f; // Makes evo count smaller, so each evolution grows in small increments

    bool FirstTimeLoadingSave = false;
    

    internal int LoadCurrentSlot()
    {
        CharacterData data = new CharacterData();

        if (File.Exists(Application.persistentDataPath + "/" + "CurrentSaveSlot" + ".dat"))
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "CurrentSaveSlot" + ".dat", FileMode.Open);

            // Create an object to store information from the file in and then close the file
            data = (CharacterData)bf.Deserialize(file);

            file.Close();
        }
        else
        {
            // Default to slot 1
            data.SaveSlotInUse = 1;

            Debug.Log("Error! Current slot not found!");
        }
        

        return data.SaveSlotInUse;
    }

    // Load save slot
    internal string Load(int SaveFileSlot)
    {
        CharacterData data = new CharacterData();

        if(File.Exists(Application.persistentDataPath + "/" + "SaveSlot" + SaveFileSlot + ".dat"))
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveSlot" + SaveFileSlot + ".dat", FileMode.Open);

            // Create an object to store information from the file in and then close the file
            data = (CharacterData)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            data.CharacterName = "Error!";
            Debug.Log("Save slot data not found!");
        }        

        return data.CharacterName;
    }

    // Load whole character
    internal GameObject Load(string CharacterName)
    {
        // If existing save file then read data
        CharacterData data = new CharacterData(); // Store save data

        // Check if save is new or not
        if (File.Exists(Application.persistentDataPath + "/" + CharacterName + ".dat")) // Existing file
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + CharacterName + ".dat", FileMode.Open);

            // Create an object to store information from the file in and then close the file
            data = (CharacterData)bf.Deserialize(file);
            file.Close();
        }
        else // New file
        {
            // Set empty data to defaults      

            // General stats
            data.CharacterMoves = new int[3] { 5, 0, 0 }; // Default moves are tackle, empty, empty
            data.Level = 1; data.Experience = 0; data.EvolutionCount = 0;
            data.Health = 20; data.Happiness = 50; data.Fullness = 50;

            // Battle stats
            data.Attack = 1f; data.Accuracy = 1f; data.CritChance = 1f;
            data.Defence = 1f; data.DodgeChance = 1f; data.Speed = 1f;

            // Spec Points  
            data.AirPoints = 0; data.EarthPoints = 0; data.FirePoints = 0;
            data.NaturePoints = 0; data.WaterPoints = 0;
        }

        // Load character from parts        
        GameObject CharacterToReturn = new GameObject(CharacterName); // Parent to all parts
        if(CharacterToReturn.name == "") { CharacterToReturn.name = "NO NAME FOUND"; }

        /*/ Load Body /*/
        GameObject Body = Load(CharacterName, "Body");

        // Add character script to body
        Body.AddComponent<Character>();
        Character charRef = Body.GetComponent<Character>();

        // Add character AI script to body
        Body.AddComponent<Character_AI>();

        // Load general stats
        charRef.Level = data.Level; charRef.Experience = data.Experience;
        charRef.Health = data.Health; charRef.Happiness = data.Happiness;
        charRef.CurrentFullness = data.Fullness;
        charRef.CurrentEvolutionStage = data.EvolutionCount;

        // Load battle stats
        charRef.Attack = data.Attack; charRef.Accuracy = data.Accuracy;
        charRef.CritChance = data.CritChance; charRef.Defence = data.Defence;
        charRef.DodgeChance = data.DodgeChance; charRef.Speed = data.Speed;

        // Load spec points
        charRef.AirPoints = data.AirPoints; charRef.EarthPoints = data.EarthPoints;
        charRef.FirePoints = data.FirePoints; charRef.NaturePoints = data.NaturePoints;
        charRef.WaterPoints = data.WaterPoints;
        
        // Adjust scale multiplier and seperation multiplier to factor in evolution
        if(data.EvolutionCount > 0)
        {
            ScaleMultiplier *= (EvolutionModifier + ((float)data.EvolutionCount / EvolutionCountDivision));
            SeperationMultipler *= (EvolutionModifier + ((float)data.EvolutionCount / EvolutionCountDivision));
        }
        else // Hasnt evolved yet
        {

        }

        // Load character types from the int array
        for(int i = 0; i < data.CharacterTypes.Length; i++)
        {
            switch (data.CharacterTypes[i])
            {
                case 0: // Non-elemental
                    charRef.CharactersElementTypes.Add(Elements.ElementType.NonElemental);
                    MatToApply = NonElementalMat; // Set material
                    break;
                case 1: // Air
                    charRef.CharactersElementTypes.Add(Elements.ElementType.Air);
                    MatToApply = AirMat; // Set material
                    break;
                case 2: // Earth
                    charRef.CharactersElementTypes.Add(Elements.ElementType.Earth);
                    MatToApply = EarthMat; // Set material
                    break;
                case 3: // Fire
                    charRef.CharactersElementTypes.Add(Elements.ElementType.Fire);
                    MatToApply = FireMat; // Set material
                    break;
                case 4: // Nature
                    charRef.CharactersElementTypes.Add(Elements.ElementType.Nature);
                    MatToApply = NatureMat; // Set material
                    break;
                case 5: // Water
                    charRef.CharactersElementTypes.Add(Elements.ElementType.Water);
                    MatToApply = WaterMat; // Set material
                    break;
                default:
                    break;
            }            
        }

        // Load character moves from the int array
        for(int i = 0; i < data.CharacterMoves.Length; i++)
        {
            switch (data.CharacterMoves[i])
            {
                case 0: // Empty move
                    charRef.MoveSlots[i] = Elements.ElementalMoves.EmptyMoveSlot;
                    break;
                case 1: // Air strike
                    charRef.MoveSlots[i] = Elements.ElementalMoves.AirStrike;
                    break;
                case 2: // Earthquake
                    charRef.MoveSlots[i] = Elements.ElementalMoves.EarthQuake;
                    break;
                case 3: // Fire Blaze
                    charRef.MoveSlots[i] = Elements.ElementalMoves.FireBlaze;
                    break;
                case 4: // Natures Wrath
                    charRef.MoveSlots[i] = Elements.ElementalMoves.NaturesWrath;
                    break;
                case 5: // Tackle
                    charRef.MoveSlots[i] = Elements.ElementalMoves.Tackle;
                    break;
                case 6: // Water Blast
                    charRef.MoveSlots[i] = Elements.ElementalMoves.WaterBlast;
                    break;
                default:
                    Debug.Log("Error loading moves from file");
                    break;
            }
        }

        // Set body mat
        Body.GetComponent<MeshRenderer>().material = MatToApply;

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
        Body.GetComponent<CapsuleCollider>().height = baseSize.y * 2.4f;


        /*/ Load Face /*/
        GameObject Face = Load(CharacterName, "Face");
        // Scale size down
        Face.transform.localScale *= ScaleMultiplier;
        // Add cube collider
        Face.AddComponent<BoxCollider>();
        // Move face up
        Face.transform.position = new Vector3(Face.transform.position.x, Face.transform.position.y + (4 * PartSeperationOffset), Face.transform.position.z);
        // Set parent
        Face.transform.parent = CharacterToReturn.transform;

        // Load Facial Features
        string[] facialParts = LoadFacialConfig(CharacterName);

        // Loop through, spawn each facial feature, set the face as parent to all facial features
        foreach(string part in facialParts)
        {
            if(File.Exists(Application.persistentDataPath + "/" + CharacterName + part + ".dat"))
            {
                GameObject prefabToSpawn;
                Vector3 SpawnPos = Face.transform.position;
                SpawnPos.z -= FacialOffset_Z;
                SpawnPos += LoadFacialFeature(CharacterName, part) / FacialOffsetDivison;

                // Select prefab to spawn
                if (part.Contains("Eye")) { prefabToSpawn = EyePrefab; }
                else if (part.Contains("Nose")) { prefabToSpawn = NosePrefab; }
                else if (part.Contains("Mouth")) { prefabToSpawn = MouthPrefab; }
                else { prefabToSpawn = EyePrefab; }

                // Spawn facial object
                GameObject facialObject = Instantiate(prefabToSpawn);
                // Set position
                facialObject.transform.position = SpawnPos;
                // Set name
                facialObject.name = part;
                // Scale down
                facialObject.transform.localScale *= ScaleMultiplier;
                // Set parent
                facialObject.transform.parent = Face.transform;

                //Debug.Log(facialObject.name + " " + facialObject.transform.position.ToString());
            }
            else
            {
                Debug.Log("File not found " + "/" + CharacterName + part + ".dat");
            }            
        }

        /*/ Load Arm1 /*/
        GameObject Arm1 = Load(CharacterName, "Arm1");
        // Scale size down
        Arm1.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move arm left and up
        Arm1.transform.position = new Vector3(Arm1.transform.position.x - (2 * PartSeperationOffset), Arm1.transform.position.y + (2 * PartSeperationOffset), Arm1.transform.position.z);
        // Set parent
        Arm1.transform.parent = CharacterToReturn.transform;

        /*/ Load Arm2 /*/
        GameObject Arm2 = Load(CharacterName, "Arm2");
        // Scale size down
        Arm2.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move arm right and up
        Arm2.transform.position = new Vector3(Arm2.transform.position.x + (2 * PartSeperationOffset), Arm2.transform.position.y + (2 * PartSeperationOffset), Arm2.transform.position.z);
        // Set parent
        Arm2.transform.parent = CharacterToReturn.transform;

        /*/ Load Leg1 /*/
        GameObject Leg1 = Load(CharacterName, "Leg1");
        // Scale size down
        Leg1.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move leg left and down
        Leg1.transform.position = new Vector3(Leg1.transform.position.x - PartSeperationOffset, Leg1.transform.position.y - (3 * PartSeperationOffset), Leg1.transform.position.z);
        // Set parent
        Leg1.transform.parent = CharacterToReturn.transform;

        /*/ Load Leg2 /*/
        GameObject Leg2 = Load(CharacterName, "Leg2");
        // Scale size down
        Leg2.transform.localScale *= (ScaleMultiplier / 1.5f);
        // Move leg right and down
        Leg2.transform.position = new Vector3(Leg2.transform.position.x + PartSeperationOffset, Leg2.transform.position.y - (3 * PartSeperationOffset), Leg2.transform.position.z);
        // Set parent
        Leg2.transform.parent = CharacterToReturn.transform;

        // Set up rididbodies and character joints
        SetUpCharacterAsRagdoll(Body, Face, Arm1, Arm2, Leg1, Leg2);

        // Set up tags
        Body.tag = "Player"; Face.tag = "Player"; Arm1.tag = "Player";
        Arm2.tag = "Player"; Leg1.tag = "Player"; Leg2.tag = "Player";

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
            GameObjectToReturn.GetComponent<MeshRenderer>().material = MatToApply;

        }
        else // File not found
        {
            Debug.Log("CHARACTER PART FILE NOT FOUND!");

            // Set GO to empty GO
            GameObjectToReturn = new GameObject();
        }

        return GameObjectToReturn;
    }

    // Load facial configuration i.e. how many of each facial feature to load and their names
    string[] LoadFacialConfig(string CharacterName)
    {
        CharacterData data = new CharacterData();

        if (File.Exists(Application.persistentDataPath + "/" + CharacterName + "FacialConfig" + ".dat"))
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + CharacterName + "FacialConfig" + ".dat", FileMode.Open);

            // Create an object to store information from the file in and then close the file
            data = (CharacterData)bf.Deserialize(file);

            file.Close();
        }

        return data.FacialConfig;
    }

    // Load facial feature and return its relative position to the face
    Vector3 LoadFacialFeature(string CharacterName, string CharacterPart)
    {
        CharacterData data = new CharacterData();

        if (File.Exists(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat"))
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat", FileMode.Open);

            // Create an object to store information from the file in and then close the file
            data = (CharacterData)bf.Deserialize(file);

            file.Close();
        }

        // Convert back to a vector 3
        Vector3 RelativePositionToFace = new Vector3(data.Facial_X, data.Facial_Y, data.Facial_Z);

        return RelativePositionToFace;
    }

    // If first time return true, if not then return false
    internal bool CheckFirstTimeLoad(string CharName)
    {
        CharacterData data = new CharacterData();

        if (File.Exists(Application.persistentDataPath + "/" + CharName + "FirstTimeLoading" + ".dat"))
        {
            // Create a binary formatter and open the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + CharName +  "FirstTimeLoading" + ".dat", FileMode.Open);

            // Create an object to store information from the file in and then close the file
            data = (CharacterData)bf.Deserialize(file);

            file.Close();
        }

        if (data.FirstTimeLoadingSave)
        {
            // Set first time loading to false
            gameObject.GetComponent<Save_Character>().SaveFirstTimeLoading(CharName, false);
            return true;
        }
        return false;
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
