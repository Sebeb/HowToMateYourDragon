using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDragonController : MonoBehaviour {
    public SkinnedMeshRenderer bodyMesh;
    public SkinnedMeshRenderer hornsMesh;
    public SkinnedMeshRenderer wingsMesh;
    public SkinnedMeshRenderer tailMesh;
    public SkinnedMeshRenderer mainBodyMesh;
    public SkinnedMeshRenderer mainHornsMesh;
    public SkinnedMeshRenderer mainWingsMesh;
    public SkinnedMeshRenderer mainTailMesh;
    public Texture[][] textures;

    // Use this for initialization
    void Start () {
        // mainBodyMesh = Game.player.bodyMesh;
        // mainHornsMesh = Game.player.hornsMesh;
        // mainWingsMesh = Game.player.wingsMesh;
        // mainTailMesh = Game.player.tailMesh;
        /*print(bodyMesh);
        print(Game.player);
        Game.player.bodyMesh = bodyMesh;
        Game.player.hornsMesh = hornsMesh;
        Game.player.wingsMesh = wingsMesh;
        Game.player.tailMesh = tailMesh;
        */
        // Game.player.GetAttributes();
        // textures = Game.player.GetTextures();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
