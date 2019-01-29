using FytCore;

namespace Anais {

    public enum ExplorationUIStates {
        HALT,
        WAIT_FOR_INPUT,
        TRACE_PATH,
        WAIT_FOR_CONFIRM,
        FIRE_MOVE_ACTION
    }

    public class ExplorationUIHaltState : IState<ExplorationUI> {

        public void Enter(ExplorationUI entity) {
            
        }

        public void Process(ExplorationUI entity, FytInput input) {
            
        }

        public void Exit(ExplorationUI entity) {
            
        }

    }

    public class ExplorationUIWaitForInputState : IState<ExplorationUI> {

        public void Enter(ExplorationUI entity) {
            
        }

        public void Process(ExplorationUI entity, FytInput input) {
            if (input.Tapping()) {
                Node node = entity.GetMovementNode(input);
                if (node != null) {
                    entity.SetSelection(node);
                    entity.StateMachine.ChangeState(ExplorationUIStates.TRACE_PATH);
                }
            }
        }

        public void Exit(ExplorationUI entity) {

        }

    }

    public class ExplorationUITracePathState : IState<ExplorationUI> {

        public void Enter(ExplorationUI entity) {
            entity.RetracePath();
            entity.StateMachine.ChangeState(ExplorationUIStates.WAIT_FOR_CONFIRM);
        }

        public void Process(ExplorationUI entity, FytInput input) {
            
        }

        public void Exit(ExplorationUI entity) {
            
        }

    }

    public class ExplorationUIWaitForConfirmState : IState<ExplorationUI> {

        public void Enter(ExplorationUI entity) {
            
        }

        public void Process(ExplorationUI entity, FytInput input) {
            if (input.Tapping()) {
                Node node = entity.GetMovementNode(input);
                if (node != null) {
                    if (entity.IsCurrentRetraceNode(node)) {
                        entity.StateMachine.ChangeState(ExplorationUIStates.FIRE_MOVE_ACTION);
                    } else {
                        entity.SetSelection(node);
                        entity.StateMachine.ChangeState(ExplorationUIStates.TRACE_PATH);
                    }
                }
            }
        }

        public void Exit(ExplorationUI entity) {

        }

    }

    public class ExplorationUIFireMoveActionState : IState<ExplorationUI> {

        public void Enter(ExplorationUI entity) {
            entity.FireMoveAction();
            entity.StateMachine.ChangeState(ExplorationUIStates.HALT);
        }

        public void Process(ExplorationUI entity, FytInput input) {
            
        }

        public void Exit(ExplorationUI entity) {

        }

    }

}
