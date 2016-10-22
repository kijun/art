using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartScript : MonoBehaviour {

	public ScreenFader fader;
    public BoxCollider2D end;

    bool restarted = false;
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !restarted) {
            restarted = true;
            StartCoroutine(Run());
        }
	}

    IEnumerator Run() {
        fader.fadeIn = false;
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("Opening Scene");
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.tag != "Player") return;
        if (!restarted) {
            restarted = true;
            StartCoroutine(Run());
        }
    }
}
