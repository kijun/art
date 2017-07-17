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

    public Coord (Coord c) {
        this.x = c.x;
        this.y = c.y;
    }

    public Coord Move (int dx, int dy) {
        this.x += dx;
        this.y += dy;
        return this;
    }

    public Vector2 ToVector2() {
        return new Vector2(x, y);
    }

    public override string ToString() {
        return $"({x}, {y})";
    }

    public static Coord Max(Coord c1, Coord c2) {
        return new Coord(Mathf.Max(c1.x, c2.x), Mathf.Max(c1.y, c2.y));
    }

    public static Coord Min(Coord c1, Coord c2) {
        return new Coord(Mathf.Min(c1.x, c2.x), Mathf.Min(c1.y, c2.y));
    }
}
