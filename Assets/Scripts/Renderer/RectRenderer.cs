using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RectRenderer : ShapeRenderer {


    /***** STATIC: READONLY *****/
    static readonly Vector2 TEXTURE_MID_POINT = new Vector2(0.5f, 0.5f);


    /***** PUBLIC: VARIABLES *****/
    // should be assigned
    public MeshFilter innerMeshFilter;
    public MeshRenderer innerMeshRenderer;
    public MeshFilter borderMeshFilter;
    public MeshRenderer borderMeshRenderer;


    /***** PRIVATE: VARIABLES *****/
    [SerializeField]
    RectProperty _property = new RectProperty(color:Color.black);
    [SerializeField]
    RectProperty cachedProperty = new RectProperty(color:Color.black, width:float.Epsilon);


    /***** OVERRIDE: SHAPE RENDERER *****/
    protected override void UpdateGameObject() {
        center = property.center;
        width = property.width;
        height = property.height;
        angle = property.angle;
    }

    protected override void UpdateMeshIfNeeded() {
        if (!HasInnerMesh()) {
            UpdateInnerMesh();
        }

        // TODO: epsilon
        if (property.border.style != BorderStyle.None &&
            (property.width != cachedProperty.width ||
             property.height != cachedProperty.height)) {
            UpdateBorderMesh();
        } else if (property.border.MeshNeedsUpdate(cachedProperty.border)) {
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
        /* Return true if GameObject was modified in Editor Mode.
         * Only the transform is modifiable. */

        return GameObjectToShapeProperty() != property;
    }

    protected override ShapeProperty GameObjectToShapeProperty() {
        var goProperty = property.Clone() as RectProperty;
        goProperty.center = center;
        goProperty.angle = angle;
        goProperty.width = width;
        goProperty.height = height;
        return goProperty;
    }

    protected override void SetProperty(ShapeProperty newProperty) {
        property = newProperty as RectProperty;
    }

    protected override void CacheProperty() {
        cachedProperty = property;
        propertyObjectChanged = false;
    }


    /***** PRIVATE: MESH UPDATE *****/
    void UpdateInnerMesh() {
        CreateInner();
    }

    void UpdateBorderMesh() {
        switch (property.border.style) {
            case BorderStyle.Dash:
                CreateBorderDashed();
                break;
            case BorderStyle.Solid:
                CreateBorderSolid();
                break;
            case BorderStyle.None:
                RemoveBorder();
                break;
            default:
                break;
        }
    }

    void UpdateInnerMeshColor(Color color) {
        MeshUtil.UpdateColor(innerMeshRenderer, color);
    }

    void UpdateBorderMeshColor(Color color) {
        MeshUtil.UpdateColor(borderMeshRenderer, color);
    }

    void RemoveBorder() {
        // TODO just disable
        using (var vh = new VertexHelper()) {
            MeshUtil.UpdateMesh(borderMeshFilter, vh);
        }
    }


    /***** PRIVATE: MESH CREATION *****/
    void CreateInner() {
        Color32 color32 = Color.white;
        using (var vh = new VertexHelper()) {
            vh.AddVert(new Vector3(-0.5f, -0.5f), color32, TEXTURE_MID_POINT);
            vh.AddVert(new Vector3(-0.5f, 0.5f), color32, TEXTURE_MID_POINT);
            vh.AddVert(new Vector3(0.5f, -0.5f), color32, TEXTURE_MID_POINT);
            vh.AddVert(new Vector3(0.5f, 0.5f), color32, TEXTURE_MID_POINT);
            vh.AddTriangle(0,1,2);
            vh.AddTriangle(2,1,3);
            MeshUtil.UpdateMesh(innerMeshFilter, vh);
        }
    }

    void CreateBorderSolid() {
        using (var vh = new VertexHelper()) {
            foreach (Bounds b in BorderSectionBounds()) {
                MeshUtil.AddRect(b, vh);
            }

            MeshUtil.UpdateMesh(borderMeshFilter, vh);
        }
    }

    void CreateBorderDashed() {
        using (var vh = new VertexHelper()) {
            var sections = BorderSectionBounds();
            var top = sections[0];
            var right = sections[1];
            var bottom = sections[2];
            var left = sections[3];
            var outerBounds = BorderOuterBounds;

            // Horizontal
            float scaledDashLength = property.border.dashLength / property.width;
            float scaledGapLength = property.border.gapLength / property.width;
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
            scaledDashLength = property.border.dashLength / property.height;
            scaledGapLength = property.border.gapLength / property.height;
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
        }
    }


    /****** PRIVATE: MESH HELPERS *****/
    bool HasInnerMesh() {
        var innerMesh = innerMeshFilter.mesh;
        if (innerMesh == null || innerMesh.vertices.Length == 0) {
            return false;
        }
        return true;
    }

    Bounds BorderOuterBounds {
        get {
            var borderOuterBounds = new Bounds(Vector3.zero, new Vector3(1,1,0)); // Original bound
            var borderFrame = new Vector2(scaledBorderWidth, scaledBorderHeight);


            // adjust for border position
            if (property.border.position == BorderPosition.Center) {
                borderOuterBounds.Expand(borderFrame);
            } else if (property.border.position == BorderPosition.Outside) {
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


    /***** PUBLIC: PROPERTIES *****/
    public RectProperty property {
        get {
            return _property;
        }
        set {
            propertyObjectChanged = true;
            _property = value;
        }
    }


    /***** PRIVATE: PROPERTIES *****/
     Vector2 center {
        get { return transform.position; }
        set { transform.position = new Vector3(value.x, value.y, transform.position.z); }
    }

    float height {
        get {
            return transform.localScale.y;
        }

        set {
            transform.localScale = transform.localScale.SwapY(value);
        }
    }

    float width {
        get {
            return transform.localScale.x;
        }
        set {
            transform.localScale = transform.localScale.SwapX(value);
        }
    }

    float angle {
        get {
            return transform.eulerAngles.z;
        }
        set {
            transform.eulerAngles = transform.eulerAngles.SwapZ(value);
        }
    }

    Bounds bounds {
        get {
            return new Bounds(transform.position, transform.localScale);
        }
    }

    float scaledBorderWidth {
        get  {
            return property.border.thickness/property.width;
        }
    }

    float scaledBorderHeight {
        get {
            return property.border.thickness/height;
        }
    }

    Rect rect {
        get {
            return new Rect(transform.position, transform.localScale);
        }
    }

    /*
    Vector2 right {
        get {
            return Vector2.zero;
        }
    }
    */
}

