using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Load_Character LoadRef;
    public Text CharacterNameText;
    GameObject CharacterRef;
    public GameObject CameraRef;

    bool MoveRagdoll = false;
    float DistFromCamera;
    Transform Ragdoll;
    Vector3 DragOffset;

    void Awake()
    {
        // Load character based on current save slot in use        
        CharacterRef = LoadRef.Load(LoadRef.Load(LoadRef.LoadCurrentSlot())); // Get slot no. then character name then load character
        CharacterNameText.text = CharacterRef.name;
        CharacterRef.transform.position += new Vector3(0f, 4f, 0f); // Spawn above ground
        CharacterRef.AddComponent<Character>(); // Give it character script

        CameraRef.GetComponent<CameraFollow>().SetPlayerRef(CharacterRef);        
    }

    void Start ()
    {
        

    }
	
	void Update ()
    {
        if (Input.touchCount == 1) // user is touching the screen
        {
            Touch touch = Input.GetTouch(0); // get the touch

            Vector3 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began) // check for the first touch
            {
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100))
                {
                    if(hit.transform.name == "Face")
                    {
                        MoveRagdoll = true;
                        Ragdoll = hit.transform;
                        DistFromCamera = hit.transform.position.z - Camera.main.transform.position.z;
                        Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera);
                        newPos = Camera.main.ScreenToWorldPoint(newPos);
                        DragOffset = Ragdoll.position - newPos;

                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (MoveRagdoll)
                {
                    Vector3 newPos = new Vector3(touchPos.x, touchPos.y, DistFromCamera);
                    newPos = Camera.main.ScreenToWorldPoint(newPos);
                    Ragdoll.position = newPos + DragOffset;
                }
            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                MoveRagdoll = false;
            }
        }

    }
}
