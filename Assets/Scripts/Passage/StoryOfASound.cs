using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;
using OscJack;

/***
 * wrapper for animation, customized for each scene/level
 */
public enum GraphicEntityMutexFlag {
    None           = 0,
    Position       = 1 << 0,
    Velocity       = 1 << 1,
    Rotation       = 1 << 2,
    Scale          = 1 << 3,
    Opacity        = 1 << 4,
    Color          = 1 << 5,
    Representation = 1 << 7,
}

public class GraphicEntity1 : MonoBehaviour {
    Board board;

    // how do i create an entity?
    // large row:
    // beat finds empty row
    // we
    public GraphicEntity1 New(GridRect gr, Board board, Color color) {
        this.board = board;
        board.LockTiles(gr);
        // display status hidden
        rp = GridRectToRectParams(gr, rp);
        NoteFactory.CreateRect(rp);
    }

    RectParams RectParamsFromGridRect(GridRect gr, RectParams rp = null) {
        if (rp != null) {
        }
    }

    public void Move(int x, int y) {
        // moves square by (x, y)
    }

    public void Transform(GridRect gr) {
        // moves and resizes square
    }

    public void RemoveOverlap(GridRect gr) {
        // create multiple GE and delete itself
    }

    public void Split(int width, int height) {
        // splits existing x to this as much as it can
    }

    void RunAnimation(string keyPath, AnimationCurve curve)
        var lockFlag = AnimationKeyPath.ToTileMutexFlag(keyPath);
        mutex |= lockFlag;
        animatable.AddAnimationCurve(keyPath, curve);
        float animFinishTime = 0;
        foreach (var k in curve.keys) {
            animFinishTime = Mathf.Max(animFinishTime, k.time);
        }

        StartCoroutine(C.WithDelay(() => { mutex ^= lockFlag; }, animFinishTime));
        if (propProbability.IsNonZero() && Random.value < propProbability) {
            var next = TileAtLocation(propLocation);
            if (next != null) {
                StartCoroutine(C.WithDelay(() => {
                    next.RunAnimation(keyPath, curve, propLocation, propProbability, propDelay);
                }, propDelay));
            }
        }
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

public class StoryOfASound : MonoBehaviour {

    public Color black;
    public Color red;
    public Color blue;
    public Color yellow;

    public TimeSignature timeSignature;
    public Camera camera;
    public CameraAnimatable cameraAnimatable;

    public int rows; // automatically decides the rest of the game
    int cols;

    Board1 board;

    void Start() {
        //CreateTiles();
        var sideLength = CameraHelper.Height / (1.414f * rows + 0.414f);
        cols = (int)(CameraHelper.Width / (1.414f * sideLength));
        board = new Board1(cols, rows);
        StartCoroutine(Run());
    }

    /*
    void CreateTiles() {

        board = Tile2.CreateBoard(cols, rows);
    }
    */

    void PlaceRow() {
        // search for vacant spots
        GridRect emptyRow = board.FindEmptyRow();
        if (emptyRow == null) {
            emptyRow = new GridRect(0, Random.Range(0, rows), cols, 1);
            foreach (var ge in board.graphicEntities) {
                ge.DeleteRect(randomRow, Beat(1));
            }
        }
        var ge = GraphicEntity.New(randomRow);
        ge.color = red;
        ge.FadeIn(Beat(1));
    }

    void BreakRect() {
        board.FindGraphicEntityWithSize();

    }

    void Rotate() {
        // search for vacant spots
        GridRect emptyRow = board.FindEmptyRow();
        if (emptyRow != null) {
            // gradually
            var ge = GraphicEntity.New(emptyRow);
        } else {

        }
    }

    IEnumerator Run() {
        // on every update,
        System.Action<Tile2, Location> f = (Tile2 target, Location l) => {};

        // find all lines empty
        bool[] emptyLines = new bool[cols + rows];
        for (int i = 0; i < rows + cols; i++) {
            Tile2[] line;
            if (i < cols) {
                // check if col is empty
                line = board.GetCol(i);
            } else {
                line = board.GetRow(i - col);
            }
            if (line.Count(t => t.IsLocked()) == 0) {
                emptyLines[i] = true;
            }
        }

        // if we have lines that are empty, pick a random line and fill it up
        if (emptyLines.Count(b => b) > 0) {
        }

        // else, pick any line
        // and chop off obj that crosses those cells


        f = (Tile2 target, Location loc) => {
            // can this automatically lock the target?
            target.RunAnimation(
                AnimationKeyPath.Opacity,
                AnimationCurveUtils.FromPairs(0, 1, NoteValueToDuration(0, 1), 0f, NoteValueToDuration(0, 6.9f), 0f, NoteValueToDuration(0,7.85f), 1)
            ); // this locks
            StartCoroutine(C.WithDelay(() => {
                var nextTarget = target.TileAtLocation(loc, TileMutexFlag.Opacity);
                if (nextTarget != null) {
                    f(nextTarget, loc);
                }
            }, NoteValueToDuration(0, 0.1f)));
        };
        // max length

        yield return Rest(2);

        foreach (var rest in Loop(6, 0, 1, 0)) {
            // Find target
            bool found = false;
            int runCount = 0;
            while (!found && runCount < 100) {
                int x = Random.Range(0, cols);
                int y = Random.Range(0, rows);
                var tile = board[x, y];
                if (!tile.IsLocked(TileMutexFlag.Opacity)) {
                    var randomAxis = Location.None;
                    f(tile, randomAxis);
                    found = true;
                }
                runCount++;
            }

            yield return rest;
        }

    }

    Tile2 RandomTile() {
        return (Tile2)board.GetValue2(Random.Range(0, board.Length));
    }

    IEnumerable<int> Times(int measures) {
        return Enumerable.Range(0, measures);
    }

    IEnumerable<IEnumerator> Loop(float measuresToLoop, float beatsToLoop, float measuresPerRest, float beatsPerRest) {
        var loopDuration = NoteValueToDuration(measuresToLoop, beatsToLoop);
        var restDuration = NoteValueToDuration(measuresPerRest, beatsPerRest);
        int loopCount = Mathf.RoundToInt(loopDuration / restDuration);

        for (int i = 0; i < loopCount; i++) {
            yield return Rest(measuresPerRest, beatsPerRest);
        }
    }

    IEnumerator Rest(float measures=1, float beats = 0) {
        yield return new WaitForSeconds(NoteValueToDuration(measures, beats));
    }

    float NoteValueToDuration(float measures, float beats) {
        return (measures * timeSignature.beatsPerMeasure + beats) * BeatDurationInSeconds;
    }

    float BeatDurationInSeconds {
        get {
            return 60f / timeSignature.beatsPerMinute;
        }
    }

    float Beat {
        get {
            return 60f / timeSignature.beatsPerMinute;
        }
    }
}
