using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;

namespace One {
    public class RendererNewType : MonoBehaviour {
        public EngineNewType engine;
        public ViewNewType viewPrefab;

        Dictionary<Node, ViewNewType> views = new Dictionary<Node, ViewNewType>();

        void Update() {
            Render(engine.root);
        }

        public void Render(Node root) {
            foreach (Node node in root.Traverse()) {
                RenderNode(node);
                //Debug.Log(node.nodeId);
            }
            // TODO dirty?
            // remove kk
        }

        void RenderNode(Node node) {
            if (!views.ContainsKey(node)) {
                views[node] = Instantiate<ViewNewType>(viewPrefab);
            }
            ViewNewType view = views[node];
            view.SetNode(node);
            var color = node.GetValue<Color>("color");
            var position = node.GetValue<Coord>("position");
            var size = node.GetValue<Coord>("size");
            var level = node.GetValue<int>("level");
            view.UpdateView(color, position, size, level);
        }
    }

}
