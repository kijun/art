using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseC : MonoBehaviour {
    /*Wash, <^v>*/

    public Color color;

    public CellB cellB;
    public float period = 0.5f;
    public int count = 20;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < count; i++) {
            cellB.entryAngle = Random.Range(0, 8) * 45;
            cellB.count = Random.Range(1, 3);
            cellB.Run();
            yield return new WaitForSeconds(period);
        }
    }
}




