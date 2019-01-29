using UnityEngine;
using UnityEngine.Tilemaps;

namespace Anais {

    public class MapData {

        private static readonly string TILE_COLLISION = "system_0";

        private bool[,] collision;
        private Node[,] nodes;

        private BoundsInt bounds;
        private int xLogic, yLogic;

        public int Width { get; }
        public int Height { get; }

        public MapData() {
            // Find the tilemap corresponding to collision data
            Tilemap collisionMap = GameObject.Find("Collision").GetComponent<Tilemap>();

            // Get the bounds of the terrain, and set logical offset, to correspond to the 2D array structure
            BoundsInt bounds = GameObject.Find("Tilemap (terrain)").GetComponent<Tilemap>().cellBounds;
            this.bounds = bounds;

            Width = bounds.xMax - bounds.xMin;
            Height = bounds.yMax - bounds.yMin;
            collision = new bool[Width, Height];
            nodes = new Node[Width, Height];

            xLogic = -bounds.xMin;
            yLogic = -bounds.yMin;

            // Loop in the terrain bounds, and check for tiles on the collision layer
            for (int x = bounds.xMin; x < bounds.xMax; x++) {
                for (int y = bounds.yMin; y < bounds.yMax; y++) {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    if (collisionMap.HasTile(position)) {
                        if (collisionMap.GetTile(position).name == TILE_COLLISION) {
                            collision[x + xLogic, y + yLogic] = true;
                        }
                    }
                    // Create node here
                    nodes[x + xLogic, y + yLogic] = new Node(x, y);
                }
            }

        }

        public bool GetCollision(int x, int y) {
            // If the coordinates are out of bounds, return true. If not, return the data from the 2D array
            int arrayX = x + xLogic;
            int arrayY = y + yLogic;
            if (OutOfBounds(arrayX, arrayY)) {
                return true;
            } else {
                return collision[arrayX, arrayY];
            }
        }

        public Node GetNode(int x, int y) {
            // If the coordinates are out of bounds, return null. If not, return the data from the 2D array
            int arrayX = x + xLogic;
            int arrayY = y + yLogic;
            if (OutOfBounds(arrayX, arrayY)) {
                return null;
            } else {
                return nodes[arrayX, arrayY];
            }
        }

        public Node GetNodeDirect(int x, int y) {
            if (OutOfBounds(x, y)) {
                return null;
            } else {
                return nodes[x, y];
            }
        }

        private bool OutOfBounds(int x, int y) {
            return (x < 0 || x >= Width || y < 0 || y >= Height);
        }

    }

}
