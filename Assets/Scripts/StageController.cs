using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
    private PatternControllerFactory patternFactory;

    void Awake() {
        patternFactory = GetComponent<PatternControllerFactory>();
        Debug.Log(patternFactory);
    }

    public void RunStage(Stage stage) {
        foreach(KeyValuePair<BasePattern, float> entry in stage.patterns) {
            StartCoroutine(StartWithDelay(entry.Value, entry.Key));
        }
    }

    IEnumerator StartWithDelay(float delay, BasePattern bparam) {
        Debug.Log("starting with delay");
        yield return new WaitForSeconds(delay);
        Debug.Log("alright time to go");
        BasePatternController bctrl = patternFactory.CreateController(bparam);
        yield return StartCoroutine(bctrl.Run());
    }
}


