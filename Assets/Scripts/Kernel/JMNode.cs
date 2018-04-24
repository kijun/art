using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel {
/*
 * AST Nodes
 */
[System.Serializable]
public class BaseNode {
    public List<BaseNode> children = new List<BaseNode>();
    public int symbolId = -1; // annotation blabblab
    public float width = 1;
    public float offset;

    public BaseNode(int symbolId = -1) {
        this.symbolId = symbolId;
    }

    public void AddChild(BaseNode child) {
        children.Add(child);
    }

    public virtual void Render(Canvas canvas) {
        Debug.Log("render " + this);
        canvas.Zoom(offset, 0, width, 1);
        RenderNode(canvas);
        foreach (var child in children) {
            child.Render(canvas);
        }
        canvas.UndoZoom();
    }

    public virtual void RenderNode(Canvas canvas) {
    }
}

// line can be dumb rectangle or smart object and it should be smart
// otherwise rect should be used
// but it's really a rect
[System.Serializable]
public class LineNode : BaseNode {
    public ColorS color = new ColorS(0, 0, 0, 0f);

    public override void RenderNode(Canvas canvas) {
        Debug.Log("RENDERING LINE");
        canvas.Fill(color.ToColor());
    }

    public LineNode(int symbolId = -1) : base(symbolId) {
    }
}

[System.Serializable]
public class RectBorderNode : BaseNode {
}

[System.Serializable]
public class RectMarginNode : BaseNode {
    public float top;
    public float left;
    public float right;
    public float bottom;

    public override void Render(Canvas canvas) {
        canvas.Zoom(left, bottom, 1 - left - right, 1 - top - bottom);
        base.Render(canvas);
        canvas.UndoZoom();
    }
}

[System.Serializable]
public class LineGapNode : LineNode {
    public LineGapNode(int symbolId = -1) : base(symbolId) {
    }
}

[System.Serializable]
public class LineRowNode : BaseNode {

    public LineRowNode(int symbolId = -1) : base(symbolId) {
    }
}

// specific to JM
[System.Serializable]
public class JM1CompositeRowNode : BaseNode {
}
[System.Serializable]
public class JM1RootNode : BaseNode {
}

[System.Serializable]
public class ColorS {
    float r, g, b, a;

    public ColorS(float r, float g, float b, float a) {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color ToColor() {
        return new Color(r, g, b, a);
    }
}
}
