using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {
    private float speed = 10;
    public int returnDist;
    public float worldSizeX;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x >= worldSizeX + returnDist)
            transform.position += Vector3.left * (worldSizeX + returnDist * 2);


    }
}
