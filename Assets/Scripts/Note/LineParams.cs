using UnityEngine;

[System.Serializable]
public struct LineParams2 {
    public float x;
    public float y;
    public float length;
    public float level;
    public float width;
    public Color color;
    public float rotation;
    public Vector2 position {
        get { return new Vector2(x, y); }
        set {
            x = value.x;
            y = value.y;
        }
    }
    public Vector2 scale {
        get { return new Vector2(width, length); }
        set {
            width = value.x;
            length = value.y;
        }
    }

    public RectParams ToRectParams() {
        return new RectParams {
            x=x, y=y, width=width, height=length, level=level, color=color, rotation=rotation
        };
    }
}

[System.Serializable]
public struct RectParams {
    public float x;
    public float y;
    public float width;
    public float height;
    public float level;
    public Color color;
    public float rotation;
    public Vector2 position {
        get { return new Vector2(x, y); }
        set {
            x = value.x;
            y = value.y;
        }
    }
    public Vector2 scale {
        get { return new Vector2(width, height); }
        set {
            width = value.x;
            height = value.y;
        }
    }
}
