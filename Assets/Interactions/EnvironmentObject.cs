using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : NPC
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void interact()
    {
        Debug.Log("this is an environment object");
        Debug.Log("environment object name: " + this.characterName);

    }
}
