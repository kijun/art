using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;


public class Variations : MonoBehaviour {

    public Color black;
    public Color red;
    public Color blue;
    public Color yellow;
    public Color orange;
    public Color white;

    Color[] blues;
    Color[] oranges;

    public TimeSignature timeSignature;
    public Camera camera;
    public CameraAnimatable cameraAnimatable;

    public int rows; // automatically decides the rest of the game
    int cols;

    GridRect boardRect;

    Board2 board;

    void Start() {
        StartCoroutine(SetupAndLaunch());
    }

    IEnumerator SetupAndLaunch() {
        yield return null;
        var gap = 0.309f;
        //var gap = 0.1f;
        var sideLength = CameraHelper.Height / ((1+gap)*rows + gap);
        cols = (int)(CameraHelper.Width / ((1+gap) * sideLength));
        board = new Board2(cols, rows, sideLength, gap * sideLength);

        //Debug.Log($"Creating {rows} rows, {cols} cols");
        boardRect = new GridRect(0, 0, cols, rows);
        blues = new Color[9];
        blues[0] = blue.WithAlpha(0);
        blues[1] = blue.WithAlpha(0.125f);
        blues[2] = blue.WithAlpha(0.125f*2);
        blues[3] = blue.WithAlpha(0.125f*3);
        blues[4] = blue.WithAlpha(0.125f*4);
        blues[5] = blue.WithAlpha(0.125f*5);
        blues[6] = blue.WithAlpha(0.125f*6);
        blues[7] = blue.WithAlpha(0.125f*7);
        blues[8] = blue;

        oranges = new Color[9];
        oranges[0] = orange.WithAlpha(0);
        oranges[1] = orange.WithAlpha(0.125f);
        oranges[2] = orange.WithAlpha(0.125f*2);
        oranges[3] = orange.WithAlpha(0.125f*3);
        oranges[4] = orange.WithAlpha(0.125f*4);
        oranges[5] = orange.WithAlpha(0.125f*5);
        oranges[6] = orange.WithAlpha(0.125f*6);
        oranges[7] = orange.WithAlpha(0.125f*7);
        oranges[8] = orange;
        //StartCoroutine(Run());
        //StartCoroutine(Run2());
        //AddRow();
        //StartCoroutine(Run());
        StartVisualization();
    }

    void StartVisualization() {
        //StartCoroutine(Core());
        StartCoroutine(Section4());
        /*
        StartCoroutine(Section1());
        StartCoroutine(Section2());
        StartCoroutine(Section3());
        StartCoroutine(SectionClosing());
        */
    }

    IEnumerator Core() {
        foreach (var rest in Loop(64, 0, 0, 2)) {
            AddRect(Random.Range(0, cols/3), Random.Range(0, rows/3), blues[Random.Range(1, 9)], true);
            yield return rest;
        }
    }

    bool AddRow(Color color, bool force=false) {
        // search for vacant spots
        // MUTEX
        Debug.Log("StoryOfASound: Looking for empty row");
        GridRect emptyRow = board.FindEmptyRow();
        Debug.Log("Found empty row" + emptyRow);
        if (emptyRow == null) {
            emptyRow = new GridRect(0, Random.Range(0, rows), cols, 1);
            foreach (var ge in board.GraphicEntities()) {
                ge.DeleteRect(emptyRow, Beat(1));
            }
        }

        var newGe = GraphicEntity2.New(emptyRow, board);
        newGe.SetColor(color);
        //newGe.SetOpacity(1, Beat(1));
        return true;
    }

    GraphicEntity2 AddRect(int width, int height, Color color, bool allowStacking = false) {
        Debug.Log(width + " " + height);
        GridRect emptyRect = board.FindEmptyRectWithSize(width, height);
        if (emptyRect == null && allowStacking) {
            emptyRect = board.FindRandomRectWithSize(width, height);
        }
        if (emptyRect != null) {
            var ge = GraphicEntity2.New(emptyRect, board);
            ge.SetColor(color);
            //ge.SetOpacity(opacity, Beat(1));
            return ge;
        }
        return null;
    }

    GraphicEntity2 AddRectAtPosition(int minX, int minY, int width, int height, Color color) {
        GridRect emptyRect = new GridRect(minX, minY, width, height);
        if (emptyRect != null) {
            var ge = GraphicEntity2.New(emptyRect, board);
            ge.SetColor(color);
            ge.SetOpacity(1, Beat(1));
            return ge;
        }
        return null;
    }

    /***** SECTION 4 FINAL *****/
    IEnumerator Section4() {
        //yield return Rest(73, 0);
        //StartCoroutine(Section4StoryOfASound());
        //24 -> 37 // probability
        List<GA2> actions = null;
        while (true) {
            actions = GenerateActions(actions);
            var g = AddRectAtPosition(cols/2, rows/2, Random.Range(2, 2), Random.Range(2, 2), blues[5]);
            StartCoroutine(Section4StoryOfASound(g, actions));
            while (IsAlive()) {
                yield return Rest(1);
            }
            // TODO display status
            // TODO fade, end level
            // if dead
            //StartCoroutine(Section4StoryOfASound(actions));
        }

        // REPEAT ONLY ON CERTAIN CONDITION (and nearest beat)
        /*
        foreach (var rest in Loop(40, 0, 1, 0)) {
            StartCoroutine(Section4StoryOfASound());
            yield return rest;
        }
        */
        yield return null;
    }

    bool IsAlive() {
        // Die after certain number of measures
        return true;
    }

    IEnumerator Section4StoryOfASound(GraphicEntity2 ge, List<GA2> actions) {
        yield return null;
        ge.ApplyActions(actions);
    }

    List<GA2> GenerateActions(List<GA2> prevActions) {
        var a = new List<GA2>();
        if (prevActions == null) {
            //var b = new GAParam2<float>(Beat(1));
            var b = Beat(2);
            a.Add(Move(2, 0, b));
            a.Add(Move(-2, Range(-2, 3), b));
            a.Add(Move(0, Range(-2, 3), b));
            a.Add(Move(0, Range(-2, 3), b));
            a.Add(RotateFor(30, b));
            a.Add(RestA(b));
            a.Add(new GA2 {
                type = GA2Type.Transform,
                rect = new GAParam2<GridRect>(g => g.rect.Translate(Range(-2, 3), Range(-2, 3)).Resize(Range(1, 4), Range(1, 4))),
                duration = new GAParam2<float>(b)
            });
            a.Add(IncreaseOpacity(-0.17f, b));
            a.Add(new GA2 {
                type = GA2Type.Remove,
                conditional = ge => ge.opacity.IsZero()
            });
            a.Add(RotateTo(0, b));
            var sp = Split(b);
            sp.probability = new GAParam2<float>(0.5f);
            a.Add(sp);
            a.Add(RestA(b/2));
            var cc = ChangeColor(orange, b);
            cc.probability = new GAParam2<float>(.25f);
            a.Add(cc);
            a.Add(Move(Range(-1,2), Range(-1,2), b));
            a.Add(Move(Range(-1,2), Range(-1,2), b));
            a.Add(RotateFor(Range(0, 12) * 30, b));
            a.Add(Move(Range(-1,2), Range(-1,2), b));
            a.Add(Move(Range(-1,2), Range(-1,2), b));
            a.Add(RotateFor(Range(0, 12) * 30, b));
            a.Add(new GA2 {
                type = GA2Type.Remove,
                duration = new GAParam2<float>(b * 2),
                probability = new GAParam2<float>(0.6f)
            });
            a.Add(new GA2 {
                type = GA2Type.Repeat,
                duration = new GAParam2<float>(b),
            });
            // Add movements
            // Add deletions
            // Add transforms
            // Add color change
            // Add opacity change
            // Add rotation
            // Add rest
            // Add splits - one one is too
            // Add merges
            // Add duplications
        } else {
            // modify
            a = prevActions;
        }
        return a;
    }

    float Range(float x, float y) {
        return Random.Range(x, y);
    }

    int Range(int x, int y) {
        return Random.Range(x, y);
    }

    GA2 Move(int x, int y, float duration = 0) {
        return new GA2 {
            type = GA2Type.Movement,
            a1 = new GAParam2<float>(x),
            a2 = new GAParam2<float>(y),
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 RotateFor(float degrees, float duration) {
        return new GA2 {
            type = GA2Type.RotateFor,
            a1 = new GAParam2<float>(degrees),
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 RotateTo(float degrees, float duration) {
        return new GA2 {
            type = GA2Type.RotateTo,
            a1 = new GAParam2<float>(degrees),
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 IncreaseOpacity(float op, float duration) {
        return new GA2 {
            type = GA2Type.IncreaseOpacity,
            a1 = new GAParam2<float>(g => g.opacity + op),
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 ChangeOpacity(float op, float duration) {
        return new GA2 {
            type = GA2Type.Opacity,
            a1 = new GAParam2<float>(op),
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 Remove(float duration) {
        return new GA2 {
            type = GA2Type.Remove,
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 Repeat(float duration) {
        return new GA2 {
            type = GA2Type.Repeat,
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 RestA(float duration) {
        return new GA2 {
            type = GA2Type.Rest,
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 ChangeColor(Color color, float duration) {
        return new GA2 {
            type = GA2Type.ColorChange,
            color = new GAParam2<Color>(color),
            duration = new GAParam2<float>(duration)
        };
    }

    GA2 Split(float duration) {
        return new GA2 {
            type = GA2Type.Split,
            duration = new GAParam2<float>(duration)
        };
    }


    /****** FUNTIONAL *****/
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

    int MeasureToBeats(int measures, int beats = 0) {
        return measures * timeSignature.beatsPerMeasure + beats;
    }

    float Beat(float beats) {
        //return beats * BeatDurationInSeconds;
        return beats * 60f / timeSignature.beatsPerMinute;
    }

    void Run(IEnumerator rest, IEnumerator function) {
        StartCoroutine(WithRest(rest, function));
    }

    void Run(IEnumerator function) {
        StartCoroutine(function);
    }

    IEnumerator WithRest(IEnumerator rest, IEnumerator function) {
        yield return rest;
        yield return function;
    }


    /***** PARKING LOT *****/
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
        foreach (var g in board.GraphicEntities()) {
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
        foreach (var g in board.GraphicEntities()) {
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

}
