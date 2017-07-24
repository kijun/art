using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum GraphicAction2Type {
    None,
    Movement,
    Opacity,
    IncreaseOpacity,
    Transform,
    RotateTo,
    RotateFor,
    Merge,
    Split,
    ColorChange,
    Remove,
    Duplicate,
    Rest,
    // from where to where?
    Repeat
}


// TODO use namespaces
public class Composite<T> {
    public T val;
    public System.Func<GraphicEntity2, T> eval;

    public T GetValue(GraphicEntity2 ge) {
        if (eval != null) {
            return eval(ge);
        }
        return val;
    }
}

public class GraphicAction2 {
    public GraphicAction2Type type;
    public float a1;
    public float a2;
    public float duration;
    public Color color;
    public GridRect rect;
    public float probability = 1;
    public System.Func<GraphicEntity2, bool> conditional;
}

public class GraphicEntity2 : MonoBehaviour {
    // GE1 with memory
    Board2 board;
    public GridRect rect; // TODO haxor
    GraphicEntityMutexFlag mutex;
    Animatable2 animatable;
    List<GraphicAction2> actions;

    public static GraphicEntity2 New(GridRect rect, Board2 board) {
        var ge = new GameObject().AddComponent<GraphicEntity2>();
        ge.Initialize(rect, board);
        return ge;
    }

    public void Initialize(GridRect rect, Board2 board) {
        //Debug.Log($"Creating GraphicEntity: {rect}");
        this.board = board;
        this.rect = rect;
        board.LockTiles(rect, this);
        var rectParams = board.GridRectToRectParams(rect);
        animatable = NoteFactory.CreateRect(rectParams);
    }

    public void ApplyActions(List<GraphicAction2> origActions, int fromIndex=0) {
        // TODO modify actions at a given rate
        //origActions.Insert(Random.Range(0, origActions.Count), new GraphicAction2 {});
        // modify duration, range of values, rect, color, etc

        this.actions = origActions;
        StartCoroutine(_ApplyActions(actions, fromIndex));
    }

    public void Duplicate(float duration = 0) {
        var ge = GraphicEntity2.New(rect, board);
        ge.ApplyActions(actions);
    }

    IEnumerator _ApplyActions(List<GraphicAction2> actions, int fromIndex) {
        yield return null;
        // should actions be deleted?
        bool repeat = false;
        foreach (var action in actions.GetRange(fromIndex, actions.Count - fromIndex)) {
            fromIndex++;
            // TODO apply this to original action... how? oh, just to duplication
            if (Random.value > action.probability) continue;
            if (action.conditional != null && !action.conditional(this)) continue;
            var animDur = action.duration - 0.2f;
            switch (action.type) {
                case GraphicAction2Type.Movement:
                    Move((int)action.a1, (int)action.a2, animDur);
                    break;
                case GraphicAction2Type.Opacity:
                    SetOpacity(action.a1, animDur);
                    break;
                case GraphicAction2Type.Transform:
                    Transform(action.rect, animDur);
                    break;
                case GraphicAction2Type.RotateTo:
                    RotateTo(action.a1, animDur);
                    break;
                case GraphicAction2Type.RotateFor:
                    RotateFor(action.a1, animDur);
                    break;
                case GraphicAction2Type.Merge:
                    break;
                case GraphicAction2Type.ColorChange:
                    SetColor(action.color, animDur);
                    break;
                case GraphicAction2Type.Remove:
                    Remove(animDur);
                    break;
                case GraphicAction2Type.Duplicate:
                    Duplicate(animDur);
                    break;
                case GraphicAction2Type.Split:
                    var ges = BreakToUnitSquares(animDur);
                    foreach (var ge in ges) {
                        // but carry everything else?
                        ge.ApplyActions(actions, fromIndex);
                    }
                    Remove();
                    break;
                case GraphicAction2Type.Repeat:
                    repeat = true;
                    break;
            }
            yield return new WaitForSeconds(action.duration);
            if (repeat) break;
        }
        if (repeat) ApplyActions(actions);
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
        //Debug.Log($"GraphicEntity2: Transform {origin} -> {targetRect}");
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

    public void Merge(GraphicEntity2 another, float duration = 0) {
        // TODO race condition!!
        var target = rect.Merge(another.rect);
        another.Remove(duration);
        Transform(target, duration);
    }

    public void DeleteRect(GridRect gr, float duration = 0) {
        // create multiple GE and delete itself
    }

    public IList<GraphicEntity2> BreakToUnitSquares(float duration =0) {
        var squares = new List<GraphicEntity2>();
        foreach (var gridRect in rect.SplitToUnitSquares()) {
            var g = GraphicEntity2.New(gridRect, board);
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


