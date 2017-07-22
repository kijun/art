using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;


public class StoryOfASound : MonoBehaviour {

    public Color black;
    public Color red;
    public Color blue;
    public Color yellow;
    public Color orange;
    public Color white;

    Color[] blues;
    Color[] oranges;
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
        //CreateTiles();
        /*var sideLength = CameraHelper.Height / (1.414f * rows + 0.414f);
        cols = (int)(CameraHelper.Width / (1.414f * sideLength));
        board = new Board1(cols, rows, sideLength, 0.414f * sideLength);
        */

        var gap = 0.309f;
        //var gap = 0.1f;
        var sideLength = CameraHelper.Height / ((1+gap)*rows + gap);
        cols = (int)(CameraHelper.Width / ((1+gap) * sideLength));
        board = new Board1(cols, rows, sideLength, gap * sideLength);

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

        OSCHandler.Instance.Init();
        //AddRow();
        //StartCoroutine(Run());
    }

    IEnumerator HideTitle() {
        yield return Rest(1, 2);
        title.gameObject.active = false;
    }

    bool started = false;

    void Update() {
        if (!started) {
            OSCHandler.Instance.UpdateLogs();

            if(OSCHandler.Instance.Servers["Visuals"].log.Count > 0) {
                Debug.Log("received ping");
                started = true;
                StartVisualization();
            }
        }
    }

    void StartVisualization() {
        StartCoroutine(HideTitle());
        StartCoroutine(Section1());
        StartCoroutine(Section2());
        StartCoroutine(Section3());
        StartCoroutine(Section4());
        StartCoroutine(SectionClosing());
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

    /***** SECTION 4 FINAL *****/
    IEnumerator Section4() {
        yield return Rest(73, 0);
        GameObject.FindObjectsOfType<GraphicEntity1>().ForEach(g => g.Remove(Beat(1.9f)));
        yield return Rest(1, 0);
        //StartCoroutine(Section4StoryOfASound());
        //24 -> 37 // probability
        foreach (var rest in Loop(40, 0, 1, 0)) {
            StartCoroutine(Section4StoryOfASound());
            yield return rest;
        }
    }

    IEnumerator Section4StoryOfASound() {
        var g = AddRect(Random.Range(1, 1), Random.Range(1, 1), blues[8], allowStacking:false);
        yield return Rest(0, 1.25f);
        g.Move(Coord.FromDirection(DirectionHelper.Random), Beat(1));
        yield return Rest(0, 1.25f);
        g.RotateFor(360, Beat(1));
        yield return Rest(0, 1.25f);
        /*
        //g.SetOpacity(0, Beat(1));
        yield return Rest(0, 1.25f);
        //g.SetOpacity(blues[7].a, Beat(1));
        yield return Rest(0, 1.25f);
        */
        if (g != null) StartCoroutine(Section4Lifecycle(g));
    }

    IEnumerator Section4Lifecycle(GraphicEntity1 g) {
        var targetRect = new GridRect(g.rect.min.x - 1, g.rect.min.y - 1, Random.Range(1, 5), Random.Range(1, 4));
        g.Transform(targetRect, Beat(1));
        if (targetRect.area > 1) {
            g.SetOpacity(Mathf.Max(0, g.opacity - 0.17f), Beat(1));
        }
        yield return Rest(0, 1.25f);
        if (g.opacity.IsZero()) {
            g.Remove();
            yield break;
        }
        // Break to unit squares
        g.RotateTo(0, Beat(1));
        yield return Rest(0, 2f);
        var squares = g.BreakToUnitSquares();
        yield return Rest(0, 1.25f);
        var rot = Random.Range(0, 12) * 30;
        foreach (var unit in squares) {
            var colorRand = Random.value;
            if (colorRand < 0.45f) {
                unit.SetColor(orange.WithAlpha(unit.opacity));
            }
            //else if (colorRand < 0.66f) {
                //unit.SetColor(blue.WithAlpha(unit.opacity));
            //}
            int dx = Random.Range(-1, 2);
            int dy = Random.Range(-1, 2);
            unit.Move(dx, dy, Beat(1.25f));
            unit.RotateFor(rot, Beat(2));
        }
        yield return Rest(0, 2.5f);
        rot = Random.Range(0, 12) * 30;
        foreach (var unit in squares) {
            int dx = Random.Range(-1, 2);
            int dy = Random.Range(-1, 2);
            unit.Move(dx, dy, Beat(1));
            unit.RotateFor(rot, Beat(2));
        }
        yield return Rest(0, 2.5f);
        foreach (var unit in squares) {
            if (unit != null) {
                if (Random.value < 0.2f) {
                    StartCoroutine(Section4Lifecycle(unit));
                } else {
                    unit.Remove(Beat(2));
                }
            }
        }
    }

    IEnumerator Section4Rotation() {
        foreach (var rest in Loop(22, 0, 0, 2)) {
            //var gs = board.GraphicEntities(board.FindRandomRectWithSize(Random.Range(1, 4), Random.Range(1, 4)));
            var gs = board.GraphicEntities().ToArray().Shuffle().Take(Random.Range(1, 7));
            gs.ForEach(r => r.RotateFor(Random.Range(-1, 2) * 30, Beat(1)));
            yield return rest;
        }
    }


    IEnumerator Section4Movement(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            var ge = board.GraphicEntities().ToArray().Shuffle().Take(Random.Range(2, 5));
            var m = Random.Range(1, 3);
            var x = 0;
            var y = 0;
            var rand = Random.value;
            if (rand < 0.25f) {
                x = m;
            } else if (rand < 0.50f) {
                x = -m;
            } else if (rand < 0.75f) {
                y = m;
            } else {
                y = -m;
            }

            foreach (var g in ge) {
                g.Move(x, y, Beat(1.5f));
            }

            yield return rest;
        }
    }

    IEnumerator Section4Transform() {
        yield return Rest(1, 1);
        foreach (var rest in Loop(22, 0, 0, 2)) {
            var gs = board.GraphicEntities().ToArray().Shuffle().Take(Random.Range(1, 3));
            //GraphicWithSizeLessThan(4, 4);
            foreach (var g in gs) {
                if (g != null) {
                    var newRect = g.rect.Resize(g.width*2, g.height*2).Translate(-g.width/2, -g.height/2);
                    g.Transform(newRect, Beat(2));
                    g.SetOpacity(g.opacity - 0.1f, Beat(2));
                            //.ForEach(g => g.Transform(g.rect.Resize(7, 10).Translate(-1, 0), Beat(2)));
                    //board.GraphicEntities().ToArray().Take(2).ForEach(g => g.Transform(g.rect.Resize(7, 10).Translate(-1, 0), Beat(2)));
                }
            }
            yield return rest;
        }
        yield return null;
    }

    IEnumerator Section4AddShapes() {
        foreach (var rest in Loop(22, 0, 0, 3)) {
            AddRect(Random.Range(1, 4), Random.Range(1, 3), blues[7], allowStacking:true);
            yield return rest;
        }
    }

    // Naming things
    IEnumerator Section4Shatter(IEnumerable<IEnumerator> loop) {
        foreach (var pulse in loop) {
            var g = board.FindGraphicWithSizeGreaterThan(1, 1);
            if (g != null) g.BreakToUnitSquares();
            yield return pulse;
        }
    }

    IEnumerator Section4Delete() {
        foreach (var rest in Loop(22, 0, 0, 3)) {
            var g = board.FindRandomGraphic();
            if (g != null) {
                g.Remove(Beat(2));
            }
            yield return rest;
        }
    }


    /*
    IEnumerator Section4Me() {
        yield return null;
    }
    */

    /***** SECTION 3 *****/
    IEnumerator Section3() {
        // 24
        yield return Rest(49, 0);
        board.GraphicEntities().ForEach(g => g.Remove(Beat(2f)));

        Run(Rest(2, 0), Section2Rotation(Loop(23, 0, 0, 3), 1, 1));
        Run(Rest(1, 0), Section3AddRect(Loop(10, 0, 1, 0)));
        Run(Rest(3, 1), Section3Shatter(Loop(24-4, 3, 1, 1)));
        Run(Rest(4, 2), Section1Orange(MeasureToBeats(24-5), 3, 6));
        Run(Rest(5, 1), Section3Movement(Loop(24-7, 2, 0, 3)));
        Run(Rest(6, 0), Section3Delete(Loop(24-8, 2, 0, 2)));

        Run(Rest(10, 0), Section3AddRect(Loop(11, 0, 0, 2)));
    }

    IEnumerator Section3AddRect(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            AddRect(Random.Range(1, 4), Random.Range(1, 4), blues[7]);
            yield return rest;
        }
    }

    IEnumerator Section3Movement(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            var g = board.FindRandomGraphic();
            if (g != null) {

                var rand = Random.value;
                if (rand < 0.25f) {
                    g.Move(1, 0, Beat(1f));
                } else if (rand < 0.50f) {
                    g.Move(-1, 0, Beat(1f));
                } else if (rand < 0.75f) {
                    g.Move(0, 1, Beat(1f));
                } else {
                    g.Move(0, -1, Beat(1f));
                }
            }

            yield return rest;
        }
    }

    IEnumerator Section3Shatter(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            var g = board.FindGraphicWithSizeGreaterThan(1, 1);
            if (g != null && g.rotation.IsZero()) g.BreakToUnitSquares();
            yield return rest;
        }
    }

    IEnumerator Section3Delete(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            var g = board.FindRandomGraphicWithSize(1, 1);
            if (g != null) g.Remove(Beat(0.3f));
            yield return rest;
        }
    }

    /***** SECTION 2 *****/
    // 0 refresh
    // 1 rotation effect, color change
    // 2 composition
    // 3 refresh
    IEnumerator Section2() {
        yield return Rest(24);
        board.GraphicEntities().ForEach(g => g.SetColor(blue));
        Run(Rest(0, 5.8f), Section2Rotation(Loop(23-12, 3, 2, 0)));
        Run(Rest(0, 2), Section2Shape(Loop(23, 1, 2, 0)));
        Run(Rest(0, 3), Section2Cascade(Loop(12, 0, 2, 0)));
        Run(Rest(0, 4), Section1Orange(MeasureToBeats(22), 8, 8));
        Run(Rest(12, 3), Section2Cascade2(Loop(11, 0, 2, 0)));
        Run(Rest(12, 5.8f), Section2Rotation(Loop(23-12, 3, 2, 0), 3, 6, true));
    }

    // let's
    // build a rect
    // build two rects
    // line and a cross...?
    /*
    IEnumerator Section2PrebakedShape(IEnumerable<IEnumerator> loop) {
        int index = 0;
        foreach (var rest in loop) {
            switch (index) {
                case
            }
            foreach (var g in board.FindAllGraphicsWithSize(1, 1)) {
                Coord pos = g.rect.min;
                if (pos) {
                    g.SetOpacity(0, Beat(0.3f));
                }
            }

            yield return rest;
            index++;
        }
    }
    */

    IEnumerator Section2Shape(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            bool[,] collection = new bool[cols,rows];
            for (int i = 0; i < cols; i++) {
                for (int j = 0; j < rows; j++) {
                    if (Random.value < 0.25) {
                        collection[i, j] = true;
                    } else {
                        collection[i, j] = false;
                    }
                }
            }

            var visibleColor = Random.value > 0.8f ? orange : blue;
            var useSameColor = Random.value > 0.5f;

            board.FindAllGraphicsWithSize(1, 1).ForEach(g => {
                if (!collection[g.rect.min.x, g.rect.min.y]) {
                    g.SetOpacity(0, Beat(0.3f));
                } else {
                    if (useSameColor) {
                        g.SetColor(visibleColor);
                    } else {
                        //g.SetColor(Random.value > 0.8f ? orange : blue);
                    }
                }
            });

            //board.FindAllGraphicsWithSize(1,1).ToArray().Shuffle().Take(30).ForEach(g => g.SetOpacity(0, Beat(0.3f)));
            yield return rest;
        }
    }

    /*
    IEnumerator Section2Rotation(IEnumerable<IEnumerator> loop, int min=1, int max=1) {
        var rotating = new HashSet<GraphicEntity1>();
        foreach (var rest in loop) {
            var cnt = Random.Range(min, max+1);
            for (int i = 0; i < cnt; i++) {
                var g = board.FindGraphicAtPosition(Random.Range(0, cols), Random.Range(0, rows));
                if (!rotating.Contains(g)) {
                    g.RotateTo(360, Beat(2));
                    rotating.Add(g);
                }
            }
            rotating.Clear();
            yield return rest;
        }
    }
    */

    IEnumerator Section2Rotation(IEnumerable<IEnumerator> loop, int min=1, int max=1, bool rotateLines=false) {
        var rotating = new HashSet<GraphicEntity1>();
        foreach (var rest in loop) {
            if (rotateLines && Random.value < 0.3f) {
                if (Random.value < 0.5f) {
                    foreach (var g in board.FindGraphicsForRow(Random.Range(0, rows))) {
                        g.RotateTo(360, Beat(1.9f));
                    }
                } else {
                    foreach (var g in board.FindGraphicsForColumn(Random.Range(0, cols))) {
                        g.RotateTo(360, Beat(1.9f));
                    }
                }
            } else {
                var cnt = Random.Range(min, max+1);
                for (int i = 0; i < cnt; i++) {
                    var g = board.FindGraphicAtPosition(Random.Range(0, cols), Random.Range(0, rows));
                    if (g != null && !rotating.Contains(g)) {
                        g.RotateTo(360, Beat(1.9f));
                        rotating.Add(g);
                    }
                }
                rotating.Clear();
            }
            yield return rest;
        }
    }

    IEnumerator Section2Cascade(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            var rand = Random.value;
            if (rand < 0.33f) {
                CascadeLeft();
            } else if (rand < 0.67f) {
                CascadeRight();
            } else {
                CascadeTop();
            }
            yield return rest;
        }
    }

    IEnumerator Section2Cascade2(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            var rand = Random.value;
            if (rand < 0.25f) {
                if (Random.value < 0.6f) {
                    if (Random.value < 0.5f) {
                        CascadeLeft();
                    } else {
                        CascadeRight();
                    }
                } else {
                    if (Random.value < 0.5f) {
                        CascadeTop();
                    } else {
                        CascadeDown();
                    }
                }
                //Random.value < 0.6f ? (Random.value > 0.5f ? CascadeLeft() : CascadeRight()) CascadeVertically(Random.value < 0.5f ? Direction.Up : Direction.Down, Random.Range(1, 5)) :
            } else {
                if (Random.value < 0.5f) {
                    CascadeVertically(Random.value < 0.5f ? Direction.Up : Direction.Down, Random.Range(1, 5));
                } else {
                    CascadeHorizontally(Random.value < 0.5f ? Direction.Right : Direction.Left, Random.Range(1, 5));
                }
            }
            yield return rest;
        }
    }

    void CascadeRight() {
        //CascadeHorizontally(Direction.Right, Random.Range(1, rows+1));
        CascadeHorizontally(Direction.Right, rows);
    }

    void CascadeLeft() {
        //CascadeHorizontally(Direction.Left, Random.Range(1, rows+1));
        CascadeHorizontally(Direction.Right, rows);
    }

    void CascadeTop() {
        CascadeVertically(Direction.Up, cols);
    }

    void CascadeDown() {
        CascadeVertically(Direction.Down, cols);
    }

    void CascadeVertically(Direction initialDirection, int swapCount) {
        bool isUpDir = initialDirection == Direction.Up;
        for (int i = 0; i < cols; i++) {
            var graphics = board.FindGraphicsForColumn(i);
            if (!isUpDir) graphics = graphics.Reverse();
            int index = 1;
            foreach (var g in graphics) {
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(0, Beat(0.2f));
                }, Beat(index * 0.3f)));
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(1, Beat(1f));
                }, Beat(index * 0.3f+0.3f)));
                index++;
            };
            if ((i + 1) % swapCount == 0) {
                Debug.Log("swaping direction");
                isUpDir = !isUpDir;
            }
        }
    }

    void CascadeHorizontally(Direction initialDirection, int swapCount) {
        bool isRightDir = initialDirection == Direction.Right;
        for (int i = 0; i < rows; i++) {
            var graphics = board.FindGraphicsForRow(i);
            if (!isRightDir) graphics = graphics.Reverse();
            int index = 1;
            foreach (var g in graphics) {
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(0, Beat(0.2f));
                }, Beat(index * 0.3f)));
                StartCoroutine(C.WithDelay(() => {
                    g.SetOpacity(1, Beat(1f));
                }, Beat(index * 0.3f+0.3f)));
                index++;
            };
            if ((i + 1) % swapCount == 0) {
                Debug.Log("swaping direction");
                isRightDir = !isRightDir;
            }
        }
    }


    /***** SECTION 1 *****/
    // 24 measures
    IEnumerator Section1() {
        var rect = AddRect(cols, rows, blues[0]);
        rect.BreakToUnitSquares();

        Run(Rest(2),  Section1Snake(Loop(11, 0, 4, 0)));
        Run(Rest(12),  Section1Snake(Loop(12, 0, 2, 0)));
        Run(Rest(8),  Section1Orange(MeasureToBeats(6), 5, 7));
        Run(Rest(14),  Section1Orange(MeasureToBeats(10), 3, 5));
        Run(Rest(12), Section1Fade(Loop(12, 0, 2, 0)));

        yield return null;
    }

    IEnumerator Section1Orange(int durationInBeats, int minRest, int maxRest) {
        int beatsRested = 0;
        while (beatsRested < durationInBeats) {
            var g = board.RandomGraphicEntity();
            if (g != null) g.SetColor(orange);
            var beatsToRest = Random.Range(minRest, maxRest);
            beatsRested += beatsToRest;
            yield return Rest(0, beatsToRest);
        }
    }

    IEnumerator Section1Fade(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
            int index = 0;
            var row = board.FindGraphicsForRow(Random.Range(0, rows));
            if (Random.value > 0.5f) row = row.Reverse();
            foreach (var g in row) {
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

    IEnumerator Section1Snake(IEnumerable<IEnumerator> loop) {
        foreach (var rest in loop) {
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
                        gr.SetColor(blue.WithAlpha(2f/3f));
                    } else {
                        gr.SetOpacity(Mathf.Min(1, currOpacity + 2f/3f));
                    }
                    // what i want - start with new opacity, fade down
                }, Beat(ij*2)));
                ij++;
            }
            yield return rest;
        }
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
