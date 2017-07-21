using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;
//using OscJack;

public class AgentController : MonoBehaviour {

    public Color baseColor;
    public int rows; // automatically decides the rest of the game
    int cols;

    Tile[,] board;

    void Start() {
        CreateTiles();
        StartCoroutine(Run());
        StartCoroutine(RunOrange());
        StartCoroutine(RunRotation());
        StartCoroutine(RunEnlarge());
        StartCoroutine(RunEnlargeY());
    }

    void Update() {
        /*
        Debug.Log(OscMaster.MasterDirectory.TotalMessageCount);
        if (OscMaster.HasData("/Velocity1")) {
            foreach (var x in OscMaster.GetData("/Velocity1")) {
            //    Debug.Log(x);
            }
            OscMaster.ClearData("/Velocity1");
        }
        if (OscMaster.HasData("/Note1")) {
            foreach (var x in OscMaster.GetData("/Note1")) {
                //Debug.Log(x);
            }
            OscMaster.ClearData("/Note1");
        }
        */
    }

    void CreateTiles() {
        var sideLength = CameraHelper.Height / (1.414f * rows + 0.414f);
        cols = (int)(CameraHelper.Width / (1.414f * sideLength));

        board = Tile.CreateBoard(cols, rows, sideLength);
    }

    IEnumerator RunOrange() {
        StartCoroutine(RandomTile().ChangeColor(orange, NoteValueToDuration(0, 2), 0));
        yield return Rest(2,1);
        foreach (var rest in Loop(64, 0, 1, 3)) {
            StartCoroutine(RandomTile().ChangeColor(orange, NoteValueToDuration(0, 2), 0));
            yield return rest;
        }
    }

    IEnumerator RunRotation() {
        yield return Rest(16);
        foreach (var rest in Loop(32, 0, 2, 0)) {
            var tile = RandomTile();
            var rot = tile.animatable.rotation % 360;

            RandomTile().RunAnimation(
                    AnimationKeyPath.Rotation,
                    AnimationCurveUtils.FromPairs(
                        0, rot,
                        NoteValueToDuration(0, 1), rot+Random.Range(360, 360)),
                    Location.Axis,
                    0.6f,
                    NoteValueToDuration(0, 1)
            );
            yield return rest;
        }
        foreach (var rest in Loop(32, 0, 2, 0)) {
            var tile = RandomTile();
            var rot = tile.animatable.rotation % 360;

            RandomTile().RunAnimation(
                    AnimationKeyPath.Rotation,
                    AnimationCurveUtils.FromPairs(
                        0, rot,
                        NoteValueToDuration(0, 1), rot+Random.Range(90, 360)),
                    Location.Axis,
                    0.6f,
                    NoteValueToDuration(0, 1)
            );
            yield return rest;
        }
    }

    IEnumerator RunEnlarge() {
        yield return Rest(28);
        foreach (var rest in Loop(64, 0, 1, 0)) {
            RandomTile().RunAnimation(
                    AnimationKeyPath.RelScaleX,
                    AnimationCurveUtils.FromPairs(0, 1f, NoteValueToDuration(1, 0), 1.9f, NoteValueToDuration(2, 0), 1),
                    Location.Bottom,
                    0.8f,
                    NoteValueToDuration(0, 1)
            );
            yield return rest;
        }
    }

    IEnumerator RunEnlargeY() {
        yield return Rest(40);
        foreach (var rest in Loop(64, 0, 1, 0)) {
            RandomTile().RunAnimation(
                    AnimationKeyPath.RelScaleY,
                    AnimationCurveUtils.FromPairs(0, 1f, NoteValueToDuration(1, 0), 5f, NoteValueToDuration(2, 0), 1),
                    Location.Top,
                    1f,
                    NoteValueToDuration(0, 1)
            );
            yield return rest;
        }
    }

    IEnumerator Run() {
        System.Action<Tile, Location> f = (Tile target, Location l) => {};

        f = (Tile target, Location loc) => {
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
                    /*
                    if (Random.value < 0.45f) randomAxis |= Location.Left;
                    if (Random.value < 0.45f) randomAxis |= Location.Right;
                    if (Random.value < 0.45f) randomAxis |= Location.Top;
                    if (Random.value < 0.45f) randomAxis |= Location.Bottom;
                    */
                    f(tile, randomAxis);
                    found = true;
                }
                runCount++;
            }

            yield return rest;
        }
        foreach (var rest in Loop(64, 0, 1, 0)) {



            // Find target
            bool found = false;
            int runCount = 0;
            while (!found && runCount < 100) {
                int x = Random.Range(0, cols);
                int y = Random.Range(0, rows);
                var tile = board[x, y];
                if (!tile.IsLocked(TileMutexFlag.Opacity)) {
                    var randomAxis = Location.None;
                    if (Random.value < 0.30f) randomAxis |= Location.Left;
                    if (Random.value < 0.30f) randomAxis |= Location.Right;
                    if (Random.value < 0.30f) randomAxis |= Location.Top;
                    if (Random.value < 0.30f) randomAxis |= Location.Bottom;
                    if (randomAxis == Location.None && Random.value > 0.5f) {
                        randomAxis |= Location.Bottom;
                    }
                    f(tile, randomAxis);
                    found = true;
                }
                runCount++;
            }

            yield return rest;
        }
            /*

            RandomTile().RunAnimation(
                    AnimationKeyPath.Opacity,
                    AnimationCurveUtils.FromPairs(0, 1, NoteValueToDuration(0, 2), 0.0f, NoteValueToDuration(1, 0), 1),
                    Location.Left,
                    0.95f,
                    NoteValueToDuration(0, 1)
            );

            RandomTile().RunAnimation(
                    AnimationKeyPath.RelScaleY,
                    AnimationCurveUtils.FromPairs(0, 1f, NoteValueToDuration(1, 0), 5f, NoteValueToDuration(2, 0), 1),
                    Location.Top,
                    1f,
                    NoteValueToDuration(0, 1)
            );
            */
    }


    Tile RandomTile() {
        return (Tile)board.GetValue2(Random.Range(0, board.Length));
    }

    /*
    Tile[] Row(int row) {
    }

    Tile[] Column(int column) {
    }
    */

    public Color orange;
    public Color red;
    public Color blue;
    public Color purple;
    public Color peach;

    public float defaultLineWidth;
    public float defaultLineSpeed;
    public float defaultLineHeight;

    public Color lineColor;

    public TimeSignature timeSignature;
    public Camera camera;
    public CameraAnimatable cameraAnimatable;

    /*
    void Start() {
//        NoteFactory.CreateCircle(new CircleProperty());
        StartCoroutine(RunLineIntro());
//        StartCoroutine(RunShapes());
        StartCoroutine(RunSquareBeats());
        StartCoroutine(RunHorizontalLines());
        StartCoroutine(RunDiagonalLines());
        StartCoroutine(RunRisingGraph());
        StartCoroutine(RunCamera());
        StartCoroutine(RunCameraZoom());
        //StartCoroutine(RunCameraPosition());
        //
        StartCoroutine(RunCircles());
    }
    */

    IEnumerator RunCircles() {
        Color[] colors = {orange, red, purple};
        System.Action MakeCircle = () => {
            var randomColor = Color.Lerp(peach, blue, Random.Range(0.0f, 0.5f));
            var diameter = CameraHelper.Height * Random.Range(0.7f, 0.8f);
            var cp = new CircleProperty(color:randomColor, diameter:diameter, center: CameraHelper.RandomPositionNearCenter(diameter/2));
            var anim = NoteFactory.CreateCircle(cp);
            Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                0, 0,
                Beat*4, 1,
                Beat*6, 1,
                Beat*8, 0
            );

            anim.DestroyIn(Beat*9);
            anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
        };

        // until measure 40
        foreach (var rest in Loop(64, 0, 2, 0)) {
            MakeCircle();
            yield return rest;
        }
    }

    IEnumerator RunCamera() {
        var choices = new float[]{0, 90, 180, 270};
        var obliqueChoices = new float[]{10, 30, 45, 60, 80};
        var restChoices = new float[]{2, 3, 4, 6, 8};
        var longRestChoices = new float[]{5, 6};
        yield return Rest(0, 4f);
        foreach (var i in Times(5000)) {
            var axis = RandomHelper.Pick(choices);
            if (Random.value > 0.80f) {
                var angle = RandomHelper.Pick(obliqueChoices);
                var swing = Random.Range(-2f, 2f);
                camera.transform.rotation = Quaternion.Euler(0, 0, axis + angle + swing);
                yield return Rest(0, RandomHelper.Pick(longRestChoices));
            } else {
                camera.transform.rotation = Quaternion.Euler(0, 0, axis);
                yield return Rest(0, RandomHelper.Pick(restChoices));
            }
        }
    }

    IEnumerator RunCameraPosition() {
        var restChoices = new float[]{2.1f, 3.1f};
        var deltaPos = new float[]{5f, -5f};
        foreach (var i in Times(5000)) {
            var pos = camera.transform.position;
            var xd = RandomHelper.Pick(deltaPos);
            var yd = RandomHelper.Pick(deltaPos);

            if (Mathf.Abs(pos.x) > 15) {
                xd = -Mathf.Sign(pos.x) * Mathf.Abs(xd);
            }

            if (Mathf.Abs(pos.y) > 15) {
                yd = -Mathf.Sign(pos.y) * Mathf.Abs(yd);
            }


            camera.transform.position = camera.transform.position +
                new Vector3(xd, yd);

            yield return Rest(0, RandomHelper.Pick(restChoices));
        }
    }

    IEnumerator RunCameraZoom() {
        var cameraZoomLevels = new float [] {2, 5, 13};
        var restChoices = new float[]{3.25f, 4.75f};
        var prevOrthoSize = 0f;
        foreach (var i in Times(5000)) {
            var size = RandomHelper.Pick(cameraZoomLevels);
            if (prevOrthoSize != size) {
                camera.orthographicSize = size;
                cameraAnimatable.orthographicSizeVelocity = -size / 20f;
                //cameraAnimatable.orthographicSizeVelocity = -size / 20f * Random.Range(-1, 2);
                prevOrthoSize = size;
            }
            //cameraAnimatable.orthographicSizeVelocity = -size / 20f * Random.Range(-2, 3);
            yield return Rest(0, RandomHelper.Pick(restChoices));
        }
    }

    IEnumerator RunSquareBeats() {
        Color[] colors = {orange, red, purple};
        float[] heights = {0.08f, 0.2f, 0.13f};
        System.Action MakeShape = () => {
            var randomColor = Color.Lerp(red, blue, Random.Range(0.5f, 1f));
            var randHeight = RandomHelper.Pick(heights);
            var anim = NoteFactory.CreateRectInViewport(
                x:Random.Range(0, 9)/10f+0.1f, y:Random.Range(0, 9)/10f + 0.1f, width:randHeight/1.6f, height: randHeight, color: randomColor, level: ShapeZLevel.Back/1000f);
            Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                0, 0,
                Beat, 1,
                Beat*3, 1,
                Beat*4, 0
            );

            anim.DestroyIn(Beat*6);
            anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
        };

        // until measure 40
        foreach (var rest in Loop(64, 0, 0, 1)) {
            MakeShape();
            yield return rest;
        }
    }

    IEnumerator RunHorizontalLines() {
        yield return Rest(8);
        System.Action MakeShape = () => {
            var randomColor = Color.Lerp(orange, red, Random.value); // closer to red
            var anim = NoteFactory.CreateRectInViewport(
                x:0.5f, y:Random.Range(0, 10)/10f, width:1f, height: 0.03f, color: randomColor);
            var maxOpacity = Random.Range(0.7f, 1f);
            Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                0, 0,
                Beat*2, maxOpacity,
                Beat*6, maxOpacity,
                Beat*8, 0
            );

            anim.DestroyIn(Beat*10);
            anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
        };

        foreach (var rest in Loop(32, 0, 0, 2)) {
            MakeShape();
            yield return rest;
        }

        yield return Rest(8);

        foreach (var rest in Loop(16, 0, 0, 2)) {
            MakeShape();
            yield return rest;
        }
    }

    IEnumerator RunDiagonalLines() {
        yield return Rest(24);
        System.Action MakeShape = () => {
            var randomColor = Color.Lerp(orange, red, Random.value); // closer to red
            var anim = NoteFactory.CreateRectInViewport(
                x:0.5f, y:Random.Range(0, 10)/10f, width:0.7f, height: 0.1f, color: randomColor, rotation: Random.value*360);
            var maxOpacity = Random.Range(0.5f, 1f);
            Keyframe[] kff = KeyframeHelper.CreateKeyframes(
                0, 0,
                Beat, maxOpacity,
                Beat*2, maxOpacity,
                Beat*3, 0
            );

            anim.DestroyIn(Beat*4);
            anim.AddAnimationCurve(AnimationKeyPath.Opacity, new AnimationCurve(kff));
        };

        foreach (var rest in Loop(16, 0, 0, 4)) {
            MakeShape();
            yield return rest;
        }

        yield return Rest(8);

        foreach (var rest in Loop(16, 0, 0, 4)) {
            MakeShape();
            yield return rest;
        }
    }

    IEnumerator RunRisingGraph() {
        yield return Rest(48);
        foreach (var rest in Loop(16, 0, 4, 0)) {
            CreateLineGraph();
            yield return rest;
        }
    }


    IEnumerator RunOorange() {
        foreach (var i in Times(10)) {
            AnimateRect(Direction.Down, 1, Vector2.one*0.5f, orange, CameraHelper.RandomXOffset(0.9f), level:0);
            yield return Rest(1);
        }

        int ornamentation = 1;
        float boundFactor = 0.1f;
        foreach (var i in Times(20)) {
            var progress = i / 20f;

            // Gradually add ornamentation
            var offset = CameraHelper.RandomXOffset(0.6f);
            var scale = Vector2.one * Random.Range(0.2f, 0.5f); // the big one
            var speed = 1;

            var boundingRect = CameraHelper.BoundingRectOnPerimeter(
                    Location.Top, CameraHelper.Width * boundFactor, CameraHelper.Height * boundFactor, offset);
            var foundationRect = RectHelper.RectFromCenterAndSize(boundingRect.center, scale);
            var formation = GenerateFormation(foundationRect, boundingRect, Random.Range(1, 3));
            foreach (var rect in formation) {
                var lineSpeed =  speed * Random.Range(1f-0.3f*progress, 1f + 0.3f*progress);
                var level = 0;
                if (rect == formation[0]) {
                    lineSpeed = speed;
                    level = -1;
                }
                AnimateRect(
                        Direction.Down,
                        lineSpeed,
                        rect.size,
                        new Color[]{orange, blue, red}[Random.Range(0, Mathf.CeilToInt(3*progress))],
                        rect.center,
                        0);
            }
            yield return Rest(2);

            ornamentation++;
            boundFactor += 0.05f;
        }
    }

    IEnumerator RunLineIntro() {
//   '       '    '        '    '    xx '    '    '
//   4    8    12   16   20   24   28   32   36   40   44   48   52 (end)
        var score = @"
0000 0001 0000 0000 0003 2001 0101 0002 3565 2000 0100 1000 0000 0000
1000 1000 0005 0004 0000 0050 0000 0102 3565 2000 1010 1000 0000 0000
0000 0010 0000 0002 2000 0101 0211 1102 3565 2333 0033 0101 0000 0000
0000 0000 0000 0000 2000 0100 0001 0002 3565 2000 0000 0000 0000 0000
";
        score = @"
1010 0010 1001 0000 0001 0001 0100 0000 0000 0000 0000 0000 0000 0000
0000 1000 0000 0010 0100 0010 0000 1000 0000 0000 0000 0000 0000 0000
0000 0000 0000 0000 0000 0000 0000 0001 0000 1000 0000 0010 0010 0000
0000 0000 0000 0000 0000 0000 0000 0000 0100 0010 0110 0000 1000 1000
";
        var ornamentation = @"
3020100010203040504
0101010101010101010
0102030405040302010
1111111111111111111
";
        score = score.Trim();
        var voicesInString = score.Split('\n').Select(str => str.Replace(" ", "")).ToArray();

        Debug.Log(voicesInString);
        int[,] voices = new int[voicesInString.Length, voicesInString[0].Length];
        int scoreLen = voices.GetLength(1);
        Debug.Log(voices.GetLength(0));
        Debug.Log(voices.GetLength(1));

        // initialize score Obj
        for (int i = 0; i < scoreLen; i++) {
            foreach (var j in Times(4)) {
                voices[j, i] = int.Parse(voicesInString[j][i].ToString());
            }
        }

        for (int i = 0; i<scoreLen; i++) {
            _Lines(Direction.Down, voices[0, i]);
            _Lines(Direction.Up, voices[1, i]);
            _Lines(Direction.Right, voices[2, i]);
            _Lines(Direction.Left, voices[3, i]);
            yield return Rest();
        }
    }

    IEnumerator RunHorizontal() {
        yield return Rest(6);
        _Lines(Direction.Right, 1);
        yield return Rest(3);
    }

    void _Lines(Direction dir, int count) {
        foreach (var i in Times((int)(count))) {
            float offset = 0;
            if (dir == Direction.Left|| dir == Direction.Right) {
                offset = CameraHelper.RandomYOffset(1f, 0.1f);
            } else {
                offset = CameraHelper.RandomXOffset(1f, 0.1f);
            }
            _Line(dir, defaultLineHeight, offset, -defaultLineSpeed*Random.value*0.1f);
        }
    }

    void _Line(Direction dir, float height, float offset, float deltaSpeed=0) {
        var scale = dir.Align(new Vector2(defaultLineWidth, height*(Random.value*0.7f+0.65f)));
        var lp = new LineParams2 {
            position = CameraHelper.PerimeterPositionForMovingObject(dir, offset, scale, 0),
            color = lineColor,
            scale = scale,
            rotation = 0,
            level = ShapeZLevel.Front};
        var mp = new MotionParams { velocity = dir.ToVelocity(defaultLineSpeed + deltaSpeed) };

        NoteFactory.CreateLine(lp, mp);
    }



    IEnumerator RunBlue() {
        foreach (var i in Times(10)) {
            AnimateRect(Direction.Right, 3, Vector2.one, blue, CameraHelper.RandomYOffset(0.6f));
            yield return Rest(0, timeSignature.beatsPerMeasure/2f);
        }

        foreach (var i in Times(20)) {
            yield return Rest(1);
        }
    }

    IEnumerator RunRed() {
        foreach (var i in Times(10)) {
            AnimateRect(Direction.Up, 3, Vector2.one, red, CameraHelper.RandomXOffset(0.6f));
            yield return Rest(0, timeSignature.beatsPerMeasure);
        }

        foreach (var i in Times(20)) {
            yield return Rest(1);
        }
    }

    List<Rect> GenerateFormation(Rect foundation, Rect boundary, int count, bool nonOverlapping=true) {
        var rects = new List<Rect>();
        rects.Add(foundation);
        int retries = 0;
        while (rects.Count <= count && retries < 1000) {
            var enclosed = new Vector2(
                    foundation.width.RandomFraction(0.05f, 0.5f),
                    foundation.height.RandomFraction(0.05f, 0.8f));
            var candidate = RectHelper.RectFromCenterAndSize(boundary.RandomPosition(), enclosed);
            if (nonOverlapping && rects.Any(r => r.Overlaps(candidate))) {
                retries++;
                continue;
            }
            rects.Add(candidate);
        }
        return rects;
    }

    void AnimateRect(Direction dir, float speed, Vector2 scale, Color color, float offset=0, float rotation=0, float level=0) {
        var lp = new LineParams2 { position = CameraHelper.PerimeterPositionForMovingObject(dir, offset, scale, rotation),
                                   color = color,
                                   scale = scale,
                                   rotation = rotation,
                                   level = level};
        var mp = new MotionParams { velocity = dir.ToVelocity(speed) };

        NoteFactory.CreateLine(lp, mp);
    }

    void AnimateRect(Direction dir, float speed, Vector2 scale, Color color, Vector2 position, float rotation=0, float level=0) {
        var lp = new LineParams2 { position = position,
                                   color = color,
                                   scale = scale,
                                   rotation = rotation,
                                   level = level};
        var mp = new MotionParams { velocity = dir.ToVelocity(speed) };

        NoteFactory.CreateLine(lp, mp);
    }

    /** helpers, to be extracted into another class */

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

    void CreateLineGraph () {

        int numberOfLines = Random.Range(10, 15);

        float baseLength = 1.5f;
        float lengthIncrement = 0.5f;
        int lengthInflectionCount = Random.Range(2,4);

        /*
        if (camera.orthographicSize > 10) {
            numberOfLines = (int)(numberOfLines * 3f);
            lengthIncrement = 0.5f;
        }
        */

        float baseSpeed = 0.6f;
        float speedIncrement = 0.04f;
        int speedInflectionCount = 2;

        float baseWidth = 0.05f;
        float baseGap = 0.1f;


        var equalizerWidth = numberOfLines * baseWidth + (numberOfLines - 1) * baseGap;
        var lengthInflections = RandomHelper.Points(0, numberOfLines, lengthInflectionCount);
        lengthInflections.Add(int.MaxValue);
        var nextLenInflection = lengthInflections[0];
        lengthInflections.RemoveAt(0);

        var speedInflections = RandomHelper.Points(0, numberOfLines, speedInflectionCount);
        speedInflections.Add(int.MaxValue);
        var nextSpeedInflection = speedInflections[0];
        speedInflections.RemoveAt(0);

        var currXPos = ((int)(CameraHelper.Width - equalizerWidth) * Random.value - CameraHelper.HalfWidth)/100*100;
        var currLenDir= 1;
        var currLen = baseLength;
        var currSpeedDir = 1;
        var currSpeed = baseSpeed;

        for (int i = 0; i < numberOfLines; i++) {
            var line = new LineParams2();
            line.length = currLen;
            line.width = baseWidth;
            line.x = currXPos;
            line.y = (CameraHelper.Height+baseLength*2) / -2f - 2f;
            line.color = lineColor;
            NoteFactory.CreateLine(line, new MotionParams{velocity= Vector2.up * currSpeed});

            // next
            currXPos += baseWidth + baseGap;
            currLen += currLenDir * lengthIncrement;
            currSpeed += speedIncrement * currSpeedDir;

            if (nextLenInflection <= i || currLen < float.Epsilon || currLen > 4f) {
                // reverse dir
                currLenDir *= -1;
                if (lengthInflections.Count > 0) {
                    nextLenInflection = lengthInflections[0];
                    lengthInflections.RemoveAt(0);
                }
            }

            if (nextSpeedInflection <= i) {
                // reverse dir
                currSpeedDir *= -1;
                nextSpeedInflection = speedInflections[0];
                speedInflections.RemoveAt(0);
            }
        }
    }

    T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }
}


