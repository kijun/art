using UnityEngine;
using System.Collections;

public static class CameraExtensions {
    public static Bounds WorldBounds(this Camera cam) {
        return new Bounds(cam.transform.position,
                          new Vector2(CameraHelper.Width, CameraHelper.Height));
    }
}

public static class UnityExtensions {
    /*
     * VECTOR3
     */
    public static Vector3 SwapX(this Vector3 v, float val) {
        v.x = val;
        return v;
    }

    public static Vector3 SwapY(this Vector3 v, float val) {
        v.y = val;
        return v;
    }

    public static Vector3 SwapZ(this Vector3 v, float val) {
        v.z = val;
        return v;
    }

    public static Vector3 IncrX(this Vector3 v, float val) {
        v.x += val;
        return v;
    }

    public static Vector3 IncrY(this Vector3 v, float val) {
        v.y += val;
        return v;
    }

    public static Vector3 IncrZ(this Vector3 v, float val) {
        v.z += val;
        return v;
    }

    /*
     * VECTOR2
     */

    public static Vector2 Incr(this Vector2 v, float x, float y) {
        v.x += x;
        v.y += y;
        return v;
    }

    /*
     * BOUNDS
     */
    public static Bounds WithMinMax(this Bounds b, Vector3 min, Vector3 max) {
        b.SetMinMax(min, max);
        return b;
    }

    public static Bounds FromPoints(this Bounds b, Vector3 p1, Vector3 p2) {
        var min = new Vector3(Mathf.Min(p1.x, p2.x), Mathf.Min(p1.y, p2.y), Mathf.Min(p1.z, p2.z));
        var max = new Vector3(Mathf.Max(p1.x, p2.x), Mathf.Max(p1.y, p2.y), Mathf.Max(p1.z, p2.z));
        b.SetMinMax(min, max);
        return b;
    }

    public static Vector2 TopLeft(this Bounds b) {
        return new Vector2(b.min.x, b.max.y);
    }

    public static Vector2 TopRight(this Bounds b) {
        return new Vector2(b.max.x, b.max.y);
    }

    public static Vector2 BottomLeft(this Bounds b) {
        return new Vector2(b.min.x, b.min.y);
    }

    public static Vector2 BottomRight(this Bounds b) {
        return new Vector2(b.max.x, b.min.y);
    }

    /*
     * DUNNO
     */
    public static void SetAlpha(this Material m, float val) {
        var color = m.color;
        color.a = val;
        m.color = color;
    }

    public static Vector2 RandomPoint(this Bounds b) {
        var center = b.center;
        var extents = b.extents;
        return new Vector2(Random.Range(center.x - extents.x, center.x + extents.x),
                           Random.Range(center.y - extents.y, center.y + extents.y));
    }

    public static Vector2 RandomPoint(this Rect rect) {
        return new Vector2(Random.Range(rect.xMin, rect.xMax),
                           Random.Range(rect.yMin, rect.yMax));
    }

    public static MonoBehaviour ChooseOne(this MonoBehaviour[] scripts) {
        return scripts[Random.Range(0, scripts.Length)];
    }

    public static float RandomValue(this Range r) {
        return Random.Range(r.minimum, r.maximum);
    }

    public static IEnumerator Glide(this Transform trans, Vector3 startPos, Vector3 endPos, float duration) {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            yield return null;
            elapsedTime += Time.deltaTime;
            trans.position = Vector3.Lerp(startPos, endPos, elapsedTime/duration);
        }
        yield return null;
    }

    public static IEnumerator CoroutineWithWait(this MonoBehaviour script, IEnumerator coroutine, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        yield return script.StartCoroutine(coroutine);
    }

    // Game Object
    public static bool IsPlayer(this GameObject obj) {
        if (obj.tag == "Player") return true;
        return false;
    }

    public static bool IsCamera(this GameObject obj) {
        if (obj.tag == "MainCamera") return true;
        return false;
    }

    public static void SetAlpha(this GameObject obj, float alpha) {
        var rend = obj.GetComponent<Renderer>();
        if (rend != null) {
            rend.material.SetAlpha(alpha);
        }
    }

    public static float GetAlpha(this GameObject obj) {
        var rend = obj.GetComponent<Renderer>();
        if (rend != null) {
            return rend.material.color.a;
        }
        return 1;
    }
}
