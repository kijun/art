using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel {
public class MainApp : MonoBehaviour {

    JM1ProductionTable pt = new JM1ProductionTable();
    JM1NodePropertyGenerator pg = new JM1NodePropertyGenerator();
    Canvas canvas = new Canvas();

    void Start() {
        var root = pt.Produce(new JM1RootNode());
        Debug.Log(root);
        pg.GenerateProperty(root);
        root.Render(canvas);
    }
}
}


