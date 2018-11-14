using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Load_Character LoadRef;
    public Text CharacterNameText;

	void Start ()
    {
        // Load character based on current save slot in use        
        GameObject TempCharacter = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot())); // Get slot no. then character name then load character
        CharacterNameText.text = TempCharacter.name;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
