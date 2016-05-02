using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]

public class Shake : MonoBehaviour {

    private float period;
    private float range;
    private float fixedAngle;

	// Use this for initialization
	void Start () {
        period = Random.Range(0.03f, 0.5f);
        fixedAngle = Random.Range(0f, 360f);
        range = Random.Range(30f,135f);
        StartCoroutine(Oscillate());
	}

    IEnumerator Oscillate() {
        float sign = 1;
        while (true) {
            //GetComponent<Rigidbody2D>().angularVelocity = sign * 1000;
            transform.eulerAngles = new Vector3(0, 0, fixedAngle + range *sign);
            sign *= -1;
            yield return new WaitForSeconds(period);
        }
    }

	// Update is called once per frame
	void Update () {

	}
}
