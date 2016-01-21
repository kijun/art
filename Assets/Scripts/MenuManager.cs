using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {
    public static MenuManager instance;

    public Canvas canvas;
    public Button gameTitleButton;
    public Button musicOnButton;
    public Button playButton;
    public Button replayButton;
    public Text subtitle;

    private List<Button>buttons = new List<Button>();

    void Awake() {
        // setup singleton
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // add buttons
        buttons.Add(gameTitleButton);
        buttons.Add(musicOnButton);
        buttons.Add(playButton);
        buttons.Add(replayButton);
        ActivateButton(gameTitleButton);
    }

    // button event handlers

    public void GameTitlePressed() {
        ActivateButton(musicOnButton);
        subtitle.gameObject.SetActive(false);
    }

    public void MusicOnPressed() {
        Application.OpenURL("http://listenonrepeat.com/watch/?v=6liAgg4SN88");
        ActivateButton(playButton);
    }

    public void PlayPressed() {
        canvas.gameObject.SetActive(false);
        Application.LoadLevel("Game");
        //GameManager.instance.StartGame();
    }

    public void ReplayPressed() {
        canvas.gameObject.SetActive(false);
        Application.LoadLevel("Game");
        //GameManager.instance.StartGame();
    }

    // state management

    public void GameEnded() {
        canvas.gameObject.SetActive(true);
        ActivateButton(replayButton);
    }

    // helpers

    public void ActivateButton(Button activeButton) {
        foreach (var b in buttons) {
            if (activeButton != b) {
                b.gameObject.SetActive(false);
            }
        }

        if (activeButton != null) {
            activeButton.gameObject.SetActive(true);
        }
    }
}
