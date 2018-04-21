using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Kernel {
/*
 * Production Tables
 */

public class ProductionTable {
}

public class JM1ProductionTable : ProductionTable {

    Dictionary<System.Type, Dictionary<ProductionOutputSchema, float>> table = new Dictionary<System.Type, Dictionary<ProductionOutputSchema, float>>();

    public JM1ProductionTable() {
        //https://stackoverflow.com/questions/223058/how-to-inherit-constructors?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        AddRule(typeof(JM1RootNode),    PO(new RectBorderNode()));
        AddRule(typeof(RectBorderNode), PO(new RectMarginNode()));
        AddRule(typeof(RectMarginNode), PO(new JM1CompositeRowNode()));
        AddRule(typeof(JM1CompositeRowNode), PO(new LineRowNode(1)), 0.3f);
        AddRule(typeof(JM1CompositeRowNode),
                PO(new LineRowNode(1), new LineGapNode(), new LineRowNode(1)),
                0.3f);
        AddRule(typeof(JM1CompositeRowNode),
                PO(new LineRowNode(1), new LineGapNode(2), new LineRowNode(3), new LineGapNode(2), new LineRowNode(1)), 0.3f);
        AddRule(typeof(LineRowNode), PO(new LineNode()), 0.25f);
        AddRule(typeof(LineRowNode), PO(new LineNode(), new LineNode()), 0.25f);
        AddRule(typeof(LineRowNode), PO(new LineNode(), new LineNode(), new LineNode()), 0.25f);
        AddRule(typeof(LineRowNode), PO(new LineNode(), new LineNode(), new LineNode(), new LineNode()), 0.25f);
    }

    public void AddRule(System.Type input, ProductionOutputSchema output, float probability=1) {
        if (!table.ContainsKey(input)) {
            table[input] = new Dictionary<ProductionOutputSchema, float>();
        }
        table[input][output] = probability;
    }

    public ProductionOutputSchema PO(params BaseNode[] nodes) {
        return new ProductionOutputSchema(nodes);
    }

    /**
     * TODO should be outside
     */
    public BaseNode Produce(BaseNode root) {
        Debug.Log(root.GetType());
        if (table.ContainsKey(root.GetType())) {
            var schemaTable = table[root.GetType()];

            ProductionOutputSchema chosenSchema = null;
            float schemaValue = -1;
            foreach (var curr in schemaTable) {
                var currSchemaValue = Random.value * curr.Value;
                if (currSchemaValue > schemaValue) {
                    chosenSchema = curr.Key;
                    schemaValue = currSchemaValue;
                }
            }

            if (chosenSchema != null) {
                foreach (var child in chosenSchema.GenerateNodes()) {
                    root.AddChild(Produce(child));
                }
            }
        }

        return root;
    }
}

public class ProductionOutputSchema {
    BaseNode[] outputNodes;
    public ProductionOutputSchema(params BaseNode[] nodes) {
        outputNodes = nodes;
    }

    public List<BaseNode> GenerateNodes() {
        var bn = new List<BaseNode>();
        foreach (var prototype in outputNodes) {
            if (prototype.symbolId != -1) {
                var match = bn.Find(node => node.symbolId == prototype.symbolId);
                if (match != null) {
                    bn.Add(match);
                    continue;
                }
            }

            BaseNode newNode;
            using (MemoryStream ms = new System.IO.MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, prototype);
                ms.Position = 0;
                newNode = (BaseNode)formatter.Deserialize(ms);
            }
            bn.Add(newNode);
        }
        return bn;
    }
}


}
