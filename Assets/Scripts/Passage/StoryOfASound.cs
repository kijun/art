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
    public Color orange;
    public Color white;

    Color[] blues;

    public TimeSignature timeSignature;
    public Camera camera;
    public CameraAnimatable cameraAnimatable;

    public int rows; // automatically decides the rest of the game
    int cols;

    GridRect boardRect;

    Board1 board;

    void Start() {
        //CreateTiles();
        var sideLength = CameraHelper.Height / (1.414f * rows + 0.414f);
        cols = (int)(CameraHelper.Width / (1.414f * sideLength));
        //Debug.Log($"Creating {rows} rows, {cols} cols");
        board = new Board1(cols, rows, sideLength, 0.414f * sideLength);
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
        //StartCoroutine(Run());
        //StartCoroutine(Run2());
        StartCoroutine(Section1());
        StartCoroutine(Section2());
        StartCoroutine(Section3());

        //AddRow();
        //StartCoroutine(Run());
    }

    void FFixedUpdate() {
        if (OscMaster.HasData("/Velocity1")) {
            foreach (var x in OscMaster.GetData("/Velocity1")) {
                var val = float.Parse(x+"");
                if (val.IsNonZero()) {
                    Debug.Log(val);
                    //AddRect(1,1);
                }
            }
            OscMaster.ClearData("/Velocity1");
        }
        if (OscMaster.HasData("/Note1")) {
            foreach (var x in OscMaster.GetData("/Note1")) {
            }
            OscMaster.ClearData("/Note1");
        }
    }

    void UUpdate() {
        if (OscMaster.HasData("/Velocity1")) {
            foreach (var x in OscMaster.GetData("/Velocity1")) {
                var val = float.Parse(x+"");
                if (val.IsNonZero()) {
                    Debug.Log(val);
                    //AddRect(1,1);
                }
            }
            OscMaster.ClearData("/Velocity1");
        }
        if (OscMaster.HasData("/Note1")) {
            foreach (var x in OscMaster.GetData("/Note1")) {
            }
            OscMaster.ClearData("/Note1");
        }
    }


    void Dispatch() {
        // action
        // but how do you know whether an action is capable or not?
        // random - return
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

    GraphicEntity1 AddRect(int width, int height, Color color) {
        GridRect emptyRect = board.FindEmptyRectWithSize(width, height);
        if (emptyRect != null) {
            var ge = GraphicEntity1.New(emptyRect, board);
            ge.SetColor(color);
            ge.SetOpacity(1, Beat(1));
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

    /***** SECTION 3 *****/
    IEnumerator Section3() {
        yield return Rest(49, 0);
        board.GraphicEntities().ForEach(g => g.Remove(Beat(2f)));
        yield return Rest(1, 0);
        StartCoroutine(Section3Orange());
        StartCoroutine(Section3Shatter());
        StartCoroutine(Section3Delete());
        StartCoroutine(Section3Movement());
        //board.FindAllGraphicsWithSize(1, 1).ForEach(g => .SetOpacity(0.65f, Beat(1)));
        foreach (var rest in Loop(22, 0, 1, 0)) {
            AddRect(Random.Range(1, 4), Random.Range(1, 4), blue);
            yield return rest;
        }
    }

    IEnumerator Section3Movement() {
        yield return Rest(4, 1);
        foreach (var rest in Loop(24, 0, 1, 0)) {

            //
            var g = board.FindRandomGraphicWithSize(1, 1);

            var rand = Random.value;
            if (rand < 0.25f) {
                g.Move(1, 0, Beat(1.5f));
            } else if (rand < 0.50f) {
                g.Move(-1, 0, Beat(1.5f));
            } else if (rand < 0.75f) {
                g.Move(0, 1, Beat(1.5f));
            } else {
                g.Move(0, -1, Beat(1.5f));
            }

            yield return rest;
        }
    }

    IEnumerator Section3Shatter() {
        yield return Rest(2, 1);
        foreach (var rest in Loop(19, 3, 2, 0)) {
            var g = board.FindGraphicWithSizeGreaterThan(1, 1);
            if (g != null) g.BreakToUnitSquares();
            yield return rest;
        }
    }

    IEnumerator Section3Delete() {
        yield return Rest(5, 0);
        foreach (var rest in Loop(19, 3, 0, 3)) {
            var g = board.FindRandomGraphicWithSize(1, 1);
            if (g != null) g.Remove(Beat(2));
            yield return rest;
        }
    }

    IEnumerator Section3Orange() {
        yield return Rest(3, 2);
        int beatsRested = 0;
        while (beatsRested < 4*16) {
            board.RandomGraphicEntity().SetColor(orange);
            var beatsToRest = Random.Range(7, 12);
            beatsRested += beatsToRest;
            yield return Rest(0, beatsToRest);
        }
    }

    /***** SECTION 2 *****/
    // 0 refresh
    // 1 rotation effect, color change
    // 2 composition
    // 3 refresh
    IEnumerator Section2() {
        StartCoroutine(Section2Rotation());
        StartCoroutine(Section2Cascade());
        StartCoroutine(Section2Shape());
        yield return Rest(24, 0);
        board.FindAllGraphicsWithSize(1, 1).ForEach(g => g.SetOpacity(1, Beat(1)));
        yield return Rest(0, 3);
    }

    IEnumerator Section2Cascade() {
        yield return Rest(24, 3);
        float beatsRested = 0;
        while (beatsRested <= 4*22) {
            var rand = Random.value;
            if (rand < 0.33f) {
                CascadeLeft();
            } else if (rand < 0.67f) {
                CascadeRight();
            } else {
                CascadeTop();
            }
            //var beatsToRest = Random.Range(4*2, 4*2.8f);
            var beatsToRest = 8;//Random.Range(4*2, 4*2.8f);
            yield return Rest(0, beatsToRest);
            beatsRested += beatsToRest;
        }
    }

    IEnumerator Section2Shape() {
        yield return Rest(24, 2);
        foreach (var rest in Loop(23, 1, 2, 0)) {
            board.FindAllGraphicsWithSize(1, 1).ToArray().Shuffle().Take(30).ForEach(g => g.SetOpacity(0, Beat(0.3f)));
            yield return rest;
        }
    }

    void CascadeRight() {
        for (int i = 0; i < rows; i++) {
            int index = 1;
            foreach (var g in board.FindGraphicsForRow(i)) {
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(0, Beat(0.2f));
                }, Beat(index * 0.3f)));
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(1, Beat(1f));
                }, Beat(index * 0.3f+0.3f)));
                index++;
            };
        }
    }

    void CascadeLeft() {
        for (int i = 0; i < rows; i++) {
            int index = 1;
            foreach (var g in board.FindGraphicsForRow(i).Reverse()) {
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(0, Beat(0.2f));
                }, Beat(index * 0.3f)));
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(1, Beat(1f));
                }, Beat(index * 0.3f+0.3f)));
                index++;
            };
        }
    }

    void CascadeTop() {
        for (int i = 0; i < cols; i++) {
            int index = 1;
            foreach (var g in board.FindGraphicsForColumn(i)) {
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(0, Beat(0.2f));
                }, Beat(index * 0.3f)));
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(1, Beat(1f));
                }, Beat(index * 0.3f+0.3f)));
                index++;
            };
        }
    }

    IEnumerator Section2Rotation() {
        yield return Rest(24, 1);
        int beatsRested = 0;
        while (beatsRested < 4*24-1) {
            board.FindRandomGraphicWithSize(1, 1).RotateTo(360, Beat(2));
            var beatsToRest = Random.Range(4, 4);
            beatsRested += beatsToRest;
            yield return Rest(0, beatsToRest);
        }
    }

    IEnumerator Section1Orange() {
        yield return Rest(8, 0);
        int beatsRested = 0;
        while (beatsRested < 4*16) {
            board.FindAllGraphicsWithSize(1, 1).ToArray().Shuffle().Take(Random.Range(1, 2)).ForEach(g => g.SetColor(orange));
            var beatsToRest = Random.Range(5, 7);
            beatsRested += beatsToRest;
            yield return Rest(0, beatsToRest);
        }
    }

    IEnumerator Section1Fade() {
        yield return Rest(12, 0);
        foreach (var rest in Loop(12, 0, 2, 0)) {
            int index = 0;
            foreach (var g in board.FindGraphicsForRow(Random.Range(0, rows))) {
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(1, Beat(0.5f));
                }, Beat(index * 0.3f)));
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(0, Beat(1));
                }, Beat(index * 0.3f + 2f)));
                index++;
            };
            yield return rest;
        }
    }

    IEnumerator Section1() {
        StartCoroutine(Section1Orange());
        StartCoroutine(Section1Fade());
        var rect = AddRect(cols, rows, blues[0]);
        rect.BreakToUnitSquares();
        yield return Rest(2, 0);
        foreach (var rest in Loop(24, 0, 4, 0)) {
            var g = board.FindRandomGraphicWithSize(1, 1);
            //g.SetOpacity(g.opacity + 0.5f, Beat(0.4f));

            //g = board.FindRandomGraphicWithSize(1, 1);
            List<GraphicEntity1> ge = new List<GraphicEntity1>();
            ge.Add(g);
            for (int i = 0; i < 9; i++) { // beats - 1
                foreach (var nextGE in board.FindAdjacentGraphics(g.rect)) {
                    if (!ge.Contains(nextGE)) {
                        ge.Add(nextGE);
                        g = nextGE;
                        break;
                    }
                }
                Debug.Log(g);
            }

            int ij = 0;
            foreach (var gr in ge) {
                StartCoroutine(C.WithDelay(() => {
                    var currOpacity = gr.opacity;
                    if (gr.color.RGBEquals(orange)) {
                        gr.SetColor(blue.WithAlpha(1f/2f));
                    } else {
                        gr.SetOpacity(Mathf.Min(1, currOpacity + 1f/2f));
                    }
                    // what i want - start with new opacity, fade down
                }, Beat(ij*2)));
                ij++;
            }
            yield return rest;
        }
    }

    IEnumerator Run2() {
        yield return Rest(60);
        foreach (var rest in Loop(32, 0, 1, 0)) {
            yield return rest;
        }
    }

    IEnumerator Run() {
        //var rect = AddRect(cols, rows, blue);
        //rect.BreakToUnitSquares();
        //yield return Rest(2, 0);
        //foreach (var rest in Loop(16, 0, 1, 0)) {
            //board.FindRandomGraphicWithSize(1, 1).SetColor(orange);
        //    board.FindRandomGraphicWithSize(1, 1).SetOpacity(0, Beat(1));
            /*
            var g = board.FindRandomGraphicWithSize(1, 1);
            if (g.rect.min.x > 0) {
                g.Move(-1, 0, Beat(4));
            }
            */
        //    yield return rest;
        //}
        foreach (var rest in Loop(32, 0, 1, 0)) {
            AddRect(Random.Range(1, 4), Random.Range(1, 4), blue);
            yield return rest;
        }
        /*
        AddRect(3, 3, white);
        AddRect(2, 2, white);
        AddRect(2, 2, white);
        AddRect(1, 1, white);
        AddRect(1, 2, white);
        AddRect(2, 1, white);
        yield return Rest(0, 1);
        AddRect(1, 1, blue);
        yield return Rest(0, 1);
        AddRect(1, 1, red);
        yield return Rest(0, 1);
        AddRect(1, 1, blue);
        yield return Rest(0, 1);
        AddRect(1, 1, white);
        yield return Rest(0, 1);
        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            if (g.rect.min.y < 2) {
                g.Move(0, 1, Beat(1));
            } else {
                g.Move(0, -1, Beat(2));
            }
        }
        yield return Rest(0, 1);
        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            g.SetOpacity(0.5f, Beat(2));
        }
        yield return Rest(0, 2);
        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            g.SetOpacity(1, Beat(2));
        }
        // TODO find all graphic with size
        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            if (g.rect.width == 1 && g.rect.height == 1) {
                g.Rotate(225, Beat(3));
            }
        }
        yield return Rest(0, 3);
        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            g.Rotate(360, Beat(3));
        }
        yield return Rest(0, 4);
        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            var target = new GridRect(g.rect);
            target.max = new Coord(target.min).Move(1, 1);
            g.Transform(target, Beat(4));
        }
        yield return Rest(0, 5);
        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            g.Remove();
        }
        yield return Rest(0, 1);
        var r1 = AddRectAtPosition(0, 0, 1, 1, red);
        yield return Rest(0, 1);
        var r2 = AddRectAtPosition(1, 0, 1, 1, yellow);
        yield return Rest(0, 1);
        r1.Merge(r2, Beat(2));
        yield return Rest(0, 3);
        r1.Remove();
        yield return Rest(0, 1);
        var rbig = AddRectAtPosition(0, 0, 4, 4, blue);
        yield return Rest(0, 1);
        rbig.BreakToUnitSquares();
        yield return Rest(0, 1);

        foreach (GraphicEntity1 g in board.GraphicEntities()) {
            g.Transform(new GridRect(0, 0, 1, 1), Beat(2));
        }

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
