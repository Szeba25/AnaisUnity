using System.Collections.Generic;

namespace Anais {

    public class NodeCollection {

        private List<Node> nodes;

        public int Count { get => nodes.Count; }
        public Node this[int key] { get => nodes[key]; }

        public NodeCollection() {
            nodes = new List<Node>();
        }

        public void Clear() {
            nodes.Clear();
        }

        public void Add(Node node) {
            nodes.Add(node);
        }

    }

}
