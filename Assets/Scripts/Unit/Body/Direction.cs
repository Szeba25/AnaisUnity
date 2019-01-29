using UnityEngine;

namespace Anais {

    public enum Direction {
        DOWN, LEFT, RIGHT, UP
    }

    public static class DirectionMapper {

        public static Direction GetDirection(Vector2Int next, Vector2Int prev) {
            if (next.x < prev.x) {
                return Direction.LEFT;
            } else if (next.x > prev.x) {
                return Direction.RIGHT;
            } else {
                if (next.y < prev.y) {
                    return Direction.DOWN;
                } else {
                    return Direction.UP;
                }
            }
        }

    }

}
