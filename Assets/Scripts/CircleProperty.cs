using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class CircleProperty : MonoBehaviour, IObjectProperty {

    const float MAX_FRAGMENT_LENGTH = 0.03f; // make it small enough to be invisible

    // basic circle property
    public Color color;

    // border
    public BorderStyle borderStyle;
    public BorderPosition borderPosition;
    public Color borderColor;
    public float borderWidth;
    public float dashLength;
    public float gapLength;

    // Prefab should assign these to child gameobjects
    public MeshFilter innerMeshFilter;
    public MeshRenderer innerMeshRenderer;
    public MeshFilter borderMeshFilter;
    public MeshRenderer borderMeshRenderer;

    public float diameter {
        get {
            return transform.localScale.x;
        }

        set {
            transform.localScale = new Vector3(value, value, 1);
        }
    }

    public void OnUpdate() {
        Render();
        DefaultShapeStyle.SetDefaultCircleStyle(this);
    }

    /* RENDERING */

    void Render() {
        // inner
        RenderBorderless();
        switch (borderStyle) {
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
            default:
                break;
            /* case BorderStyle.None:
                RenderBorderless();
                break;
                */
        }
        //GetComponent<MeshRenderer>().sharedMaterial.color = color;
    }

    void RenderBorderless() {
        int numTris = Mathf.CeilToInt(diameter * Mathf.PI / MAX_FRAGMENT_LENGTH);
        float centerAngle = 2*Mathf.PI/numTris;
        Color c = Color.white;

        using (var vh = new VertexHelper()) {
            // create verticies
            vh.AddVert(Vector3.zero, c, Vector2.zero); // midpoint
            for (int i = 0; i < numTris; i++) {
                float angle = centerAngle * i;
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);
                vh.AddVert(new Vector3(x, y, 0), c, Vector2.zero);
            }

            // create triangles
            for (int i = 0; i < numTris-1; i++) {
                vh.AddTriangle(0, i+2, i+1);
            }
            // come around
            vh.AddTriangle(0, 1, numTris);
            MeshUtil.UpdateMesh(innerMeshFilter, vh);
            MeshUtil.UpdateColor(innerMeshRenderer, color);
        }
    }

    /*
    void RenderBorderSolid() {
    }
    */

    void RenderBorderSolid() {
        int numTris = 200;
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
            uvs[innerIdx] = new Vector2(0.5f, 0);
            uvs[outerIdx] = new Vector2(1.5f, 0);

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

        // TODO this code....
        tris[tris.Length-8] = 1;
        tris[tris.Length-6] = 1;
        tris[tris.Length-5] = 2;
        tris[tris.Length-2] = 1;

        line.vertices = verts;
        line.uv = uvs;
        line.triangles = tris;

        GetComponent<MeshFilter>().mesh = line;

        var tex = new Texture2D(2,1, TextureFormat.ARGB32, false);
        tex.SetPixel(0, 0, color);
        tex.SetPixel(1, 0, borderColor);
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        GetComponent<Renderer>().sharedMaterial.mainTexture = tex;
    }

    void RenderBorderDash() {

    }

    /* */

}

