using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board1 {
    /* MUTEEEEXXXX */
    public GraphicEntity1[,] graphicEntities;
    int width;
    int height;

    public Board1(int width, int height) {
        this.width = width;
        this.height = height;

        graphicEntities = new GraphicEntity1[width, height];
    }

    /*** GRID QUERY ***/
    public GridRect FindEmptyRect(int rectWidth, int rectHeight) {
        var emptyRects = new List<GridRect>();
        for (int x = 0; x < width - rectWidth; x++) {
            for (int y = 0; y < height - rectHeight; y++) {
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
            return emptyRects.GetRandom();
        }

        return null;
    }

    public GridRect FindEmptyRow() {
        // returns -1 if such row could not be found
        return FindEmptyRect(width, 1);
    }


    public GridRect FindEmptyColumn() {
        return FindEmptyRect(1, height);
    }

    public GridRect FindEmptyRectWithSize(int width, int height) {
        return null;
    }

    public GridRect FindAdjacentEmptyRect(GridRect gr, bool canOverlap = true) {
        return null;
    }

    public GridRect FindExpandedRect(GridRect gr) {
        return null;
    }

    /*** GRAPHIC ENTITY QUERY ***/
    public GraphicEntity1 FindGraphicWithSize(int width, int height) {
        return null;
    }

    public GraphicEntity1 FindGraphicWithSizeGreaterThan(int width, int height) {
        return null;
    }

    public GraphicEntity1 FindGraphicWithSizeLessThan(int width, int height) {
        return null;
    }

    public GraphicEntity1 FindSquareGraphicWithSideGreaterThan(int sideLength) {
        return null;
    }

    public GraphicEntity1 RandomGraphicEntity() {
        return null;
    }

    /*** ACCESS ***/
    public void LockTiles(GridRect rect, GraphicEntity1 ge) {
        for (int x = rect.minX; x <= rect.maxX; x++) {
            for (int y = rect.minY; y <= rect.maxY; y++) {
                graphicEntities[x, y] = ge;
            }
        }
    }

    public void UnlockTiles(GridRect rect) {
        for (int x = rect.minX; x <= rect.maxX; x++) {
            for (int y = rect.minY; y <= rect.maxY; y++) {
                graphicEntities[x, y] = null;
            }
        }
    }

    /*** Display ***/
    public RectParams GridRectToRectParams(GridRect grid) {
        return new RectParams { x=grid.minX, y=grid.minY, width=grid.width, height=grid.height, color= Color.black};
    }

    public IEnumerable GraphicEntities {
        get {
            return graphicEntities;
        }
    }
}
