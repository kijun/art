using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * how to name each process?
 * we have a TransformationProbabilityTable
 * TransformationKey
 * but does it also have a
 *
 * (termination is indicated by no-op (or do we require keys?)
 *
 * Sentence - just symbols SymbolicSentence - tree of nodes
 * VisuallyDefinedSentence - still tree of nodes
 * SymbolToVisualTranslator - but does it require multiple types of data? or just
 * VisualSentenceRenderer
 *
 */

public struct Grammar

struct Key {
  public readonly int Dimension1;
  public readonly bool Dimension2;
  public Key(int p1, bool p2) {
    Dimension1 = p1;
    Dimension2 = p2;
  }
  // Equals and GetHashCode ommitted
}

public class ViewModel {
    public class
        // properties and stuff
}

public class Symbol {
    public List<Symbol> children;
}

public class TransformationProbabilityTable {
    Dictionary<TransformationKey, float> probTable;
    public void Add(string k1, string k2, float probability) { // should really be a probability curve
    }

    public Symbol ProduceSymbolicSentence(string startAlphabet) {
        var root = new Symbol(startAlphabet)
        if (probTable[curr]) {
            // TODO calculate the highest probability
            // if

        }

        // TODO flatten
    }
}

/*
 * Production Tables
 */
public class ProductionTable {
}

public struct ProductionRule {
    public Type from;
    public ProductionOutput from;
}

public class JM1ProductionTable {

    Dictionary<ProductionRule, float> table = new Dictionary<ProductionRule, float>();

    public void AddProductionRule(Type input, ProductionOutput output, float probability=1) {
        table.Add(new ProductionRule(input, output), probability); // probably better to run a function
    }

    /**
     * TODO should be somewhere else
     */
    public BaseNode RunProduction(Type input) {

    }
}

public class ProductionOutput {
}

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

public class JM1NodePropertyGenerator : NodePropertyGenerator {

    public Randomizer(?) lineRowDistribution;
    public Randomizer(?) compositeRowDistribution;

    public void GenerateProperty(LineRowNode node) {
        switch (node.chilren.count) {
            case 2:
                float width1 = lineRowDistribution.GenerateWidth(lines=2);
                node.children[0].width = width1;
                node.children[1].offset = width1;
                node.children[1].width = 1 - width1;
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    public void GenerateProperty(JM1CompositeRowNode node) {
        // if one line, full, if multiple - line gap first?
    }
}

/*
 * AST Nodes
 */

public class BaseNode {
    public List<BaseNode> children = new List<BaseNode>();

    public void Draw(Canvas canvas) {
        foreach (var child in children) {
            child.Draw(canvas);
        }
    }
}

// line can be dumb rectangle or smart object and it should be smart
// otherwise rect should be used
// but it's really a rect
public class LineNode {
    public float width;
    public float offset;
    public Color color;
    public void Draw(Canvas canvas) {
        canvas.Set(offset, 0, width, 1);
        canvas.Fill(color);
        canvas.Reset();
    }
}

public class RectBorderNode {
}

public class RectMarginNode {
}

public class LineGapNode : LineNode {
    public LineGapNode {
        color = new Color(0,0,0,0); // transparent
    }
}

public class LineRowNode {
    public float width;
    public float offset;
}

// specific to JM
public class JM1CompositeRowNode {
}






public class AST {
}

public class Symbolickj

public class ViewModel

public class Grammar : MonoBehaviour {

	Dictionary<string, string>
	// Use this for initialization
	void Start () {
	}

    void Render() {
        /*
         * TPT = {
         *
         * }
         * TPT.Add("start", "jm1")
         * TPT.Add("jm1", "rectWithBorder")
         * TPT.Add("rectWithBorder", "rectWithMargin")
         * TPT.Add("rectWithMargin", "mclaughlin1CompositeView")
         *
         * how would you convert the statement above?
         *
         * I'd first construct
         *
         * TPT.Add("jm1compositeView", "jm1lineRect_1", gaussian(delta, margin))
         * TPT.Add("jm1compositeView", "jm1lineRect_1 lineGap jm1lineRect_1", gaussian(delta, margin))
         * TPT.Add("jm1compositeView", "jm1lineRect_1 lineGap jm1lineRect_2 lineGap jm1lineRect_1", gaussian(delta, margin))
         * TPT.Add("jm1lineRect", "line", 1) // share similar probability function
         * TPT.Add("jm1lineRect", "line line", 1)
         * TPT.Add("jm1lineRect", "line line line", 1)
         * TPT.Add("jm1lineRect", "line line line line", 1) // how to define width?
         *
         * okay so that's the grammar
         *
         * from the grammar need to define ViewModel, which has no probability built into it
         *
         * rectWithBorder(0.1, color)
         * |
         * rectWithMargin(0.1, color)
         * |
         * jm1compositeView()
         * |
         * jm1lineRect(0.1) lineGap(0.5) jm1lineRect(0.1)
         *
         * scale is always proportional, not definite
         *
         *
         * Rend.Register("jm1lineRect", new ViewRenderer(5, 4,3)) // general renderer
         * Rend.Register("rectWithBorder", new ViewRenderer(5, 4,3)) // general renderer
         * Rend.GetRenderer("jm1lineRect").GetScale()
         * ViewModelGenerator("jm1lineRect", new LineRectViewModelGenerator()) // 1% scale
         *
         * all lines have same distribution... what
         * let's just work on it for now, there's some insertion here, but i can't really identify it
         *
         * jm1lineRect
         *
         * vm = produceViewModel(jmSymbolicSentence)
         *
         * draw(vm, canvas)
         *
         * vm = border(0.2, c1)
         * margin(0.1, c2)
         * lineRect(0.4, c3) lineGap(0.3) lineRect(0.4, c3)
         * line(1)                        line(1)
         *
         * but how does jm1compositeView produce three different views within itself?
         * actually, there are cases where views can overlap each other,
         * or subdivide in various curious ways
         * def produceSymbolicSentence
         *
         * def produceViewModel(symbolicNode):
         *    if (symbolicMode.viewModel) return symbolicNode.viewModel
         *    rd = getRenderer(symbolicNode) // jm1 to jm1renderer
         *    symbolicNode.viewModel = rd.produceViewModelNode()
         *    foreach childNode in symbolicNode.children()
         *      cvm = produceViewModel(childNode)
         *      symbolicNode.viewModel.addChild(cvm)
         *    symbolicNode.normalizeChildScale()
         *    return symbolicNode.viewModel
         *
         * def draw(viewModel, canvas):
         *    draw()
         *
         * it doesn't really make sense to have scale reside within itself - does it?
         * (generally) 3 : 2 : 1
         * but it means jm1lineRect must already know of its own scale, right?
         * i think we can make it as a part of view
         *
         * class GenericViewModel() {
         *      void Draw() {
         *      }
         * }
         *
         * class LineRectViewModel() {
         *    void Draw(canvas) {
         *        Canvas.Fill(color)
         *    }
         * }
         *
         * lineRenderer
         * each component knows how to draw itself - is that really true in this case?
         *
         *
         * ColorPT
         *
         * startSymbol
         * "jm"
         * new Node(e)
         * getNextNode(out text, ?)
         */
    }

	// Update is called once per frame
	void Update () {

	}
}
