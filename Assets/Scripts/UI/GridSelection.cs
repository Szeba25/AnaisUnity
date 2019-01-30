using UnityEngine;

namespace Anais {

    public class GridSelection {

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

        public GridSelection(GameObject graphicsObject, GameObject categoryObject, Vector2Int position) {
            // Set the category
            graphicsObject.transform.parent = categoryObject.transform;

            // Set the rest of the values
            this.graphicsObject = graphicsObject;
            Position = position;
            Visible = false;
        }

    }

}
