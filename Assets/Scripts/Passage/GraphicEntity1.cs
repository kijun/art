using UnityEngine;

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
        Debug.Log($"Creating GraphicEntity: {rect}");
        this.board = board;
        this.rect = rect;
        board.LockTiles(rect, this);
        var rectParams = board.GridRectToRectParams(rect);
        animatable = NoteFactory.CreateRect(rectParams);
    }

    //public void SetColor(Color color);
    public void Move(int x, int y, float duration = 0) {
        // moves square by (x, y) tile units
        //
        // lock properties: Translation, Existence (automatic)
        // lock destination tiles (we ignore the in-between)
        // apply move
        // unlock properties
        // unlock origin tiles
        var origin = new GridRect(rect);
        var target = rect.Translate(x, y);
        // if target
        _LockProperty(GraphicEntityMutexFlag.Translation);
        board.LockTiles(target, this);
        if (duration.IsZero()) {
            animatable.position += new Vector2(x, y);
        } else {
        _RunAnimation(AnimationKeyPath.RelPosX,
                AnimationCurveUtils.FromPairs(
                    // start position
                    // end position
                ));
        _RunAnimation(AnimationKeyPath.RelPosY,
                AnimationCurveUtils.FromPairs(
                    // start position
                    // end position
                ));
        }
        _UnlockProperty(GraphicEntityMutexFlag.Translation);
        board.UnlockTiles(origin);
        // relock in case we have overlap
        board.LockTiles(target, this);
        rect = target;
    }

    public void Transform(GridRect gr) {
        // moves and resizes square
    }

    public void DeleteRect(GridRect gr, float duration = 0) {
        // create multiple GE and delete itself
    }

    public void Split(int width, int height) {
        // splits existing x to this as much as it can
    }

    public void SetColor(Color color, float duration = 0) {
        //
    }

    public void SetOpacity(float opacity, float duration = 0) {
        //
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
}

