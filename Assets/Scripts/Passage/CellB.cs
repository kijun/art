using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellB : BaseCell {
    public Vector2 scale = new Vector2(0.5f, 2);
    public int count = 1;
    public float speed = 1;
    public float entryAngle = 0;
    public float angularVelocity = 0;

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
            p.position = entryQ * new Vector2(x, startDistanceFromOrigin(scale));
            p.velocity = entryQ * new Vector2(0, -1 * speed);
            p.angularVelocity = angularVelocity;
            x += distBetweenObj;
        }
        yield return null;
    }

    Animatable2 CreatePlane(Quaternion entryAngle) {
        return Instantiate<Animatable2>(prefab, scale, entryAngle);
    }
}
