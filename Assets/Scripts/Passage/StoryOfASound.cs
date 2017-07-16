using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;
using OscJack;


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

    void Dispatch() {
        // action
        // but how do you know whether an action is capable or not?
        // random - return
    }

    bool AddRow() {
        // search for vacant spots
        // MUTEX
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

    bool BreakRect() {
        // MUTEX
        var g = board.FindGraphicWithSizeGreaterThan(1, 1);
        if (g != null) {
            g.Subdivide();
            // to smallest?
        }
    }

    bool HideAndReveal() {
        // MUTEX
        var g = board.FindGraphicWithSizeGreaterThan(0, 0);
        g.FadeOutAndIn(Beat(1), Beat(1));
    }

    bool Rotate() {
        // search for vacant spots
        var g = board.FindSquareGraphicWithSideGreaterThan(0);
        g.Rotate(360, Beat(1));
    }

    bool MoveToAdjacent() {
        // search for vacant spots
        foreach (var g in board.graphicEntities) {
            var target = board.FindAdjacentEmptyRect(g.rect);
            if (target != null) {
                g.MoveAndResize(emptyRect);
                return true;
            }
        }
        return false;
    }

    bool Shrink() {
        var g = board.FindGraphicWithSizeGreaterThan(1, 1);
        if (g != null) {
            g.MoveAndResize(g.rect.Subrect());
            return true;
        }
        return false;
    }

    bool Expand() {
        foreach (var g in board.graphicEntities) {
            var target = board.FindExpandedRect(g.rect);
            if (target != null) {
                g.MoveAndResize(emptyRect);
                break;
            }
        }
        return false;
    }

    bool Remove() {
        var g = board.RandomGraphicEntity();
        if (g != null) {
            g.Remove();
            return true;
        }
        return false;
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
