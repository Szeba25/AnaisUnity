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

        public void Initialize(Node parent, int totalCost, int extraCost, int index) {
            Parent = parent;
            TotalCost = totalCost;
            ExtraCost = extraCost;
            Index = index;
        }

        public void InitializeMaximized() {
            Parent = null;
            TotalCost = int.MaxValue;
            ExtraCost = 0;
            Index = -1;
        }

        public void InitializeMinimized() {
            Parent = null;
            TotalCost = 0;
            ExtraCost = 0;
            Index = -1;
        }

    }
}
