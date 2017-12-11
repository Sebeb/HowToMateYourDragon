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
        transform.position += Vector3.right * speed * Time.deltaTime * 4;
        //if (transform.position.x >= 700 + returnDist)
        //    transform.position -= Vector3.left * (Game.controller.worldSize[0] + returnDist * 2);


    }
}
