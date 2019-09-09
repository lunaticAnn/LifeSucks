using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Input Handler Singleton will process the game input event and getting it processed for the whole game.
    public static InputHandler instance = null;


    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }
    
}
