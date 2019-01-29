using FytCore;

namespace Anais {

    public enum ExplorationStates {
        CALCULATE_PATH,
        PREPARE_UI,
        WAIT_FOR_UI_ACTION,
        RUN_ACTION
    }

    public class ExplorationCalculatePathState : IState<ExplorationScene> {

        public void Enter(ExplorationScene entity) {
            entity.CalculatePath();
            entity.StateMachine.ChangeState(ExplorationStates.PREPARE_UI);
        }

        public void Process(ExplorationScene entity, FytInput input) {

        }

        public void Exit(ExplorationScene entity) {
            
        }

    }

    public class ExplorationPrepareUIState : IState<ExplorationScene> {

        public void Enter(ExplorationScene entity) {
            entity.PrepareUI();
            entity.StateMachine.ChangeState(ExplorationStates.WAIT_FOR_UI_ACTION);
        }

        public void Process(ExplorationScene entity, FytInput input) {

        }

        public void Exit(ExplorationScene entity) {

        }

    }

    public class ExplorationWaitForUIActionState : IState<ExplorationScene> {

        public void Enter(ExplorationScene entity) {

        }

        public void Process(ExplorationScene entity, FytInput input) {
            if (entity.IsMoveActionFiredOnUI()) {
                entity.StateMachine.ChangeState(ExplorationStates.RUN_ACTION);
            }
        }

        public void Exit(ExplorationScene entity) {

        }

    }

    public class ExplorationRunActionState : IState<ExplorationScene> {

        public void Enter(ExplorationScene entity) {

        }

        public void Process(ExplorationScene entity, FytInput input) {
            if (!entity.GetPartyLeader().Unit.Body.IsMoving()) {
                entity.StateMachine.ChangeState(ExplorationStates.CALCULATE_PATH);
            }
        }

        public void Exit(ExplorationScene entity) {

        }

    }

}
