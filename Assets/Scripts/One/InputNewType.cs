using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;

namespace One {

public class InputNewType : MonoBehaviour {
    public EngineNewType engine;
    void Start() {
        StartCoroutine(RandomInputs());
    }

    IEnumerator RandomInputs() {
        while (true) {
            /*
            if (Random.value > 0.5f) engine.InputEvent("right", null);
            if (Random.value > 0.5f) engine.InputEvent("right", null);
            if (Random.value > 0.5f) engine.InputEvent("right", null);
            if (Random.value > 0.5f) engine.InputEvent("right", null);
            if (Random.value > 0.5f) engine.InputEvent("left", null);
            if (Random.value > 0.5f) engine.InputEvent("left", null);
            if (Random.value > 0.5f) engine.InputEvent("left", null);
            if (Random.value > 0.5f) engine.InputEvent("up", null);
            if (Random.value > 0.5f) engine.InputEvent("up", null);
            if (Random.value > 0.5f) engine.InputEvent("up", null);
            if (Random.value > 0.5f) engine.InputEvent("up", null);
            if (Random.value > 0.5f) engine.InputEvent("down", null);
            if (Random.value > 0.5f) engine.InputEvent("down", null);
            if (Random.value > 0.5f) engine.InputEvent("down", null);
            */
            yield return new WaitForSeconds(0.4545f);
        }
    }

    void Update() {
        if (engine == null) return;
        if (Input.GetKeyDown("right")) {
            engine.InputEvent("right", null);
        }
        if (Input.GetKeyDown("left")) {
            engine.InputEvent("left", null);
        }
        if (Input.GetKeyDown("up")) {
            engine.InputEvent("up", null);
        }
        if (Input.GetKeyDown("down")) {
            engine.InputEvent("down", null);
        }
    }

    void FixedUpdate() {
        if (engine == null) return;
        engine.TickEvent();
    }
}

}
