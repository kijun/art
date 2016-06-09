using UnityEngine;
using System.Collections;

public class ResourceLoader {
    // instantiate static // maybe no need to be monobehaviour

    const string LINE_PREFAB_PATH = "Shapes/UnitLine";
    const string RECT_PREFAB_PATH = "Shapes/UnitRect";
    const string CIRCLE_PREFAB_PATH = "Shapes/UnitCircle";


    // this should probably be separate...
    // TODO line property should be factored out
    public static LineRenderer InstantiateLine(LineProperty property) {
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

    public static CircleProperty InstantiateCircle(CircleProperty2 property) {
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
        return go.GetComponent<CircleProperty>();
    }

    /*
    public static RectProperty InstantiateRect() {
    }
    */
}
