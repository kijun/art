using UnityEngine;

public interface IShapeProperty {
    Vector2 center {
        get; set;
    }

    float angle {
        get; set;
    }

    Color color {
        get; set;
    }

    BorderProperty borderProperty {
        get; set;
    }

    Vector2 scale {
        get;
    }
}
