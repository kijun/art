using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [HideInInspector] public bool journeying;

    private BoardManager boardScript;

    void Awake() {
        // Assign static instance
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Setup params
        boardScript = GetComponent<BoardManager>();
        journeying = false;

        InitGame();
        // setup ship
        // show UI
        StartGame(); // TODO change
    }

    void InitGame() {
        boardScript.SetupScene();
    }

    void StartGame() {
        journeying = true;
    }
}

