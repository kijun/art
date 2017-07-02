using UnityEngine;

static class CameraHelper {
    public static float HalfWidth {
        get {
            return Camera.main.orthographicSize / Screen.height * Screen.width;
        }
    }

    public static float Width {
        get {
            return 2*HalfWidth;
        }
    }

    public static float HalfHeight {
        get {
            return Camera.main.orthographicSize;
        }
    }

    public static float Height {
        get {
            return 2 * HalfHeight;
        }
    }

    public static Rect Rect {
        get {
            return Camera.main.rect;
        }
    }

    public static Rect WorldRect {
        get {
            var origin = Camera.main.transform.position;
            return new Rect(
                    new Vector2(origin.x-HalfWidth, origin.y - HalfHeight),
                    new Vector2(Width, Height));
        }
    }

    public static float Diameter {
        get {
            var inGameHeight = Camera.main.orthographicSize * 2;
            var inGameWidth = (float)Screen.width / (float)Screen.height * inGameHeight;

            var cameraDiameter = Mathf.Sqrt(inGameHeight * inGameHeight + inGameWidth * inGameWidth);

            return cameraDiameter;
        }
    }

    public static Vector2 PositionOnTop(float xOffset = 0, Vector2 scale = new Vector2(), float rotation = 0)  {
        float extraHeight = BoundingRectForRotatedVector(scale, rotation).y / 2;
        return new Vector2(xOffset, HalfHeight + extraHeight);
    }

    public static Vector2 PositionOnBottom(float xOffset = 0, Vector2 scale = new Vector2(), float rotation = 0) {
        float extraHeight = BoundingRectForRotatedVector(scale, rotation).y / 2;
        return new Vector2(xOffset, -1 * (HalfHeight + extraHeight));
    }

    public static Vector2 PositionOnRight(float yOffset = 0, Vector2 scale = new Vector2(), float rotation = 0) {
        float extraWidth = BoundingRectForRotatedVector(scale, rotation).x / 2;
        return new Vector2(HalfWidth + extraWidth, yOffset);
    }

    public static Vector2 PositionOnLeft(float yOffset = 0, Vector2 scale = new Vector2(), float rotation = 0) {
        float extraWidth = BoundingRectForRotatedVector(scale, rotation).x / 2;
        return new Vector2(-1 * (HalfWidth + extraWidth), yOffset);
    }

    // perimeter position for object
    public static Vector2 PerimeterPositionForMovingObject(Direction dir, float offset = 0, Vector2 scale = new Vector2(), float rotation = 0) {
        var perimeter = Vector2.zero;

        switch (dir) {
            case Direction.Down:
                perimeter = PositionOnTop(offset, scale, rotation);
                break;
            case Direction.Up:
                perimeter = PositionOnBottom(offset, scale, rotation);
                break;
            case Direction.Right:
                perimeter = PositionOnLeft(offset, scale, rotation);
                break;
            case Direction.Left:
                perimeter = PositionOnRight(offset, scale, rotation);
                break;
        }

        return perimeter;
    }

    public static Rect BoundingRectOnPerimeter(Location loc, float width, float height, float offset) {
        var rect = new Rect();
        switch (loc) {
            case Location.Top:
                rect = Rect.MinMaxRect(offset-width/2, HalfHeight, offset+width/2, HalfHeight + height);
                break;
            case Location.Bottom:
                rect = Rect.MinMaxRect(offset-width/2, -HalfHeight-height, offset+width/2, -HalfHeight);
                break;
            case Location.Right:
                rect = Rect.MinMaxRect(HalfWidth, offset-height/2, HalfWidth+width, offset+height/2);
                break;
            case Location.Left:
                rect = Rect.MinMaxRect(-HalfWidth-width, offset-height/2, -HalfWidth, offset+height/2);
                break;
        }
        return rect;
    }

    static Vector2 BoundingRectForRotatedVector(Vector2 scale, float rotation) {
        return Quaternion.Euler(0, 0, rotation) * scale;
    }

    public static float RandomXOffset(float range = 1, float quantized = 0) {
        var offset = (Random.value * Width - HalfWidth) * range;

        if (quantized > float.Epsilon) {
            float optionsPerUnitLength = 1 / quantized;
            offset = (Mathf.RoundToInt(offset * optionsPerUnitLength)) / optionsPerUnitLength;
        }
        return offset;
    }

    public static float RandomYOffset(float range = 1, float quantized = 0) {
        var offset = (Random.value * Height - HalfHeight) * range;
        if (quantized > float.Epsilon) {
            float optionsPerUnitLength = 1 / quantized;
            offset = (Mathf.RoundToInt(offset * optionsPerUnitLength)) / optionsPerUnitLength;
        }
        return offset;
    }

    public static float ViewportToWorldXPos(float x) {
        return Camera.main.ViewportToWorldPoint(new Vector2(x, 0)).x;
    }

    public static float ViewportToWorldYPos(float y) {
        return Camera.main.ViewportToWorldPoint(new Vector2(0, y)).y;
    }

    public static Vector2 ViewportToWorldPoint(float x, float y) {
        return Camera.main.ViewportToWorldPoint(new Vector2(x, y));
    }

    public static Vector2 ViewportToWorldScale(float width, float height) {
        return new Vector2(Width * width, Height * height);
    }

    public static Vector2 RandomPositionNearCenter(float maxDistance = 0) {
        var adjHalfWidth = HalfWidth - maxDistance;
        var adjHalfHeight = HalfHeight - maxDistance;
        return new Vector2(
                Random.Range(-adjHalfWidth, adjHalfWidth),
                Random.Range(-adjHalfHeight, adjHalfHeight));
    }

    public static Vector2 RandomPositionNearPerimiter(float maxDistance = 0) {
        var x = Random.Range(HalfWidth - maxDistance, HalfWidth);
        var y = Random.Range(HalfHeight - maxDistance, HalfHeight);
        x *= Mathf.Sign(Random.Range(-1f, 1f));
        y *= Mathf.Sign(Random.Range(-1f, 1f));
        return new Vector2(x, y);
    }
}
