using UnityEngine;

namespace Anais {

    public class GameCore : MonoBehaviour {

        public Database Database { get; private set; }
        public Party Party { get; private set; }

        void Start() {
            Database = new Database();
            Party = new Party();
        }

    }

}
