using UnityEngine;
using System.Collections.Generic;

/***
 * wrapper for animation, customized for each scene/level
 */
public enum GraphicEntityMutexFlag {
    // use noun form, as if verb lock is being prepended to all
    None           = 0,
    Translation    = 1 << 0,
    Rotation       = 1 << 1,
    Scale          = 1 << 2,
    Color          = 1 << 3,
    Opacity        = 1 << 4,
    Border         = 1 << 5,
    Division       = 1 << 6,
    Existence      = 1 << 7,
    Shape          = 1 << 8, // Rect, Circle, Triangle, Subrects, Subtriangles
}

public class GraphicEntity1 : MonoBehaviour {
    Board1 board;
    public GridRect rect; // TODO haxor
    GraphicEntityMutexFlag mutex;
    Animatable2 animatable;

    // how do i create an entity?
    // large row:
    // beat finds empty row
    // we
    public static GraphicEntity1 New(GridRect rect, Board1 board) {
        var ge = new GameObject().AddComponent<GraphicEntity1>();
        ge.Initialize(rect, board);
        return ge;
    }

    public void Initialize(GridRect rect, Board1 board) {
        //Debug.Log($"Creating GraphicEntity: {rect}");
        this.board = board;
        this.rect = rect;
        board.LockTiles(rect, this);
        var rectParams = board.GridRectToRectParams(rect);
        animatable = NoteFactory.CreateRect(rectParams);
    }
    // 2 note
    // 12 papapapa
    // 24 beat
    // 32 no beat
    // 36 beat again
    // 52 no beat : 2m

    //public void SetColor(Color color);
    public void Move(int x, int y, float duration = 0) {
        // moves square by (x, y) tile units
        //
        // lock properties: Translation, Existence (automatic)
        // lock destination tiles (we ignore the in-between)
        // apply move
        // unlock properties
        // unlock origin tiles
        // TODO fix larger rect problem hax
        Transform(rect.Translate(x, y), duration);
    }

    public void Move(Coord delta, float duration = 0) {
        // moves square by (x, y) tile units
        //
        // lock properties: Translation, Existence (automatic)
        // lock destination tiles (we ignore the in-between)
        // apply move
        // unlock properties
        // unlock origin tiles
        // TODO fix larger rect problem hax
        Transform(rect.Translate(delta.x, delta.y), duration);
    }

    public void Transform(GridRect targetRect, float duration=0) {
        // moves and resizes square
        var origin = rect;
        //Debug.Log($"GraphicEntity1: Transform {origin} -> {targetRect}");
        var target = board.GridRectToRectParams(targetRect);
        // if target
        _LockProperty(GraphicEntityMutexFlag.Translation);

        board.LockTiles(targetRect, this);
        // if (duration.IsZero()) {
        //    animatable.position = targetPosition;
        //} else {
            _RunAnimation(AnimationKeyPath.RelPosX, 0, animatable.position.x, duration, target.x);
            _RunAnimation(AnimationKeyPath.RelPosY, 0, animatable.position.y, duration, target.y);
            _RunAnimation(AnimationKeyPath.RelScaleX, 0, animatable.localScale.x, duration, target.width);
            _RunAnimation(AnimationKeyPath.RelScaleY, 0, animatable.localScale.y, duration, target.height);
        //}
        _UnlockProperty(GraphicEntityMutexFlag.Translation);
        board.UnlockTiles(origin);
        // relock in case we have overlap
        board.LockTiles(targetRect, this);
        rect = targetRect;
    }

    public void Merge(GraphicEntity1 another, float duration = 0) {
        // TODO race condition!!
        var target = rect.Merge(another.rect);
        another.Remove(duration);
        Transform(target, duration);
    }

    public void DeleteRect(GridRect gr, float duration = 0) {
        // create multiple GE and delete itself
    }

    public IList<GraphicEntity1> BreakToUnitSquares(float duration =0) {
        var squares = new List<GraphicEntity1>();
        foreach (var gridRect in rect.SplitToUnitSquares()) {
            var g = GraphicEntity1.New(gridRect, board);
            g.SetColor(animatable.color);
            squares.Add(g);
        }
        Remove(duration, DONTUNLOCK:true);
        return squares;
        // splits existing x to this as much as it can
    }

    public void SetColor(Color color, float duration = 0) {
        animatable.color = color;
        // TODO color animation
    }

    public void SetOpacity(float opacity, float duration = 0) {
        if (duration.IsZero()) {
            animatable.opacity = opacity;
        } else {
            _RunAnimation(AnimationKeyPath.Opacity, 0, animatable.opacity, duration, opacity);
        }
    }

    public void RotateTo(float rotation, float duration = 0) {
        _RunAnimation(AnimationKeyPath.Rotation, 0, animatable.rotation, duration, rotation);
    }

    public void RotateFor(float dr, float duration = 0) {
        _RunAnimation(AnimationKeyPath.Rotation, 0, animatable.rotation, duration, animatable.rotation + dr);
    }

    public void Remove(float duration = 0, bool DONTUNLOCK=false) {
        if (duration.IsNonZero()) {
            SetOpacity(0, duration);
            StartCoroutine(C.WithDelay(() => {
                if (!DONTUNLOCK) board.UnlockTiles(rect);
                Destroy(animatable.gameObject);
                Destroy(gameObject);
            }, duration+0.5f));
        } else {
            // TODO opacity
            if (!DONTUNLOCK) board.UnlockTiles(rect);
            Destroy(animatable.gameObject);
            Destroy(gameObject);
        }
    }

    /* VALS */

    public int width {
        get {
            return rect.width;
        }
    }

    public int height {
        get {
            return rect.height;
        }
    }

    public float opacity {
        get {
            return animatable.opacity;
        }
    }

    public Color color {
        get {
            return animatable.color;
        }
    }

    public float rotation {
        get {
            return animatable.rotation;
        }
    }

    void _RunAnimation(string keyPath,
                       float startTime, float startValue,
                       float endTime, float endValue) {
        animatable.AddAnimationCurve(keyPath, AnimationCurveUtils.FromPairs(startTime, startValue, endTime, endValue));
    }

    void _RunAnimation(string keyPath, AnimationCurve curve) {
        animatable.AddAnimationCurve(keyPath, curve);
        /*
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
        */
    }

    void _LockProperty(GraphicEntityMutexFlag flag) {
        //
        mutex |= flag | GraphicEntityMutexFlag.Existence;
    }

    void _UnlockProperty(GraphicEntityMutexFlag flag) {
        mutex ^= flag;
        // if every other property is released, graphic entity
        // can be deleted as well
        if (mutex == GraphicEntityMutexFlag.Existence) {
            mutex = GraphicEntityMutexFlag.None;
        }
    }

    public override string ToString() {
        return $"Graphic: {rect}";
    }
}

