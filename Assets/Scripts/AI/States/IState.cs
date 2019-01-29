using FytCore;

namespace Anais {

    public interface IState<E> {

        void Enter(E entity);
        void Process(E entity, FytInput input);
        void Exit(E entity);

    }

}
