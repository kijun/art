using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// execute in edit mode
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LineRenderer : MonoBehaviour, IObjectProperty {

    const int MAX_FRAGMENTS = 10000;

    /* PROPERTIES */
    public Color color;
    public BorderStyle style;
    public float dashLength = 0.1f;
    public float gapLength = 0.1f;

    Vector3 angleCache;
    Vector3 scaleCache;

    void Start() {
        CacheTransform();
    }

    void Update() {
        // TODO check if edit mode
        if (angleCache != transform.eulerAngles || scaleCache != transform.localScale) {
            CacheTransform();
            OnUpdate();
        }
    }

    void CacheTransform() {
        angleCache = transform.eulerAngles;
        scaleCache = transform.localScale;
    }

    /* RENDERING */
    /* we'll try rendering piecemeal (might be able to construct complicated situations with dashed lines */
    void Render() {
        if (style == BorderStyle.None || style == BorderStyle.Solid || gapLength == 0) {
            RenderSolid();
        } else if (style == BorderStyle.Dash) {
            RenderDashed();
        }
    }

    void RenderSolid() {
        using (var vh = new VertexHelper()) {
            MeshUtil.AddRect(BoundsUtil.UnitBounds, vh);
            MeshUtil.UpdateMesh(GetComponent<MeshFilter>(), vh);
            MeshUtil.UpdateColor(GetComponent<MeshRenderer>(), color);
        }
    }

    void RenderDashed() {
        int numFragments = Mathf.CeilToInt(Length/(dashLength + gapLength));
        if (numFragments > MAX_FRAGMENTS) {
            Debug.LogError("Line has too many fragments (" + numFragments + ")");
            return;
        }

        using (var vh = new VertexHelper()) {
            for (int i=0; i<numFragments; i++) {
                float deltaX = i * (dashLength+gapLength)/Length;
                var min = new Vector2(-0.5f + deltaX, -0.5f);
                var max = new Vector2(-0.5f + deltaX + dashLength/Length, 0.5f);
                MeshUtil.AddRect(new Bounds().WithMinMax(min, max), vh);
            }
            MeshUtil.UpdateMesh(GetComponent<MeshFilter>(), vh);
            MeshUtil.UpdateColor(GetComponent<MeshRenderer>(), color);
        }
    }

    /*
     * PROPERTIES
     */

    public float Length {
        get {
            return transform.localScale.x;
        }
        set {
            transform.localScale = transform.localScale.SwapX(value);
        }
    }

    public float Width {
        get {
            return transform.localScale.y;
        }
        set {
            transform.localScale = transform.localScale.SwapY(value);
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


    public void OnUpdate() {
        Render();
        CacheEndPoints();
        DefaultShapeStyle.SetDefaultLineStyle(this);
    }

    void SetEndPoints(Vector2 p1, Vector2 p2) {
        var z = transform.position.z;
        var midpoint = (p1+p2)/2f;

        Length = (p1-p2).magnitude;
        Angle = Mathf.Atan2(p2.y-p1.y, p2.x-p1.x) * Mathf.Rad2Deg;
        transform.position = new Vector3(midpoint.x, midpoint.y, z);

        CacheEndPoints();
    }

    void CacheEndPoints() {
        var directional = transform.rotation * new Vector2(Length/2, 0);
        var p1 = directional + transform.position;
        var p2 = -1*directional + transform.position;

        if (Vector2.Distance(cachedEndPoint1, p1) < Vector2.Distance(cachedEndPoint2, p2)) {
            cachedEndPoint1 = p1;
            cachedEndPoint2 = p2;
        } else {
            cachedEndPoint1 = p2;
            cachedEndPoint2 = p1;
        }
    }

    Vector2 cachedEndPoint1;
    Vector2 cachedEndPoint2;

    public Vector2 EndPoint1 {
        get {
            return cachedEndPoint1;
            //return transform.rotation * new Vector2(Length/2, 0) + transform.position;
        }

        set {
            SetEndPoints(value, EndPoint2);
        }
    }

    public Vector2 EndPoint2 {
        get {
            return cachedEndPoint2;
            //return -1 * (transform.rotation * new Vector2(Length/2, 0)) + transform.position;
        }

        set {
            SetEndPoints(EndPoint1, value);
        }
    }
}

