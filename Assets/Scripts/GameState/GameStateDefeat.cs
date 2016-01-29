using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateDefeat : GameState {
    public static GameState instance;
    public Text outputText;

    protected void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public override void Run(GameStateChangeRequestDelegate onChange) {
        base.Run(onChange);
        onChangeDelegate(GameStatePlay.instance);
        //StartCoroutine(WaitForInput());
    }

    /*
    IEnumerator WaitForInput() {
        /*
        outputText.text = "Your mind has been wandering...\n\nabout a glass tank high as a cathedral\na palm tree which plays the harp\na square with a horseshoe marble table\na marble tablecloth\nset with foods and beverages\nalso of marble\n\ntry again.";
        yield return new WaitForSeconds(5f);
        while (!Input.anyKeyDown) {
            yield return null;
        }
        outputText.text = "";
        onChangeDelegate(GameStatePlay.instance);
    }
    */

    public override void CleanUp() {
    }

}

