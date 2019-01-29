using UnityEngine;

namespace Anais {

    public class RouteNode {

        public Vector2Int Position { get; }
        public int TerrainCost { get; }

        public RouteNode(Vector2Int position, int terrainCost) {
            Position = position;
            TerrainCost = terrainCost;
        }

    }

}
