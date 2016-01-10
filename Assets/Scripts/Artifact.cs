using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// probably shouldn't be artifact
public class Artifact : MonoBehaviour {
    [TextArea(3,10)]
    public string poetry;

    public void ShrinkAndHide() {
        // slerp size
        StartCoroutine(ShrinkSelf());
    }

    IEnumerator ShrinkSelf() {
        while (transform.localScale.sqrMagnitude > float.Epsilon) {
            transform.localScale *= 0.90f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
