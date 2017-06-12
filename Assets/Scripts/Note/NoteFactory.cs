using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteFactory : MonoBehaviour {
    /*
     * Interfaces with animatables
     */
    static Animatable2 _prefab;

    public static void CreateLine(LineParams2 lp, MotionParams mp) {
        var p = CreatePlane(lp.scale, lp.rotation);
        p.localScale = lp.scale;
        p.position = lp.position;
        // must come after position
        p.level = lp.level;
        p.velocity = mp.velocity;
        p.color = lp.color;
    }

    static Animatable2 CreatePlane(Vector2 localScale, float entryAngle) {
        return GameObject.Instantiate<Animatable2>(prefab, localScale, Quaternion.Euler(0, 0, entryAngle));
    }

    static Animatable2 prefab {
        get {
            if (_prefab == null) {
                _prefab = Resources.Load<Animatable2>("Animatable2");
            }
            return _prefab;
        }
    }

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
