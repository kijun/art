using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board2 {
    /* MUTEEEEXXXX */
    // TODO multiple entities
    GraphicEntity2[,] graphicEntities;
    int width;
    int height;
    float tileLength;
    float gapLength;
    GridRect boardRect;
    Vector2 bottomLeftAnchor;

    public Board2(int width, int height, float tileLength, float gapLength) {
        this.width = width;
        this.height = height;
        this.tileLength = tileLength;
        this.gapLength = gapLength;
        this.bottomLeftAnchor = new Vector2(
                width * tileLength + (width-1)*gapLength,
                height * tileLength + (height - 1)*gapLength)/-2f;
        this.boardRect = new GridRect(0, 0, width, height);

        graphicEntities = new GraphicEntity2[width, height];
    }

    /*** GRID QUERY ***/
    public GridRect FindEmptyRectWithSize(int rectWidth, int rectHeight) {
        var emptyRects = new List<GridRect>();
        for (int x = 0; x <= width - rectWidth; x++) {
            for (int y = 0; y <= height - rectHeight; y++) {
                IEnumerable<GraphicEntity2> entities = graphicEntities.GetRect<GraphicEntity2>(x, y, rectWidth, rectHeight);
                var count = rectWidth * rectHeight;
                foreach (var entity in entities) {
                    if (entity == null) count--;
                }

                if (count == 0) {
                    emptyRects.Add(new GridRect(x, y, rectWidth, rectHeight));
                }
            }
        }

        if (emptyRects.Count > 0) {
            var rect = emptyRects.GetRandom();
            //Debug.Log($"Board: Found empty rect {rect}");
            return rect;
        }

        //Debug.Log($"Board: Could not find empty rect");
        return null;
    }

    public GridRect FindEmptyRow() {
        // returns -1 if such row could not be found
        return FindEmptyRectWithSize(width, 1);
    }


    public GridRect FindEmptyColumn() {
        return FindEmptyRectWithSize(1, height);
    }

    public GridRect FindAdjacentEmptyRect(GridRect gr, bool canOverlap = true) {
        return null;
    }


    public GridRect FindExpandedRect(GridRect gr) {
        return null;
    }

    public GridRect FindRandomRectWithSize(int rectWidth, int rectHeight) {
        return new GridRect(Random.Range(0, width - rectWidth), Random.Range(0, height - rectHeight), rectWidth, rectHeight);
    }

    /*** GRAPHIC ENTITY QUERY ***/
    public GraphicEntity2 FindFirstGraphicWithSize(int width, int height) {
        foreach (var g in GraphicEntities()) {
            if (g.width == width && g.height == height) return g;
        }
        return null;
    }

    public IEnumerable<GraphicEntity2> FindGraphicsForRow(int rowNum) {
        if (rowNum >= 0 && rowNum < height) {
            foreach (var g in graphicEntities.GetRow(rowNum)) {
                if (g != null) yield return g;
            }
        }
    }

    public IEnumerable<GraphicEntity2> FindGraphicsForColumn(int colNum) {
        if (colNum >= 0 && colNum < width) {
            foreach (var g in graphicEntities.GetCol(colNum)) {
                if (g != null) yield return g;
            }
        }
    }

    public IEnumerable<GraphicEntity2> FindAllGraphicsWithSize(int width, int height, GridRect rect = null) {
        foreach (var g in GraphicEntities(rect)) {
            if (g.width == width && g.height == height) yield return g;
        }
    }

    public GraphicEntity2 FindRandomGraphicWithSize(int width, int height, GridRect rect = null) {
        List<GraphicEntity2> targets = new List<GraphicEntity2>();
        foreach (var g in GraphicEntities(rect)) {
            if (g.width == width && g.height == height) targets.Add(g);
        }
        if (targets.Count > 0) { return targets[Random.Range(0, targets.Count)]; }
        return null;
    }

    public GraphicEntity2 FindGraphicWithSizeGreaterThan(int width, int height) {
        // TODO HAX
        foreach (var g in GraphicEntities().ToArray().Shuffle()) {
            if ((g.width > width && g.height >= height) ||
                (g.width >= width && g.height > height)) {
                return g;
            }
        }
        return null;
    }

    public GraphicEntity2 FindGraphicWithSizeLessThan(int width, int height) {
        foreach (var g in GraphicEntities()) {
            if ((g.width < width && g.height <= height) ||
                (g.width <= width && g.height < height)) {
                return g;
            }
        }
        return null;
    }

    public GraphicEntity2 FindSquareGraphicWithSideGreaterThan(int sideLength) {
        foreach (var g in GraphicEntities()) {
            if (g.width == sideLength && g.height == sideLength) return g;
        }
        return null;
    }

    /*
    public List<GraphicEntity2> FindGraphicEntitiesInside(GridRect rect) {
        var list = new List<GraphicEntity2>();
        foreach (
    }
    */

    public GraphicEntity2 RandomGraphicEntity() {
        return FindRandomGraphic();
    }

    public GraphicEntity2 FindRandomGraphic() {
        // TODO performance!!
        var gs = GraphicEntities().ToArray().Shuffle();
        if (gs.Count() > 0) {
            return gs.First();
        }
        return null;
    }


    public GraphicEntity2 FindGraphicAtPosition(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return graphicEntities[x, y];
        }
        return null;
    }

    public IEnumerable<GraphicEntity2> FindAdjacentGraphics(GridRect gr, bool canOverlap = true) {
        var rects = new GridRect[4] {
            gr.Translate(0, 1),
            gr.Translate(0, -1),
            gr.Translate(-1, 0),
            gr.Translate(1, 0)}.Shuffle();
        foreach (var rect in rects) {
            // TODO yield overlap, remove current rect
            var g = FindGraphicAtPosition(rect.min.x, rect.min.y);
            if (g != null) yield return g;
        }
    }

    /***** COORD *****/
    public Coord RandomCoord() {
        return new Coord(Random.Range(0, width), Random.Range(0, height));
    }

    /*** ACCESS ***/
    public void LockTiles(GridRect rect, GraphicEntity2 ge) {
        //Debug.Log($"Board2: Lock {rect}");
        for (int x = rect.min.x; x <= rect.max.x; x++) {
            for (int y = rect.min.y; y <= rect.max.y; y++) {
                if (x >= 0 && x < width && y >= 0 && y < height) {
                    graphicEntities[x, y] = ge;
                } else {
                    //Debug.Log($"lock failed {x} {y}");
                }
            }
        }
    }

    public void UnlockTiles(GridRect rect) {
        for (int x = rect.min.x; x <= rect.max.x; x++) {
            for (int y = rect.min.y; y <= rect.max.y; y++) {
                if (x >= 0 && x < width && y >= 0 && y < height) {
                    graphicEntities[x, y] = null;
                } else {
                    //Debug.Log("unlock failed {x} {y}");
                }
            }
        }
    }

    /*** Display ***/
    public RectParams GridRectToRectParams(GridRect grid) {
        var minPos = bottomLeftAnchor + new Vector2(grid.min.x * (tileLength + gapLength), grid.min.y * (tileLength + gapLength));
        var size = new Vector2(grid.width * tileLength + (grid.width - 1) * gapLength,
                               grid.height * tileLength + (grid.height - 1) * gapLength);

        var center = minPos + size/2f;
        return new RectParams { x=center.x, y=center.y, width=size.x, height=size.y, color= Color.black};
    }

    public IEnumerable<GraphicEntity2> GraphicEntities(GridRect rect = null) {
        if (rect == null) {
            foreach (var g in graphicEntities) {
                if (g != null) yield return g;
            }
        } else {
            for (int x = 0; x < rect.width; x++) {
                for (int y = 0; y < rect.height; y++) {
                    Debug.Log($"GraphicEntities: x={x} y={y} {rect}");
                    var g = graphicEntities[x+rect.min.x, y+rect.min.y];
                    if (g != null) yield return g;
                }
            }
        }
    }
}

