using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_FireBall :  Item {

    // Stats
    
	void Start ()
    {
        // Init stats
        Health = 1;
        Uses = 1;
        ExpPointsGiven = 5;
        itemType = ElementType.Fire;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        // Interact code
    }
}
