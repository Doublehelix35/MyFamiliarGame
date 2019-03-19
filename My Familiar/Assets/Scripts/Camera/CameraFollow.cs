using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float Speed = 1f;

    GameObject PlayerRef; // Player's parent object
	
	// Update is called once per frame
	void Update ()
    {
        if(PlayerRef != null)
        {
            Vector3 dir = new Vector3(PlayerRef.transform.position.x - transform.position.x, 0f, 0f);
            transform.Translate(dir * Speed * Time.deltaTime);
        }        
	}

    public void SetPlayerRef(GameObject playerRef)
    {
        // Check if player ref is null         
        if(playerRef = null)
        {
            PlayerRef = playerRef;
            Debug.Log("Player ref is Null!");
        }
        else
        {
            PlayerRef = playerRef;
        }        
    }
}
