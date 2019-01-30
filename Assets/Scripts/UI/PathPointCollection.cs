using System.Collections.Generic;
using UnityEngine;

namespace Anais {

    public class PathPointCollection {

        private List<GameObject> pathPoints;

        public PathPointCollection() {
            pathPoints = new List<GameObject>();
        }

        public void Add(GameObject pathPointObject, GameObject categoryObject) {
            pathPointObject.SetActive(false);
            pathPointObject.transform.parent = categoryObject.transform;
            pathPoints.Add(pathPointObject);
        }

        public void HideAll() {
            for (int i = 0; i < pathPoints.Count; i++) {
                pathPoints[i].SetActive(false);
            }
        }

        public void ShowFrom(Node node) {
            int i = 0;
            while (node != null) {
                if (i < pathPoints.Count) {
                    pathPoints[i].SetActive(true);
                    pathPoints[i].transform.position = MathUtils.WorldVectorFromTileCentered(new Vector2Int(node.X, node.Y));
                    node = node.Parent;
                    i++;
                } else {
                    Debug.Log("PathPointCollection: can't handle " + i + " path length for display!");
                    node = null;
                }
            }
        }

    }

}
