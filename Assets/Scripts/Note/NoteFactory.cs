using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteFactory : MonoBehaviour {
    /*
     * Interfaces with animatables
     */
    static Animatable2 _prefab;

    public static Animatable2 CreateLine(LineParams2 lp, MotionParams mp) {
        var p = CreatePlane(lp.scale, lp.rotation);
        p.localScale = lp.scale;
        p.position = lp.position;
        // must come after position
        p.level = lp.level;
        p.velocity = mp.velocity;
        p.color = lp.color;
        return p;
    }

    public static Animatable2 CreateRectInViewport(float x, float y, float width, float height, Color color, float rotation=0, float level=0, Vector2 velocity=new Vector2()) {
        var position = CameraHelper.ViewportToWorldPoint(x, y);
        var scale = CameraHelper.ViewportToWorldScale(width, height);
        var lp = new LineParams2 {
            position = position,
            scale = scale,
            color = color,
            rotation = rotation,
            level = level
        };

        var mp = new MotionParams {
            velocity = velocity
        };

        return CreateLine(lp, mp);
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
