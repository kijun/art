using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionTest : MonoBehaviour {

    public UnityEngine.UI.Text debug;

	// Use this for initialization
	void Start () {
        foreach (var d in Display.displays) {
            debug.text += $"{d.systemWidth}, {d.systemHeight}\n";
        }
        StartCoroutine(Setup());
	}

    IEnumerator Setup() {
        while (!Screen.fullScreen) {
            yield return null;
        }
        yield return null;
        debug.text = "";
        yield return new WaitForSeconds(2);
        foreach (var d in Display.displays) {
            debug.text += $"{d.systemWidth}, {d.systemHeight}\n";
        }
        yield return new WaitForSeconds(2);
        var second = Display.displays[1];
        yield return new WaitForSeconds(2);
        Screen.SetResolution(second.systemWidth, second.systemHeight, true);
        yield return new WaitForSeconds(2);
        second.SetRenderingResolution(second.systemWidth, second.systemHeight);
        debug.text = $"monitor res: {second.systemWidth}, {second.systemHeight}\n";
        yield return new WaitForSeconds(2);
        debug.text = $"screen res: {Screen.width}, {Screen.height}\n";
        yield return new WaitForSeconds(2);
        Resolution[] resolutions = Screen.resolutions;
        debug.text = "Avail res: \n";
        foreach (Resolution res in resolutions) {
            debug.text += $"{res.width}, {res.height}\n";
        }
    }

	// Update is called once per frame
	void Update () {

	}
}
