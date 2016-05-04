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
                RenderSolid();
                break;
            case BorderStyle.Dash:
                if (dashLength == 0) {
                    RenderSolid();
                } else {
                    RenderDash();
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
            tris[3*(i-1)+1] = i+1;
            tris[3*(i-1)+2] = i;
        }

        // reset to first triangle
        tris[tris.Length - 2] = 1;

        line.vertices = verts;
        line.uv = uvs;
        line.triangles = tris;

        Debug.Log(verts.Length + " " + verts[0] + verts[1] + verts[2]);

        Debug.Log(line.vertices);
        Debug.Log(line.uv);
        Debug.Log(line.triangles);

        GetComponent<MeshFilter>().mesh = line;
    }

    void RenderSolid() {
    }

    void RenderDash() {
    }

    /* */

}

