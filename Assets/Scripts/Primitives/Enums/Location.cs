using UnityEngine;
using System.Linq;

[System.Flags]
public enum Location {
    None        = 0,
    Top         = 1 << 0,
    Right       = 1 << 1,
    Bottom      = 1 << 2,
    Left        = 1 << 3,
    TopRight    = 1 << 4,
    TopLeft     = 1 << 5,
    BottomRight = 1 << 6,
    BottomLeft  = 1 << 7,

    Axis        = Top | Right | Bottom | Left,
    Diagonal    = TopRight | TopLeft | BottomRight | BottomLeft,

    Any         = Axis | Diagonal
}

public static class LocationMethods {
    static Location[] LocationPrimitives = new Location[]{Location.Top, Location.Right, Location.Bottom, Location.Left, Location.TopRight, Location.TopLeft, Location.BottomRight, Location.BottomLeft};
    /**
     * Usage:
     *   Location.Axis.ChooseRandom() = Location.Top
     *
     */
    public static Location ChooseRandom(this Location loc) {
        return loc.ChooseRandom(LocationPrimitives);
    }

    public static Location ChooseRandom(this Location loc, Location choices) {
        return (loc&choices).ChooseRandom(LocationPrimitives);
    }

    public static Location ChooseRandom(this Location loc, Location[] choices) {
        choices = choices.Where(l => (l & loc) == l).ToArray();

        if (choices.Length == 0) return Location.None;
        return choices[Random.Range(0, choices.Length)];
    }
}

