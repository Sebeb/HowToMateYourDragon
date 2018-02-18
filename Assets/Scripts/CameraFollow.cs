using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public Vector2 bounds;
	// Use this for initialization
	void Start () {
        target = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Mathf.Clamp(target.transform.position.x,-Game.controller.worldSize.x+bounds.x,Game.controller.worldSize.x - bounds.x), 
            Mathf.Clamp(target.transform.position.y,-Game.controller.worldSize.y +bounds.y,Game.controller.worldSize.y - bounds.y), transform.position.z);
	}
}
