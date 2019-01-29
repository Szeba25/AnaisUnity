using UnityEngine;

namespace Anais {

    public class MathUtils {

        private static readonly float BODY_X_OFFSET = 0.5f;
        private static readonly float BODY_Y_OFFSET = 0.25f;
        private static readonly float CENTERED_OFFSET = 0.5f;

        public static Vector2Int TileVectorFromWorld(Vector3 reference) {
            float nx = reference.x;
            float ny = reference.y;
            if (nx < 0) nx -= 1;
            if (ny < 0) ny -= 1;
            return new Vector2Int((int)nx, (int)ny);
        }

        public static Vector3 WorldVectorFromTile(Vector2Int reference) {
            return new Vector3(reference.x + BODY_X_OFFSET, reference.y + BODY_Y_OFFSET, 0.0f);
        }

        public static Vector3 WorldVectorFromTileCentered(Vector2Int reference) {
            return new Vector3(reference.x + CENTERED_OFFSET, reference.y + CENTERED_OFFSET, 0.0f);
        }

        public static float NextPosition(float pos, float targetPos, int d) {
            return (pos * (d - 1) + targetPos) / d;
        }

    }

}
