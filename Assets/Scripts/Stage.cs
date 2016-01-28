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
        stage.AddPattern(0, new MoonParameter());

        stages.Add(stage);
    }

    public void Start() {
        stageCtrl.RunStage(stages[0]);
    }
}


public class StageController : MonoBehaviour {
    private PatternControllerFactory patternFactory;

    void Awake() {
        patternFactory = GetComponent<PatternControllerFactory>();
    }

    public void RunStage(Stage stage) {
        foreach(KeyValuePair<BaseParameter, float> entry in stage.patterns) {
            StartWithDelay(entry.Value, entry.Key);
        }
    }

    IEnumerator StartWithDelay(float delay, BaseParameter bparam) {
        yield return new WaitForSeconds(delay);
        BaseController bctrl = patternFactory.CreateController(bparam);
        yield return StartCoroutine(bctrl.Run());
    }
}

// it probably makes sense to have stage controller run this
public class Stage {

    public int chapter;
    public int verse;
    public string introduction;
    public float duration;

    public Dictionary<BaseParameter, float> patterns = new Dictionary<BaseParameter, float>();
    //private List<Pattern.BasePattern> patterns = new List<Pattern.BasePattern>();

    public Stage(int chapter, int verse) {
        this.chapter = chapter;
        this.verse = verse;
    }

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () { }

    // at the right time
    //
    //
    public void AddPattern(float startTime, BaseParameter bparam) {
        // insert into priority queue
        patterns.Add(bparam, startTime);
    }

}

// stage has obstacles
// pattern holds obstacles and parameters
// can't I just instantiate obstacles and let it run?

public abstract class BaseController : MonoBehaviour {
    public abstract void Initialize(BaseParameter bp);
    public abstract IEnumerator Run();
}

public class MoonController : BaseController {
    private MoonParameter param;

    public override void Initialize(BaseParameter bp) {
        param = (MoonParameter)bp;
    }

    public override IEnumerator Run() {
        // do your thing with moon param
        yield return null;
    }
}

public abstract class BaseParameter : ScriptableObject {
    // execution every frame
    public Type controllerType;
//    public abstract void InitializeController(BaseController g);
}


// TODO might have to supply multiple parameters
// use object initializer
public class MoonParameter: BaseParameter {

    public float duration = 5f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 0.5f;
    public float sweepAngle = 25f;
    public float pauseBetweenBeams = 5f;
    public Type controllerType = typeof(MoonController);

    /*
    public override void InitializeController(BaseController bc) {
        MoonController mc = (MoonController)bc;
        bc.Initialize(this);
    }
    */
}

public enum Transition {
    Linear, EaseIn, EaseOut
}

public class PatternControllerFactory : MonoBehaviour {
    public MoonController moonPrefab;
    //public SnowController snowPrefab;
    //public EyeController  eyePrefab;

    public BaseController CreateController(BaseParameter bparam) {
        BaseController bctrl = null;

        /* TODO type switcher
        switch(bparam.controllerType) {
            case typeof(MoonController):
                bctrl = Instantiate(moonPrefab);
                break;
            /*
            case typeof(SnowController):
                go = Instantiate(snowPrefab);
                break;
            case typeof(EyeController):
                go = Instantiate(eyePrefab);
                break;
            default:
                break;
        }
        */
        if (bparam.controllerType == typeof(MoonController)) {
            bctrl = Instantiate(moonPrefab);
        }

        //
        if (bctrl) bctrl.Initialize(bparam);
        return bctrl;
    }
}


