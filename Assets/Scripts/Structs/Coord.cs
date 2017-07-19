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
        return new Coord(x+dx, y+dy);
    }

    public Vector2 ToVector2() {
        return new Vector2(x, y);
    }

    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Coord p = obj as Coord;
        if ((System.Object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
    }

    public bool Equals(Coord p)
    {
        // If parameter is null return false:
        if ((object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (x == p.x) && (y == p.y);
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
