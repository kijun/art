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
        using (var vh = new VertexHelper()) {
            foreach (Bounds b in BorderSectionBounds()) {
                Debug.Log(b);
                AddRect(b, vh);
            }

            Mesh newMesh = Mesh.Instantiate(borderMeshFilter.sharedMesh);
            vh.FillMesh(newMesh);
            borderMeshFilter.mesh = newMesh;

            Material newMat = Material.Instantiate(borderMeshRenderer.sharedMaterial);
            newMat.color = borderColor;
            borderMeshRenderer.material = newMat;
        }
    }

    void RenderBorderDashed() {
        using (var vh = new VertexHelper()) {
            var sections = BorderSectionBounds();
            var top = sections[0];
            var right = sections[1];
            var bottom = sections[2];
            var left = sections[3];

            // Horizontal
            float scaledDashLength = dashLength / Width;
            float scaledGapLength = gapLength / Width;
            int numRects = Mathf.CeilToInt(top.size.x / (scaledDashLength + scaledGapLength));

            for (int i = 0; i<numRects; i++) {
                float displacement = (scaledDashLength + scaledGapLength) * i;
                Vector2 anchor = top.TopLeft().Incr(displacement, 0);
                var rect = new Bounds().FromPoints(anchor, anchor.Incr(scaledDashLength, -scaledBorderHeight));
                AddRect(rect, vh);

                // BOT
                anchor = bottom.TopLeft().Incr(displacement, 0);
                rect = new Bounds().FromPoints(anchor, anchor.Incr(scaledDashLength, -scaledBorderHeight));
                AddRect(rect, vh);
            }

            // Vertical
            scaledDashLength = dashLength / Height;
            scaledGapLength = gapLength / Height;
            numRects = Mathf.CeilToInt(right.size.y / (scaledDashLength + scaledGapLength));

            for (int i = 0; i<numRects; i++) {
                float displacement = (scaledDashLength + scaledGapLength) * i;
                Vector2 anchor = right.TopLeft().Incr(0, -displacement);
                var rect = new Bounds().FromPoints(anchor, anchor.Incr(scaledBorderWidth, -scaledDashLength));
                AddRect(rect, vh);

                // BOT
                anchor = left.TopLeft().Incr(0, -displacement);
                rect = new Bounds().FromPoints(anchor, anchor.Incr(scaledBorderWidth, -scaledDashLength));
                AddRect(rect, vh);
            }

            // draw bot
            vh.FillMesh(borderMeshFilter.mesh);
        }
        borderMeshRenderer.material.color = borderColor;
    }

    Bounds[] BorderSectionBounds() {
        var borderOuterBounds = new Bounds(Vector3.zero, new Vector3(1,1,0)); // Original bound
        Debug.Log("1" + borderOuterBounds);
        var borderFrame = new Vector2(scaledBorderWidth, scaledBorderHeight);

        Debug.Log("2" + borderFrame);

        // adjust for border position
        if (borderPosition == BorderPosition.Center) {
            borderOuterBounds.Expand(borderFrame);
        } else if (borderPosition == BorderPosition.Outside) {
            borderOuterBounds.Expand(borderFrame*2f);
        }
        Debug.Log("3" + borderOuterBounds);

        // calculate inner bounds from outer bounds
        var borderInnerBounds = borderOuterBounds;
        borderInnerBounds.Expand(-2f*borderFrame);

        Debug.Log("inner bound= " + borderInnerBounds + "outer bounds= " + borderOuterBounds);

        // top
        var top = new Bounds().FromPoints(borderOuterBounds.TopLeft(),
                                          borderInnerBounds.TopRight());
        var right = new Bounds().FromPoints(borderOuterBounds.TopRight(),
                                            borderInnerBounds.BottomRight());
        var bottom = new Bounds().FromPoints(borderOuterBounds.BottomRight(),
                                             borderInnerBounds.BottomLeft());
        var left = new Bounds().FromPoints(borderOuterBounds.BottomLeft(),
                                           borderInnerBounds.TopLeft());

        return new []{top, right, bottom, left};
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
