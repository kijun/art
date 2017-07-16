using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;
/*
 * Looks
    * Adjacent cascading color
    * Change of translucency
    * Rect to circle
    * Multiple shapes with translucency
 * Transition
    * Return to original
    * Gradually change color
    * Animation - KeyframeAnim - should have start time, end time
    * Virality - percentage but also activation time
*/

public class Tile : MonoBehaviour {
    /***** PUBLIC: Variable *****/
    public int row;
    public int col;

    /***** PRIVATE: Variable *****/
    Dictionary<Location, Tile> adjacent = new Dictionary<Location, Tile>();
    Location adjacentFlags = Location.None;
    TileMutexFlag mutex = TileMutexFlag.None;

    /***** PUBLIC STATIC METHOD *****/
    public static Tile[,] CreateBoard(int cols, int rows, float length) {
        Debug.Log("Creating board " + cols + " " + rows);
        var board = new Tile[cols, rows]; // x, y
        var diagonal = length * 1.414f;
        var xmin = -(diagonal*(cols-1))/2;
        var ymin = -(diagonal*(rows-1))/2;
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                var rp = new RectParams {
                    x = xmin + i * diagonal,
                    y = ymin + j * diagonal,
                    width = length,
                    height = length,
                    color = new Color32(1, 104, 200, 255)
                };

                var anim = NoteFactory.CreateRect(rp);
                var tile = anim.gameObject.AddComponent<Tile>();
                board[i, j] = tile;
            }
        }

        for (int x = 0; x < cols; x++) {
            for (int y = 0; y < rows; y++) {
                var tile = (Tile)board.GetValue2(x, y);
                tile.AddAdjacentTile(Location.TopLeft,     (Tile)board.GetValue2(x-1, y-1));
                tile.AddAdjacentTile(Location.Top,         (Tile)board.GetValue2(x,   y-1));
                tile.AddAdjacentTile(Location.TopRight,    (Tile)board.GetValue2(x+1, y-1));
                tile.AddAdjacentTile(Location.Left,        (Tile)board.GetValue2(x-1, y));
                tile.AddAdjacentTile(Location.Right,       (Tile)board.GetValue2(x+1, y));
                tile.AddAdjacentTile(Location.BottomLeft,  (Tile)board.GetValue2(x-1, y+1));
                tile.AddAdjacentTile(Location.Bottom,      (Tile)board.GetValue2(x,   y+1));
                tile.AddAdjacentTile(Location.BottomRight, (Tile)board.GetValue2(x+1, y+1));
            }
        }

        return board;
    }

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
            //StartCoroutine(TileAtLocation(Location.Axis).ChangeColor(color, note, virality));
            //yield return new WaitForSeconds(note);
            //StartCoroutine(TileAtLocation(Location.Axis).ChangeColor(color, note, virality));
            //if (adjacent.ContainsKey(Location.Right)) {
            //}
        }
    }

    public Tile TileAtLocation(Location loc, TileMutexFlag mutexFlag = TileMutexFlag.None) {
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

    public void AddAdjacentTile (Location loc, Tile tile) {
        if (tile != null) {
            adjacentFlags |= loc;
            adjacent.Add(loc, tile);
        }
    }

    public Animatable2 animatable {
        get {
            return GetComponent<Animatable2>();
        }
    }
/*
 * behaviors?
 *
 * change color
 * disappear
 * enlarge (certain direction)
 * movement (permanent, temporary)
 * additional (max bill)
 * rotate
 * graphical (font, ikko tanaka)
 * turn to circle, or a particular shape
 *
 */
}

