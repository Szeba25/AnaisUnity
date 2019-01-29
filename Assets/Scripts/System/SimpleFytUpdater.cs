using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FytCore {

    public class SimpleFytUpdater : MonoBehaviour {

        private const int FPS = 60;
        private const float SECOND = 1.0f;
        private const float FRAME_TIME = SECOND / FPS;

        private float accumulator;

        private float debugTimer;
        private int frameCounter;
        private int updateCounter;

        private FytInput input;
        private List<IFytObject> fytObjects;

        void Start() {
            Debug.Log("System init: Simple fyt updater");

            accumulator = 0.0f;

            debugTimer = 0.0f;
            frameCounter = 0;
            updateCounter = 0;

            input = new FytInput();

            fytObjects = new List<IFytObject>();
            foreach(IFytObject obj in FindObjectsOfType<MonoBehaviour>().OfType<IFytObject>()) {
                fytObjects.Add(obj);
            }
        }

        void Update() {
            float currentDeltaTime = Time.deltaTime;
            accumulator += currentDeltaTime;

            while (accumulator >= FRAME_TIME) {
                FytEarlyUpdateAll();
                FytUpdateAll();
                FytLateUpdateAll();
                input.Process();
                updateCounter++;
                accumulator -= FRAME_TIME;
            }

            input.Poll();
            frameCounter++;

            debugTimer += currentDeltaTime;
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
