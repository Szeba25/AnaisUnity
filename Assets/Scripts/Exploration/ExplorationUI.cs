using FytCore;
using System.Collections.Generic;
using UnityEngine;

namespace Anais {

    public class ExplorationUI {

        private Party party;
        private IUnitObject partyLeader;
        private SmoothCamera smoothCamera;

        private NodeCollection movementNodes;
        private Node retraceNode;
        private GridSelection nodeSelection;
        private PathPointCollection pathPoints;

        public StateMachine<ExplorationUI, ExplorationUIStates> StateMachine { get; private set; }

        private bool locked;
        private bool moveActionFired;

        public ExplorationUI(Party party, IUnitObject partyLeader, SmoothCamera smoothCamera, GridSelection nodeSelection, PathPointCollection pathPoints) {
            this.party = party;
            this.partyLeader = partyLeader;
            this.smoothCamera = smoothCamera;

            movementNodes = null;
            retraceNode = null;
            this.nodeSelection = nodeSelection;
            this.pathPoints = pathPoints;

            Dictionary<ExplorationUIStates, IState<ExplorationUI>> states = new Dictionary<ExplorationUIStates, IState<ExplorationUI>>();
            states.Add(ExplorationUIStates.HALT, new ExplorationUIHaltState());
            states.Add(ExplorationUIStates.WAIT_FOR_INPUT, new ExplorationUIWaitForInputState());
            states.Add(ExplorationUIStates.TRACE_PATH, new ExplorationUITracePathState());
            states.Add(ExplorationUIStates.WAIT_FOR_CONFIRM, new ExplorationUIWaitForConfirmState());
            states.Add(ExplorationUIStates.FIRE_MOVE_ACTION, new ExplorationUIFireMoveActionState());
            StateMachine = new StateMachine<ExplorationUI, ExplorationUIStates>(this, states);
            StateMachine.Initialize(ExplorationUIStates.HALT);
        }

        public void Process(FytInput input) {
            if (!locked) {
                StateMachine.Process(input);
                if (input.Dragging()) {
                    smoothCamera.ResetAlpha();
                    smoothCamera.ClearTransformTarget();
                    Vector3 dragAmount = input.WorldDragAmount();
                    smoothCamera.ChangePositionTarget(-dragAmount.x, -dragAmount.y);
                }
            } else {
                smoothCamera.Alpha = SmoothCamera.ALPHA_SLOW;
                partyLeader.Unit.Body.ApplyTransformToCamera(smoothCamera);
            }
        }

        public void Prepare(NodeCollection movementNodes) {
            this.movementNodes = movementNodes;

            retraceNode = null;
            nodeSelection.Visible = false;
            pathPoints.HideAll();
            moveActionFired = false;

            Unlock();
            StateMachine.ChangeState(ExplorationUIStates.WAIT_FOR_INPUT);
        }

        private void Lock() {
            locked = true;
        }

        private void Unlock() {
            locked = false;
        }

        public Node GetMovementNode(FytInput input) {
            Vector2Int tilePosition = MathUtils.TileVectorFromWorld(input.MouseWorldPosition());
            for (int i = 0; i < movementNodes.Count; i++) {
                Node node = movementNodes[i];
                if (node.X == tilePosition.x && node.Y == tilePosition.y) {
                    return node;
                }
            }
            return null;
        }

        public void SetSelection(Node node) {
            retraceNode = node;
            nodeSelection.Position = new Vector2Int(node.X, node.Y);
            nodeSelection.Visible = true;
        }

        public bool IsCurrentRetraceNode(Node node) {
            return ReferenceEquals(retraceNode, node);
        }

        public void FireMoveAction() {
            partyLeader.Unit.Body.RetracePath(retraceNode);
            moveActionFired = true;
            Lock();
        }

        public void RetracePath() {
            pathPoints.ShowFrom(retraceNode);
        }

        public bool IsMoveActionFired() {
            return moveActionFired;
        }

    }

}
