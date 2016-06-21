using UnityEngine;
using UnityEngine.UI;

// execute in edit mode
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LineRenderer : ShapeRenderer {

    const int MAX_FRAGMENTS = 10000;

    public LineProperty property = new LineProperty();
    LineProperty cachedProperty = new LineProperty();

    /* ShapeRenderer */

    protected override void UpdateGameObject() {
        center = property.center;
        length = property.length;
        width = property.width;
        angle = property.angle;
    }

    protected override void UpdateMeshIfNeeded() {
        if (property.border.MeshNeedsUpdate(cachedProperty.border)) {
            // border property change
            UpdateMesh();
        } else if (property.border.style == BorderStyle.Dash &&
                   !Mathf.Approximately(property.length, cachedProperty.length)) {
            UpdateMesh();
        }

        if (property.color != cachedProperty.color) {
            UpdateMeshColor(property.color);
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

    protected override void CacheProperty() {
        cachedProperty = property;
        propertyObjectChanged = false;
    }

    /*
     * Mesh
     */

    void UpdateMesh() {
        if (property.border.style == BorderStyle.None ||
            property.border.style == BorderStyle.Solid ||
            Mathf.Approximately(property.border.gapLength, 0)) {
            CreateSolid();
        } else if (property.border.style == BorderStyle.Dash) {
            CreateDashed();
        }
    }

    void UpdateMeshColor(Color color) {
        MeshUtil.UpdateColor(GetComponent<MeshRenderer>(), color);
    }

    void CreateSolid() {
        using (var vh = new VertexHelper()) {
            MeshUtil.AddRect(BoundsUtil.UnitBounds, vh);
            MeshUtil.UpdateMesh(GetComponent<MeshFilter>(), vh);
        }
    }

    void CreateDashed() {
        float fullSegmentLength = (property.border.dashLength + property.border.gapLength);
        int numFragments = Mathf.CeilToInt(property.length/fullSegmentLength);
        if (numFragments > MAX_FRAGMENTS) {
            Debug.LogError("Line has too many fragments (" + numFragments + ")");
            return;
        }

        using (var vh = new VertexHelper()) {
            for (int i=0; i<numFragments; i++) {
                float deltaX = i * fullSegmentLength/property.length;
                var min = new Vector2(-0.5f + deltaX, -0.5f);
                var max = new Vector2(-0.5f + deltaX + property.border.dashLength/property.length, 0.5f);
                MeshUtil.AddRect(new Bounds().WithMinMax(min, max), vh);
            }
            MeshUtil.UpdateMesh(GetComponent<MeshFilter>(), vh);
        }
    }

    /*
     * Properties
     */
    public Vector2 center {
        get { return transform.position; }
        set { transform.position = new Vector3(value.x, value.y, transform.position.z); }
    }

    public float length {
        get { return transform.localScale.x; }
        set { transform.localScale = new Vector3(value, width, 1); }
    }

    public float width {
        get { return transform.localScale.y; }
        set { transform.localScale = new Vector3(length, value, 1); }
    }

    public float angle {
        get { return transform.eulerAngles.z; }
        set { transform.eulerAngles = transform.eulerAngles.SwapZ(value); }
    }

    void SetEndPoints(Vector2 p1, Vector2 p2) {
        property.points = new Tuple<Vector2, Vector2>(p1, p2);
    }
}

