using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel {
public class MainApp : MonoBehaviour {

    JM1ProductionTable pt = new JM1ProductionTable();
    JM1NodePropertyGenerator pg = new JM1NodePropertyGenerator();
    Canvas canvas = new Canvas();

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < 10000; i++) {
            var root = pt.Produce(new JM1RootNode());
            pg.Reset();
            pg.GenerateProperty(root);
            root.Render(canvas);
            yield return new WaitForSeconds(1);
            /*
            while (!Input.GetKey("space")) {
                yield return null;
            }
            */
            foreach (var anim in FindObjectsOfType<Animatable2>()) {
                Destroy(anim.gameObject);
            }
        }
    }
}
}


