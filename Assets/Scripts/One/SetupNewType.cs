using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;

namespace One {

public class SetupNewType : MonoBehaviour {
    public EngineNewType engine;
    public InputNewType  input;
    public RendererNewType renderer;

    void Start() {
        // Setup engine with schema
        // Setup input,
        input.engine = engine;
        renderer.engine = engine;
    }

    /*
    void Update() {
        Update();
    }
    */
}

}
