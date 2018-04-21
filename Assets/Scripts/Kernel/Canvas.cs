using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel {
public class Canvas : MonoBehaviour {

    Animatable2 rectPrefab;
    // rect (normalized)
    Stack<Rect> zoomStack = new Stack<Rect>();

    void Start() {
        zoomStack.Push(new Rect(0, 0, 1, 1));
    }

    public void Zoom(float offsetX, float offsetY, float width, float height) {
        var newSize = currZoom.size.MultiplyEach(width, height);
        var newZoom = new Rect(currZoom.min + currZoom.size.MultiplyEach(offsetX, offsetY), newSize);
        zoomStack.Push(newZoom);
    }

    public void UndoZoom() {
        if (zoomStack.Count > 1) {
            zoomStack.Pop();
        } else {
            Debug.LogError("tried to pop root stack");
        }
    }

    public void Fill(Color color) {
        Rect rect = currZoomToWorldScale;
        var rp = new RectParams {
            position = rect.center, scale = rect.size, level=zoomStack.Count, color=color
        };

        NoteFactory.CreateRect(rp);
        //rect = NodeFactory.CreateRect(
    }

    Rect currZoom {
        get {
            return zoomStack.Peek();
        }
    }

    Rect currZoomToWorldScale {
        get {
            return CameraHelper.ViewportToWorldRect(currZoom);
        }
    }
}

public class MainApp : MonoBehaviour {

    JM1ProductionTable pt = new JM1ProductionTable();
    JM1NodePropertyGenerator pg = new JM1NodePropertyGenerator();
    Canvas canvas;

    void Start() {
        var root = pt.Produce(new JM1RootNode());
        pg.GenerateProperty(root);
        root.Render(canvas);
    }
}
}

