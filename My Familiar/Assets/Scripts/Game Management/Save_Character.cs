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

    // Save character stats and types
    internal void Save(string CharacterName, GameObject GameObjectToSave)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + ".dat");

        // Create an object to save information to
        CharacterData data = new CharacterData();

        // Save evolution count
        data.EvolutionCount = GameObjectToSave.GetComponent<Character>().CurrentEvolutionStage;

        // Save level
        data.Level = GameObjectToSave.GetComponent<Character>().Level;

        // Save character types to int array
        data.CharacterTypes = new int[GameObjectToSave.GetComponent<Character>().CharactersElementTypes.Count];
        for (int i = 0; i < data.CharacterTypes.Length; i++)
        {
            // Convert from Element.ElementType to int for serialization
            switch (GameObjectToSave.GetComponent<Character>().CharactersElementTypes[i])
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
        Debug.Log("Save Char" + "/" + CharacterName + CharacterPart + ".dat");

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

    // Save facial feature
    internal void SaveFacialFeature(string CharacterName, string CharacterPart, Vector3 PositionRelativeToFace)
    {
        // Create a binary formatter and a new file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CharacterName + CharacterPart + ".dat");
        Debug.Log("Save Char" + "/" + CharacterName + CharacterPart + ".dat");

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
        Debug.Log("Save Char" + "/" + CharacterName + "FacialConfig" + ".dat");

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
}

[System.Serializable]
class CharacterData
{
    public string CharacterName;
    public string CharacterPart;
    public int SaveSlotInUse;

    public string MaterialName;
    public int Level;
    public int EvolutionCount; // How many evolutions has it had?
    public int[] CharacterTypes = { 0 }; // 0 = non-elemental 1 = air 2 = earth 3 = fire 4 = nature 5 = water
    public int[] CharacterMoves = { 5 }; // 0 = empty move, 1 = AirStrike, 2 = EarthQuake, 3 = FireBlaze, 4 = NaturesWrath, 5 = Tackle, 6 = WaterBlast

    // Facial features
    public float Facial_X, Facial_Y, Facial_Z;
    public string[] FacialConfig;

    // Mesh data
    public float[] Vertices_x, Vertices_y, Vertices_z;
    public int[] Triangles;
}