using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStatePlay : GameState {
    public static GameState instance;
    private const string PrefKeyStageLastPlayed = "StageLastPlayed";

    public PlayerController playerPrefab;

    public Text outputText;

    private List<Stage> stages = new List<Stage>();
    private StageController stageCtrl;
    private Stage currentStage;
    private PlayerController playerCtrl;

    // Object Setup

    protected void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


        stages = new List<Stage>();
        stageCtrl = GetComponent<StageController>();
        TempSetupStages();

SetCurrentStage(stages[0]);

        //playerCtrl = Instantiate<PlayerController>(playerPrefab);
        playerCtrl = playerPrefab;//Instantiate<PlayerController>(playerPrefab);
        playerCtrl.OnHit = OnHit;
    }
    // Main Script

    public override void Run(GameStateChangeRequestDelegate onChange) {
        Debug.Log("run");
        playerCtrl.Reset();
        base.Run(onChange);
        playerCtrl.ChangeState(PlayerController.State.Normal);
        StartCoroutine(RunStages());
    }

    IEnumerator RunStages() {
        for (int i = LoadLastStageIndex(); i<stages.Count; i++) {
            Debug.Log("Running stage " + i);
            Stage stage = stages[i];
            SetCurrentStage(stage);
            yield return StartCoroutine(stageCtrl.RunStage(stage));
            yield return new WaitForSeconds(stage.duration);
        }
        OnWin();
    }

    // Stage Accesors
    void SetCurrentStage(Stage stage) {
        currentStage = stage;
        PlayerPrefs.SetInt(PrefKeyStageLastPlayed, stages.IndexOf(stage));
    }

    int LoadLastStageIndex() {
        return PlayerPrefs.GetInt(PrefKeyStageLastPlayed, 0);
    }

    // Clean Up
    public override void CleanUp() {
        stageCtrl.CleanUp();
    }

    // Game State Management
    void Stop() {
        StopAllCoroutines();
        stageCtrl.Stop();
        playerCtrl.ChangeState(PlayerController.State.Destroyed);

        // stop all activators
        foreach (Activator a in FindObjectsOfType<Activator>()) {
            a.Reset();
        }
    }

    void OnHit() {
        // stop coroutines
        Stop();

        // effect
        //CameraFollow.instance.ScreenShake();
        outputText.text = "Your mind has been wandering...\n\nabout a glass tank high as a cathedral\na palm tree which plays the harp\na square with a horseshoe marble table\na marble tablecloth\nset with foods and beverages\nalso of marble\n\ntry again.";

        StartCoroutine(Transition());
    }

    IEnumerator Transition() {
        Time.timeScale = 0;
        Debug.Log("waiting");
        //yield return new WaitForSeconds(3f);
        while (!Input.anyKeyDown) {
            yield return null;
        }
        playerCtrl.transform.position = new Vector2(0, -2);
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Time.timeScale = 1;
        onChangeDelegate(GameStateDefeat.instance);
    }

    void OnWin() {
        Stop();
        outputText.text = "You won.\n\nPlay from the beginning.";
        SetCurrentStage(stages[0]);
        StartCoroutine(Transition());
    }

    // TODO: load from UnityAsset
    void TempSetupStages() {
        // 0:0
        var stage = new Stage(0, 0);
        stage.introduction = "As he left the familiar airspace, Quinti found himself tormented with the thoughts he'd left behind. \n\nHis first memory was of winter.";
        //stage.AddPattern(0, new MoonPattern());
        stage.duration = 100f;
        stages.Add(stage);

        // 0:1
        stage = new Stage(0, 1);
        stage.introduction = "The moon is scarier";
        stage.AddPattern(0, new MoonPattern{sweepDuration = 5f, sweepAngle = 45f});
        stage.duration = 3f;
        stages.Add(stage);
    }

}

