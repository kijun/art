using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseA : MonoBehaviour {

    public CellA cellA;
    public float period = 0.5f;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < 640000; i++) {
            cellA.count = 2*Random.Range(2,6) + 1;
            cellA.Run();
            yield return new WaitForSeconds(period);
        }
    }
}


