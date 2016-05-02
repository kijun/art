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

public class LineProperty : MonoBehaviour {

    /* PROPERTIES */
    float _width = 1f; // 1pixel
    public Color _color;
    public BorderStyle _style;
    public float _dashLength;
    public float _gapLength;

    /* RENDERING */
    void Render() {
        var line = new Mesh();
        if (_style == BorderStyle.Solid) {
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(0,0,0);
            vertices[1] = new Vector3(0,1,0);
            vertices[2] = new Vector3(1,0,0);
            vertices[3] = new Vector3(1,1,0);
            line.vertices = vertices;
        }
    }

    /* */
}
