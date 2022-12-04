using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<string> Scene2Requirements = new();
    void Start()
    {
        Scene2Requirements.Add("Cigar");
        Scene2Requirements.Add("Handkerchief");
        Scene2Requirements.Add("Pocket Watch");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
