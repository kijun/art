using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RectRenderer : ShapeRenderer {
    static Vector2 TextureMidPoint = TextureMidPoint;

    public RectProperty property = new RectProperty();
    RectProperty cachedProperty = new RectProperty();

    // Prefab should assign these to child gameobjects
    public MeshFilter innerMeshFilter;
    public MeshRenderer innerMeshRenderer;
    public MeshFilter borderMeshFilter;
    public MeshRenderer borderMeshRenderer;

    /* ShapeRenderer */

    protected override void UpdateGameObject() {
        center = property.center;
        diameter = property.diameter;
    }

    protected override void UpdateMeshIfNeeded() {
        if ((Mathf.Abs(property.diameter - innerMeshDiameter) / innerMeshDiameter) >
                INNER_MESH_RETAIN_THRESHOLD) {
            // mesh dimension change
            UpdateInnerMesh();
            UpdateBorderMesh();
        } else if (property.border.MeshNeedsUpdate(cachedProperty.border)) {
            // border property change
            UpdateBorderMesh();
        }

        // color change
        if (property.color != cachedProperty.color) {
            UpdateInnerMeshColor(property.color);
        }

        if (property.border.style != BorderStyle.None &&
            property.border.color != cachedProperty.border.color) {
            UpdateBorderMeshColor(property.border.color);
        }

    }

    protected override bool GameObjectWasModified() {
        // TODO
        return false;
    }

    protected override ShapeProperty GameObjectToShapeProperty() {
        // TODO
        return null;
    }

    /*
     * Create Mesh
     */

    void UpdateInnerMesh() {
        CreateInner();
        innerMeshDiameter = property.diameter;
    }

    void UpdateBorderMesh() {
        switch (property.border.style) {
            case BorderStyle.Solid:
                CreateBorderSolid();
                break;
            case BorderStyle.Dash:
                // TODO
                CreateBorderSolid();
                break;
            case BorderStyle.None:
                RemoveBorder();
                break;
        }
    }

    void UpdateInnerMeshColor(Color color) {
        MeshUtil.UpdateColor(innerMeshRenderer, color);
    }

    void UpdateBorderMeshColor(Color color) {
        MeshUtil.UpdateColor(borderMeshRenderer, color);
    }


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
            case BorderStyle.None:
                RenderBorderNone();
                break;
            default:
                break;
        }
        RenderInner();
        DefaultShapeStyle.SetDefaultRectStyle(this);
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
            MeshUtil.UpdateMesh(innerMeshFilter, vh);
            MeshUtil.UpdateColor(innerMeshRenderer, color);
        }
    }

    void RenderBorderNone() {
        using (var vh = new VertexHelper()) {
            MeshUtil.UpdateMesh(borderMeshFilter, vh);
        }
    }

    void RenderBorderSolid() {
        using (var vh = new VertexHelper()) {
            foreach (Bounds b in BorderSectionBounds()) {
                MeshUtil.AddRect(b, vh);
            }

            MeshUtil.UpdateMesh(borderMeshFilter, vh);
            MeshUtil.UpdateColor(borderMeshRenderer, borderColor);
        }
    }

    void RenderBorderDashed() {
        using (var vh = new VertexHelper()) {
            var sections = BorderSectionBounds();
            var top = sections[0];
            var right = sections[1];
            var bottom = sections[2];
            var left = sections[3];
            var outerBounds = BorderOuterBounds;

            // Horizontal
            float scaledDashLength = dashLength / Width;
            float scaledGapLength = gapLength / Width;
            int numRects = Mathf.CeilToInt(top.size.x / (scaledDashLength + scaledGapLength));

            for (int i = 0; i<numRects; i++) {
                float displacement = (scaledDashLength + scaledGapLength) * i;
                Vector2 anchor = top.TopLeft().Incr(displacement, 0);
                var rect = new Bounds().FromPoints(
                        anchor,
                        outerBounds.ClosestPoint(anchor.Incr(scaledDashLength, -scaledBorderHeight)));
                MeshUtil.AddRect(rect, vh);

                // BOT
                anchor = bottom.TopLeft().Incr(displacement, 0);
                rect = new Bounds().FromPoints(
                        anchor,
                        outerBounds.ClosestPoint(anchor.Incr(scaledDashLength, -scaledBorderHeight)));
                MeshUtil.AddRect(rect, vh);
            }

            // Vertical
            scaledDashLength = dashLength / Height;
            scaledGapLength = gapLength / Height;
            numRects = Mathf.CeilToInt(right.size.y / (scaledDashLength + scaledGapLength));

            for (int i = 0; i<numRects; i++) {
                float displacement = (scaledDashLength + scaledGapLength) * i;
                Vector2 anchor = right.TopLeft().Incr(0, -displacement);
                var rect = new Bounds().FromPoints(
                        anchor,
                        outerBounds.ClosestPoint(anchor.Incr(scaledBorderWidth, -scaledDashLength)));
                MeshUtil.AddRect(rect, vh);

                // BOT
                anchor = left.TopLeft().Incr(0, -displacement);
                rect = new Bounds().FromPoints(
                        anchor,
                        outerBounds.ClosestPoint(anchor.Incr(scaledBorderWidth, -scaledDashLength)));
                MeshUtil.AddRect(rect, vh);
            }

            // draw bot
            MeshUtil.UpdateMesh(borderMeshFilter, vh);
            MeshUtil.UpdateColor(borderMeshRenderer, borderColor);
        }
    }

    Bounds BorderOuterBounds {
        get {
            var borderOuterBounds = new Bounds(Vector3.zero, new Vector3(1,1,0)); // Original bound
            var borderFrame = new Vector2(scaledBorderWidth, scaledBorderHeight);


            // adjust for border position
            if (borderPosition == BorderPosition.Center) {
                borderOuterBounds.Expand(borderFrame);
            } else if (borderPosition == BorderPosition.Outside) {
                borderOuterBounds.Expand(borderFrame*2f);
            }

            return borderOuterBounds;
        }
    }

    Bounds[] BorderSectionBounds() {
        var borderOuterBounds = BorderOuterBounds;
        var borderFrame = new Vector2(scaledBorderWidth, scaledBorderHeight);

        // calculate inner bounds from outer bounds
        var borderInnerBounds = borderOuterBounds;
        borderInnerBounds.Expand(-2f*borderFrame);

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

    public void OnUpdate() {
        Render();
    }

    /*
     * PROPERTIES
     */
    public float height {
        get {
            return transform.localScale.y;
        }

        set {
            transform.localScale = transform.localScale.SwapY(value);
        }
    }

    public float width {
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
        set {
            transform.eulerAngles = transform.eulerAngles.SwapZ(value);
        }
    }

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

    Rect rect {
        get {
            return new Rect(transform.position, transform.localScale);
        }
    }

    Vector2 right {
        get {
            return Vector2.zero;
        }
    }

    public Vector2 Size {
        get {
            return new Vector2(Width, Height);
        }

        set {
            transform.localScale = transform.localScale.SwapX(value.x).SwapY(value.y);
        }
    }

    public Vector2 Center {
        get {
            return transform.position;
        }

        set {
            transform.position = value;
        }
    }

    public Rect2 rect2 {
        get {
            return new Rect2(Center, Size, Angle);
        }

        set {
            Center = value.center;
            Size = value.size;
            Angle = value.angle;
        }
    }

    /*
    void SetWidth(float width, RectAnchor anchor = RectAnchor.Center) {
        switch (anchor) {
            case RectAnchor.Left:
                break;
            case RectAnchor.Right:
                break;
            default:
                Width = width;
                break;
        }
    }
    */

    //
    // mola
    //

}

