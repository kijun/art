using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MoonPatternController : BasePatternController {
    private MoonPattern param;

    public GameObject beam;
    /*
    public float param.pauseBetweenBeams = 5f;
    public float param.sweepDuration = 2f;
    public float param.fadeInDuration = 1f;
    public float param.fadeOutDuration = 0.5f;
    public float param.sweepAngle = 25f;
    */

    private Renderer beamR;

    public override void Initialize(BasePattern bp) {
        param = (MoonPattern)bp;
    }

    public override IEnumerator Run() {
        // do your thing with moon param
        return Searchlight();
    }

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().sortingLayerName = "Midground";
        beamR = beam.GetComponent<Renderer>();
	}

    IEnumerator Searchlight() {
        SetBeamToAngle(-param.sweepAngle);
        bool sweepRight = true;
        while (true) {
            // fade in beam
            yield return StartCoroutine(FadeBeam(0, 1));

            // rotate beam
            if (sweepRight) {
                yield return StartCoroutine(RotateBeamToAngle(-1 * param.sweepAngle, param.sweepAngle));
            } else {
                yield return StartCoroutine(RotateBeamToAngle(param.sweepAngle, -1 * param.sweepAngle));
            }

            // fade out beam
            yield return StartCoroutine(FadeBeam(1, 0));

            sweepRight = !sweepRight;

            // rest
            yield return new WaitForSeconds(param.pauseBetweenBeams);
        }
    }

    void SetBeamToAngle(float angle) {
        var rot = transform.rotation;
        rot.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rot;
    }

    IEnumerator FadeBeam(float from, float to) {
        var currentColor = beamR.material.color;
        currentColor.a = from;
        beamR.material.color = currentColor;
        var fadeInStart = Time.time;
        while (fadeInStart + param.fadeInDuration> Time.time) {
            var alpha = Mathf.Lerp(from, to, (Time.time - fadeInStart) / param.fadeInDuration);
            currentColor = beamR.material.color;
            currentColor.a = alpha;
            beamR.material.color = currentColor;
            yield return null;
        }
    }

    IEnumerator RotateBeamToAngle(float startAngle, float angle) {
        float sweepStart = Time.time;
        while (param.sweepDuration + sweepStart > Time.time) {
            float targetAngle = Mathf.Lerp(startAngle, angle, (Time.time - sweepStart) / param.sweepDuration);
            Debug.Log(targetAngle);
            SetBeamToAngle(targetAngle);
            yield return null;
        }
    }
}

