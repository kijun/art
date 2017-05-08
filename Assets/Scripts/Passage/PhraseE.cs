using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseE : MonoBehaviour {
    /* Random Entry */

    public CellD cellD;
    public float period = 0.5f;
    public int count = 20;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < count; i++) {
            cellD.Run();
            yield return new WaitForSeconds(period);
        }
    }
}






