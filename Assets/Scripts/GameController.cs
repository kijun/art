using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    private GameState currentState;

    // SETUP
    public void Awake() {
    }

    //  START
    public void Start() {
        ChangeState(GameStatePlay.instance);
    }

    public void ChangeState(GameState gameStateInstance) {
        if (currentState != null) {
            currentState.CleanUp();
        }
        currentState = gameStateInstance;
        Debug.Log(currentState);
        currentState.Run(ChangeState);
    }
}

