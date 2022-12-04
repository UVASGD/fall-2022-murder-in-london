using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class Character : NPC
{
    public Sprite image;

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
        Debug.Log("this is a character");
        Debug.Log("character name: " + this.characterName);

    }
}
