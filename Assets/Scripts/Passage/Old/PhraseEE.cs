using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseEE : MonoBehaviour {
    /* Random Entry */

    public CellE cellE;
    public float period = 0.5f;
    public int count = 20;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < count; i++) {
            cellE.Run();
            yield return new WaitForSeconds(period);
        }
    }
}







