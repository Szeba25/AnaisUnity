namespace Anais {

    public class Node {

        public int X { get; private set; }
        public int Y { get; private set; }

        public long OpenListId { get; set; }
        public long ClosedListId { get; set; }

        public Node Parent { get; set; }
        public int TotalCost { get; set; }
        public int ExtraCost { get; set; }
        public int Index { get; set; }

        /// <summary>
        /// Create a node at x and y coordinates, with default parameters.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Node(int x, int y) {
            X = x;
            Y = y;
            OpenListId = 0;
            ClosedListId = 0;
            Parent = null;
            TotalCost = 0;
            ExtraCost = 0;
            Index = -1;
        }

        /// <summary>
        /// Initialize the node with a custom parameter set.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="totalCost"></param>
        /// <param name="extraCost"></param>
        /// <param name="index"></param>
        public void Initialize(Node parent, int totalCost, int extraCost, int index) {
            Parent = parent;
            TotalCost = totalCost;
            ExtraCost = extraCost;
            Index = index;
        }

        /// <summary>
        /// Initialize the node with the maximum possible cost.
        /// </summary>
        public void InitializeMaximized() {
            Parent = null;
            TotalCost = int.MaxValue;
            ExtraCost = 0;
            Index = -1;
        }

        /// <summary>
        /// Initialize the node with the minimum possible cost.
        /// </summary>
        public void InitializeMinimized() {
            Parent = null;
            TotalCost = 0;
            ExtraCost = 0;
            Index = -1;
        }

    }
}
