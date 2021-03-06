﻿using FytCore;
using System.Collections.Generic;

namespace Anais {

    public class UnitObjectCollection {

        private List<IUnitObject> unitObjects;

        public int Count { get => unitObjects.Count; }
        public IUnitObject this[int key] { get => unitObjects[key]; }

        public UnitObjectCollection() {
            unitObjects = new List<IUnitObject>();
        }

        public void Set(IEnumerable<IUnitObject> list) {
            unitObjects.Clear();
            foreach (IUnitObject obj in list) {
                unitObjects.Add(obj);
            }
        }

        public void Process(FytInput input) {
            for (int i = 0; i < unitObjects.Count; i++) {
                unitObjects[i].Process(input);
            }
        }

    }

}
