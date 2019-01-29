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

        /// <summary>
        /// Create a new node graph search. The search is NOT ready when constructed. (Call SetMapData and Initialize).
        /// </summary>
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

        /// <summary>
        /// Called to process as much iterations as possible, and timing the operation.
        /// </summary>
        public void TimedCalculate() {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Calculate();
            stopwatch.Stop();
            UnityEngine.Debug.Log("NodeGraphSearch: calculation finished in " + stopwatch.Elapsed.TotalMilliseconds + "ms and " +
                iterations + " iterations");
        }

        /// <summary>
        /// Called to process as much iterations as possible.
        /// </summary>
        public void Calculate() {
            Calculate(maxIterations);
        }

        /// <summary>
        /// Called to process pathfinding calculation. You can specify a max iteration count here.
        /// </summary>
        /// <param name="maxCycleIterations"></param>
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

        /// <summary>
        /// Sets the map data on which the pathfinder will operate.
        /// </summary>
        /// <param name="mapData"></param>
        public void SetMapData(MapData mapData) {
            if (!initialized) { 
                this.mapData = mapData;
            } else {
                UnityEngine.Debug.Log("NodeGraphSearch: cannot change map data on an initialized search");
            }
        }

        /// <summary>
        /// Sets a new maximum iteration count. After this iteration count the pathfinder will stop.
        /// </summary>
        /// <param name="maxIterations"></param>
        public void SetMaxIterations(int maxIterations) {
            if (!initialized) {
                this.maxIterations = maxIterations;
            } else {
                UnityEngine.Debug.Log("NodeGraphSearch: cannot change max iterations on an initialized search");
            }
        }

        /// <summary>
        /// Sets a new maximum cost. Nodes which cost more than this value are ignored in the next search.
        /// </summary>
        /// <param name="maxCost"></param>
        public void SetMaxCost(int maxCost) {
            if (!initialized) {
                this.maxCost = maxCost;
            } else {
                UnityEngine.Debug.Log("NodeGraphSearch: cannot change max cost on an initialized search");
            }
        }

        /// <summary>
        /// Initialize the pathfinder before starting the search.
        /// </summary>
        /// <param name="searchAgent"></param>
        /// <param name="allAgents"></param>
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

        /// <summary>
        /// Add all search agents prior searching to the closed list.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="allAgents"></param>
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

        /// <summary>
        /// Stop the pathfinding process.
        /// </summary>
        private void Stop() {
            initialized = false;
            finished = true;
        }

        /// <summary>
        /// Add sorrounding nodes to the heap (open list).
        /// </summary>
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

        /// <summary>
        /// Add a new node to the open list.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="parent"></param>
        /// <param name="extraCost"></param>
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

        /// <summary>
        /// Check for the specified coordinate, if the pathfinder should ignore this.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if the pathfinder should ignore the coordinate</returns>
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

        /// <summary>
        /// Adds a path from a coordinate to the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddPathFrom(IUnitObject obj, int x, int y) {
            if (finished) {
                Node node = mapData.GetNode(x, y);
                if (node.ClosedListId == searchID) {
                    obj.Unit.Body.RetracePath(node);
                }
            }
        }

        /// <summary>
        /// Get all nodes the pathfinder found, and store it in the passed list.
        /// </summary>
        /// <param name="list"></param>
        public void GetAllNodes(List<Node> list) {
            GetNodesWithMaxCost(list, int.MaxValue);
        }

        /// <summary>
        /// Get nodes with a maximum cost specified.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="maxCost"></param>
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
