using UnityEngine;
using System.Collections;

public enum BorderStyle {
    Solid, Dash
}

/*

public class RectProperty : MonoBehaviour {
}

public class CircleProperty : MonoBehaviour {
}

public class TriangleProperty : MonoBehaviour {

}
*/

// execute in edit mode
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LineProperty : MonoBehaviour {

    /* PROPERTIES */
    public Color _color;
    public BorderStyle _style;
    public float _dashLength = 6;
    public float _gapLength = 6;

    Vector3 angleCache;
    Vector3 scaleCache;

    void Start() {
        CacheTransform();
    }

    void Update() {
        // check if edit mode
        if (angleCache != transform.eulerAngles || scaleCache != transform.localScale) {
            CacheTransform();
            OnPropertyChange();
        }
    }

    void CacheTransform() {
        angleCache = transform.eulerAngles;
        scaleCache = transform.localScale;
    }

    /* RENDERING */
    void Render() {
        var line = new Mesh();
        var vert = new Vector3[4];
        vert[0] = new Vector3(-0.5f, -0.5f, 0);
        vert[1] = new Vector3(-0.5f,  0.5f, 0);
        vert[2] = new Vector3(0.5f,  -0.5f, 0);
        vert[3] = new Vector3(0.5f,   0.5f, 0);
        line.vertices = vert;
        //line.uv = new Vector2[] {vert[0], vert[1], vert[2], vert[3]};
        line.uv = new Vector2[] {Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero};
        line.triangles = new int[] {0, 1, 2, 2, 1, 3};
        /*
        if (_style == BorderStyle.Solid) {
        }
        */
        GetComponent<MeshFilter>().mesh = line;
        GetComponent<MeshRenderer>().material.color = _color;
    }

    public void OnPropertyChange() {
        Debug.Log("on property change");
        Render();
    }

    public void OnValidate() {
        Debug.Log("property changed");
    }
}
