using FytCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anais {

    public class ExplorationScene : MonoBehaviour, IFytObject {

        private static readonly int MAX_PATH_LENGTH = 50;

        public GameObject uiCloneObjectsPrefab;
        public GameObject nodeSelectionPrefab;
        public GameObject pathPointPrefab;

        private GameObject uiCloneObjects;
        private GameObject nodeSelectionObject;
        private List<GameObject> pathPoints;

        private GameCore gameCore;
        private List<IUnitObject> units;
        private SmoothCamera smoothCamera;
        private IUnitObject partyLeader;

        private MapData collision;
        private NodeGraphSearch pathfinder;
        private List<Node> movementNodes;

        private ExplorationUI explorationUI;

        public StateMachine<ExplorationScene, ExplorationStates> StateMachine { get; private set; }

        void Awake() {
            uiCloneObjects = Instantiate(uiCloneObjectsPrefab);
            nodeSelectionObject = Instantiate(nodeSelectionPrefab);
            nodeSelectionObject.SetActive(false);
            nodeSelectionObject.transform.parent = uiCloneObjects.transform;
            pathPoints = new List<GameObject>();
            for (int i = 0; i < MAX_PATH_LENGTH; i++) {
                GameObject pathPoint = Instantiate(pathPointPrefab);
                pathPoint.SetActive(false);
                pathPoint.transform.parent = uiCloneObjects.transform;
                pathPoints.Add(pathPoint);
            }
        }

        void Start()  {
            gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
            units = new List<IUnitObject>();
            RefreshUnits();
            smoothCamera = GameObject.Find("MainCamera").GetComponent<SmoothCamera>();
            partyLeader = GameObject.Find("PartyLeader").GetComponent<PartyLeader>();

            collision = new MapData();
            pathfinder = new NodeGraphSearch();
            pathfinder.SetMapData(collision);
            pathfinder.SetMaxCost(350);
            movementNodes = new List<Node>();

            explorationUI = new ExplorationUI(gameCore.Party, partyLeader, smoothCamera, nodeSelectionObject, pathPoints);

            Dictionary<ExplorationStates, IState<ExplorationScene>> states = new Dictionary<ExplorationStates, IState<ExplorationScene>>();
            states.Add(ExplorationStates.CALCULATE_PATH, new ExplorationCalculatePathState());
            states.Add(ExplorationStates.PREPARE_UI, new ExplorationPrepareUIState());
            states.Add(ExplorationStates.RUN_ACTION, new ExplorationRunActionState());
            states.Add(ExplorationStates.WAIT_FOR_UI_ACTION, new ExplorationWaitForUIActionState());
            StateMachine = new StateMachine<ExplorationScene, ExplorationStates>(this, states);
            StateMachine.Initialize(ExplorationStates.CALCULATE_PATH);
        }

        public void FytEarlyUpdate(FytInput input) {
            
        }

        public void FytUpdate(FytInput input) {
            for (int i = 0; i < units.Count; i++) {
                units[i].Process(input);
            }
            StateMachine.Process(input);
            explorationUI.Process(input);
        }

        public void FytLateUpdate(FytInput input) {
            
        }

        private void RefreshUnits() {
            units.Clear();
            foreach(IUnitObject obj in FindObjectsOfType<MonoBehaviour>().OfType<IUnitObject>()) {
                units.Add(obj);
            }
        }

        /* StateMachine state methods */

        public void PrepareUI() {
            explorationUI.Prepare(movementNodes);
        }

        public bool IsMoveActionFiredOnUI() {
            return explorationUI.IsMoveActionFired();
        }

        public IUnitObject GetPartyLeader() {
            return partyLeader;
        }

        public void CalculatePath() {
            pathfinder.Initialize(partyLeader, units);
            pathfinder.TimedCalculate();
            pathfinder.GetAllNodes(movementNodes);
        }

    }

}
