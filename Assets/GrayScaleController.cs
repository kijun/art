using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayScaleController : MonoBehaviour {

    public UnityStandardAssets.ImageEffects.Grayscale gray;
    public IEnumerator currCoroutine;

	void Update () {
      if (Input.GetKey(KeyCode.Q)) {
          // no flicker
          StopC();
          gray.rampOffset = 0;
      } else if (Input.GetKey(KeyCode.W)) {
          StopC();
          StartC(LerpGray(0, 0.2f, 1));
      } else if (Input.GetKey(KeyCode.E)) {
          StopC();
          StartC(LerpGray(0, 0.2f, 5));
      } else if (Input.GetKey(KeyCode.R)) {
          StopC();
          StartC(LerpGray(0, 0.2f, 10));
      }
	}

    void StopC() {
        if (currCoroutine != null) StopCoroutine(currCoroutine);
        currCoroutine = null;
    }

    void StartC(IEnumerator c) {
        currCoroutine = c;
        StartCoroutine(c);
    }

    IEnumerator LerpGray(float from, float to, float frequency) {
        float startTime = Time.time;
        while (true) {
            gray.rampOffset = Mathf.Lerp(from, to, (Mathf.Sin((Time.time - startTime)*frequency) + 1f) / 2f);
            yield return null;
        }
    }
}
