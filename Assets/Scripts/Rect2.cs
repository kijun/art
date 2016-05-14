using UnityEngine;
using System.Collections;

public struct Rect2 {
    public enum Direction {
        Top, Right, Bottom, Left, TopRight, BottomRight, BottomLeft, TopLeft
    }

    public Vector2 center;
    public Vector2 size;
    public float angle;

    public Rect2(Vector2 myCenter, Vector2 mySize, float myAngle) {
        center = myCenter;
        size = mySize;
        angle = myAngle;
        Debug.Log("new rect: " + center + size + angle);
    }

    public float width {
        get {
            return size.x;
        }

        set {
            size.x = value;
        }
    }

    public float height {
        get {
            return size.y;
        }

        set {
            size.y = value;
        }
    }

    Vector2 LocalPoint(Direction dir) {
        float x, y;
        switch (dir) {
            case Direction.Top:
                x = 0;
                y = height/2;
                break;
            case Direction.Right:
                x = width/2;
                y = 0;
                break;
            case Direction.Bottom:
                x = 0;
                y = -height/2;
                break;
            case Direction.Left:
                x = -width/2;
                y = 0;
                break;
            case Direction.TopRight:
                x = width/2;
                y = height/2;
                break;
            case Direction.BottomRight:
                x = width/2;
                y = -height/2;
                break;
            case Direction.BottomLeft:
                x = -width/2;
                y = -height/2;
                break;
            case Direction.TopLeft:
                x = -width/2;
                y = height/2;
                break;
            default:
                Debug.LogError("unknown direction " + dir);
                x = 0;
                y = 0;
                break;
        }
        return new Vector2(x, y);
    }

    Vector2 RotateByDegrees(Vector2 p, float deg) {
        return Quaternion.Euler(0, 0, deg) * p;
    }

    Vector2 LocalToParentPosition(Vector2 p) {
        return RotateByDegrees(p, angle) + center;
    }

    Vector2 ParentToLocalPosition(Vector2 p) {
        return RotateByDegrees(p-center, -angle);
    }

    public Vector2 Point(Direction dir) {
        Debug.Log(dir+ " | " + LocalPoint(dir) + " | " + LocalToParentPosition(LocalPoint(dir)));
        return LocalToParentPosition(LocalPoint(dir));
    }

    public void MovePoint(Vector2 pos, Direction dir) {
        var prevLocalPos = LocalPoint(dir);
        var newLocalPos = ParentToLocalPosition(pos);

        // strip extraneous axis
        switch (dir) {
            case Direction.Top:
            case Direction.Bottom:
                newLocalPos.x = 0;
                break;
            case Direction.Right:
            case Direction.Left:
                newLocalPos.y = 0;
                break;
            default:
                break;
        }
        var displacement = newLocalPos - prevLocalPos;
        size += displacement;
        // half of displacement gets applied to the fixed half
        var newLocalCenter = displacement/2f;
        center = LocalToParentPosition(newLocalCenter);
    }
}
