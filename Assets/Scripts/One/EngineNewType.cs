using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;

namespace One {

public class EngineNewType : MonoBehaviour {
    public Node root = new Node();

    public void TickEvent() {
        HandleTick();
    }

    public void InputEvent(string key, object val) {
        //Debug.Log(key + " " + val);
        switch (key) {
            case "right":
                HandleRight();
                break;
            case "left":
                HandleLeft();
                break;
            case "up":
                HandleUp();
                break;
            case "down":
                HandleDown();
                break;
        }
    }

    void IncreaseLocalSymmetry() {
    }

    void AddLevel() {
    }

    void RemoveLevel() {
    }

    void NestNode() {
    }


    // the sense of belonging - to a tree - does not quite exist s- how
    // by simulating vertices?and drawing lines?
    //
    // views = color
    // localScale
    // localRotation
    //
    //
    // TODO new node that borrows values from paren

    void HandleUp() {
        // insert random node at a random position
        var parent = GetRandomNode();
        var node = new Node();
        node.SetValue("size", new Coord(Random.Range(1, 5), Random.Range(1, 5)));
        node.SetValue("position", new Coord(Random.Range(-5, 5), Random.Range(-5, 5)));
        node.SetValue("color", ColorUtil.RandomColor());
        parent.AddChild(node);
    }

    void HandleDown() {
        // remove a (random) node
        var node = GetRandomNode();
        if (node != root) {
            node.RemoveFromTree();
        }
    }

    void HandleLeft() {
        // change color of random node
        var node = GetRandomNode();
        node.SetValue("color", ColorUtil.RandomColor());
    }

    void HandleRight() {
        // change size of random node
        var node = GetRandomNode();
        node.SetValue("size", new Coord(Random.Range(1, 5), Random.Range(1, 5)));
    }

    void HandleTick() {
        // hmm do something at some probability
    }

    Node GetRandomNode() {
        Node picked = root;
        int idx = 1;
        foreach (var node in root.Traverse()) {
            var rand = Random.Range(0, idx);
            if (rand == 0) {
                picked = node;
            }
            idx++;
        }
        return picked;
    }
}

}

