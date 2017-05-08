using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellA : MonoBehaviour {
    /*
     *       []
     *
     *
     *  []        []
     *
     */

    public Animatable2 prefab;
    public Vector2 scale = new Vector2(0.5f, 2);
    public int count = 1;
    public float speed = 1;
    public Color color;
    public Vector2 scaleVelocity;

    public float startAngle = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

    public void Run() {
        StartCoroutine(_Run());
        /*Debug.Log("starting coroutine");*/
    }

    public IEnumerator _Run() {
        // shoot three to the middle
        var rotation = startAngle;
        var unitOfRotation = 360f / count;

        for (int i = 0; i < count; i++) {
            var p = CreatePlane();
            p.localScale = scale;
            p.rotation = rotation;
            p.velocity = Quaternion.Euler(0, 0, rotation) * Vector2.down * speed;
            p.scaleVelocity = scaleVelocity;

            var width = scale.x ;
            var height = scale.y;
            var entryAngle = rotation;
            var distanceFromOrigin = cameraDiameter / 2 + Mathf.Sqrt(width * width + height * height) / 2;
            var objectPos =
                Quaternion.Euler(0, 0, entryAngle) * (Vector2.up * distanceFromOrigin);

            p.position = objectPos;
            p.color = color;

            rotation += unitOfRotation;
        }

        yield return null;
    }

    Animatable2 CreatePlane() {
        return Instantiate<Animatable2>(prefab);
    }

    float cameraDiameter {
        get {
            var inGameHeight = Camera.main.orthographicSize * 2;
            var inGameWidth = (float)Screen.width / (float)Screen.height * inGameHeight;

            var cameraDiameter = Mathf.Sqrt(inGameHeight * inGameHeight + inGameWidth * inGameWidth);

            return cameraDiameter;
        }
    }
}

