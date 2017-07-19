using UnityEngine;
using System.Collections.Generic;

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
        this.max = new Coord(xmin+width-1, ymin+height-1);
    }

    public GridRect (GridRect gr) {
        this.min = gr.min;
        this.max = gr.max;
    }

    /*** TRANSFORMERS ***/
    public GridRect Translate(int x, int y) {
        return new GridRect(min.x + x, min.y + y, width, height);
    }

    public GridRect Merge(GridRect rect2) {
        return new GridRect(Coord.Min(min, rect2.min), Coord.Max(max, rect2.max));
    }

    public GridRect Resize(int width, int height) {
        return new GridRect(min, new Coord(min.x + width - 1, min.y + height - 1));
    }

    /*** HEL **/
    public IEnumerable<GridRect> SplitToUnitSquares() {
        for (int i = min.x; i <= max.x; i++) {
            for (int j = min.y; j <= max.y; j++) {
                yield return new GridRect(i, j, 1, 1);
            }
        }
    }

    /*** COMPARATORS ***/
    public bool Contains(GridRect other) {
        if (other.min.x >= min.x &&
            other.min.y >= min.y &&
            other.max.x <= max.x &&
            other.max.y <= min.y) {
            return true;
        }
        return false;
    }

    /*
    public GridRect Subtract(GridRect rect) {
        if
    }
    */

    /*** PROPERTIES ***/
    public int width {
        get {
            return max.x - min.x + 1;
        }
    }

    public int height {
        get {
            return max.y - min.y + 1;
        }
    }

    public int area {
        get {
            return width * height;
        }
    }

    public override string ToString() {
        if (min.Equals(max)) {
            return $"GridRect: {min}";
        }
        return $"GridRect: {min} - {max}";
    }
}
