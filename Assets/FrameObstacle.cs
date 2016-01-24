using UnityEngine;
using System.Collections;

public class FrameObstacle : MonoBehaviour {

    public GameObject top;
    public GameObject left;
    public GameObject right;
    public GameObject bottom;
    public float shrinkDuration = 3f;
    public float timeBetweenSet = 2f;

    Vector2 topOrigPos;
    Vector2 leftOrigPos;
    Vector2 rightOrigPos;
    Vector2 bottomOrigPos;

	// Use this for initialization
	void Start () {
        topOrigPos = top.transform.position;
        leftOrigPos = left.transform.position;
        rightOrigPos = right.transform.position;
        bottomOrigPos = bottom.transform.position;
        top.GetComponent<Renderer>().sortingLayerName = "UI";
        left.GetComponent<Renderer>().sortingLayerName = "UI";
        right.GetComponent<Renderer>().sortingLayerName = "UI";
        bottom.GetComponent<Renderer>().sortingLayerName = "UI";
        StartCoroutine(Enclose());
	}

    IEnumerator Enclose() {
        var topTargetPos = topOrigPos;
        var leftTargetPos = leftOrigPos;
        var rightTargetPos = rightOrigPos;
        var bottomTargetPos = bottomOrigPos;

        topTargetPos.y -= 3;
        leftTargetPos.x += 2;
        rightTargetPos.x -= 2;
        bottomTargetPos.y += 3;

        while (true) {
            var startTime = Time.time;
            while (Time.time < startTime + shrinkDuration) {
                top.transform.position = Vector2.Lerp(topOrigPos, topTargetPos, (Time.time - startTime)/shrinkDuration);
                left.transform.position = Vector2.Lerp(leftOrigPos, leftTargetPos, (Time.time - startTime)/shrinkDuration);
                right.transform.position = Vector2.Lerp(rightOrigPos, rightTargetPos, (Time.time - startTime)/shrinkDuration);
                bottom.transform.position = Vector2.Lerp(bottomOrigPos, bottomTargetPos, (Time.time - startTime)/shrinkDuration);
                yield return null;
            }
            yield return new WaitForSeconds(timeBetweenSet);
            startTime = Time.time;
            while (Time.time < startTime + shrinkDuration) {
                top.transform.position = Vector2.Lerp(topTargetPos, topOrigPos, (Time.time - startTime)/shrinkDuration);
                left.transform.position = Vector2.Lerp(leftTargetPos, leftOrigPos, (Time.time - startTime)/shrinkDuration);
                right.transform.position = Vector2.Lerp(rightTargetPos, rightOrigPos, (Time.time - startTime)/shrinkDuration);
                bottom.transform.position = Vector2.Lerp(bottomTargetPos, bottomOrigPos, (Time.time - startTime)/shrinkDuration);
                yield return null;
            }
            yield return new WaitForSeconds(timeBetweenSet);
        }
        yield break;
    }
}
