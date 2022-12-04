using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    private static GameProgressManager _gameProgressManager;

    public static GameProgressManager Instance { get { return _gameProgressManager; } }


    private void Awake()
    {
        if (_gameProgressManager != null && _gameProgressManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _gameProgressManager = this;
        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    public static List<string> Scene2Requirements = new List<string>
    {
        "Cigar",
        "Handkerchief",
        "Pocket Watch",
    };
    public static List<string> Scene4Requirements = new List<string>
    {
        "Cigar",
        "Handkerchief",
        "Pocket Watch",
        "Notes",
        "Poster",
        "Handgun"
    };

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
