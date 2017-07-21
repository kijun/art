using UnityEngine;

[System.Flags]
public enum Direction {
    None        = 0,
    Up          = 1 << 0,
    Down        = 1 << 1,
    Right       = 1 << 2,
    Left        = 1 << 3,

    UpRight    = 1 << 4,
    UpLeft = 1 << 5,
    DownRight = 1 << 6,
    DownLeft  = 1 << 7,

    Axis        = Up | Right | Down | Left,
    Diagonal    = UpRight | UpLeft | DownRight | DownLeft,

    Any         = Axis | Diagonal
}

public static class DirectionHelper {
    public static Direction Random {
        get {
            return (Direction)UnityEngine.Random.Range(1, 5);
        }
    }
}

public static class DirectionMethods {

    public static Vector2 ToVelocity(this Direction dir, float speed)
    {
        var v = Vector2.zero;
        switch (dir) {
            case Direction.Up:
                v = new Vector2(0, speed);
                break;
            case Direction.Down:
                v = new Vector2(0, -1 * speed);
                break;
            case Direction.Right:
                v = new Vector2(speed, 0);
                break;
            case Direction.Left:
                v = new Vector2(-1 * speed, 0);
                break;
        }

        return v;
    }

    public static Vector2 ToVector2(this Direction dir) {
        var v = Vector2.zero;
        switch (dir) {
            case Direction.Up:
                v = Vector2.up;
                break;
            case Direction.Right:
                v = Vector2.right;
                break;
            case Direction.Down:
                v = Vector2.down;
                break;
            case Direction.Left:
                v = Vector2.left;
                break;
        }

        return v;
    }

    public static Vector2 Align(this Direction dir, Vector2 v)
    {
        switch (dir) {
            case Direction.Up:
            case Direction.Down:
                break;
            case Direction.Right:
            case Direction.Left:
                v = new Vector2(v.y, v.x);
                break;
        }

        return v;
    }
}
