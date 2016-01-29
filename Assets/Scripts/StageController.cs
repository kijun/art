using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
    public float fadeTime = 5f;
    private PatternControllerFactory patternFactory;
    public Text stageIntro;

    void Awake() {
        patternFactory = GetComponent<PatternControllerFactory>();
        Debug.Log(patternFactory);
    }

    public IEnumerator RunStage(Stage stage) {

        stageIntro.text = String.Format("{0}:{1}\n\n\n\n{2}", stage.chapter, stage.verse, stage.introduction);
        //yield return new WaitForSeconds(textWaitTime);
        while (!Input.anyKeyDown) {
            yield return null;
        }
        stageIntro.text = "";

        foreach(KeyValuePair<BasePattern, float> entry in stage.patterns) {
            StartCoroutine(StartWithDelay(entry.Value, entry.Key));
        }
    }

    IEnumerator StartWithDelay(float delay, BasePattern bparam) {
        yield return new WaitForSeconds(delay);
        BasePatternController bctrl = patternFactory.CreateController(bparam);
        yield return StartCoroutine(bctrl.Run());
    }

    public void Stop() {
        // will this care of future coroutines?
        StopAllCoroutines();
    }
}


