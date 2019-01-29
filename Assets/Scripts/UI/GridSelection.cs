using UnityEngine;

namespace Anais {

    class GridSelection {

        private GameObject graphicsObject;

        private Vector2Int position;
        private bool visible;

        public bool Visible {
            get => visible;
            set {
                visible = value;
                graphicsObject.SetActive(visible);
            }
        }

        public Vector2Int Position {
            get => position;
            set {
                position = value;
                graphicsObject.transform.position = MathUtils.WorldVectorFromTileCentered(position);
            }
        }

        public GridSelection(GameObject graphicsObject, Vector2Int position) {
            this.graphicsObject = graphicsObject;
            Position = position;
            Visible = false;
        }

    }

}
