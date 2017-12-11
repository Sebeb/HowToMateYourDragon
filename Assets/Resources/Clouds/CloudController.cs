using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {
    public float speed = 2;
    private int returnDist = 2000;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x >= Game.controller.worldSize[0] + returnDist)
            transform.position -= Vector3.left * (Game.controller.worldSize[0] + returnDist * 2);


    }
}
