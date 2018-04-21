using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kernel {
/**
 * Property Generators
 */
public class NodePropertyGenerator {
    /*
    public void GenerateProperty(BaseNode node) {
        foreach (dynamic child in node.children) {
            Debug.LogError(child.GetType());
            GenerateProperty(child);
        }
    }
    */
}

public class Distribution {
}

public class JM1NodePropertyGenerator : NodePropertyGenerator {

    public Distribution lineRowDistribution;
    public Distribution compositeRowDistribution;

    public void GenerateProperty(BaseNode node) {
        foreach (dynamic child in node.children) {
            GenerateProperty(child);
        }
    }

    public void GenerateProperty(LineRowNode node) {
        switch (node.children.Count) {
            case 1:
                ((LineNode)node.children[0]).width = 1;
                ((LineNode)node.children[0]).color = RandomColor();
                break;
            case 2:
                ((LineNode)node.children[0]).width = 0.5f;
                ((LineNode)node.children[1]).width = 0.5f;
                ((LineNode)node.children[1]).offset = 0.5f;
                ((LineNode)node.children[0]).color = RandomColor();
                ((LineNode)node.children[1]).color = RandomColor();
                /*
                float width1 = lineRowDistribution.GenerateWidth(lines=2);
                node.children[0].width = width1;
                node.children[1].offset = width1;
                node.children[1].width = 1 - width1;
                */
                break;
            case 3:
                ((LineNode)node.children[0]).width = 0.5f;
                ((LineNode)node.children[1]).width = 0.5f;
                ((LineNode)node.children[1]).offset = 0.5f;
                ((LineNode)node.children[0]).color = RandomColor();
                ((LineNode)node.children[1]).color = RandomColor();
                break;
            case 4:
                ((LineNode)node.children[0]).width = 0.5f;
                ((LineNode)node.children[1]).width = 0.5f;
                ((LineNode)node.children[1]).offset = 0.5f;
                ((LineNode)node.children[0]).color = RandomColor();
                ((LineNode)node.children[1]).color = RandomColor();
                break;
        }
       // base.GenerateProperty(node);
        foreach (dynamic child in node.children) {
            GenerateProperty(child);
        }
    }

    public ColorS RandomColor() {
        return new ColorS(Random.value, Random.value, Random.value, 0.7f);
    }

    public void GenerateProperty(RectMarginNode node) {
        node.left = Random.value * 0.1f;
        node.right = Random.value * 0.1f;
        node.bottom = Random.value * 0.1f;
        node.top = Random.value * 0.1f;
//        base.GenerateProperty(node);
        // if one line, full, if multiple - line gap first?
        // how to (?)
        foreach (dynamic child in node.children) {
            GenerateProperty(child);
        }
    }

    public void GenerateProperty(JM1CompositeRowNode node) {
        switch (node.children.Count) {
            case 1:
                ((LineRowNode)node.children[0]).width = 1;
                break;
            case 3:
                ((LineRowNode)node.children[0]).width = 1.0f/3;
                ((LineGapNode)node.children[1]).width = 1.0f/3;
                ((LineGapNode)node.children[1]).offset = 1.0f/3;
                ((LineRowNode)node.children[2]).width = 1.0f/3;
                break;
            case 5:
                ((LineRowNode)node.children[0]).width = 1.0f/5;
                ((LineGapNode)node.children[1]).width = 1.0f/5;
                ((LineGapNode)node.children[1]).offset = 1.0f/5;
                ((LineRowNode)node.children[2]).width = 1.0f/5;
                ((LineRowNode)node.children[2]).offset = 2.0f/5;
                ((LineGapNode)node.children[3]).width = 1.0f/5;
                ((LineGapNode)node.children[3]).offset = 3.0f/5;
                ((LineRowNode)node.children[4]).width = 1.0f/5;
                break;
        }

//        base.GenerateProperty(node);
        // if one line, full, if multiple - line gap first?
        // how to (?)
        foreach (dynamic child in node.children) {
            GenerateProperty(child);
        }
    }
}


}
