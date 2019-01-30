using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anais {

    public class GameComponents : MonoBehaviour {

        public MapData MapData { get; private set; }
        public NodeGraphSearch Pathfinder { get; private set; }
        public UnitObjectCollection Units { get; private set; }

        void Start() {
            MapData = new MapData();
            Pathfinder = new NodeGraphSearch();
            Pathfinder.SetMapData(MapData);
            Units = new UnitObjectCollection();
            FindAllUnitObjects();
        }

        public void FindAllUnitObjects() {
            Units.Set(FindObjectsOfType<MonoBehaviour>().OfType<IUnitObject>());
        }

    }

}
