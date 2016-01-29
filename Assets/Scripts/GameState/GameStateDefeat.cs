using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateDefeat : GameState {
    public static GameState instance;
    private Text defeatTextUI;

    protected void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Run() {
        // show text
        //
        while (!Input.anyKeyDown) {
            yield return null;
        }
        onChangeDelegate(GameStatePlay.instance);
    }

    public override void CleanUp() {
    }

}

