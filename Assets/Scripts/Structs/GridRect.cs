using UnityEngine;

[System.Serializable]
public class GridRect {
    public Coord min;
    public Coord max;

    /*** INITIALIZER ***/
    public GridRect (Coord a, Coord b) {
        this.min = new Coord(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
        this.max = new Coord(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
    }

    public GridRect (int xmin, int ymin, int width, int height) {
        this.min = new Coord(xmin, ymin);
        this.max = new Coord(xmin+width, ymin+height);
    }

    public GridRect (GridRect gr) {
        this.min = gr.min;
        this.max = gr.max;
    }

    /*** TRANSFORMERS ***/
    public GridRect Translate(int x, int y) {
        return new GridRect(min.x + x, min.y + y, width, height);
    }

    /*
    public GridRect Subtract(GridRect rect) {
        if
    }
    */

    /*** PROPERTIES ***/
    public int width {
        get {
            return max.x - min.x;
        }
    }

    public int height {
        get {
            return max.y - min.y;
        }
    }

    public override string ToString() {
        return $"GridRect: {min} - {max}";
    }
}
