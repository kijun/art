using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellType {
    // lets try to use two args to define all
    Velocity,
    Rotation,
    Scale,
    Color,
    Destruction
}
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

    /* Creation logic */

    public Animatable objectPrefab;
    public Range objectHeight;
    public Range objectWidth;
    public Range objectInitialAngle;
    public Range objectsPerActivation = new Range(1);

    /* */

    public CellDefinition[] cells;

	// Use this for initialization
	void Start () {
        StartCoroutine(Run());
	}

    IEnumerator Run() {
        // create
        yield return new WaitForSeconds(startTime);
        while (endTime < float.Epsilon || Time.time < endTime) {
            var numObj = Mathf.RoundToInt(objPerActivation.RandomValue());
            for (int i = 0; i < numObj; i++) {
                StartCoroutine(CreateAndMoveObject());
            }
            yield return new WaitForSeconds(timeBetweenActivation.RandomValue());
        }
    }
}
