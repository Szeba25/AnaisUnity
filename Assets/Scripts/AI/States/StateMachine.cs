using FytCore;
using System.Collections.Generic;

namespace Anais {

    public class StateMachine<E, S> {

        private E entity;
        private Dictionary<S, IState<E>> states;

        private S currentStateId;
        private IState<E> currentState;

        public StateMachine(E entity, Dictionary<S, IState<E>> states) {
            this.entity = entity;
            this.states = states;
        }

        public void Initialize(S startingStateId) {
            currentStateId = startingStateId;
            currentState = states[startingStateId];
            currentState.Enter(entity);
        }

        public void ChangeState(S newStateId) {
            currentState.Exit(entity);
            currentStateId = newStateId;
            currentState = states[newStateId];
            currentState.Enter(entity);
        }

        public void Process(FytInput input) {
            currentState.Process(entity, input);
        }

    }

}
