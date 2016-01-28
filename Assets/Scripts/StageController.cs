using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
    private PatternControllerFactory patternFactory;

    void Awake() {
        patternFactory = GetComponent<PatternControllerFactory>();
    }

    public void RunStage(Stage stage) {
        foreach(KeyValuePair<BasePattern, float> entry in stage.patterns) {
            StartWithDelay(entry.Value, entry.Key);
        }
    }

    IEnumerator StartWithDelay(float delay, BasePattern bparam) {
        yield return new WaitForSeconds(delay);
        BasePatternController bctrl = patternFactory.CreateController(bparam);
        yield return StartCoroutine(bctrl.Run());
    }
}


