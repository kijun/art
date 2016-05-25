using UnityEngine;
using System.Collections;

public class ResourceLoader : MonoBehaviour {

    // this should probably be separate...
    // TODO line property should be factored out
    public LineProperty InstantiateLine() {
    }

    public CircleProperty InstantiatePoint() {
    }

    public RectProperty InstantiateRect() {
    }

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}

/*
public class LineProperty {
    public Color color;
    public BorderStyle style;
    public float dashLength;
    public float gapLength;
    public float length;
    public float width;
}
*/
