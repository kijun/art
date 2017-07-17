using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board1 {
    /* MUTEEEEXXXX */
    GraphicEntity1[,] graphicEntities;
    int width;
    int height;
    float tileLength;
    float gapLength;
    Vector2 bottomLeftAnchor;

    public Board1(int width, int height, float tileLength, float gapLength) {
        this.width = width;
        this.height = height;
        this.tileLength = tileLength;
        this.gapLength = gapLength;
        this.bottomLeftAnchor = new Vector2(
                width * tileLength + (width-1)*gapLength,
                height * tileLength + (height - 1)*gapLength)/-2f;

        graphicEntities = new GraphicEntity1[width, height];
    }

    /*** GRID QUERY ***/
    public GridRect FindEmptyRectWithSize(int rectWidth, int rectHeight) {
        var emptyRects = new List<GridRect>();
        for (int x = 0; x <= width - rectWidth; x++) {
            for (int y = 0; y <= height - rectHeight; y++) {
                IEnumerable<GraphicEntity1> entities = graphicEntities.GetRect<GraphicEntity1>(x, y, rectWidth, rectHeight);
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

    /*** GRAPHIC ENTITY QUERY ***/
    public GraphicEntity1 FindGraphicWithSize(int width, int height) {
        foreach (var g in GraphicEntities()) {
            if (g.width == width && g.height == height) return g;
        }
        return null;
    }

    public GraphicEntity1 FindGraphicWithSizeGreaterThan(int width, int height) {
        foreach (var g in GraphicEntities()) {
            if ((g.width > width && g.height >= height) ||
                (g.width >= width && g.height > height)) {
                return g;
            }
        }
        return null;
    }

    public GraphicEntity1 FindGraphicWithSizeLessThan(int width, int height) {
        foreach (var g in GraphicEntities()) {
            if ((g.width < width && g.height <= height) ||
                (g.width <= width && g.height < height)) {
                return g;
            }
        }
        return null;
    }

    public GraphicEntity1 FindSquareGraphicWithSideGreaterThan(int sideLength) {
        foreach (var g in GraphicEntities()) {
            if (g.width == sideLength && g.height == sideLength) return g;
        }
        return null;
    }

    public GraphicEntity1 RandomGraphicEntity() {
        // TODO make random
        return GraphicEntities().First();
    }

    /*** ACCESS ***/
    public void LockTiles(GridRect rect, GraphicEntity1 ge) {
        Debug.Log($"Board1: Lock {rect}");
        for (int x = rect.min.x; x < rect.max.x; x++) {
            for (int y = rect.min.y; y < rect.max.y; y++) {
                graphicEntities[x, y] = ge;
            }
        }
    }

    public void UnlockTiles(GridRect rect) {
        for (int x = rect.min.x; x < rect.max.x; x++) {
            for (int y = rect.min.y; y < rect.max.y; y++) {
                graphicEntities[x, y] = null;
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

    public IEnumerable<GraphicEntity1> GraphicEntities() {
        foreach (var g in graphicEntities) {
            if (g != null) yield return g;
        }
    }
}
