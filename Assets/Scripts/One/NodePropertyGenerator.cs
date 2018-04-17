using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Marfa {
/**
 * Property Generators
 */
public class NodePropertyGenerator {
    public void GenerateProperty(BaseNode node) {
        foreach (var child in node.children) {
            GenerateProperty(child);
        }
    }
}

public class Distribution {
}

public class JM1NodePropertyGenerator : NodePropertyGenerator {

    public Distribution lineRowDistribution;
    public Distribution compositeRowDistribution;

    public void GenerateProperty(LineRowNode node) {
        switch (node.children.Count) {
            case 2:
                /*
                float width1 = lineRowDistribution.GenerateWidth(lines=2);
                node.children[0].width = width1;
                node.children[1].offset = width1;
                node.children[1].width = 1 - width1;
                */
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    public void GenerateProperty(JM1CompositeRowNode node) {
        // if one line, full, if multiple - line gap first?
        // how to (?)
    }
}


}
