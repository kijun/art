using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CircleCollider2D))]
public class DoorAndKeyObstacle : MonoBehaviour {

    public GameObject door;
    public GameObject key;
    public Transform keyProgressPrefab;
    public float secondsToOpen = 2f;

    private bool doorOpened;
    private Transform keyProgress;
    private float openProgress;

    void Awake() {
        var cc = GetComponent<CircleCollider2D>();
        cc.radius = key.GetComponent<Renderer>().bounds.size.x/2f;
        cc.offset = key.transform.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (doorOpened) return;
        if (other.gameObject.IsPlayer()) {
            StartCoroutine("OpenDoor");
        }
    }

    IEnumerator OpenDoor() {
        if (keyProgress == null) {
            keyProgress = Instantiate<Transform>(keyProgressPrefab);
            keyProgress.transform.position = key.transform.position.SwapZ(10);
            keyProgress.localScale = Vector3.zero;
        }

        LerpDelegate openFunc = delegate(float openFraction) {
            openProgress = openFraction;
            SetBallSize(openProgress);
            SetDoorAlpha(1-openProgress);
        };

        IEnumerator lerpCoroutine = LerpHelper.Lerp(openFunc, secondsToOpen);
        while (lerpCoroutine.MoveNext()) {
            yield return null;
        }

        //yield return StartCoroutine(LerpHelper.Lerp(openFunc, secondsToOpen));

        doorOpened = true;
        door.SetActive(false);
    }

    IEnumerator CloseDoor() {
        float startingProgress = openProgress;

        LerpDelegate closeFunc = delegate(float timeFraction) {
            openProgress = startingProgress - startingProgress*timeFraction;

            SetBallSize(openProgress);
            SetDoorAlpha(1-openProgress);
        };

        yield return StartCoroutine(LerpHelper.Lerp(closeFunc, secondsToOpen*startingProgress));
    }

    void SetBallSize(float openFraction) {
        keyProgress.localScale = new Vector3(openFraction, openFraction, openFraction);
    }

    void SetDoorAlpha(float alpha) {
        door.SetAlpha(alpha);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (doorOpened) return;
        if (openProgress < float.Epsilon) return;

        if (other.gameObject.IsPlayer()) {
            StopCoroutine("OpenDoor");
            StartCoroutine("CloseDoor");
        }
    }
}
