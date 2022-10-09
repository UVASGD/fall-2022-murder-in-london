using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlls which game state the game is currently in.
// Currently coding for either a investigative phase or a dialogue phase.
public class GameStateManager : MonoBehaviour
{


    enum GameState: int
    {
        investigation = 0,
        dialogue = 1,
    }

    public int game_state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
