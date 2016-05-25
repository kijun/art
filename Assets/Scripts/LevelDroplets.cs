using UnityEngine;
using System.Collections;

public class LevelDroplets : MonoBehaviour {

    public CircleProperty circlePrefab;
    //public LineProperty linePrefab;
    public RectProperty rectPrefab;
    public Range sleepTime = new Range(1, 3);

	// Use this for initialization
	void Start () {
        StartCoroutine(Drop());
	}

	// Update is called once per frame
	void Update () {
	}

    IEnumerator Drop() {
        while (true) {
            // area
            Bounds area = Camera.main.WorldBounds();
            var target = area.RandomPoint();
            switch (Random.Range(0, 2)) {
                case 0:
                    var obj = Instantiate(circlePrefab, target, Quaternion.identity) as CircleProperty;
                    obj.OnUpdate();
                    break;
                case 1:
                    var obj2 = Instantiate(rectPrefab, target, Quaternion.identity) as RectProperty;
                    obj2.OnUpdate();
                    break;
            }
            yield return new WaitForSeconds(sleepTime.RandomValue());
        }
    }
}
