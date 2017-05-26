using UnityEngine;

public enum Direction {
    None, Up, Right, Down, Left
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
}
