using UnityEngine;

public static class ScreenUtil {
    public static Vector2 ScreenLocationToWorldPosition(Direction dir, Vector2 displacement = new Vector2()) {
        Vector2 pos = (Vector2)Camera.main.transform.position + displacement;
        switch (dir) {
            case Direction.Top:
                pos = pos.Incr(0, CameraUtil.HalfHeight);
                break;
            case Direction.Right:
                pos = pos.Incr(CameraUtil.HalfWidth, 0);
                break;
            case Direction.Bottom:
                pos = pos.Incr(-CameraUtil.HalfWidth, 0);
                break;
            case Direction.Left:
                pos = pos.Incr(0, -CameraUtil.HalfWidth);
                break;
            case Direction.TopRight:
                pos = pos.Incr(CameraUtil.HalfWidth, CameraUtil.HalfHeight);
                break;
            case Direction.BottomRight:
                pos = pos.Incr(CameraUtil.HalfWidth, -CameraUtil.HalfHeight);
                break;
            case Direction.BottomLeft:
                pos = pos.Incr(-CameraUtil.HalfWidth, -CameraUtil.HalfHeight);
                break;
            case Direction.TopLeft:
                pos = pos.Incr(-CameraUtil.HalfWidth, CameraUtil.HalfHeight);
                break;
            case Direction.Center:
                break;
            default:
                Debug.LogError("unknown direction " + dir);
                break;
        }
        return pos;
    }
}
