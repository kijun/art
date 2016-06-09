using UnityEngine;
using System;

public static class TupleHelper {
    public static Tuple<Vector2, Vector2> Sort(Tuple<Vector2, Vector2> tup) {
        Vector2 a, b;
        if (tup.Item1.x < tup.Item2.x) {
            a = tup.Item1;
            b = tup.Item2;
        } else if (tup.Item1.x > tup.Item2.x) {
            b = tup.Item1;
            a = tup.Item2;
        } else { // x is same
            if (tup.Item1.y < tup.Item2.y) {
                a = tup.Item1;
                b = tup.Item2;
            } else if (tup.Item1.y > tup.Item2.y) {
                b = tup.Item1;
                a = tup.Item2;
            } else {
                a = tup.Item1;
                b = tup.Item1;
            }
        }
        return new Tuple<Vector2, Vector2>(a, b);
    }
}
