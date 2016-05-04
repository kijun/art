using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircleProperty : MonoBehaviour {

    // basic circle property
    public Color color;

    // border
    public BorderStyle style;
    public Color borderColor;
    public float borderWidth;
    public float dashLength;
    public float gapLength;

    public float diameter {
        get {
            return transform.localScale.x * 100f;
        }

        set {
            transform.localScale = new Vector3(value/100f, value/100f, 1);
        }
    }

    public void OnPropertyChange() {
        Render();
    }


    /* RENDERING */

    void Render() {
        switch (style) {
            case BorderStyle.None:
                RenderBorderless();
                break;
            case BorderStyle.Solid:
                RenderBorderSolid();
                break;
            case BorderStyle.Dash:
                if (dashLength == 0) {
                    RenderBorderSolid();
                } else {
                    RenderBorderDash();
                }
                break;
        }
        GetComponent<MeshRenderer>().sharedMaterial.color = color;
    }

    void RenderBorderless() {
        int numTris = 40;
        float centerAngle = 2*Mathf.PI/numTris;

        var line = new Mesh();
        var verts = new Vector3[numTris+1]; // center + one for each
        var uvs = new Vector2[verts.Length];
        var tris = new int[numTris * 3];

        verts[0] = Vector3.zero;
        uvs[0] = Vector2.zero;

        for (int i = 1; i<verts.Length; i++) {
            float angle = centerAngle * (i-1);
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            verts[i] = new Vector3(x, y, 0);
            Debug.Log("vert " + i + " " + verts[i]);
            uvs[i] = Vector2.zero;
            tris[3*(i-1)] = 0;
            tris[3*(i-1)+1] = i+1; // wrap around
            tris[3*(i-1)+2] = i;
        }

        tris[tris.Length-2] = 1;

        line.vertices = verts;
        line.uv = uvs;
        line.triangles = tris;

        GetComponent<MeshFilter>().mesh = line;
    }

    void RenderBorderSolid() {
        int numTris = 40;
        float centerAngle = 2*Mathf.PI/numTris;

        var line = new Mesh();
        var verts = new Vector3[numTris*2+1];
        var uvs = new Vector2[verts.Length];
        var tris = new int[numTris*3*3]; // 1 for the inner circle, 2 for outer border

        verts[0] = Vector3.zero;
        uvs[0] = Vector2.zero;

        float innerCircleRatio = 1 - borderWidth/diameter;

        for (int i = 0; i<numTris; i++) {
            float angle = centerAngle * i;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            int innerIdx = 2*i + 1;
            int outerIdx = 2*i + 2;

            // inner
            verts[innerIdx] = new Vector3(x*innerCircleRatio, y*innerCircleRatio, 0);
            // outer
            verts[outerIdx] = new Vector3(x, y, 0);

            // TODO adjust this after generating a new texture
            uvs[innerIdx] = Vector2.zero;
            uvs[outerIdx] = Vector2.one;

            // 3 4
            // 1 2
            // We need to define 3 triangles, one for the inner circle, two for the outer rim
            // Triangle 1, 0, i, i+1
            tris[9*i] = 0;
            tris[9*i+1] = innerIdx+2;
            tris[9*i+2] = innerIdx;
            tris[9*i+3] = innerIdx+2;
            tris[9*i+4] = outerIdx+2;
            tris[9*i+5] = outerIdx;
            tris[9*i+6] = innerIdx;
            tris[9*i+7] = innerIdx+2;
            tris[9*i+8] = outerIdx;
        }

        tris[tris.Length-8] = 1;
        tris[tris.Length-6] = 1;
        tris[tris.Length-5] = 2;
        tris[tris.Length-2] = 1;

        line.vertices = verts;
        line.uv = uvs;
        line.triangles = tris;

        GetComponent<MeshFilter>().mesh = line;
    }

    void RenderBorderDash() {
    }

    /* RENDERING HELPER */
    void AddTriangle() {
    }

    void AddRectangle() {
    }

    /* */

}

