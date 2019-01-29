using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Anais {

    public class NodeGraphSearch {

        private long searchID;

        private MapData mapData;

        private Node current;
        private MinHeap openList;

        private bool initialized;
        private bool finished;
        private int maxIterations;
        private int maxCost;
        private int iterations;

        public NodeGraphSearch() {
            searchID = 0;

            mapData = null;

            current = null;
            openList = new MinHeap();

            initialized = false;
            finished = false;
            maxIterations = int.MaxValue;
            maxCost = int.MaxValue;
            iterations = 0;
        }

        public void TimedCalculate() {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Calculate();
            stopwatch.Stop();
            UnityEngine.Debug.Log("NodeGraphSearch: calculation finished in " + stopwatch.Elapsed.TotalMilliseconds + "ms and " +
                iterations + " iterations");
        }

        public void Calculate() {
            Calculate(maxIterations);
        }

        public void Calculate(int maxCycleIterations) {
            if (initialized) {
                int cycleIterations = 0;
                while (cycleIterations < maxCycleIterations) {
                    if (openList.Empty() || iterations >= maxIterations) {
                        Stop();
                        break;
                    }

                    cycleIterations++;
                    iterations++;

                    current = openList.Poll();
                    current.OpenListId = 0;
                    current.ClosedListId = searchID;
                    AddNodesToHeap();
                }
                if (iterations >= maxIterations) {
                    Stop();
                }
            } else {
                UnityEngine.Debug.Log("NodeGraphSearch: calculate called on uninitialized search");
            }
        }

        public void SetMapData(MapData mapData) {
            if (!initialized) { 
                this.mapData = mapData;
            } else {
                UnityEngine.Debug.Log("NodeGraphSearch: cannot change map data on an initialized search");
            }
        }

        public void SetMaxIterations(int maxIterations) {
            if (!initialized) {
                this.maxIterations = maxIterations;
            } else {
                UnityEngine.Debug.Log("NodeGraphSearch: cannot change max iterations on an initialized search");
            }
        }

        public void SetMaxCost(int maxCost) {
            if (!initialized) {
                this.maxCost = maxCost;
            } else {
                UnityEngine.Debug.Log("NodeGraphSearch: cannot change max cost on an initialized search");
            }
        }

        public void Initialize(IUnitObject searchAgent, List<IUnitObject> allAgents) {
            searchID++;

            Vector2Int tilePosition = searchAgent.Unit.Body.TilePosition;
            Node startNode = mapData.GetNode(tilePosition.x, tilePosition.y);
            startNode.InitializeMinimized();

            openList.Clear();

            startNode.OpenListId = searchID;
            openList.Add(startNode);

            AddSearchAgentsToClosed(searchAgent, allAgents);

            initialized = true;
            finished = false;
            iterations = 0;

            current = null;
        }

        private void AddSearchAgentsToClosed(IUnitObject subject, List<IUnitObject> allAgents) {
            for (int i = 0; i < allAgents.Count; i++) {
                IUnitObject obj = allAgents[i];
                if (!ReferenceEquals(subject, obj)) {
                    Vector2Int tilePosition = obj.Unit.Body.TilePosition;
                    Node node = mapData.GetNode(tilePosition.x, tilePosition.y);
                    node.ClosedListId = searchID;
                    node.InitializeMaximized();
                }
            }
        }

        private void Stop() {
            initialized = false;
            finished = true;
        }

        private void AddNodesToHeap() {
            int x = current.X;
            int y = current.Y;

            bool up = mapData.GetCollision(x, y + 1);
            bool right = mapData.GetCollision(x + 1, y);
            bool left = mapData.GetCollision(x - 1, y);
            bool down = mapData.GetCollision(x, y - 1);

            AddNode(x + 1, y, current, 10);
            AddNode(x - 1, y, current, 10);
            AddNode(x, y + 1, current, 10);
            AddNode(x, y - 1, current, 10);

            if (!left && !up) AddNode(x - 1, y + 1, current, 14);
            if (!right && !down) AddNode(x + 1, y - 1, current, 14);
            if (!left && !down) AddNode(x - 1, y - 1, current, 14);
            if (!right && !up) AddNode(x + 1, y + 1, current, 14);
        }

        private void AddNode(int x, int y, Node parent, int extraCost) {
            if (!mapData.GetCollision(x, y) && !IgnoreThis(x, y)) {
                int newTotalCost = parent.TotalCost + extraCost;
                if (newTotalCost <= maxCost) {
                    Node added = mapData.GetNode(x, y);
                    added.Initialize(parent, newTotalCost, extraCost, -1);
                    added.OpenListId = searchID;
                    openList.Add(added);
                }
            }
        }

        private bool IgnoreThis(int x, int y) {
            Node node = mapData.GetNode(x, y);
            if (node.ClosedListId == searchID) {
                return true;
            } else if (node.OpenListId == searchID) {
                int extraCost = 14;
                if (node.X == current.X || node.Y == current.Y) {
                    extraCost = 10;
                }
                if (node.TotalCost > current.TotalCost + extraCost) {
                    node.TotalCost = current.TotalCost + extraCost;
                    node.Parent = current;
                    openList.Decrease(node.Index);
                }
                return true;
            } else {
                return false;
            }
        }

        /*
         * Untility functions for accessing and displaying the path
         */

        public void AddPathFrom(IUnitObject obj, int x, int y) {
            if (finished) {
                Node node = mapData.GetNode(x, y);
                if (node.ClosedListId == searchID) {
                    obj.Unit.Body.RetracePath(node);
                }
            }
        }

        public void GetAllNodes(List<Node> list) {
            GetNodesWithMaxCost(list, int.MaxValue);
        }

        public void GetNodesWithMaxCost(List<Node> list, int maxCost) {
            list.Clear();
            for (int x = 0; x < mapData.Width; x++) {
                for (int y = 0; y < mapData.Height; y++) {
                    Node node = mapData.GetNodeDirect(x, y);
                    if (node.ClosedListId == searchID && node.TotalCost <= maxCost) {
                        list.Add(node);
                    }
                }
            }
        }

    }

}
