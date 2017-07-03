using UnityEngine;
using System.Collections;

public static class UnityExtensions {
    /*** VECTOR3 ***/
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

    /*** VECTOR2 ***/
    public static Vector2 SwapX(this Vector2 v, float val) {
        v.x = val;
        return v;
    }

    public static Vector2 SwapY(this Vector2 v, float val) {
        v.y = val;
        return v;
    }

    public static Vector2 MultiplyEach(this Vector2 v, float x, float y) {
        return new Vector2(v.x * x, v.y * y);
    }

    public static Vector2 MultiplyEach(this Vector2 v, Vector2 multiplier) {
        return new Vector2(v.x * multiplier.x, v.y * multiplier.y);
    }

    public static Vector2 DivideEach(this Vector2 v, float x, float y) {
        return new Vector2(v.x / x, v.y / y);
    }

    public static Vector2 DivideEach(this Vector2 v, Vector2 multiplier) {
        return new Vector2(v.x / multiplier.x, v.y / multiplier.y);
    }

    public static Direction ToDirection(this Vector2 v) {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y)) {
            if (v.x > 0) {
                return Direction.Right;
            } else {
                return Direction.Left;
            }
        } else {
            if (v.y > 0) {
                return Direction.Up;
            } else {
                return Direction.Down;
            }
        }
        return Direction.None;
    }

    public static void SetAlpha(this Material m, float val) {
        // alpha = [0, 255]
        if (!float.Equals(m.color.a, val)) {
            var color = m.color;
            color.a = val;
            m.color = color;
        }
    }

    public static float GetAlpha(this Material m) {
        // alpha = [0, 255]
        return m.color.a;
    }

    public static void SetOpacity(this Material m, float val) {
        // opacity = [0, 1]
        SetAlpha(m, val);
    }

    public static float GetOpacity(this Material m)  {
        return GetAlpha(m);
    }

    public static Vector2 RandomPoint(this Bounds b) {
        var center = b.center;
        var extents = b.extents;
        return new Vector2(Random.Range(center.x - extents.x, center.x + extents.x),
                           Random.Range(center.y - extents.y, center.y + extents.y));
    }

    public static float RandomValue(this Range r) {
        return Random.Range(r.minimum, r.maximum);
    }

    public static float RandomFraction(this float v, float minFraction, float maxFraction) {
        return Random.Range(v * minFraction, v * maxFraction);
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

    /*** RECT ***/
}
