using System.Collections;
using System.Collections.Generic;

public class Board {
    /* MUTEEEEXXXX */

    public IEnumerator<GraphicEntity1> graphicEntities;

    public GridRect FindEmptyRow() {
    }

    public GridRect FindEmptyColumn() {
    }

    public GridRect FindEmptyRectWithSize(int width, int height) {
    }

    public GridRect FindAdjacentEmptyRect(GridRect gr, bool canOverlap = true) {
    }

    public GridRect FindExpandedRect(GridRect gr) {
    }

    public GraphicEntity FindGraphicWithSize(int width, int height) {
    }

    public GraphicEntity FindGraphicWithSizeGreaterThan(int width, int height) {
    }

    public GraphicEntity FindGraphicWithSizeLessThan(int width, int height) {
    }

    public GraphicEntity FindSquareGraphicWithSideGreaterThan(int sideLength) {
    }

    public GraphicEntity RandomGraphicEntity() {
    }
}

public class Board {
    GraphicEntity1[,] graphicEntities;
    int width;
    int height;

    public Board(int width, int height) {
        this.width = width;
        this.height = height;

        graphicEntities = new GraphicEntity1[width, height];
    }

    public void LockTile(int x, int y, GraphicEntity1 ge) {
        graphicEntities[x,y] = ge;
    }

    public void LockTiles(GridRect gr) {
        graphicEntities[x,y] = ge;
    }

    public void FreeTile(int x, int y) {
        graphicEntities[x,y] = null;
    }

    public GraphicEntity1 GraphicEntityAt(int x, int y) {
        return graphicEntities[x, y];
    }

    public IEnumerable GraphicEntities {
        get {
            return graphicEntities;
        }
    }

    public GridRect FindEmptyRow() {
        // returns -1 if such row could not be found
        return FindEmptyRect(width, 1);
    }

    public GridRect FindEmptyRect(int rectWidth, int rectHeight) {
        var emptyRects = new List<GridRect>();
        for (int x = 0; x < width - rectWidth; x++) {
            for (int y = 0; y < height - rectHeight; y++) {
                if (graphicEntities.GetRect(x, y, rectWidth, rectHeight).Count(ge => (ge == null)) > rectWidth * rectHeight) {
                    emptyRects.Add(new GridRect(x, y, rectWidth, rectHeight));
                }
            }
        }

        if (emptyRects.Count > 0) {
            return emptyRects.GetRandom();
        }

        return null;
    }

    public GridRect FindEmptyColumn() {
        return FindEmptyRect(1, height);
    }
}
