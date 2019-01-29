using UnityEngine;

namespace Anais {

    public class GameCore : MonoBehaviour {

        public Party Party { get; private set; }

        void Start() {
            Party = new Party();
        }

    }

}
