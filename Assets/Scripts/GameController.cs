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
        stage.introduction = "As he left the familiar airspace, Quinti found himself tormented with the thoughts he'd left behind. \nHis first memory was of winter.";
        stage.AddPattern(0, new MoonPattern());

        stages.Add(stage);
    }

    public void Start() {
        stageCtrl.RunStage(stages[0]);
    }
}

