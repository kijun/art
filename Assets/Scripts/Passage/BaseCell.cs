using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCell : MonoBehaviour {
    public Animatable2 prefab;

    protected float cameraDiameter {
        get {
            var cameraDiameter = Mathf.Sqrt(inGameHeight * inGameHeight + inGameWidth * inGameWidth);
            return cameraDiameter;
        }
    }

    protected float startDistanceFromOrigin(Vector2 scale) {
        return cameraDiameter / 2 + scale.magnitude / 2;
    }

    protected float inGameHeight {
        get {
            return Camera.main.orthographicSize * 2;
        }
    }

    protected float inGameWidth {
        get {
            return (float)Screen.width / (float)Screen.height * inGameHeight;
        }
    }
}


