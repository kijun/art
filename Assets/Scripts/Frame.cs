using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class Frame : MonoBehaviour {

    public float lockDelay = 2f;
    public float lockDuration = 3f;
    public float unlockDelay = 1f;
    public float defaultAlpha = 0.2f;

    private PlayerController player;
    private bool activated = false;

    void Start() {
        foreach (Transform c in transform) {
            c.GetComponent<Renderer>().material.SetAlpha(defaultAlpha);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other);
        if (other.gameObject.tag == "Player") {
            StartCoroutine(LockAndUnlock());
        }
    }

    IEnumerator LockAndUnlock() {
        if (activated) yield break;
        yield return StartCoroutine(
                LerpHelper.Lerp(
                    delegate(float ratio) {SetAlpha(defaultAlpha + ratio*(1f-defaultAlpha)); },
                    lockDelay));
        yield return new WaitForSeconds(lockDuration);
        yield return StartCoroutine(
                LerpHelper.Lerp(
                    delegate(float ratio) {SetAlpha(1-ratio);},
                    unlockDelay));
        activated = true;
        enabled = false;
        // TODO make it reusable
        //Destroy(gameObject);
    }

    void SetAlpha(float alpha) {
        foreach (Transform c in transform) {
            c.GetComponent<Renderer>().material.SetAlpha(alpha);
        }
    }
}
