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
    public float grayOnRamp = 0.4f;

    float minRampOffset = -0.70f;

    bool lerp = false;
    bool powerOn = false;

	void Update () {
        float brightnessDial = MidiMaster.GetKnob(77, (-minRampOffset)/1.7f); // knob 2
        maxRampOffset = brightnessDial * 1.7f + minRampOffset;
        if (!powerOn) {
            if (MidiMaster.GetKeyDown(41)) {
                StartC(LerpToOn(setPowerOn: true));
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
            StartC(LerpToOff());
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

    IEnumerator LerpToOn(bool setPowerOn = false) {
        float startTime = Time.time;
        float startOff = gray.rampOffset;
        float approachDuration = 0.08f;
        maxRampOffset = 0;
        while (Time.time - startTime < approachDuration) {
            gray.rampOffset = Mathf.Lerp(grayOnRamp*1.5f/2, grayOnRamp, (Time.time - startTime) / approachDuration);
            yield return null;
        }

        startTime = Time.time;
        approachDuration = 2f;
        while (Time.time - startTime < approachDuration) {
            gray.rampOffset = Mathf.Lerp(grayOnRamp, maxRampOffset, (Time.time - startTime) / approachDuration);
            yield return null;
        }
        lerp = false;

        if (setPowerOn) powerOn = true;
    }

    IEnumerator LerpToMax() {
        float startTime = Time.time;
        float startOff = gray.rampOffset;
        float approachDuration = 0.5f;
        while (Time.time - startTime < approachDuration) {
            gray.rampOffset = Mathf.Lerp(startOff, maxRampOffset, (Time.time - startTime) / approachDuration);
            yield return null;
        }
        lerp = false;
    }

    IEnumerator LerpToOff() {
        float startTime = Time.time;
        float startOff = gray.rampOffset;
        float approachDuration = 4.0f;
        while (Time.time - startTime < approachDuration) {
            gray.rampOffset = Mathf.Lerp(startOff, 0, (Time.time - startTime) / approachDuration);
            yield return null;
        }
        gray.rampOffset = 0;
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
                float currLerpParam = Mathf.InverseLerp(0, maxRampOffset, gray.rampOffset);
                float sinVal = Mathf.Sin(Time.time - startTime);
                float upOrDown = Mathf.Sign(sinVal);

                startTime = Time.time - (Mathf.Asin(currLerpParam * 2f - 1f) / lerpFrequency);
                //startTime = startTime + 2 * (Mathf.PI / 2 - startTime);
                // if opposite direction, (pi / 2) - startTime
            }
            gray.rampOffset = Mathf.Lerp(0, maxRampOffset, (Mathf.Sin((Time.time - startTime)*lerpFrequency) + 1f) / 2f);
            yield return null;
        }
    }
}
