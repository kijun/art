using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// execute in edit mode
[ExecuteInEditMode]
[SelectionBase]
//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RectProperty : MonoBehaviour {
    static Vector2 TextureMidPoint = TextureMidPoint;

    /*
     * ATTRIBUTES
     */
    public Color color;
    public BorderStyle borderStyle;
    public Color borderColor;
    public float borderThickness;
    public BorderPosition borderPosition;
    public float dashLength;
    public float gapLength;

    // Assume that it is already filled by a prefab
    public MeshFilter innerMeshFilter;
    public MeshRenderer innerMeshRenderer;
    public MeshFilter borderMeshFilter;
    public MeshRenderer borderMeshRenderer;

    /*
     * SETUP
     */
    /*
    void Start() {
        CreateChildObjects();
    }

    void CreateChildObjects() {
        if (innerMeshFilter != null) return;

        var inner = new GameObject();
        innerMeshFilter = inner.AddComponent<MeshFilter>();
        innerMeshRenderer = inner.AddComponent<MeshRenderer>();
        innerMeshRenderer.material = Material.
        inner.transform.parent = transform;

        var border = new GameObject();
        borderMeshFilter = border.AddComponent<MeshFilter>();
        borderMeshRenderer = border.AddComponent<MeshRenderer>();
        border.transform.parent = transform;
    }
    */

    /*
     * RENDERING
     */
    public void Render() {
        switch (borderStyle) {
            case BorderStyle.Dash:
                RenderBorderDashed();
                break;
            case BorderStyle.Solid:
                RenderBorderSolid();
                break;
            default:
                break;
        }
        RenderInner();
    }

    void RenderInner() {
        Color32 color32 = Color.white;
        using (var vh = new VertexHelper()) {
            vh.AddVert(new Vector3(-0.5f, -0.5f), color32, TextureMidPoint);
            vh.AddVert(new Vector3(-0.5f, 0.5f), color32, TextureMidPoint);
            vh.AddVert(new Vector3(0.5f, -0.5f), color32, TextureMidPoint);
            vh.AddVert(new Vector3(0.5f, 0.5f), color32, TextureMidPoint);
            vh.AddTriangle(0,1,2);
            vh.AddTriangle(2,1,3);
            vh.FillMesh(innerMeshFilter.mesh);
        }
        innerMeshRenderer.material.color = color;
    }

    void RenderBorderSolid() {
        var borderOuterBounds = new Bounds(Vector3.zero, new Vector3(1,1,0)); // Original bound
        var borderFrame = new Vector2(scaledBorderWidth, scaledBorderHeight);

        // adjust for border position
        if (borderPosition == BorderPosition.Center) {
            borderOuterBounds.Expand(borderFrame/2f);
        } else if (borderPosition == BorderPosition.Outside) {
            borderOuterBounds.Expand(borderFrame);
        }

        // calculate inner bounds from outer bounds
        var borderInnerBounds = borderOuterBounds;
        borderInnerBounds.Expand(-borderFrame);

        // top
        var top = new Bounds().FromPoints(borderOuterBounds.TopLeft(),
                                          borderInnerBounds.TopRight());
        var right = new Bounds().FromPoints(borderOuterBounds.TopRight(),
                                            borderInnerBounds.BottomRight());
        var bottom = new Bounds().FromPoints(borderOuterBounds.BottomRight(),
                                             borderInnerBounds.BottomLeft());
        var left = new Bounds().FromPoints(borderOuterBounds.BottomLeft(),
                                           borderInnerBounds.TopLeft());
        using (var vh = new VertexHelper()) {
            AddRect(top, vh);
            AddRect(right, vh);
            AddRect(bottom, vh);
            AddRect(left, vh);
            vh.FillMesh(borderMeshFilter.mesh);
            borderMeshRenderer.material.color = borderColor;
        }
    }

    void RenderBorderDashed() {
        // TODO: SOLID BORDER
        // scale?
        // add top
        //
        using (var vh = new VertexHelper()) {
            var bounds = Bounds;

            // draw top border
            int numRects = Mathf.CeilToInt(Width / (dashLength + gapLength));
            Vector3 leftAnchor = new Vector2(-0.5f, 0.5f);

            Debug.Log("Creating " + numRects + " for top");
            for (int i = 0; i<numRects; i++) {
                float displacement = ((dashLength + gapLength) * i) / Width;
                AddRect(leftAnchor.IncrX(displacement), dashLength, borderThickness, vh);
            }

            // draw bot
            vh.FillMesh(borderMeshFilter.mesh);
        }
        borderMeshRenderer.material.color = borderColor;
    }

    void AddRect(Bounds b, VertexHelper vh) {
        int vertIdx = vh.currentVertCount;
        vh.AddVert(b.BottomLeft(), Color.white, TextureMidPoint);
        vh.AddVert(b.TopLeft(), Color.white, TextureMidPoint);
        vh.AddVert(b.BottomRight(), Color.white, TextureMidPoint);
        vh.AddVert(b.TopRight(), Color.white, TextureMidPoint);
        vh.AddTriangle(vertIdx, vertIdx+1, vertIdx+2);
        vh.AddTriangle(vertIdx+2, vertIdx+1, vertIdx+3);
    }

    // left = left middle
    // TODO refactor
    void AddRect(Vector2 leftAnchor, float rectWidth, float rectHeight, VertexHelper vh) {
        int vertIdx = vh.currentVertCount;
        float localWidth = rectWidth/Width;
        float localHeight = rectHeight/Height;
        vh.AddVert(new Vector3(leftAnchor.x, leftAnchor.y-localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddVert(new Vector3(leftAnchor.x, leftAnchor.y+localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddVert(new Vector3(leftAnchor.x+localWidth, leftAnchor.y-localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddVert(new Vector3(leftAnchor.x+localWidth, leftAnchor.y+localHeight/2, 0), Color.white, TextureMidPoint);
        vh.AddTriangle(vertIdx, vertIdx+1, vertIdx+2);
        vh.AddTriangle(vertIdx+2, vertIdx+1, vertIdx+3);
    }


    /*
     * PROPERTIES
     */
    public float Height {
        get {
            return transform.localScale.y;
        }

        set {
            transform.localScale = transform.localScale.SwapY(value);
        }
    }

    public float Width {
        get {
            return transform.localScale.x;
        }
        set {
            transform.localScale = transform.localScale.SwapX(value);
        }
    }

    public Bounds Bounds {
        get {
            return new Bounds(transform.position, transform.localScale);
        }
    }

    public float Angle {
        get {
            return transform.eulerAngles.z;
        }
    }


    /*
     * ETC
     */
    float scaledBorderWidth {
        get  {
            return borderThickness/Width;
        }
    }

    float scaledBorderHeight {
        get {
            return borderThickness/Height;
        }
    }
}
