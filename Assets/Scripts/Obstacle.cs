using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// make sure that the generic names
// only represent pure data.
// No monobehaviours

public struct Board {
    public int width;
    public int height;

    public HashSet<BoardPosition> AdjacentPositions(BoardPosition bp) {
        var bpSet = new HashSet<BoardPosition>();
        for (int i=-1; i<2; i++) {
            for (int j=-1; j<2; j++) {
                var adjacentBP = new BoardPosition(bp.x + i, bp.y + j);
                if (adjacentBP != bp && ContainsPosition(adjacentBP)) {
                    bpSet.Add(adjacentBP);
                }
            }
        }

        return bpSet;
    }

    public bool ContainsPosition(BoardPosition bp) {
        if (bp.x >= width || bp.y >= height || bp.x < 0 || bp.y < 0) {
            return false;
        }
        return true;
    }
}

// use pure data everywhere
//
public interface IObstacle {
    HashSet<BoardPosition> ObstructingPositions();
    BoardPosition BoardPosition();
}

public class Obstacle : IObstacle {
    public enum GrowthDirection {
        Stationary, Inward, Outward
    }

    public BoardPosition boardPosition;
    public HashSet<BoardPosition> obstructingPositions;

    public HashSet<BoardPosition> ObstructingPositions(){
        return null;
        //
    }
    public BoardPosition BoardPosition() {
        return new BoardPosition();
    }
}

public class CircleObstacle : Obstacle {
    public float growthSpeed; // TODO will it be hard to cast to int?
    public int stepsToFade;
    public int maxRadius;
    public int startRadius;
    public int beginningIteration; // TODO name???
    public Board board;

    // TODO what do i do with current steps? do I save it here? the circle obstacle has a primitive unchanging data and also parts that change
    // i'd like to separate what changes from what doesn't change
    public HashSet<BoardPosition> ObstructingPositions() {
        // boardState
        //
        // depends on the turn
        //
        return null;
    }
}

// let's just assume that they enlarge and fade out
public class SquareObstacle : Obstacle {
    public BoardPosition center;
    public float initialWidth;
    public float finalWidth;
    public float widthPerStep;
    public int startingStep;
    public float stepsToFade;

    public int finalStep {
        // TODO different obstacle movement
        // rotation, oscilation etc etc
        get {
            return Mathf.CeilToInt((finalWidth - initialWidth) / widthPerStep);
        }
    }

    public float WidthForStep(float step) {
        return Mathf.Min(initialWidth + step * widthPerStep, finalWidth);
    }

    public HashSet<BoardPosition> ObstructingPositions(float step, Board board) {
        // TODO obs pos
        var set = new HashSet<BoardPosition>();
        // TODO use enumerator
        for (int i = 0; i < Mathf.CeilToInt(WidthForStep(step)); i++) {
            for (int j = 0; j < Mathf.CeilToInt(WidthForStep(step)); j++) {
                var bp = new BoardPosition(center.x + i, center.y + j);
                if (board.ContainsPosition(bp)) set.Add(bp);
            }
        }
        return set;
    }
}

public struct BoardPosition {
    public int x;
    public int y;

    public BoardPosition (int xx, int yy) {
        x = xx;
        y = yy;
    }

    public static bool operator ==(BoardPosition bp1, BoardPosition bp2) {
        return bp1.x == bp2.x && bp1.y == bp2.y;
    }

    public static bool operator !=(BoardPosition bp1, BoardPosition bp2) {
        return bp1.x != bp2.x || bp1.y != bp2.y;
    }
}

public class BoardState {
    public int iteration; // TODO better attr name
    public int width;
    public int height;
    public HashSet<BoardPosition> occupyablePositions;
    public BoardPosition positionTaken;
}

public class BoardSimulator {
}

public class BoardStateManager { // TODO better class name

}
