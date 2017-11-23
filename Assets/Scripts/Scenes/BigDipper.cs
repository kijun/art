using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;


public class BigDipper : MonoBehaviour {

    public Color black;
    public Color red;
    public Color blue;
    public Color yellow;
    public Color green;
    public Color orange;
    public Color gray;
    public Color white;

    public UnityEngine.UI.Text title;
    public UnityEngine.UI.Text credits;
    public Timer timer;

    public TimeSignature timeSignature;
    public Camera camera;
    public CameraAnimatable cameraAnimatable;

    public int rows; // automatically decides the rest of the game
    int cols;

    GridRect boardRect;

    Board1 board;

    void Start() {
        Debug.Log("logggg");
        StartCoroutine(SetupAndLaunch());
    }

    IEnumerator SetupAndLaunch() {
        yield return null;
        var gap = 0.309f;
        //var gap = 0.1f;
        var sideLength = CameraHelper.Height / ((1+gap)*rows + gap);
        cols = (int)(CameraHelper.Width / ((1+gap) * sideLength));
        board = new Board1(cols, rows, sideLength, gap * sideLength);

        //Debug.Log($"Creating {rows} rows, {cols} cols");
        boardRect = new GridRect(0, 0, cols, rows);
        StartVisualization();
    }

    void StartVisualization() {
        StartCoroutine(RunDipper());
    }

    IEnumerator RunDipper() {
        foreach (var rest in Loop(40, 0, 2, 0)) {
            // create line
           StartCoroutine(CreateLine1());
           StartCoroutine(CreateLine1());
           StartCoroutine(CreateLine2());
           StartCoroutine(CreateLine3());
           yield return rest;
        }
    }

    Vector2 RV() {
        return RandomHelper.RandomVector2(-11, 11, -11, 11);
    }

    Vector2 RV2(float x) {
        return new Vector2(Random.Range(-8, 8), x);
        //return RandomHelper.RandomVector2(-11, 11, -11, 11);
    }

    IEnumerator CreateLine2() {
        var sp = new SplineParams();
        sp.spline = new BezierSpline2D(
                RV(), RV(), RV(), RV());
        sp.spline.AddCurve(RV(), RV(), RV());
        sp.spline.AddCurve(RV(), RV(), RV());
        sp.spline.AddCurve(RV(), RV(), RV());
        sp.color = Color.Lerp(blue, red, Random.value).WithAlpha(0);
        sp.width = Random.Range(0.02f, 0.2f);
        Animatable2[] anims = NoteFactory.CreateLine(sp);
        foreach (var l in anims) {
            l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.Linear(0, 0, 1, 1));
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        foreach (var l in anims) {
            //l.velocity = RandomHelper.RandomVector2(-0.5f, 0.5f, -0.5f, 0.5f);
            //l.angularVelocity = Random.Range(-90f, 90f);
            l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.EaseInOut(0, 1, 3, 0));
            l.DestroyIn(4f);
        }
    }

    IEnumerator CreateLine3() {
        var sp = new SplineParams();
        sp.spline = new BezierSpline2D(
                RV(), RV(), RV(), RV());
        sp.spline.AddCurve(RV(), RV(), RV());
        sp.color = Color.Lerp(blue, red, Random.value).WithAlpha(0);
        sp.width = Random.Range(0.02f, 0.2f);
        Animatable2[] anims = NoteFactory.CreateLine(sp);
        foreach (var l in anims) {
            l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.Linear(0, 0, 1, 1));
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        foreach (var l in anims) {
            //l.velocity = RandomHelper.RandomVector2(-0.5f, 0.5f, -0.5f, 0.5f);
            //l.angularVelocity = Random.Range(-90f, 90f);
            l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.EaseInOut(0, 1, 3, 0));
            l.DestroyIn(4f);
        }
    }

    IEnumerator CreateLine1() {
        var sp = new SplineParams();
        /*
        sp.spline = new BezierSpline2D(
                RV(), RV(), RV(), RV());
        sp.spline.AddCurve(RV(), RV(), RV());
        sp.spline.AddCurve(RV(), RV(), RV());
        sp.spline.AddCurve(RV(), RV(), RV());
        */
        /*
        sp.spline = new BezierSpline2D(
                RV2(-10), RV2(-9), RV2(-8), RV2(-7));
        sp.spline.AddCurve(RV2(-6), RV2(-5), RV2(-4));
        sp.spline.AddCurve(RV2(-3), RV2(-2), RV2(-1));
        sp.spline.AddCurve(RV2(0), RV2(1), RV2(2));
        sp.spline.AddCurve(RV2(3), RV2(4), RV2(5));
        sp.spline.AddCurve(RV2(6), RV2(7), RV2(8));
        sp.spline.AddCurve(RV2(9), RV2(10), RV2(11));
        */
        sp.spline = new BezierSpline2D(
                RV2(-9), RV2(-7), RV2(-5), RV2(-2));
        sp.spline.AddCurve(RV2(2), RV2(6), RV2(10));
        /*
        sp.spline.AddCurve(RV2(0), RV2(1), RV2(2));
        sp.spline.AddCurve(RV2(3), RV2(4), RV2(5));
        sp.spline.AddCurve(RV2(6), RV2(7), RV2(8));
        sp.spline.AddCurve(RV2(9), RV2(10), RV2(11));
        */
        //sp.spline.AddCurve(RV(), RV(), RV());
        //sp.spline.AddCurve(RV(), RV(), RV());
        //sp.spline.AddCurve(RV(), RV(), RV());
        sp.color = Color.Lerp(blue, red, Random.value).WithAlpha(0);
        sp.width = Random.Range(0.02f, 0.2f);
        Animatable2[] anims = NoteFactory.CreateLine(sp);
        foreach (var l in anims) {
            l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.Linear(0, 0, 1, 1));
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        foreach (var l in anims) {
            //l.velocity = RandomHelper.RandomVector2(-0.5f, 0.5f, -0.5f, 0.5f);
            //l.angularVelocity = Random.Range(-90f, 90f);
            l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.EaseInOut(0, 1, 3, 0));
            l.DestroyIn(4f);
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

        var newGe = GraphicEntity1.New(emptyRow, board);
        newGe.SetColor(color);
        //newGe.SetOpacity(1, Beat(1));
        return true;
    }

    void AddSquareAtPosition(int x, int y, Color color) {
        var ge = GraphicEntity1.New(new GridRect(x, y, 1, 1), board);
        ge.SetColor(color);
    }

    GraphicEntity1 AddRect(int width, int height, Color color, bool allowStacking = false) {
        GridRect emptyRect = board.FindEmptyRectWithSize(width, height);
        if (emptyRect == null && allowStacking) {
            emptyRect = board.FindRandomRectWithSize(width, height);
        }
        if (emptyRect != null) {
            var ge = GraphicEntity1.New(emptyRect, board);
            ge.SetColor(color);
            //ge.SetOpacity(opacity, Beat(1));
            return ge;
        }
        return null;
    }

    GraphicEntity1 AddRectAtPosition(int minX, int minY, int width, int height, Color color) {
        GridRect emptyRect = new GridRect(minX, minY, width, height);
        if (emptyRect != null) {
            var ge = GraphicEntity1.New(emptyRect, board);
            ge.SetColor(color);
            ge.SetOpacity(1, Beat(1));
            return ge;
        }
        return null;
    }

    IEnumerator SectionClosing() {
        yield return Rest(122, 0);
        AddSquareAtPosition(2, rows - 1, blue);
        yield return Rest(0, 1);
        AddSquareAtPosition(3, rows - 1, blue);
        yield return Rest(0, 1);
        AddSquareAtPosition(4, rows - 1, blue);
        yield return Rest(0, 1);
        AddSquareAtPosition(5, rows - 1, blue);
        yield return Rest(0, 1);
        AddSquareAtPosition(5, 0, orange);
        yield return Rest(0, 1);
        AddSquareAtPosition(4, 0, orange);
        yield return Rest(0, 1);
        AddSquareAtPosition(3, 0, orange);
        yield return Rest(0, 1);
        AddSquareAtPosition(2, 0, orange);
        yield return Rest(0, 1);
        credits.gameObject.active = true;
    }

    /*** ROUTINE ***/
    IEnumerator FnChangeColor(int durationInBeats, int minRest, int maxRest, Color color) {
        int beatsRested = 0;
        while (beatsRested < durationInBeats) {
            board.RandomGraphicEntity().SetColor(color);
            var beatsToRest = Random.Range(minRest, maxRest);
            beatsRested += beatsToRest;
            yield return Rest(0, beatsToRest);
        }
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
