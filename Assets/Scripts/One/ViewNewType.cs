using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;

namespace One {

    public class ViewNewType : MonoBehaviour{
        // image, rect,
        Animatable2 anim;
        RectParams rp = new RectParams();
        bool dirty = false;
        Node node;
        void Start() {
            anim = NoteFactory.CreateRect(rp);
        }

        public void SetNode(Node aNode) {
            node = aNode;
        }

        public void UpdateView(Color color, Coord position, Coord size, int level) {
            dirty = true;
            if (color != null) rp.color = color;
            if (position != null) rp.position = position.ToVector2();
            if (size != null) rp.scale = size.ToVector2();
            rp.level = level;
        }

        void Update() {
            if (node != null && node.destroyed) {
                Destroy(anim.gameObject);
                Destroy(gameObject);
                return;
            }
            if (!dirty || anim == null) {
                return;
            }
            anim.color = rp.color;
            if (rp.color != null) anim.color = rp.color;
            if (rp.position != null) anim.position = rp.position;
            if (rp.scale != null) anim.localScale = rp.scale;
            anim.level = rp.level;
        }

        public void Destroy() {

        }
    }
}
