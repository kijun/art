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
        Debug.Log($"Creating {rows} rows, {cols} cols");
        board = new Board1(cols, rows);
        //StartCoroutine(Run());
        AddRow();
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

    bool AddRow(bool force=false) {
        // search for vacant spots
        // MUTEX
        Debug.Log("StoryOfASound: Looking for empty row");
        GridRect emptyRow = board.FindEmptyRow();
        Debug.Log("Found empty row" + emptyRow);
        if (emptyRow == null) {
            emptyRow = new GridRect(0, Random.Range(0, rows), cols, 1);
            foreach (var ge in board.graphicEntities) {
                ge.DeleteRect(emptyRow, Beat(1));
            }
        }

        var newGe = GraphicEntity1.New(emptyRow, board);
        newGe.SetColor(red);
        newGe.SetOpacity(Beat(1));
        return true;
    }

    bool AddRect(int width, int height) {
        GridRect emptyRect = board.FindEmptyRectWithSize(width, height);
        if (emptyRect != null) {
            var ge = GraphicEntity1.New(emptyRect, board);
            ge.SetColor(red);
            ge.SetOpacity(Beat(1));
            return true;
        }
        return false;
    }

    bool BreakRect() {
        // MUTEX
        var g = board.FindGraphicWithSizeGreaterThan(1, 1);
        if (g != null) {
            //g.Subdivide();
            // to smallest?
        }
        return false;
    }

    bool HideAndReveal() {
        // MUTEX
        var g = board.FindGraphicWithSizeGreaterThan(0, 0);
        //g.FadeOutAndIn(Beat(1), Beat(1));
        return false;
    }

    bool Rotate() {
        // search for vacant spots
        var g = board.FindSquareGraphicWithSideGreaterThan(0);
        //g.Rotate(360, Beat(1));
        return false;
    }

    bool MoveToAdjacent() {
        // search for vacant spots
        foreach (var g in board.graphicEntities) {
            var target = board.FindAdjacentEmptyRect(g.rect);
            if (target != null) {
                //g.MoveAndResize(emptyRect);
                return true;
            }
        }
        return false;
    }

    bool Shrink() {
        var g = board.FindGraphicWithSizeGreaterThan(1, 1);
        if (g != null) {
            //g.MoveAndResize(g.rect.Subrect());
            return true;
        }
        return false;
    }

    bool Expand() {
        foreach (var g in board.graphicEntities) {
            var target = board.FindExpandedRect(g.rect);
            if (target != null) {
                //g.MoveAndResize(emptyRect);
                break;
            }
        }
        return false;
    }

    bool Remove() {
        var g = board.RandomGraphicEntity();
        if (g != null) {
            //g.Remove();
            return true;
        }
        return false;
    }

    IEnumerator Run() {
        yield return Rest(0, 1);
        foreach (var g in board.graphicEntities) {
            g.Move(0, 1);
        }
        yield return null;
        /*
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
        */

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

    float Beat(float beats) {
        //return beats * BeatDurationInSeconds;
        return beats * 60f / timeSignature.beatsPerMinute;
    }
}
