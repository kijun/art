using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class AscendingObstacle : MonoBehaviour {

    public float startAlpha = 0.2f;
    public float ascendDuration = 3f;
    public float freezeDuration = 1f;
    public float fadeDuration = 1f;
    public Vector2 ascendVelocity = new Vector2(-1, -2);
    public float ascendAngularVelocity = 30f;

	// Use this for initialization
	void Start () {
        StartCoroutine(Ascend());
	}

    IEnumerator Ascend() {

        Rigidbody2D rgbd = GetComponent<Rigidbody2D>();
        rgbd.velocity = ascendVelocity;
        rgbd.angularVelocity = ascendAngularVelocity;

        // TODO lerp on the rest
        yield return StartCoroutine(LerpHelper.Lerp(
                    delegate (float ratio) {SetChildrenAlpha(startAlpha+ratio*(1f-startAlpha));},
                    ascendDuration));

        rgbd.velocity = Vector2.zero;
        rgbd.angularVelocity = 0;

        yield return new WaitForSeconds(freezeDuration);

        yield return StartCoroutine(LerpHelper.Lerp(delegate (float ratio) {SetChildrenAlpha(1-ratio);}, fadeDuration));
        Debug.Log("DONE");
    }

    void SetChildrenAlpha(float a) {
        foreach (Transform child in transform) {
            child.gameObject.SetAlpha(a);
        }
    }



	// Update is called once per frame
	void Update () {

	}
}
