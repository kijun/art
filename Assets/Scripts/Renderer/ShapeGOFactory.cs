using UnityEngine;
using System.Collections;

public static class ShapeGOFactory {
    // instantiate static // maybe no need to be monobehaviour

    const string LINE_PREFAB_PATH = "Shapes/UnitLine";
    const string RECT_PREFAB_PATH = "Shapes/UnitRect";
    const string CIRCLE_PREFAB_PATH = "Shapes/UnitCircle";

    public static ShapeRenderer InstantiateShape(ShapeProperty property) {
        ShapeRenderer shape = null;
        switch (property.shapeType) {
            case ShapeType.Circle:
                shape = InstantiateCircle((CircleProperty)property);
                break;
            case ShapeType.Line:
                shape = InstantiateLine((LineProperty)property);
                break;
            case ShapeType.Rect:
                shape = InstantiateCircle((CircleProperty)property);
                break;
            default:
                break;
        }
        return shape;
    }

    public static ShapeRenderer UpdateShapeProperty(ShapeRenderer renderer, ShapeProperty property) {
        switch (property.shapeType) {
            case ShapeType.Circle:
                ((CircleRenderer)renderer).property = (CircleProperty)property;
                break;
            case ShapeType.Line:
                ((LineRenderer)renderer).property = (LineProperty)property;
                break;
            case ShapeType.Rect:
                ((RectRenderer)renderer).property = (RectProperty)property;
                break;
            default:
                break;
        }
        return renderer;
    }

    // this should probably be separate...
    // TODO line property should be factored out
    static LineRenderer InstantiateLine(LineProperty property) {
        var prefab = Resources.Load<GameObject>(LINE_PREFAB_PATH);

        if (prefab == null) {
            Debug.LogError("couldn't load " + LINE_PREFAB_PATH);
            return null;
        }

        var go = Object.Instantiate(prefab, property.center, Quaternion.identity) as GameObject;

        if (go == null) {
            Debug.LogError("couldn't instantiate from " + property);
            return null;
        }
        var lr = go.GetComponent<LineRenderer>();
        lr.property = property;
        return lr;
    }

    static CircleRenderer InstantiateCircle(CircleProperty property) {
        var prefab = Resources.Load<GameObject>(CIRCLE_PREFAB_PATH);

        if (prefab == null) {
            Debug.LogError("couldn't load " + CIRCLE_PREFAB_PATH);
            return null;
        }

        var go = Object.Instantiate(prefab, property.center, Quaternion.identity) as GameObject;

        if (go == null) {
            Debug.LogError("couldn't instantiate from " + property);
            return null;
        }
        CircleRenderer rd = go.GetComponent<CircleRenderer>();
        rd.property = property;
        return rd;
    }

    /*
    public static RectProperty InstantiateRect() {
    }
    */
}
