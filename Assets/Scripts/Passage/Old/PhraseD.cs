using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseD : MonoBehaviour {
    /* Random Entry */

    public Color color;

    public CellC cellC;
    public float period = 0.5f;
    public int count = 20;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < count; i++) {
            cellC.entryAngle = Random.value * 360f;
            cellC.angularVelocity = Random.Range(0, 15);
            cellC.initialObjectAngle = Random.Range(0, 360);
            cellC.Run();
            yield return new WaitForSeconds(period);
        }
    }
}





