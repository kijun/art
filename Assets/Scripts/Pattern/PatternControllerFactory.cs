using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PatternControllerFactory : MonoBehaviour {
    public MoonPatternController moonPrefab;
    //public SnowController snowPrefab;
    //public EyeController  eyePrefab;

    public BasePatternController CreateController(BasePattern bparam) {
        BasePatternController bctrl = null;

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
        Debug.Log(bparam);
        Debug.Log("A" + bparam.controllerType);
        if (bparam.controllerType == typeof(MoonPatternController)) {
            Debug.Log("B");
            bctrl = Instantiate(moonPrefab);
        }

        Debug.Log("C " + bctrl);
        //
        if (bctrl) bctrl.Initialize(bparam);
        return bctrl;
    }
}


