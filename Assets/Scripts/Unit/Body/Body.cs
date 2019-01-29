using System;
using UnityEngine;

namespace Anais {

    public class Body {

        // Registered components
        private Stats stats;

        private SpriteRenderer spriteRenderer;
        private Transform transform;
        private Sprite[] sprites;

        private int speed;
        private int moveDuration;
        private Route route;
        private Direction direction;

        private int animFrame;
        private int animSpeed;
        private int frameId;

        private Vector3 target;
        public Vector2Int TilePosition { get; set; }
        public bool Material { get; set; }

        public Body(SpriteRenderer spriteRenderer, Sprite[] sprites, Transform transform) {
            // Set registered components to null
            stats = null;

            this.spriteRenderer = spriteRenderer;
            this.transform = transform;
            this.sprites = sprites;

            speed = 25;
            moveDuration = 0;
            route = new Route();
            direction = Direction.DOWN;

            animFrame = 0;
            animSpeed = 12;
            frameId = 0;

            Vector3 temp = transform.position;
            target = new Vector3(temp.x, temp.y, temp.z);
            TilePosition = MathUtils.TileVectorFromWorld(temp);
            Material = true;
        }

        public void Register(Stats stats) {
            this.stats = stats;
        }

        public void Update() {
            GetMovementFromRoute();
            UpdateAnimation();
            UpdateMovement();
            Sort();
        }

        public void RetracePath(Node node) {
            route.RetraceFromNode(node);
        }

        public bool IsMoving() {
            return (route.IsValid() || moveDuration > 0);
        }

        public void ApplyTransformToCamera(SmoothCamera smoothCamera) {
            smoothCamera.ApplyTransformTarget(transform);
        }

        private void GetMovementFromRoute() {
            if (moveDuration == 0 && route.IsValid()) {
                direction = route.TurnToNextTargetPosition(TilePosition);
                TilePosition = route.GetNextTargetPosition();
                target = MathUtils.WorldVectorFromTile(TilePosition);
                moveDuration = speed;
            }
        }

        private void UpdateAnimation() {
            if (IsMoving()) {
                if (animFrame < animSpeed) {
                    animFrame++;
                } else {
                    animFrame = 0;
                    frameId = (frameId + 1) % 4;
                }
            } else {
                frameId = 0;
            }
            spriteRenderer.sprite = sprites[((int)direction * 4) + frameId];
        }

        private void UpdateMovement() {
            if (moveDuration > 0) {
                Vector3 temp = transform.position;
                temp.x = MathUtils.NextPosition(temp.x, target.x, moveDuration);
                temp.y = MathUtils.NextPosition(temp.y, target.y, moveDuration);
                transform.position = temp;
                moveDuration--;
            }
        }

        private void Sort() {
            spriteRenderer.sortingOrder = -(int)transform.position.y * 40;
        }
        
    }

}
