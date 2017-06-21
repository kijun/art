using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseF : MonoBehaviour {
    /* Random Entry */

    public BaseCell cell;
    public float period = 0.5f;
    public int count = 20;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < count; i++) {
            cell.Run();
            yield return new WaitForSeconds(period);
        }
    }
}








