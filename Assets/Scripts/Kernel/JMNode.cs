using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel {
/*
 * AST Nodes
 */
public class BaseNode {
    public List<BaseNode> children = new List<BaseNode>();
    public int symbolId = -1; // annotation blabblab

    public BaseNode(int symbolId = -1) {
        this.symbolId = symbolId;
    }

    public void Render(Canvas canvas) {
        foreach (var child in children) {
            child.Render(canvas);
        }
    }
}

// line can be dumb rectangle or smart object and it should be smart
// otherwise rect should be used
// but it's really a rect
public class LineNode : BaseNode {
    public float width;
    public float offset;
    public Color color;
    /*
    public void Draw(Canvas canvas) {
        canvas.Set(offset, 0, width, 1);
        canvas.Fill(color);
        canvas.Reset();
    }
    */
}

public class RectBorderNode : BaseNode {
}

public class RectMarginNode : BaseNode {
}

public class LineGapNode : LineNode {
    public LineGapNode() {
        color = new Color(0,0,0,0); // transparent
    }
}

public class LineRowNode : BaseNode {
    public float width;
    public float offset;
}

// specific to JM
public class JM1CompositeRowNode : BaseNode {
}
public class JM1RootNode : BaseNode {
}
}
