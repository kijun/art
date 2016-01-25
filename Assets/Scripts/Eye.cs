using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {

    public GameObject closedEye;
    public GameObject openedEye;
    public Rigidbody2D tearPrefab;

    public int numTears = 4;
    public float delayBetweenEyes;
    public float delayBeforeTears = 2;
    public Range delayBetweenTears = new Range(0.5f, 1);
    public Range tearXVelocity = new Range(-3, 3);
    public Range tearYVelocity = new Range(-4, -6);

	// Use this for initialization
	void Start () {
        StartCoroutine(OpenAndClose());
	}

	// Update is called once per frame
	void Update () {
	}

    IEnumerator OpenAndClose () {
        closedEye.SetActive(false);
        openedEye.SetActive(true);
        yield return new WaitForSeconds(delayBeforeTears);
        for (int i = 0; i<numTears; i++) {
            var t = Instantiate<Rigidbody2D>(tearPrefab);
            t.transform.position = transform.position.IncrY(-1f);
            t.gameObject.SetActive(true);
            // TODO gravity?
            t.velocity = new Vector2(tearXVelocity.RandomValue(), tearYVelocity.RandomValue());
            yield return new WaitForSeconds(delayBetweenTears.RandomValue());
        }
        yield return new WaitForSeconds(delayBetweenEyes);
        closedEye.SetActive(true);
        openedEye.SetActive(false);
        yield return new WaitForSeconds(delayBetweenEyes);
    }
}
