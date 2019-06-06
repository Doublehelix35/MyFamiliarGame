using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class Save_Character : MonoBehaviour {

    public Text[] SaveSlotInputs;
    
    // Save current slot
    public void SaveCurrentSlot(int SlotNumber)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + "CurrentSaveSlot" + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Save slot in use
        data.SaveSlotInUse = SlotNumber;      

        // Write the object to file and close it
        bf.Serialize(file, data);
        file.Close();
    }

    // Save to a save slot
    public void Save(int SaveFileSlot)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + "SaveSlot" + SaveFileSlot + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Save Name
        if(SaveSlotInputs.Length >= SaveFileSlot)
        {
            data.CharacterName = SaveSlotInputs[SaveFileSlot - 1].text;
        }
        

        // Write the object to file and close it
        bf.Serialize(file, data);
        file.Close();
    }

    // Save character stats, types and moves
    internal void Save(string CharacterName, GameObject GameObjectToSave)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Temp character ref
        Character charRef = GameObjectToSave.GetComponent<Character>();

        // Save general stats
        data.Level = charRef.Level;
        data.Experience = charRef.Experience;
        data.Health = charRef.Health;
        data.Happiness = charRef.Happiness;
        data.Fullness = charRef.CurrentFullness;
        data.EvolutionCount = charRef.CurrentEvolutionStage;

        // Save battle stats
        data.Attack = charRef.Attack;
        data.Accuracy = charRef.Accuracy;
        data.CritChance = charRef.CritChance;
        data.Defence = charRef.Defence;
        data.DodgeChance = charRef.DodgeChance;
        data.Speed = charRef.Speed;

        // Save spec points
        data.AirPoints = charRef.AirPoints;
        data.EarthPoints = charRef.EarthPoints;
        data.FirePoints = charRef.FirePoints;
        data.NaturePoints = charRef.NaturePoints;
        data.WaterPoints = charRef.WaterPoints;

        // Save character types to int array
        data.CharacterTypes = new int[charRef.CharactersElementTypes.Count];
        for (int i = 0; i < data.CharacterTypes.Length; i++)
        {
            // Convert from Element.ElementType to int for serialization
            switch (charRef.CharactersElementTypes[i])
            {
                case Elements.ElementType.NonElemental:
                    data.CharacterTypes[i] = 0;
                    break;
                case Elements.ElementType.Air:
                    data.CharacterTypes[i] = 1;
                    break;
                case Elements.ElementType.Earth:
                    data.CharacterTypes[i] = 2;
                    break;
                case Elements.ElementType.Fire:
                    data.CharacterTypes[i] = 3;
                    break;
                case Elements.ElementType.Nature:
                    data.CharacterTypes[i] = 4;
                    break;
                case Elements.ElementType.Water:
                    data.CharacterTypes[i] = 5;
                    break;
                default:
                    break;
            }
        }

        // Save character moves to int array
        data.CharacterMoves = new int[3];
        for(int i = 0; i < data.CharacterMoves.Length; i++)
        {
            switch (GameObjectToSave.GetComponent<Character>().MoveSlots[i])
            {
                case Elements.ElementalMoves.EmptyMoveSlot:
                    data.CharacterMoves[i] = 0;
                    break;
                case Elements.ElementalMoves.AirStrike:
                    data.CharacterMoves[i] = 1;
                    break;
                case Elements.ElementalMoves.EarthQuake:
                    data.CharacterMoves[i] = 2;
                    break;
                case Elements.ElementalMoves.FireBlaze:
                    data.CharacterMoves[i] = 3;
                    break;
                case Elements.ElementalMoves.NaturesWrath:
                    data.CharacterMoves[i] = 4;
                    break;
                case Elements.ElementalMoves.Tackle:
                    data.CharacterMoves[i] = 5;
                    break;
                case Elements.ElementalMoves.WaterBlast:
                    data.CharacterMoves[i] = 6;
                    break;
                default:
                    Debug.Log("Error saving moves to file");
                    break;
            }
        }

        // Write the object to file and close it
        bf.Serialize(file, data);
        file.Close();
    }

    // Save a character part
    internal void Save(string CharacterName, string CharacterPart, GameObject GameObjectToSave)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat");
        //Debug.Log("Save Char" + "/" + CharacterName + CharacterPart + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Temp mesh filter ref
        MeshFilter meshFilterRef = GameObjectToSave.GetComponent<MeshFilter>();

        // Save vertices

        // Init arrays
        Vector3[] VertsTemp = new Vector3[meshFilterRef.mesh.vertexCount];
        data.Vertices_x = new float[meshFilterRef.mesh.vertexCount];
        data.Vertices_y = new float[meshFilterRef.mesh.vertexCount];
        data.Vertices_z = new float[meshFilterRef.mesh.vertexCount];

        // Convert from vector3 array and save as 3 floats arrays
        VertsTemp = meshFilterRef.mesh.vertices;

        for (int i = 0; i < VertsTemp.Length; i++)
        {
            data.Vertices_x[i] = VertsTemp[i].x;
            data.Vertices_y[i] = VertsTemp[i].y;
            data.Vertices_z[i] = VertsTemp[i].z;
        }

        // Save Triangles
        data.Triangles = meshFilterRef.mesh.triangles;

        // Save Material
        data.MaterialName = "Yellow";

        // Write the object to file and close it
        bf.Serialize(file, data);
        file.Close();
    }

    // Save facial feature
    internal void SaveFacialFeature(string CharacterName, string CharacterPart, Vector3 PositionRelativeToFace)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat");
        //Debug.Log("Save Char" + "/" + CharacterName + CharacterPart + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Save facial feature's position
        data.Facial_X = PositionRelativeToFace.x;
        data.Facial_Y = PositionRelativeToFace.y;
        data.Facial_Z = PositionRelativeToFace.z;

        // Write the object to file and close it
        bf.Serialize(file, data);
        file.Close();
    }

    // Save data on how many facial features there are to load
    internal void SaveFacialConfig(string CharacterName, string[] CharacterParts)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + "FacialConfig" + ".dat");
        //Debug.Log("Save Char" + "/" + CharacterName + "FacialConfig" + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();
        
        // Save facial config
        if(CharacterParts != null || CharacterParts.Length > 0)
        {
            data.FacialConfig = CharacterParts;
        }

        // Write the object to file and close it
        bf.Serialize(file, data);
        file.Close();
    }

    internal void SaveFirstTimeLoading(string CharacterName, bool isFirstTime)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + "FirstTimeLoading" + ".dat");
        //Debug.Log("Save Char" + "/" + CharacterName + "FirstTimeLoading" + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Save first time loading
        data.FirstTimeLoadingSave = isFirstTime;

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
    public int SaveSlotInUse;
    public bool FirstTimeLoadingSave = true;

    // Facial features
    public float Facial_X, Facial_Y, Facial_Z;
    public string[] FacialConfig;

    // Mesh data
    public float[] Vertices_x, Vertices_y, Vertices_z;
    public int[] Triangles;
    public string MaterialName;

    // General stats
    public int Health;
    public int Happiness;
    public int Fullness;
    public int Level;
    public int Experience;
    public int EvolutionCount; // How many evolutions has it had?
    public int[] CharacterTypes = { 0 }; // 0 = non-elemental 1 = air 2 = earth 3 = fire 4 = nature 5 = water
    public int[] CharacterMoves = { 5 }; // 0 = empty move, 1 = AirStrike, 2 = EarthQuake, 3 = FireBlaze, 4 = NaturesWrath, 5 = Tackle, 6 = WaterBlast
       
    // Battle stats
    public float Attack;
    public float Accuracy; // Determines if a move hits or misses
    public float CritChance; // Chance to get a critical hit
    public float Defence;
    public float DodgeChance; // Chance to dodge an incoming attack
    public float Speed;

    // Spec points
    public int AirPoints;
    public int EarthPoints;
    public int FirePoints;
    public int NaturePoints;
    public int WaterPoints;    
}