using FytCore;
using UnityEngine;

namespace Anais {

    public class SmoothCamera : MonoBehaviour, IFytObject {

        private static readonly float PIVOT_OFFSET = 0.5f;

        public static readonly float ALPHA_SMOOTH = 0.04f;
        public static readonly float ALPHA_SLOW = 0.07f;
        public static readonly float ALPHA_RESPONSIVE = 0.4f;

        private Transform transformTarget;
        private Vector3 positionTarget;

        public float Alpha { get; set; }

        void Start() {
            transformTarget = null;
            positionTarget = Vector3.zero;
            ResetAlpha();
        }

        public void ApplyTransformTarget(Transform transformTarget) {
            this.transformTarget = transformTarget;
        }

        public void ClearTransformTarget() {
            transformTarget = null;
        }

        public void ChangePositionTarget(float dx, float dy) {
            positionTarget.x += dx;
            positionTarget.y += dy;
        }

        public void FytEarlyUpdate(FytInput input) {
            
        }

        public void FytUpdate(FytInput input) {

        }

        public void FytLateUpdate(FytInput input) {
            // Set target position
            if (transformTarget != null) {
                Vector3 pos = transformTarget.position;
                positionTarget.Set(pos.x, pos.y + PIVOT_OFFSET, pos.z);
            }
            // Lerp
            Vector3 oldPosition = transform.position;
            Vector3 moveTo = new Vector3(positionTarget.x, positionTarget.y, oldPosition.z);
            transform.position = Vector3.Lerp(oldPosition, moveTo, Alpha);
        }

        public void ResetAlpha() {
            Alpha = ALPHA_RESPONSIVE;
        }

    }

}
