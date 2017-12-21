using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class GrayScaleController : MonoBehaviour {

    public UnityStandardAssets.ImageEffects.Grayscale gray;
    public IEnumerator currCoroutine;

    public float prevDial = 0.5f;
    public float maxRampOffset = 0;
    public float lerpFrequency = 1;

    float minRampOffset = -0.20f;

    bool lerp = false;
    bool powerOn = false;

	void Update () {
        float brightnessDial = MidiMaster.GetKnob(77, 0); // knob 2
        maxRampOffset = brightnessDial * 1.2f + minRampOffset;
        if (!powerOn) {
            if (MidiMaster.GetKeyDown(41)) {
                StartC(LerpToMax(setPowerOn: true));
            }
            return;
        }


        float frequencyDial = MidiMaster.GetKnob(78, 0); // knob 3
        lerpFrequency = 1 + 10*frequencyDial;

        bool lerpStatusChanged = false;
        if (!lerp && MidiMaster.GetKeyDown(73)) {
            lerp = true;
            lerpStatusChanged = true;
            Debug.Log("Pulse On");
        }
        if (lerp && MidiMaster.GetKeyDown(74)) {
            lerp = false;
            lerpStatusChanged = true;
            Debug.Log("Pulse Off");
        }


        if (lerpStatusChanged) {
            StopC();
            if (lerp) {
                StartC(LerpGray());
            } else {
                lerp = true;
                StartC(LerpToMax());
            }
        }

        if (!lerp) {
            gray.rampOffset = maxRampOffset;
        }

        if (MidiMaster.GetKeyDown(42)) {
            powerOn = false;
            StopC();
            StartC(LerpToMin());
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

    IEnumerator LerpToMax(bool setPowerOn = false) {
        float startTime = Time.time;
        float startOff = gray.rampOffset;
        float approachDuration = 1;
        while (Time.time - startTime < approachDuration) {
            gray.rampOffset = Mathf.Lerp(startOff, maxRampOffset, (Time.time - startTime) / approachDuration);
            yield return null;
        }
        gray.rampOffset = maxRampOffset;
        lerp = false;

        if (setPowerOn) powerOn = true;
    }

    IEnumerator LerpToMin() {
        float startTime = Time.time;
        float startOff = gray.rampOffset;
        float approachDuration = 1;
        while (Time.time - startTime < approachDuration) {
            gray.rampOffset = Mathf.Lerp(startOff, minRampOffset, (Time.time - startTime) / approachDuration);
            yield return null;
        }
        gray.rampOffset = minRampOffset;
        lerp = false;
    }

    IEnumerator LerpGray() {
        float startTime = Time.time;
        float prevMaxRamp = maxRampOffset;
        float prevLerpFreq = lerpFrequency;
        float offset = 0;
        while (true) {
            if (!prevMaxRamp.Approx(maxRampOffset))  {
                prevMaxRamp = maxRampOffset;
            }
            if (!prevLerpFreq.Approx(lerpFrequency))  {
                prevLerpFreq = lerpFrequency;
                float currLerpParam = Mathf.InverseLerp(minRampOffset, maxRampOffset, gray.rampOffset);
                float sinVal = Mathf.Sin(Time.time - startTime);
                float upOrDown = Mathf.Sign(sinVal);

                startTime = Time.time - (Mathf.Asin(currLerpParam * 2f - 1f) / lerpFrequency);
                //startTime = startTime + 2 * (Mathf.PI / 2 - startTime);
                // if opposite direction, (pi / 2) - startTime
            }
            gray.rampOffset = Mathf.Lerp(minRampOffset, maxRampOffset, (Mathf.Sin((Time.time - startTime)*lerpFrequency) + 1f) / 2f);
            yield return null;
        }
    }
}
