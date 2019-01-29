using System;
using System.Collections.Generic;
using UnityEngine;

namespace FytCore {

    public interface IFytObject {

        void FytEarlyUpdate(FytInput input);
        void FytUpdate(FytInput input);
        void FytLateUpdate(FytInput input);

    }

    public class InputTuple {

        public bool Pressed { get; set; }
        public bool JustPressed { get; set; }
        public bool Released { get; set; }

        public InputTuple() {
            Pressed = false;
            JustPressed = false;
            Released = false;
        }

    }

    public class FytInput {

        private Camera cachedCamera;

        private InputTuple leftMouseButton;
        private Dictionary<KeyCode, InputTuple> keyTable;
        private KeyCode[] keyCodes;

        private bool dragPossible;
        private bool drag;
        private Vector3 screenDragPosition;
        private Vector3 worldDragPosition;
        private bool tapSwitch;

        public FytInput() {
            cachedCamera = Camera.main;

            leftMouseButton = new InputTuple();

            keyTable = new Dictionary<KeyCode, InputTuple>();
            keyCodes = (KeyCode[]) Enum.GetValues(typeof(KeyCode));

            for (int i = 0; i < keyCodes.Length; i++) {
                keyTable[keyCodes[i]] = new InputTuple();
            }

            dragPossible = false;
            drag = false;
            screenDragPosition = Vector3.zero;
            worldDragPosition = Vector3.zero;
            tapSwitch = false;
        }

        public void Process() {
            ResetInputFlags();
            CheckForDragging();
            UpdateDragging();
        }

        private void ResetInputFlags() {
            // Reset keyboard
            for (int i = 0; i < keyCodes.Length; i++) {
                InputTuple data = keyTable[keyCodes[i]];
                data.JustPressed = false;
                data.Released = false;
            }

            // Reset mouse
            leftMouseButton.JustPressed = false;
            leftMouseButton.Released = false;

            // Reset tapping
            tapSwitch = false;
        }

        private void CheckForDragging() {
            if (!drag && dragPossible) {
                Vector3 delta = ScreenDragDelta();
                if (Math.Abs(delta.x) > 10 || Math.Abs(delta.y) > 10) {
                    drag = true;
                }
            }
        }

        private void UpdateDragging() {
            if (drag) {
                RepositionDragVectors();
            }
        }

        private Vector3 ScreenDragDelta() {
            Vector3 pos = MousePosition();
            return new Vector3(pos.x - screenDragPosition.x, pos.y - screenDragPosition.y, 0.0f);
        }

        private Vector3 WorldDragDelta() {
            Vector3 pos = MouseWorldPosition();
            return new Vector3(pos.x - worldDragPosition.x, pos.y - worldDragPosition.y, 0.0f);
        }

        private void RepositionDragVectors() {
            screenDragPosition = MousePosition();
            worldDragPosition = MouseWorldPosition();
        }

        public void Poll() {
            // Handle keyboard
            for (int i = 0; i < keyCodes.Length; i++) {
                InputTuple data = keyTable[keyCodes[i]];
                bool previouslyPressed = data.Pressed;
                data.Pressed = Input.GetKey(keyCodes[i]);
                if (!previouslyPressed && data.Pressed) {
                    data.JustPressed = true;
                } else if (previouslyPressed && !data.Pressed) {
                    data.Released = true;
                }
            }

            // Handle left mouse button, dragging, and tapping
            bool leftMousePreviouslyPressed = leftMouseButton.Pressed;
            leftMouseButton.Pressed = Input.GetMouseButton(0);
            if (!leftMousePreviouslyPressed && leftMouseButton.Pressed) {
                leftMouseButton.JustPressed = true;
                drag = false; // Reset dragging to false...
                dragPossible = true;
                // Make drag positions equal to mouse positions!
                RepositionDragVectors();
            } else if (leftMousePreviouslyPressed && !leftMouseButton.Pressed) {
                leftMouseButton.Released = true;
                if (!drag) {
                    tapSwitch = true;
                }
                drag = false; // Reset dragging to false...
                dragPossible = false;
            }
        }

        public bool KeyPressed(KeyCode keyCode) {
            return keyTable[keyCode].Pressed;
        }

        public bool KeyJustPressed(KeyCode keyCode) {
            return keyTable[keyCode].JustPressed;
        }

        public bool LeftMouseButtonPressed() {
            return leftMouseButton.Pressed;
        }

        public bool LeftMouseButtonJustPressed() {
            return leftMouseButton.JustPressed;
        }

        public Vector3 ScreenDragAmount() {
            if (drag) {
                return ScreenDragDelta();
            } else {
                return Vector3.zero;
            }
        }

        public Vector3 WorldDragAmount() {
            if (drag) {
                return WorldDragDelta();
            } else {
                return Vector3.zero;
            }
        }

        public bool Dragging() {
            return drag;
        }

        public bool Tapping() {
            return tapSwitch;
        }

        public Vector3 MousePosition() {
            return Input.mousePosition;
        }

        public Vector3 MouseWorldPosition() {
            return cachedCamera.ScreenToWorldPoint(Input.mousePosition);
        }

    }

}
