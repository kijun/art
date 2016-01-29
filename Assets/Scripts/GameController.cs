using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    private List<Stage> stages = new List<Stage>();
    private StageController stageCtrl;


    public void Awake() {
        stages = new List<Stage>();
        stageCtrl = GetComponent<StageController>();

        // 0:0
        var stage = new Stage(0, 0);
        stage.introduction = "As he left the familiar airspace, Quinti found himself tormented with the thoughts he'd left behind. \n\nHis first memory was of winter.";
        stage.AddPattern(0, new MoonPattern());
        stage.duration = 5f;
        stages.Add(stage);

        stage = new Stage(0, 1);
        stage.introduction = "The moon is scarier";
        stage.AddPattern(0, new MoonPattern{sweepDuration = 5f, sweepAngle = 45f});
        stage.duration = 10f;
        stages.Add(stage);
    }

    public void Start() {
        StartCoroutine(RunStages());
    }

    IEnumerator RunStages() {
        foreach (Stage s in stages) {
            yield return StartCoroutine(stageCtrl.RunStage(s));
            yield return new WaitForSeconds(s.duration);
        }
    }
}

