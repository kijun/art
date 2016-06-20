using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[SelectionBase]
public class CircleRenderer : ShapeRenderer {

    const float MAX_FRAGMENT_LENGTH = 0.03f; // make it small enough to be invisible

    public CircleProperty property;
    // TODO used to check dirty, should really belong to lineproperty
    CircleProperty cachedProperty;

    // Prefab should assign these to child gameobjects
    public MeshFilter innerMeshFilter;
    public MeshRenderer innerMeshRenderer;
    public MeshFilter borderMeshFilter;
    public MeshRenderer borderMeshRenderer;

    public Vector2 center {
        get {
            return transform.position;
        }

        set {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }

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
        DefaultShapeStyle.SetDefaultCircleStyle(property);
    }

    /* RENDERING */

    void Render() {
        // inner
        RenderBorderless();
        switch (property.borderStyle) {
            case BorderStyle.Solid:
                RenderBorderSolid();
                break;
            case BorderStyle.Dash:
                RenderBorderSolid();
                break;
            case BorderStyle.None:
                RenderBorderNone();
                break;
            default:
                break;
        }
    }

    void RenderBorderNone() {
        using (var vh = new VertexHelper()) {
            MeshUtil.UpdateMesh(borderMeshFilter, vh);
        }
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
                float x = Mathf.Cos(angle) * 0.5f;
                float y = Mathf.Sin(angle) * 0.5f;
                vh.AddVert(new Vector3(x, y, 0), c, Vector2.zero);
            }

            // create triangles
            for (int i = 0; i < numTris-1; i++) {
                vh.AddTriangle(0, i+2, i+1);
            }
            // come around
            vh.AddTriangle(0, 1, numTris);
            MeshUtil.UpdateMesh(innerMeshFilter, vh);
            MeshUtil.UpdateColor(innerMeshRenderer, property.color);
        }
    }

    void RenderBorderSolid() {
        int numQuads = Mathf.CeilToInt(diameter * Mathf.PI / MAX_FRAGMENT_LENGTH);
        float centerAngle = 2*Mathf.PI/numQuads;

        float scaledBorderThickness = property.borderThickness/property.diameter;
        Color c = Color.white;

        using (var vh = new VertexHelper()) {
            for (int i = 0; i<numQuads; i++) {
                float angle = centerAngle * i;
                float x = Mathf.Cos(angle) * 0.5f;
                float y = Mathf.Sin(angle) * 0.5f;

                float scaledInnerRadius = 1; // default = outer

                switch (property.borderPosition) {
                    case BorderPosition.Center:
                        scaledInnerRadius -= scaledBorderThickness/2;
                        break;

                    case BorderPosition.Inside:
                        scaledInnerRadius -= scaledBorderThickness;
                        break;

                    default:
                        break;
                }

                float scaledOuterRadius = scaledInnerRadius + scaledBorderThickness;

                vh.AddVert(new Vector3(x*scaledInnerRadius, y*scaledInnerRadius, 0),
                           c, Vector2.zero);

                vh.AddVert(new Vector3(x*scaledOuterRadius, y*scaledOuterRadius, 0),
                           c, Vector2.zero);

            }

            for (int i = 0; i<numQuads-1; i++) {
                int idxBase = 2*i; // two new vertices per quad
                vh.AddTriangle(idxBase, idxBase+2, idxBase+1);
                vh.AddTriangle(idxBase+1, idxBase+2, idxBase+3);
            }

            int finalIdxBase = 2*numQuads-2; // last two vertices
            vh.AddTriangle(finalIdxBase, 0, finalIdxBase+1);
            vh.AddTriangle(finalIdxBase+1, 0, 1);

            MeshUtil.UpdateMesh(borderMeshFilter, vh);
            MeshUtil.UpdateColor(borderMeshRenderer, property.borderColor);
        }
    }

    void RenderBorderDash() {

    }
}

