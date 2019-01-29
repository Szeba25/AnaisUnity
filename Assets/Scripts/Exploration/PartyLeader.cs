using Anais;
using FytCore;
using System.IO;
using UnityEngine;

public class PartyLeader : MonoBehaviour, IUnitObject {

    public Unit Unit { get; private set; }

    void Awake() {
        // TEST
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles/testbundle"));
        Sprite[] sprites = bundle.LoadAssetWithSubAssets<Sprite>("Assets/LoadedAssets/Spritesets/player.png");
        GetComponent<SpriteRenderer>().sprite = sprites[7];

        Body body = new Body(GetComponent<SpriteRenderer>(), sprites, transform);
        Behavior behavior = new Behavior();
        Stats stats = new Stats();
        Unit = new Unit(body, behavior, stats);
    }

    public void Process(FytInput input) {
        Unit.Body.Update();
    }

}
