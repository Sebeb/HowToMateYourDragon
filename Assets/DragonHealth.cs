using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHealth : MonoBehaviour {

    public GameObject anchor;
    public Vector2 offset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = anchor.transform.position + (Vector3)offset;
	}
}
