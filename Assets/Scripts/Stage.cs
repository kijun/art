using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


// it probably makes sense to have stage controller run this
public class Stage {

    public int chapter;
    public int verse;
    public string introduction;
    public float duration;

    public Dictionary<BasePattern, float> patterns = new Dictionary<BasePattern, float>();
    //private List<Pattern.BasePattern> patterns = new List<Pattern.BasePattern>();

    public Stage(int chapter, int verse) {
        this.chapter = chapter;
        this.verse = verse;
    }

    // at the right time
    //
    //
    public void AddPattern(float startTime, BasePattern bparam) {
        // insert into priority queue
        patterns.Add(bparam, startTime);
    }

}

// stage has obstacles
// pattern holds obstacles and parameters
// can't I just instantiate obstacles and let it run?


public enum Transition {
    Linear, EaseIn, EaseOut
}
