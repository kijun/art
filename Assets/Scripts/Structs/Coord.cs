using System;
using UnityEngine;

[Serializable]
public class Coord {
    public int x;
    public int y;

    public Coord (int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Vector2 ToVector2() {
        return new Vector2(x, y);
    }
}
