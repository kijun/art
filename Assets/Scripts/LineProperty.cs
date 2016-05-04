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
    public Color color;
    public BorderStyle style;
    public float dashLength = 6;
    public float gapLength = 6;

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
    /* we'll try rendering piecemeal (might be able to construct complicated situations with dashed lines */
    void Render() {
        if (style == BorderStyle.Solid || dashLength == 0) {
            Debug.Log("rendering solid");
            var line = new Mesh();
            var vert = new Vector3[4];
            vert[0] = new Vector3(-0.5f, -0.5f, 0);
            vert[1] = new Vector3(-0.5f,  0.5f, 0);
            vert[2] = new Vector3(0.5f,  -0.5f, 0);
            vert[3] = new Vector3(0.5f,   0.5f, 0);
            line.vertices = vert;
            line.uv = new Vector2[] {Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero};
            line.triangles = new int[] {0, 1, 2, 2, 1, 3};
            GetComponent<MeshFilter>().mesh = line;
        } else if (style == BorderStyle.Dash) {
            RenderDashed();
        }
        GetComponent<MeshRenderer>().sharedMaterial.color = color;
    }

    // how should I render dashed lines?
    // two ways -
    //  creating a texture of certain width and maping uv to an infinite canvas
    //  creating multiple line fragments
    // i think actually multiple fragments should be easier to make
    // okay let's try it, won't take all that long
    void RenderDashed() {
        // fragment 1:
        int numFragments = Mathf.CeilToInt(Length/(dashLength + gapLength));
        if (numFragments < 10000) {
            // we create the final fragment even if it cannot be fully displayed/rendered
            // if length is 100, unit is 1 and one pixel is 1/100
            // if length is 10000, size is 100, and one pixel is 1/1000
            float pixelToUnitScaleRatio = 1 / Length;
            float fragmentLength = (dashLength + gapLength) * pixelToUnitScaleRatio;
            float absDashLength = dashLength * pixelToUnitScaleRatio; // pixel to size
            Debug.Log("frag length " + fragmentLength);
            Debug.Log("abs dash length " + absDashLength);

            var line = new Mesh();
            var verts = new Vector3[numFragments * 4];
            var triangles = new int[numFragments * 6];
            var uv = new Vector2[numFragments*4];

            for (int i=0; i<numFragments; i++) {
                float deltaX = i * fragmentLength;
                int baseVertIdx = 4*i;

                verts[baseVertIdx] = new Vector3(-0.5f+deltaX, -0.5f, 0);
                verts[baseVertIdx+1] = new Vector3(-0.5f+deltaX,  0.5f, 0);
                verts[baseVertIdx+2] = new Vector3(-0.5f+deltaX + absDashLength,  -0.5f, 0);
                verts[baseVertIdx+3] = new Vector3(-0.5f+deltaX + absDashLength,   0.5f, 0);

                // 0,1,2 2,1,3
                triangles[6*i]   = baseVertIdx;
                triangles[6*i+1] = baseVertIdx + 1;
                triangles[6*i+2] = baseVertIdx + 2;

                triangles[6*i+3] = baseVertIdx + 2;
                triangles[6*i+4] = baseVertIdx + 1;
                triangles[6*i+5] = baseVertIdx + 3;

                uv[baseVertIdx] = Vector2.zero;
                uv[baseVertIdx+1] = Vector2.zero;
                uv[baseVertIdx+2] = Vector2.zero;
                uv[baseVertIdx+3] = Vector2.zero;
            }
            line.vertices = verts;
            line.triangles = triangles;
            line.uv = uv;
            GetComponent<MeshFilter>().mesh = line;
        } else {
            Debug.LogError("too many line fragments");
        }
    }

    public float Length {
        get {
            return 100f * transform.localScale.x;
        }
    }

    public void OnPropertyChange() {
        Debug.Log("on property change");
        Render();
    }

    public void OnValidate() {
        Debug.Log("property changed");
    }
}
