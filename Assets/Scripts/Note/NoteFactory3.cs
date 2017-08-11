using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureShape;

public class NoteFactory3 : MonoBehaviour {
    /*
     * Interfaces with animatables
     */
    /*
    static Animatable3 _prefab;

    public static Animatable3 CreateVector(Spline

    public static Animatable3 CreateLine(LineParams2 lp, MotionParams mp=new MotionParams()) {
        return CreateRect(lp.ToRectParams(), mp);
    }

    public static Animatable3 CreateRectInViewport(float x, float y, float width, float height, Color color, float rotation=0, float level=0, Vector2 velocity=new Vector2()) {
        var position = CameraHelper.ViewportToWorldPoint(x, y);
        var scale = CameraHelper.ViewportToWorldScale(width, height);
        var lp = new RectParams {
            position = position,
            scale = scale,
            color = color,
            rotation = rotation,
            level = level
        };

        var mp = new MotionParams {
            velocity = velocity
        };

        return CreateRect(lp, mp);
    }

    public static Animatable3 CreateCircle(CircleProperty cp) {
        var p = ShapeGOFactory.InstantiateCircle(cp);
        return p.gameObject.AddComponent<Animatable3>();
    }

    public static Animatable3 CreateRect(RectParams rp, MotionParams mp=new MotionParams()) {
        var p = CreatePlane(rp.scale, rp.rotation);
        p.localScale = rp.scale;
        p.position = rp.position;
        // must come after position
        p.level = rp.level;
        p.velocity = mp.velocity;
        p.color = rp.color;
        return p;
    }

    static Animatable3 CreatePlane(Vector2 localScale, float entryAngle) {
        return GameObject.Instantiate<Animatable3>(prefab, localScale, Quaternion.Euler(0, 0, entryAngle));
    }

    static Animatable3 prefab {
        get {
            if (_prefab == null) {
                _prefab = Resources.Load<Animatable3>("Animatable3");
            }
            return _prefab;
        }
    }
    */
}

