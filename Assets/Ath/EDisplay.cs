using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class EDisplay : MonoBehaviour {

    public SphereWithMaterialBlock prefab;
    public Color backgroundOffColor, backgroundOnColor, backgroundMaxColor;
    public float offDuration, onDuration, maxDuration;
    public int lightsPerColumn; // basically orthographic size
    public float screenToLightDisplayRatio;
    public Range lightSize;
    public float onBloomIntensity, maxBloomIntensity;
    public float onBloomSampleDistance, maxBloomSampleDistance;
    public UnityStandardAssets.ImageEffects.Bloom bloom;

    Camera _mainCam;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        Camera.main.orthographicSize = lightsPerColumn * screenToLightDisplayRatio / 2;
        _mainCam = Camera.main;
        _mainCam.backgroundColor = backgroundOffColor;
        CreateSpheres();
        //var cp = new PureShape.CircleProperty(color:backgroundOnColor, diameter:10, center: new Vector2(0,0));
        //var anim = NoteFactory.CreateCircle(cp);
	}

	// Update is called once per frame
	void Update () {
//        Debug.Log(MidiMaster.GetKnob(74, 0));

        //bloom.bloomIntensity = gMidiMaster.GetKnob(93)
      if (Input.GetKey(KeyCode.Alpha1)) {
          // off
          StartCoroutine(LerpScreen(_mainCam.backgroundColor, backgroundOffColor, offDuration));
          StartCoroutine(LerpIntensity(bloom.bloomIntensity, onBloomIntensity, offDuration));
          StartCoroutine(LerpSpread(bloom.sepBlurSpread, onBloomSampleDistance, offDuration));
      } else if (Input.GetKey(KeyCode.Alpha2)) {
          StartCoroutine(LerpScreen(_mainCam.backgroundColor, backgroundOnColor, onDuration));
          StartCoroutine(LerpIntensity(bloom.bloomIntensity, onBloomIntensity, onDuration));
          StartCoroutine(LerpSpread(bloom.sepBlurSpread, onBloomSampleDistance, onDuration));
      } else if (Input.GetKey(KeyCode.Alpha3)) {
          StartCoroutine(LerpScreen(_mainCam.backgroundColor, backgroundMaxColor, maxDuration));
          StartCoroutine(LerpIntensity(bloom.bloomIntensity, maxBloomIntensity, maxDuration));
          StartCoroutine(LerpSpread(bloom.sepBlurSpread, maxBloomSampleDistance, maxDuration));
      } else if (Input.GetKey(KeyCode.Alpha0)) {
          CreateSpheres();
      }
	}

    IEnumerator LerpScreen(Color from, Color to, float duration) {
        float startTime = Time.time;
        while (Time.time - startTime < duration) {
            _mainCam.backgroundColor = Color.Lerp(from, to, (Time.time - startTime) / duration);
            yield return null;
        }
    }

    IEnumerator LerpIntensity(float from, float to, float duration) {
        float startTime = Time.time;
        while (Time.time - startTime < duration) {
            bloom.bloomIntensity = Mathf.Lerp(from, to, (Time.time - startTime) / duration);
            yield return null;
        }
    }

    IEnumerator LerpSpread(float from, float to, float duration) {
        float startTime = Time.time;
        while (Time.time - startTime < duration) {
            bloom.sepBlurSpread = Mathf.Lerp(from, to, (Time.time - startTime) / duration);
            yield return null;
        }
    }

    void CreateSpheres() {
        foreach (var s in GameObject.FindObjectsOfType<SphereWithMaterialBlock>()) {
            Destroy(s.gameObject);
        }
        Debug.Log(CameraHelper.HeightToWidthRatio);
        int width = (int)(CameraHelper.HeightToWidthRatio * lightsPerColumn);
        int height = lightsPerColumn;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                var go = GameObject.Instantiate(prefab, new Vector3(x - width/2f+0.5f, y - height/2f+0.5f, 0), Quaternion.Euler(-90, 0, 0));
                go.transform.localScale = Vector3.one * lightSize.RandomValue();
            }
        }
        Debug.Log("Created " + width*height);
    }
}
