using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anais {

    public class Route {

        private List<RouteNode> route;

        public Route() {
            route = new List<RouteNode>();
        }

        public bool IsValid() {
            return route.Count > 0;
        }

        public void Clear() {
            route.Clear();
        }

        public Direction TurnToNextTargetPosition(Vector2Int position) {
            return DirectionMapper.GetDirection(route[route.Count - 1].Position, position);
        }

        public Vector2Int GetNextTargetPosition() {
            RouteNode node = route[route.Count - 1];
            route.RemoveAt(route.Count - 1);
            return new Vector2Int(node.Position.x, node.Position.y);
        }

        public void RetraceFromNode(Node node) {
            route.Clear();
            while (node != null) {
                AddPosition(node.X, node.Y, node.ExtraCost);
                node = node.Parent;
            }
            route.RemoveAt(route.Count - 1);
        }

        public void ActivatePathPointsFrom(List<GameObject> pathPoints, Node node) {
            int i = 0;
            while (node != null) {
                if (i < pathPoints.Count) {
                    pathPoints[i].SetActive(true);
                    pathPoints[i].transform.position = MathUtils.WorldVectorFromTileCentered(new Vector2Int(node.X, node.Y));
                    node = node.Parent;
                    i++;
                } else {
                    Debug.Log("Route: can't handle " + i + " path length for display!");
                    node = null;
                }
            }
        }

        private void AddPosition(int x, int y, int cost) {
            route.Add(new RouteNode(new Vector2Int(x, y), cost));
        }

    }

}
