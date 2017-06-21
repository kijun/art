using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellE : BaseCell {
    /* multiple [] bricks*/
    public Vector2[] scales;
    public Color[] colors;
    public float speed = 1;
    public int objsPerRun = 3;
    public bool test;

    void Start() {
        if (test) {
            Run();
        }
    }

    public void Run() {
        StartCoroutine(_Run());
    }

    public IEnumerator _Run() {
        // three runs
        //
        //
        var y = (inGameHeight + scales[0].y) / 2f * Random.Range(-0.7f, 0.7f);
        var seq1 = Random.Range(1, 3);
        var seq2 = Random.Range(4, 7);
        var seq3 = Random.Range(2, 4);
        for (int i = 0; i < seq1; i++) {
            FireLine(y);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < seq2; i++) {
            FireLine(y);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < seq3; i++) {
            FireLine(y);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    void FireLine(float y) {
        var p = CreatePlane(Quaternion.identity);
        var scale = scales[0];
        p.localScale = scale;
        p.level = level;
        var x = (inGameWidth + scale.x) / 2f;
        x *= -1;
        p.velocity = Vector2.right * speed;
        p.position = new Vector2(x, y);
        p.color = colors[0];
    }

    Animatable2 CreatePlane(Quaternion entryAngle) {
        return Instantiate<Animatable2>(prefab, Vector2.one, entryAngle);
    }

    T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }

}


