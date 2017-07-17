using System.Collections;
using System.Collections.Generic;

[System.Flags]
public enum TileMutexFlag {
    None        = 0,
    Position    = 1 << 0,
    Velocity    = 1 << 1,
    Rotation    = 1 << 2,
    Scale       = 1 << 3,
    Opacity     = 1 << 4,
    Color       = 1 << 5,
    Shape       = 1 << 6,
    EntityCount = 1 << 7,
}

//GraphicUnit

public class Tile2 {
    /***** PUBLIC: Variable *****/
    public int col;
    public int row;

    /***** PRIVATE: Variable *****/
    Dictionary<Location, Tile2> adjacent = new Dictionary<Location, Tile2>();
    Location adjacentFlags = Location.None;
    TileMutexFlag mutex = TileMutexFlag.None;

    public Tile2(int col, int row) {
        this.col = col;
        this.row = row;
    }

    /***** PUBLIC STATIC METHOD *****/
    public static Tile2[,] CreateBoard(int cols, int rows) {
        //Debug.Log("Creating board " + cols + " " + rows);
        var board = new Tile2[cols, rows]; // x, y
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                board[i, j] = new Tile2(i, j);
            }
        }

        /*
        for (int x = 0; x < cols; x++) {
            for (int y = 0; y < rows; y++) {
                var tile = (Tile2)board.GetValue2(x, y);
                tile.AddAdjacentTile(Location.TopLeft,     (Tile2)board.GetValue2(x-1, y-1));
                tile.AddAdjacentTile(Location.Top,         (Tile2)board.GetValue2(x,   y-1));
                tile.AddAdjacentTile(Location.TopRight,    (Tile2)board.GetValue2(x+1, y-1));
                tile.AddAdjacentTile(Location.Left,        (Tile2)board.GetValue2(x-1, y));
                tile.AddAdjacentTile(Location.Right,       (Tile2)board.GetValue2(x+1, y));
                tile.AddAdjacentTile(Location.BottomLeft,  (Tile2)board.GetValue2(x-1, y+1));
                tile.AddAdjacentTile(Location.Bottom,      (Tile2)board.GetValue2(x,   y+1));
                tile.AddAdjacentTile(Location.BottomRight, (Tile2)board.GetValue2(x+1, y+1));
            }
        }
        */

        return board;
    }

    public Tile2 TileAtLocation(Location loc, TileMutexFlag mutexFlag = TileMutexFlag.None) {
        var targetTiles = adjacentFlags;
        if (mutexFlag != TileMutexFlag.None) {
            foreach (var kv in adjacent) {
                if (kv.Value.IsLocked(mutexFlag)) {
                    targetTiles ^= kv.Key;
                }
            }
        }

        var chosenLocation = loc.ChooseRandom(targetTiles);
        if (chosenLocation == Location.None) return null;
        return adjacent[chosenLocation];
    }

    public bool IsLocked(TileMutexFlag flag) {
        return (mutex & flag) != 0;
    }

    void AddAdjacentTile (Location loc, Tile2 tile) {
        if (tile != null) {
            adjacentFlags |= loc;
            adjacent.Add(loc, tile);
        }
    }
}

/*
public class GraphicalObject : MonoBehaviour {
    // opacity
    public void RunAnimation(
            string keyPath,
            AnimationCurve curve,
            // TODO create a new struct
            Location propLocation = Location.None,
            float propProbability = 0,
            float propDelay = 0) {
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

    public void RunAnimation(
            string keyPath,
            Keyframe[] keyframes,
            Location propLocation = Location.None,
            float propProbability = 0,
            float propDelay = 0) {
        RunAnimation(keyPath, new AnimationCurve(keyframes), propLocation, propProbability, propDelay);
    }

    // multiple shapes?
    // changing shape?
    //public void ChangeShape(shape param?)
    //public void addshape

    public IEnumerator ChangeColor(Color color, float note, float virality) {
        if (animatable.color != color) {
            animatable.color = color;
            yield return new WaitForSeconds(note);
            //StartCoroutine(Tile2AtLocation(Location.Axis).ChangeColor(color, note, virality));
            //yield return new WaitForSeconds(note);
            //StartCoroutine(Tile2AtLocation(Location.Axis).ChangeColor(color, note, virality));
            //if (adjacent.ContainsKey(Location.Right)) {
            //}
        }
    }
}
*/
