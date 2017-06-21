using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellF : BaseCell {
    /* multiple [] bricks*/
    public Vector2[] scales;
    public Color[] colors;
    public float[] speeds;
    public int objsPerRun = 3;
    public bool test;

    void Start() {
        if (test) {
            Run();
        }
    }

    public override void Run() {
        StartCoroutine(_Run());
    }

    public IEnumerator _Run() {
        // three runs
        //
        //
        var x = (inGameWidth+ scales[0].x) / 2f * Random.Range(-0.7f, 0.7f);
        var seq1 = Random.Range(1, 4);
        for (int i = 0; i < seq1; i++) {
            FireLine(x, i);
            x += scales[0].x * 2f;
        }
        yield return null;
    }

    void FireLine(float x, int index) {
        var p = CreatePlane(Quaternion.identity);
        var scale = scales[index];
        p.localScale = scale;
        p.level = level;
        var y = (inGameHeight + scale.y) / -2f;
        p.velocity = Vector2.up * speeds[index];
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



