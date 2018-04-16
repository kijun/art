using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureShape;
namespace One {
public class Node {
    // contains a dict
    public Node parent;
    Dictionary<string, object> dict = new Dictionary<string, object>();
    List<Node> children = new List<Node>();
    public int nodeId;
    public bool destroyed = false;
    bool dirty = true;

    // TODO dirty

    public Node() {
        dict["children"] = children;
    }

    public void SetValue(string key, object val) {
        if (nodeId == 0) {
            nodeId = Random.Range(1, 100000);
        }
        // TODO reject children
        dict[key] = val;
    }

    public object GetValue(string key) {
        object obj;
        bool val = dict.TryGetValue(key, out obj);
        return obj;
    }

    public T GetValue<T>(string key) {
        var v = GetValue(key);
        if (v == null) {
            return default(T);
        }
        return (T)v;
    }

    public Node GetChild() {
        return children[0];
    }

    public List<Node> GetChildren() {
        return children;
    }

    public void AddChild(Node node) {
        children.Add(node);
        //Debug.Log("added " + node);
        node.parent = this;
    }

    public void InsertChild(int idx, Node node) {
        children.Insert(idx, node);
        node.parent = this;
    }

    public void RemoveChild(Node node) {
        node.parent = null;
        children.Remove(node);
    }

    public void RemoveChild(int index) {
        Node child = children[index];
        if (child != null) {
            child.parent = null;
            children.RemoveAt(index);
        }
    }

    public IEnumerable<Node> Traverse() {
        yield return this;
        // TODO recursion? tail optimization?
        foreach (var child in children) {
            foreach (var c in child.Traverse()) {
                yield return c;
            }
        }
    }

    public void RemoveFromTree() {
        if (parent != null) {
            foreach (var child in children) {
                parent.AddChild(child);
            }
            parent.RemoveChild(this);
        }
        destroyed = true;
    }
}

}
