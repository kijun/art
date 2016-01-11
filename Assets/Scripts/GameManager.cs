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

        //ShowIntro();

        //InitGame();
        // setup ship
        // show UI
        //StartGame(); // TODO change
    }

    public void StartGame() {
        journeying = true;
        boardScript.SetupScene();
    }

    public void EndGame() {
        // show UI
        journeying = false;
    }
}

