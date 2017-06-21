using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellB : BaseCell {
    /* [] [] [] [] [] */
    public Vector2 scale = new Vector2(0.5f, 2);
    public int count = 1;
    public float speed = 1;
    public float entryAngle = 0;
    public float angularVelocity = 0;
    public Vector2 scaleVelocity;
    public Color color;

    public void Run() {
        StartCoroutine(_Run());
    }

    public IEnumerator _Run() {
        var distBetweenObj = inGameWidth / count;
        var x = inGameWidth / -2 + distBetweenObj/2;
        var entryQ = Quaternion.Euler(0, 0, entryAngle);
        for (int i = 0; i < count; i++) {
            var p = CreatePlane(entryQ);
            p.localScale = scale;
            p.level = level;
            p.position = entryQ * new Vector2(x, startDistanceFromOrigin(scale));
            p.velocity = entryQ * new Vector2(0, -1 * speed);//.MultiplyEach(Random.Range(0.7f, 1.3f), Random.Range(0.7f,1.3f));
            p.color = color;
            p.angularVelocity = angularVelocity;
            p.scaleVelocity = scaleVelocity;
            p.nonNegativeScale = true;
            x += distBetweenObj;
        }
        yield return null;
    }

    Animatable2 CreatePlane(Quaternion entryAngle) {
        return Instantiate<Animatable2>(prefab, scale, entryAngle);
    }
}
