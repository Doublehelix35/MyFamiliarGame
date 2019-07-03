using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlots : MonoBehaviour
{
    public Load_Character LoadRef;
    public Save_Character SaveRef;

    // Texts
    public Text Slot1Text;
    public Text Slot2Text;
    public Text Slot3Text;
    string DefaultText = "Slot Empty";

    // Inputs
    public InputField Input1;
    public InputField Input2;
    public InputField Input3;

    // Buttons
    public Button DeleteSave1;
    public Button DeleteSave2;
    public Button DeleteSave3;
    public Text ButtonText1;
    public Text ButtonText2;
    public Text ButtonText3;
    string ButtonTextExisting = "Continue";
    string ButtonTextNew = "Create";

    // Character images
    public Image CharImage1;
    public Image CharImage2;
    public Image CharImage3;


    // Slot status
    bool Slot1HasSave = false;
    bool Slot2HasSave = false;
    bool Slot3HasSave = false;
    
    void Start()
    {
        // Loop through each slot and update text
        for(int i = 0; i < 3; i++)
        {
            // Create temp string to store either char name or default text (If slot is empty)
            string tempString;
            tempString = LoadRef.Load(i + 1) != "Error!" ? LoadRef.Load(i + 1) : DefaultText; // Check if load returns empty string

            switch (i)
            {
                case 0:
                    Slot1Text.text = tempString;
                    break;
                case 1:
                    Slot2Text.text = tempString;
                    break;
                case 2:
                    Slot3Text.text = tempString;
                    break;
                default:
                    break;
            }
        }

        // Check if slots are empty or not (If text is the default then it has no save)
        Slot1HasSave = Slot1Text.text == DefaultText ? false : true;
        Slot2HasSave = Slot2Text.text == DefaultText ? false : true;
        Slot3HasSave = Slot3Text.text == DefaultText ? false : true;

        // Set inputs to on/off
        Input1.gameObject.SetActive(!Slot1HasSave);
        Input2.gameObject.SetActive(!Slot2HasSave);
        Input3.gameObject.SetActive(!Slot3HasSave);

        // Set button texts
        ButtonText1.text = Slot1HasSave ? ButtonTextExisting : ButtonTextNew;
        ButtonText2.text = Slot2HasSave ? ButtonTextExisting : ButtonTextNew;
        ButtonText3.text = Slot3HasSave ? ButtonTextExisting : ButtonTextNew;

        // Set char images to on/off
        CharImage1.gameObject.SetActive(Slot1HasSave);
        CharImage2.gameObject.SetActive(Slot2HasSave);
        CharImage3.gameObject.SetActive(Slot3HasSave);

        // Set delete save buttons to on/off
        DeleteSave1.gameObject.SetActive(Slot1HasSave);
        DeleteSave2.gameObject.SetActive(Slot2HasSave);
        DeleteSave3.gameObject.SetActive(Slot3HasSave);
    }

    // Delete save and reset save slot to empty
    public void DeleteSave(int saveSlotNum)
    {

    }
}
