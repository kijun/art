using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateDefeat : GameState {
    private Text defeatTextUI;

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

