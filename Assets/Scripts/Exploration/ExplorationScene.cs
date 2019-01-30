using FytCore;
using System.Collections.Generic;
using UnityEngine;

namespace Anais {

    public class ExplorationScene : MonoBehaviour, IFytObject {

        private static readonly int MAX_PATH_LENGTH = 50;

        public GameObject uiCloneObjectsPrefab;
        public GameObject nodeSelectionPrefab;
        public GameObject pathPointPrefab;

        private GameObject uiCloneObjects;
        private GridSelection nodeSelection;
        private PathPointCollection pathPoints;

        private GameCore gameCore;
        private GameComponents components;
        private SmoothCamera smoothCamera;
        private IUnitObject partyLeader;

        private NodeCollection movementNodes;

        private ExplorationUI explorationUI;

        public StateMachine<ExplorationScene, ExplorationStates> StateMachine { get; private set; }

        void Awake() {
            uiCloneObjects = Instantiate(uiCloneObjectsPrefab);

            GameObject nodeSelectionObject = Instantiate(nodeSelectionPrefab);
            nodeSelection = new GridSelection(nodeSelectionObject, uiCloneObjects, new Vector2Int(0, 0));

            pathPoints = new PathPointCollection();
            for (int i = 0; i < MAX_PATH_LENGTH; i++) {
                GameObject pathPoint = Instantiate(pathPointPrefab);
                pathPoints.Add(pathPoint, uiCloneObjects);
            }
        }

        void Start()  {
            gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
            components = GameObject.Find("GameComponents").GetComponent<GameComponents>();
            smoothCamera = GameObject.Find("MainCamera").GetComponent<SmoothCamera>();
            partyLeader = GameObject.Find("PartyLeader").GetComponent<PartyLeader>();

            movementNodes = new NodeCollection();

            explorationUI = new ExplorationUI(gameCore.Party, partyLeader, smoothCamera, nodeSelection, pathPoints);

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
            components.Units.Process(input);
            StateMachine.Process(input);
            explorationUI.Process(input);
        }

        public void FytLateUpdate(FytInput input) {
            
        }

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
            components.Pathfinder.SetMaxCost(220);
            components.Pathfinder.Initialize(partyLeader, components.Units);
            components.Pathfinder.TimedCalculate();
            components.Pathfinder.GetAllNodes(movementNodes);
        }

    }

}
