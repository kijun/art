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

    public Color[] colors = {
    new Color32(145,58,28,255),
    new Color32(168,101,73,255),
    new Color32(36,36,36,255),
    new Color32(237,239,238,255),
    new Color32(242,217,81,255),
    new Color32(50,88,152,255),
    new Color32(188,204,213,255),
    new Color32(232,217,194,255),
    new Color32(185,182,179,255),
    new Color32(99,127,92,255),
    new Color32(164,155,134,255),
    new Color32(228,177,48,255)
    };


    public List<Color> usedColors = new List<Color>();

    public void Reset() {
        usedColors.Clear();
    }

    public void GenerateProperty(BaseNode node) {
        foreach (dynamic child in node.children) {
//            child.width = 1;
            GenerateProperty(child);
        }
    }

    public void GenerateProperty(LineRowNode node) {
        var cs = node.children.ToArray();
        float[] offsets;
        var widths = RandomHelper.NormalizedWidths(cs.Length, out offsets);

        for (int i = 0; i < cs.Length; i++) {
            var c = (LineNode)cs[i];
            c.width = widths[i];
            c.offset = offsets[i];
            c.color = RandomColor();
        }
        /*
        switch (cs.Length) {
            case 1:
                cs[0].width = RandomHelper.NormalizedWidths(1)[0];
                cs[0].color = RandomColor();
                break;
            case 2:
                cs[0].width = 0.5f;
                cs[1].width = 0.5f;
                cs[1].offset = 0.5f;
                cs[0].color = RandomColor();
                cs[1].color = RandomColor();
                float width1 = lneRowDistribution.GenerateWidth(lines=2);
                cs[0]width = width1;
                cs[1]offset = width1;
                cs[1]width = 1 - width1;
                break;
            case 3:
                cs[0].width = 0.5f;
                cs[1].width = 0.5f;
                cs[1].offset = 0.5f;
                cs[0].color = RandomColor();
                cs[1].color = RandomColor();
                break;
            case 4:
                cs[0].width = 0.5f;
                cs[1].width = 0.5f;
                cs[1].offset = 0.5f;
                cs[0].color = RandomColor();
                cs[1].color = RandomColor();
                break;
        }
*/
       // base.GenerateProperty(node);
        foreach (dynamic child in node.children) {
            GenerateProperty(child);
        }
    }

    public ColorS RandomColor() {
        Color c;
        if (usedColors.Count < 3) {
            c = RandomHelper.Pick<Color>(colors);
            usedColors.Add(c);
        } else {
            c = usedColors[Random.Range(0, usedColors.Count)];
        }
        return new ColorS(c.r, c.g, c.b, c.a);
    }

    public void GenerateProperty(RectMarginNode node) {
        if (Random.value > 0.2f) {
            node.left = Random.value * 0.1f + 0.02f;
            node.right = node.left;
            node.bottom = Random.value * 0.1f + 0.05f;
            node.top = node.bottom;
        }
//        base.GenerateProperty(node);
        // if one line, full, if multiple - line gap first?
        // how to (?)
        foreach (dynamic child in node.children) {
            child.width = 1;
            GenerateProperty(child);
        }
    }

    public void GenerateProperty(JM1CompositeRowNode node) {
        var cs = node.children.ToArray();
        float[] offsets;
        var widths = RandomHelper.NormalizedWidths(cs.Length, out offsets);

        for (int i = 0; i < cs.Length; i++) {
            var type = cs[i].GetType();
            cs[i].width = widths[i];
            cs[i].offset = offsets[i];
            /*
            if (type == typeof(LineRowNode)) {
                var c = (LineRowNode)cs[i];
            } else if (type == typeof(LineGapNode)) {
                var c = (LineGapNode)cs[i];
                c.width = widths[i];
                c.offset = offsets[i];
            }
            */
        }

        /*
        switch (node.children.Count) {
            case 1:
                ((LineRowNode)node.children[0]).width = 1;
                break;
            case 3:
                ((LineRowNode)node.children[0]).width = 1.0f/3;
                ((LineGapNode)node.children[1]).width = 1.0f/3;
                ((LineGapNode)node.children[1]).offset = 1.0f/3;
                ((LineRowNode)node.children[2]).width = 1.0f/3;
                ((LineRowNode)node.children[2]).offset = 2.0f/3;
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
                ((LineRowNode)node.children[4]).offset = 4.0f/5;
                break;
        }
        */

//        base.GenerateProperty(node);
        // if one line, full, if multiple - line gap first?
        // how to (?)
        foreach (dynamic child in node.children) {
            GenerateProperty(child);
        }
    }
}


}
