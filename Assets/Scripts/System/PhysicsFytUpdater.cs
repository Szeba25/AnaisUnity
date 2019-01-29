using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FytCore {

    public class PhysicsFytUpdater : MonoBehaviour {
        /*
         * For games utilizing the physics engine!
         * 
         * >>> Default values:
         * Fixed Timestep: 0.02
         * Maximum Allowed Timestep: 0.1
         * Time Scale: 1
         * Maximum Particle Timestep: 0.03
         * 
         * >>> Recommended values (for 60 FPS):
         * Fixed Timestep: 0.01666667
         * Maximum Allowed Timestep: 0.08
         * Time Scale: 1
         * Maximum Particle Timestep: 0.025
         */

        private const float SECOND = 1.0f;

        private float debugTimer;
        private int frameCounter;
        private int updateCounter;

        private FytInput input;
        private List<IFytObject> fytObjects;

        void Start() {
            Debug.Log("System init: Physics fyt updater");

            debugTimer = 0.0f;
            frameCounter = 0;
            updateCounter = 0;

            input = new FytInput();

            fytObjects = new List<IFytObject>();
            foreach (IFytObject obj in FindObjectsOfType<MonoBehaviour>().OfType<IFytObject>()) {
                fytObjects.Add(obj);
            }
        }

        void FixedUpdate() {
            FytEarlyUpdateAll();
            FytUpdateAll();
            FytLateUpdateAll();
            input.Process();
            updateCounter++;
        }

        void Update() {
            input.Poll();
            frameCounter++;

            debugTimer += Time.deltaTime;
            if (debugTimer >= SECOND) {
                //Debug.Log("UPS: " + updateCounter);
                //Debug.Log("FPS: " + frameCounter);
                updateCounter = 0;
                frameCounter = 0;
                debugTimer -= SECOND;
            }
        }

        private void FytEarlyUpdateAll() {
            for (int i = 0; i < fytObjects.Count; i++) {
                fytObjects[i].FytEarlyUpdate(input);
            }
        }

        private void FytUpdateAll() {
            for (int i = 0; i < fytObjects.Count; i++) {
                fytObjects[i].FytUpdate(input);
            }
        }

        private void FytLateUpdateAll() {
            for (int i = 0; i < fytObjects.Count; i++) {
                fytObjects[i].FytLateUpdate(input);
            }
        }

    }

}
