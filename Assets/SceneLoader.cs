using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour {

	// Update is called once per frame
    public ScreenFader fader;
    bool loaded = false;

	void Update () {
        if (Input.anyKeyDown && !loaded) {
            loaded = true;
            StartCoroutine(Run());
        }
	}

    IEnumerator Run() {
        fader.fadeIn = false;
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("GameFSM");
    }
}
