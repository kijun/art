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
}
