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

        // Save character types to int array
        data.CharacterTypes = new int[GameObjectToSave.GetComponent<Character>().CharactersElementTypes.Count];
        foreach (int i in data.CharacterTypes)
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
}

[System.Serializable]
class CharacterData
{
    public string CharacterName;
    public string CharacterPart;
    public int SaveSlotInUse;

    public string MaterialName;
    public int EvolutionCount; // How many evolutions has it had?
    public int[] CharacterTypes; // 0 = non-elemental 1 = air 2 = earth 3 = fire 4 = nature 5 = water

    public float[] Vertices_x, Vertices_y, Vertices_z;
    public int[] Triangles;
}