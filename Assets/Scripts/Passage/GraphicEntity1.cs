/***
 * wrapper for animation, customized for each scene/level
 */
public enum GraphicEntityMutexFlag {
    None           = 0,
    Position       = 1 << 0,
    Velocity       = 1 << 1,
    Rotation       = 1 << 2,
    Scale          = 1 << 3,
    Opacity        = 1 << 4,
    Color          = 1 << 5,
    Representation = 1 << 7,
}

public class GraphicEntity1 : MonoBehaviour {
    Board board;

    // how do i create an entity?
    // large row:
    // beat finds empty row
    // we
    public GraphicEntity1 New(GridRect gr, Board board, Color color) {
        this.board = board;
        board.LockTiles(gr);
        // display status hidden
        rp = GridRectToRectParams(gr, rp);
        NoteFactory.CreateRect(rp);
    }

    RectParams RectParamsFromGridRect(GridRect gr, RectParams rp = null) {
        if (rp != null) {
        }
    }

    public void Move(int x, int y) {
        // moves square by (x, y)
    }

    public void Transform(GridRect gr) {
        // moves and resizes square
    }

    public void DeleteRect(GridRect gr) {
        // create multiple GE and delete itself
    }

    public void Split(int width, int height) {
        // splits existing x to this as much as it can
    }

    void RunAnimation(string keyPath, AnimationCurve curve)
        var lockFlag = AnimationKeyPath.ToTileMutexFlag(keyPath);
        mutex |= lockFlag;
        animatable.AddAnimationCurve(keyPath, curve);
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
    }

}

