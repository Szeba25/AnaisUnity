using FytCore;

namespace Anais {

    public interface IUnitObject {
        Unit Unit { get; }
        void Process(FytInput input);
    }

}
