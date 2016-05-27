using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// execute in edit mode
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LineRenderer : MonoBehaviour, IObjectProperty {

    const int MAX_FRAGMENTS = 10000;

    public LineProperty property = new LineProperty();
    // TODO used to check dirty, should really belong to lineproperty
    LineProperty cachedPropertyBad = new LineProperty();

    void Start() {
    }

    bool PropertyHasChanged() {
        return !property.Equals(cachedPropertyBad);
    }

    bool TransformHasChanged() {
        if (Mathf.Approximately(property.length, TransformLength) ||
            Mathf.Approximately(property.width, TransformWidth) ||
            Mathf.Approximately(property.angle, TransformAngle)) {
            return true;
        }
        return false;
    }


    float TransformLength {
        get {
            return transform.localScale.x;
        }
    }

    float TransformWidth {
        get {
            return transform.localScale.y;
        }
    }

    float TransformAngle {
        get {
            return transform.eulerAngles.z;
        }
    }


    void Update() {
        // TODO check if edit mode
        Debug.Log("updating " + Time.time);
        if (PropertyHasChanged()) {
            Debug.Log("Property changed");
            transform.localScale = new Vector3(property.length, property.width, 1);
            transform.eulerAngles = transform.eulerAngles.SwapZ(property.angle);
            OnUpdate();
            cachedPropertyBad = property;
        } else if (TransformHasChanged()) {
            property.length = TransformLength;
            property.width = TransformWidth;
            property.angle = TransformAngle;
            OnUpdate();
            cachedPropertyBad = property;
        }
    }

    /* RENDERING */
    /* we'll try rendering piecemeal (might be able to construct complicated situations with dashed lines */
    void Render() {
        if (property.style == BorderStyle.None ||
            property.style == BorderStyle.Solid ||
            property.gapLength == 0) {
            RenderSolid();
        } else if (property.style == BorderStyle.Dash) {
            RenderDashed();
        }
    }

    void RenderSolid() {
        using (var vh = new VertexHelper()) {
            MeshUtil.AddRect(BoundsUtil.UnitBounds, vh);
            MeshUtil.UpdateMesh(GetComponent<MeshFilter>(), vh);
            MeshUtil.UpdateColor(GetComponent<MeshRenderer>(), property.color);
        }
    }

    void RenderDashed() {
        int numFragments = Mathf.CeilToInt(property.length/(property.dashLength + property.gapLength));
        if (numFragments > MAX_FRAGMENTS) {
            Debug.LogError("Line has too many fragments (" + numFragments + ")");
            return;
        }

        using (var vh = new VertexHelper()) {
            for (int i=0; i<numFragments; i++) {
                float deltaX = i * (property.dashLength+property.gapLength)/property.length;
                var min = new Vector2(-0.5f + deltaX, -0.5f);
                var max = new Vector2(-0.5f + deltaX + property.dashLength/property.length, 0.5f);
                MeshUtil.AddRect(new Bounds().WithMinMax(min, max), vh);
            }
            MeshUtil.UpdateMesh(GetComponent<MeshFilter>(), vh);
            MeshUtil.UpdateColor(GetComponent<MeshRenderer>(), property.color);
        }
    }

    /*
     * PROPERTIES
     */

    public void OnUpdate() {
        Render();
        DefaultShapeStyle.SetDefaultLineStyle(property);
    }

    void SetEndPoints(Vector2 p1, Vector2 p2) {
        property.points = new Tuple<Vector2, Vector2>(p1, p2);
    }

}

