using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Marfa {
/*
 * Production Tables
 */

public class ProductionTable {
}

public class JM1ProductionTable : ProductionTable {

    Dictionary<System.Type, Dictionary<ProductionOutputSchema, float>> table = new Dictionary<System.Type, Dictionary<ProductionOutputSchema, float>>();

    public JM1ProductionTable() {
        //https://stackoverflow.com/questions/223058/how-to-inherit-constructors?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        AddRule(typeof(JM1RootNode), new ProductionOutputSchema(new RectBorderNode()));
        /*
        AddRule(typeof(JM1CompositeRowNode),
                new ProductionOutputSchema(new LineRowNode(1), new LineGapNode(), new LineRowNode(1)));
                */
        //AddRule(JM1CompositeRowNode, PO(F(LineRowNode, 1), F(LineGapNode), F(LineRowNode, 1)));
        // HAX or overload annotation of node and use duplicate
        // meta annotation?
        //
        //
        /*
         * problem is that i cannot annotate how the first and the last are the same
         * line rows
         * if instead, we use a... but then how would we clone it?
         */
    }

    public void AddRule(System.Type input, ProductionOutputSchema output, float probability=1) {
        if (table.ContainsKey(input)) {
            table[input][output] = probability;
        }
    }

    /**
     * TODO should be outside
     */
    public BaseNode RunProduction(BaseNode root) {
        var schemas = table[root.GetType()];
        return null;
        /*

           TODO pick best schema
        foreach (var child in bestOutput.GenerateNodes()) {
            root.AddChild(child);
        }
        */
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
            var match = bn.Find(node => node.symbolId != -1 && node.symbolId == prototype.symbolId);
            if (match != null) {
                bn.Add(match);
            } else {
                BaseNode newNode;
                using (MemoryStream ms = new System.IO.MemoryStream()) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, prototype);
                    ms.Position = 0;
                    newNode = (BaseNode)formatter.Deserialize(ms);
                }
            }
        }
        return bn;
    }
}


}
