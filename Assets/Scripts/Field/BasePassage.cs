using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellType {
    None = 0,
    // lets try to use two args to define all
    VelocityEvent = 10,
    VelocityForce = 11,
    Rotation = 20,
    Scale = 30,
    Color = 40,
    Centripetal = 50,
    Destruction = 90
}

[System.Serializable]
public struct CellDefinition {
    public float startTime;
    public float endTime;
    public CellType type;

    public Range arg1;
    public Range arg2;

    public Range spread1;
    public Range spread2;
}

public class BasePassage : MonoBehaviour {

    /* Activation logic */
    public float startTime;
    public float endTime;
    public Range timeBetweenActivation;
    // start location?

    /* Creation logic */
    public Animatable2 objectPrefab;
    public Range objectHeight;
    public Range objectWidth;
    public Range objectInitialAngle;
    public Range objectsPerActivation = new Range(1);
    public Range objectEntryAngle;
    public Vector2 objectPositionRange;

    /* Passage Cells */
    public CellDefinition[] cells;

	// Use this for initialization
	void Start () {
        StartCoroutine(Run());
	}

    IEnumerator Run() {
        // create
        yield return new WaitForSeconds(startTime);
        while (endTime < float.Epsilon || Time.time < endTime) {
            var numObj = Mathf.RoundToInt(objectsPerActivation.RandomValue());
            StartCoroutine(CreateAndMoveObjects(numObj));
            yield return new WaitForSeconds(timeBetweenActivation.RandomValue());
        }
    }

    IEnumerator CreateAndMoveObjects(int count) {
        var objects = new Animatable2[count];

        // Create Objects
        for (int i = 0; i<count; i++) {
            var width = objectWidth.RandomValue();
            var height = objectHeight.RandomValue();
            var entryAngle = objectEntryAngle.RandomValue();
            var distanceFromOrigin = cameraDiameter / 2 + Mathf.Sqrt(width * width + height * height) / 2;
            var objectPos =
                Quaternion.Euler(0, 0, entryAngle) *
                (Vector2.up * distanceFromOrigin +
                 new Vector2(objectPositionRange.x * Random.value - objectPositionRange.x/2,
                             objectPositionRange.y * Random.value - objectPositionRange.y/2));

            var rotation = Quaternion.Euler(0, 0, entryAngle);
            var target = GameObject.Instantiate<Animatable2>(objectPrefab, objectPos, rotation);
            target.localScale = new Vector2(width, height);
            objects[i] = target;
        }


        foreach (var cell in cells) {
            StartCoroutine(RunCell(cell, objects));
        }

        yield return true;
    }

    IEnumerator RunCell(CellDefinition cell, Animatable2[] objects) {
        yield return new WaitForSeconds(cell.startTime);

        var arg1 = cell.arg1.RandomValue();
        var arg2 = cell.arg2.RandomValue();
        var vec = new Vector2(arg1, arg2);
        var duration = cell.endTime < float.Epsilon ? float.PositiveInfinity : cell.endTime - cell.startTime;

        // probably multiple methods
        foreach (var obj in objects) {
            switch (cell.type) {
                case CellType.VelocityEvent:
                    obj.velocity = vec;
                    break;

                case CellType.VelocityForce:
                    StartCoroutine(AddForce(obj, vec, duration));
                    break;

                case CellType.Rotation:
                    obj.angularVelocity = cell.arg1.RandomValue();
                    break;
                case CellType.Scale:
                    break;

                case CellType.Centripetal:
                    StartCoroutine(Centripetal(obj, arg1, duration));
                    break;
            }
        }
        /*
        var c = target.GetComponent<SpriteRenderer>().material.color;
        c.a = Random.Range(0.3f, 0.8f);
        target.GetComponent<SpriteRenderer>().material.color = c;

        */
    }

    IEnumerator AddForce(Animatable2 obj, Vector2 force, float duration) {
        float end = Time.time + duration;
        while (Time.time < end) {
            obj.rigidbody2D.AddForce(force);
            yield return null;
        }
    }

    IEnumerator Centripetal(Animatable2 obj, float forceMagnitude, float duration) {
        float end = Time.time + duration;
        while (Time.time < end) {
            var velocity = obj.rigidbody2D.velocity;
            var forceDirection = Quaternion.Euler(0, 0, 90) * velocity * forceMagnitude;
            obj.rigidbody2D.AddForce(forceDirection);
            yield return null;
        }
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
