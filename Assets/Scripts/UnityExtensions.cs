using UnityEngine;
using System.Collections;

public static class UnityExtensions {
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
}
